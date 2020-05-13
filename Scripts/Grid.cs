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

    public override void _Ready()
    {
        GD.Randomize();
        GD.Print("_Ready()");
        AllPieces = new Node2D[width, height];
        SpawnPieces();
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
            FirstTouch = GetGlobalMousePosition();
            Vector2i gridPos = PixelToGrid(FirstTouch);
            if (IsInGrid(gridPos))
            {
                controllingPiece = true;
            }
        }
        if (Input.IsActionJustReleased("ui_touch"))
        {
            FinalTouch = GetGlobalMousePosition();
            Vector2i gridPos = PixelToGrid(FinalTouch);
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
        return new Vector2i((int)Mathf.Round((pixelPos.x - x_start) / offset), (int)Mathf.Round((pixelPos.y - y_start) / -offset));
    }

    public override void _Process(float delta)
    {
        TouchInput();
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
        Node2D first_piece = AllPieces[pos.X, pos.Y];
        Node2D other_piece = AllPieces[otherPos.X, otherPos.Y];
        AllPieces[pos.X, pos.Y] = other_piece;
        AllPieces[otherPos.X, otherPos.Y] = first_piece;
        // first_piece.Position = GridToPixel(otherPos);
        // other_piece.Position = GridToPixel(pos);
        ((Piece)first_piece).Move(GridToPixel(otherPos));
        ((Piece)other_piece).Move(GridToPixel(pos));
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
}
