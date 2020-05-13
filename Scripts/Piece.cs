using Godot;
using System;

public class Piece : Node2D
{
    protected Tween move_tween;

    [Export]
    public String colour;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        move_tween = (Tween)GetNode("move_tween");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    public void Move(Vector2 target)
    {
        move_tween.InterpolateProperty(this,
        "position", Position, target, 0.5f, Tween.TransitionType.Sine, Tween.EaseType.In);
        move_tween.Start();
    }
}
