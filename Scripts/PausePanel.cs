using Godot;
using System;

public class PausePanel : BaseMenuPanel
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    public void OnSigQuitButton()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Scenes/GameMenu.tscn");
        //GetTree().Quit();
    }

    public void OnSigContinueButton()
    {
        GetTree().Paused = false;
        SlideOut();
    }

    public void OnSigPauseGame()
    {
        SlideIn();
    }
}
