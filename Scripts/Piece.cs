using Godot;
using System;

public class Piece : Node2D
{

    [Export]
    public String colour;

    protected Tween move_tween;

    private bool isMatched_ = false;

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
        "position", Position, target, 0.3f, Tween.TransitionType.Sine, Tween.EaseType.In);
        move_tween.Start();
    }

    private void Dim()
    {
        Sprite sprite = (Sprite)GetNode("Sprite");
        sprite.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }

    public void SetMatched(bool isMatched)
    {
        isMatched_ = isMatched;
        if (isMatched)
        {
            Dim();
        }
    }

    public bool IsMatched()
    {
        return isMatched_;
    }

}
