using Godot;
using System;

public class Grid : Node2D
{
	// Grid Variables
	[Export]
	private int width;
	[Export]
	private int height;
	[Export]
	private int x_start;
	[Export]
	private int y_start;
	[Export]
	private int offset;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
