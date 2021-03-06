using Godot;
using System;

public class GameOverPanel : BaseMenuPanel
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    public void OnButtonQuit()
    {
        GetTree().ChangeScene("res://Scenes/GameMenu.tscn");
    }

    public void OnButtonRestart()
    {
        GetTree().ReloadCurrentScene();
    }

    public void OnGameOver()
    {
        SlideIn();
    }
}
