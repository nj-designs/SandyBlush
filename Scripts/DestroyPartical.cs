using Godot;
using System;

public class DestroyPartical : Particles2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Emitting = true;
    }

    public void OnTimerTimeout()
    {
        QueueFree();
    }

}
