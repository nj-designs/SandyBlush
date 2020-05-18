using Godot;
using System;

public class MainMenuPanel : BaseMenuPanel
{
    [Signal]
    delegate void PlayPressed();

    [Signal]
    delegate void SettingsPressed();

    public void OnButton1Pressed()
    {
        GD.Print("MainMenuPanel.OnButton1Pressed()");
        EmitSignal(nameof(PlayPressed));
    }

    public void OnButton2Pressed()
    {
        GD.Print("MainMenuPanel.OnButton2Pressed()");
        EmitSignal(nameof(SettingsPressed));
    }
}
