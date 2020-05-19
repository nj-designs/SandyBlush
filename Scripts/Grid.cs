using Godot;
using System;

public class Grid : Node2D
{
    struct Vector2i
    {
        public Vector2i(int x, int y)
        {
            x_ = x;
            y_ = y;
        }

        public int X
        {
            get { return x_; }
            set { x_ = value; }
        }

        public int Y
        {
            get { return y_; }
            set { y_ = value; }
        }

        public static Vector2i operator -(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(v1.X - v2.X, v1.Y - v2.Y);
        }

        public override string ToString()
        {
            return string.Format("Vector2i[{0},{1}]", x_, y_);
        }
        private int x_;
        private int y_;
    };

    // Grid Variables
    [Export]
    private int width; //8
    [Export]
    private int height; //10
    [Export]
    private int x_start; // 64
    [Export]
    private int y_start; //800
    [Export]
    private int offset; //64
    [Export]
    private int refill_y_offset; // Spawn refill piece this many rows above target place

    enum State { WAIT, MOVE };

    private State state_;

    // All possible pices are list here
    private PackedScene[] PossiblePieces = new PackedScene[] {
        (PackedScene)GD.Load("res://Scenes/yellow_piece.tscn"),
        (PackedScene)GD.Load("res://Scenes/blue_piece.tscn"),
        (PackedScene)GD.Load("res://Scenes/pink_piece.tscn"),
        (PackedScene)GD.Load("res://Scenes/orange_piece.tscn"),
        (PackedScene)GD.Load("res://Scenes/green_piece.tscn"),
        (PackedScene)GD.Load("res://Scenes/light_green_piece.tscn"),
    };

    // Current peices in play
    private Node2D[,] AllPieces;
    // Called when the node enters the scene tree for the first time.

    private Vector2 FirstTouch = new Vector2();
    private Vector2 FinalTouch = new Vector2();

    bool controllingPiece = false;

    //Score vars
    [Signal]
    delegate void UpdateScore(int value);
    [Export]
    private int piece_value;
    private int streak = 1;

    // Effects
    private PackedScene particleEffect = (PackedScene)GD.Load("res://Scenes/ParticleEffect.tscn");
    private PackedScene animatedEffect = (PackedScene)GD.Load("res://Scenes/AnimatedExplosion.tscn");

    public override void _Ready()
    {
        GD.Randomize();
        AllPieces = new Node2D[width, height];
        SpawnPieces();
        state_ = State.MOVE;
    }

    private Vector2 GridToPixel(Vector2i pos)
    {
        return new Vector2(x_start + (offset * pos.X), y_start - (offset * pos.Y));
    }

