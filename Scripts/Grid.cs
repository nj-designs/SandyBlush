using Godot;
using System;

public class Grid : Node2D
{
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


	private PackedScene[] PossiblePieces = new PackedScene[] {
		(PackedScene)GD.Load("res://Scenes/yellow_piece.tscn"),
		(PackedScene)GD.Load("res://Scenes/blue_piece.tscn"),
		(PackedScene)GD.Load("res://Scenes/pink_piece.tscn"),
		(PackedScene)GD.Load("res://Scenes/orange_piece.tscn"),
		(PackedScene)GD.Load("res://Scenes/green_piece.tscn"),
		(PackedScene)GD.Load("res://Scenes/light_green_piece.tscn"),
	};

	private Node2D[,] AllPieces;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
		GD.Print("_Ready()");
		AllPieces = new Node2D[width, height];
		SpawnPieces();
	}

	private Vector2 GridToPixel(int column, int row) {
		return new Vector2(x_start + (offset * column), y_start - (offset * row));
	}

	private void SpawnPieces() {
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++) {
				int piece_idx = 0;
				int loops = 0;
				Node2D piece = null;
				do {
					if(++loops == 100) {
						break;
					}
					piece_idx = (int)Math.Floor(GD.RandRange(0, PossiblePieces.Length));
					piece = (Node2D)PossiblePieces[piece_idx].Instance();
				} while(MatchAt(x, y, (String)piece.Get("colour")));
				piece.Position = GridToPixel(x, y);
				AddChild(piece);
				AllPieces[x, y] = piece;
			}
		}
	}

	private bool MatchAt(int column, int row, String colour) {
		// Check Left
		if(column > 1) {
			if (AllPieces[column - 1, row] != null && AllPieces[column -2, row] != null) {
				if ((String)AllPieces[column -1, row].Get("colour") == colour && (String)AllPieces[column -2, row].Get("colour") == colour) {
					return true;
				}
			}
		}
		// Check Down
		if(row > 1 ) {
			if (AllPieces[column , row - 1] != null && AllPieces[column, row - 2] != null) {
				if ((String)AllPieces[column, row - 1].Get("colour") == colour && (String)AllPieces[column, row - 2].Get("colour") == colour) {
					return true;
				}
			}
		}
		return false;
	}


}
