using Godot;
using System;

public class AnimatedExplosion : Node2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimatedSprite animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animatedSprite.Playing = true;
    }

    public void OnAnimatedSpriteAnimationFinished()
    {
        QueueFree();
    }
}