    private void SpawnPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int piece_idx = 0;
                int loops = 0;
                Node2D piece = null;
                do
                {
                    if (++loops == 100)
                    {
                        break;
                    }
                    piece_idx = (int)Math.Floor(GD.RandRange(0, PossiblePieces.Length));
                    piece = (Node2D)PossiblePieces[piece_idx].Instance();
                } while (MatchAt(x, y, (String)piece.Get("colour")));
                piece.Position = GridToPixel(new Vector2i(x, y));
                AddChild(piece);
                AllPieces[x, y] = piece;
            }
        }
    }

    private bool MatchAt(int column, int row, String colour)
    {
        // Check Left
        if (column > 1)
        {
            if (AllPieces[column - 1, row] != null && AllPieces[column - 2, row] != null)
            {
                if ((String)AllPieces[column - 1, row].Get("colour") == colour && (String)AllPieces[column - 2, row].Get("colour") == colour)
                {
                    return true;
                }
            }
        }
        // Check Down
        if (row > 1)
        {
            if (AllPieces[column, row - 1] != null && AllPieces[column, row - 2] != null)
            {
                if ((String)AllPieces[column, row - 1].Get("colour") == colour && (String)AllPieces[column, row - 2].Get("colour") == colour)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void TouchInput()
    {
        if (Input.IsActionJustPressed("ui_touch"))
        {
            Vector2 firstTouch = GetGlobalMousePosition();
            Vector2i gridPos = PixelToGrid(firstTouch);
            if (IsInGrid(gridPos))
            {
                FirstTouch = firstTouch;
                controllingPiece = true;
            }
        }
        if (Input.IsActionJustReleased("ui_touch"))
        {
            Vector2i gridPos = PixelToGrid(GetGlobalMousePosition());
            if (IsInGrid(gridPos) && controllingPiece)
            {
                GD.Print("Swipe");
                TouchDifference(PixelToGrid(FirstTouch), gridPos);
            }
            controllingPiece = false;
        }
    }

    private Vector2i PixelToGrid(Vector2 pixelPos)
    {
        return new Vector2i((int)Mathf.Round((pixelPos.x - x_start) / offset),
                            (int)Mathf.Round((pixelPos.y - y_start) / -offset));
    }

    public override void _Process(float delta)
    {
        if (state_ == State.MOVE)
        {
            TouchInput();
        }
    }

    private bool IsInGrid(Vector2i pos)
    {
        if ((pos.X >= 0 && pos.X < width) && (pos.Y >= 0 && pos.Y < height))
        {
            return true;
        }
        return false;
    }

    private void SwapPieces(Vector2i pos, Vector2i direction)
    {
        Vector2i otherPos = new Vector2i(pos.X + direction.X, pos.Y + direction.Y);
        GD.Print(string.Format("SwapPieces() pos:{0} otherPos:{1}", pos, otherPos));
        Piece first_piece = AllPieces[pos.X, pos.Y] as Piece;
        Piece other_piece = AllPieces[otherPos.X, otherPos.Y] as Piece;
        if (first_piece != null && other_piece != null)
        {
            state_ = State.WAIT;
            if (first_piece.IsQueuedForDeletion() == false && other_piece.IsQueuedForDeletion() == false)
            {
                AllPieces[pos.X, pos.Y] = other_piece;
                AllPieces[otherPos.X, otherPos.Y] = first_piece;
                first_piece.Move(GridToPixel(otherPos));
                other_piece.Move(GridToPixel(pos));
                FindMatches();
            }
        }
    }

    private void TouchDifference(Vector2i pos1, Vector2i pos2)
    {
        Vector2i diff = pos2 - pos1;
        GD.Print(string.Format("TouchDifference() pos1:{0} pos1:{1} diff:{2}", pos1, pos2, diff));
        if (Math.Abs(diff.X) > Math.Abs(diff.Y))
        {
            if (diff.X > 0)
            {
                SwapPieces(pos1, new Vector2i(1, 0));
            }
            else if (diff.X < 0)
            {
                SwapPieces(pos1, new Vector2i(-1, 0));
            }
        }
        else if (Math.Abs(diff.Y) > Math.Abs(diff.X))
        {
            if (diff.Y > 0)
            {
                SwapPieces(pos1, new Vector2i(0, 1));
            }
            else if (diff.Y < 0)
            {
                SwapPieces(pos1, new Vector2i(0, -1));
            }
        }

    }

    private void FindMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Piece piece = (Piece)AllPieces[x, y];
                if (piece == null)
                {
                    continue;
                }

                Piece pieceUp = y < (height - 1) ? (Piece)AllPieces[x, y + 1] : null;
                Piece pieceDown = y > 0 ? (Piece)AllPieces[x, y - 1] : null;
                Piece pieceLeft = x > 0 ? (Piece)AllPieces[x - 1, y] : null;
                Piece pieceRight = x < (width - 1) ? (Piece)AllPieces[x + 1, y] : null;

                String pieceColour = (String)piece.Get("colour");
                String pieceUpColour = pieceUp != null ? (String)pieceUp.Get("colour") : "blankUp";
                String pieceDownColour = pieceDown != null ? (String)pieceDown.Get("colour") : "blankDown";
                String pieceLeftColour = pieceLeft != null ? (String)pieceLeft.Get("colour") : "blankLeft";
                String pieceRightColour = pieceRight != null ? (String)pieceRight.Get("colour") : "blankRight";

                if (pieceColour == pieceLeftColour && pieceColour == pieceRightColour)
                {
                    piece.SetMatched(true);
                    pieceLeft.SetMatched(true);
                    pieceRight.SetMatched(true);
                }
                if (pieceColour == pieceUpColour && pieceColour == pieceDownColour)
                {
                    piece.SetMatched(true);
                    pieceUp.SetMatched(true);
                    pieceDown.SetMatched(true);
                }
            }
        }
        GetParent().GetNode<Timer>("destroy_timer").Start();
    }

    public void OnDestroyTimerTimeout()
    {
        GD.Print("DestroyTimerTimeout!");
        DestroyMatched();
        GetParent().GetNode<Timer>("collapse_timer").Start();
    }

    public void OnCollaspeTimerTimeout()
    {
        GD.Print("CollaspeTimerTimeout!");
        CollaspeColumns();
        GetParent().GetNode<Timer>("refill_timer").Start();
    }

    public void OnRefillTimerTimeout()
    {
        GD.Print("RefillTimerTimeout!");
        RefillColumns();
    }

    private void DestroyMatched()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Piece piece = (Piece)AllPieces[x, y];
                if (piece != null)
                {
                    if (piece.IsMatched())
                    {
                        piece.QueueFree();
                        AllPieces[x, y] = null;
                        makeEffect(particleEffect, new Vector2i(x, y));
                        makeEffect(animatedEffect, new Vector2i(x, y));
                        EmitSignal(nameof(UpdateScore), piece_value * streak);
                    }
                }

            }
        }
    }

    private void makeEffect(PackedScene effect, Vector2i pos)
    {
        Node2D current = (Node2D)effect.Instance();
        current.Position = GridToPixel(pos);
        AddChild(current);
    }

    private void CollaspeColumns()
    {
        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height; row++)
            {
                if (AllPieces[col, row] == null)
                {
                    for (int collaspe_idx = row + 1; collaspe_idx < height; collaspe_idx++)
                    {
                        Piece piece = (Piece)AllPieces[col, collaspe_idx];
                        if (piece != null)
                        {
                            piece.Move(GridToPixel(new Vector2i(col, row)));
                            AllPieces[col, row] = AllPieces[col, collaspe_idx];
                            AllPieces[col, collaspe_idx] = null;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void RefillColumns()
    {
        streak++;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (AllPieces[x, y] == null)
                {
                    int piece_idx = 0;
                    int loops = 0;
                    Piece piece = null;
                    do
                    {
                        if (++loops == 100)
                        {
                            break;
                        }
                        piece_idx = (int)Math.Floor(GD.RandRange(0, PossiblePieces.Length));
                        piece = (Piece)PossiblePieces[piece_idx].Instance();
                    } while (MatchAt(x, y, (String)piece.Get("colour")));
                    piece.Position = GridToPixel(new Vector2i(x, y + refill_y_offset));
                    AddChild(piece);
                    AllPieces[x, y] = piece;
                    piece.Move(GridToPixel(new Vector2i(x, y)));
                }
            }
        }
        CheckMatchesPostRefill();
    }

    private void CheckMatchesPostRefill()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Piece piece = (Piece)AllPieces[x, y];
                if (piece != null)
                {
                    if (MatchAt(x, y, (String)piece.Get("colour")))
                    {
                        FindMatches();
                        GetParent().GetNode<Timer>("destroy_timer").Start();
                        return;
                    }
                }
            }
        }
        state_ = State.MOVE;
        streak = 1;
    }

}
