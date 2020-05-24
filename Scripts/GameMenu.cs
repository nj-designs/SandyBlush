using Godot;
using System;

public class GameMenu : Control
{
    private MainMenuPanel mainMenuPanel;
    private SettingsPanel settingsPanel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mainMenuPanel = GetNode<MainMenuPanel>("Main");
        settingsPanel = GetNode<SettingsPanel>("Settings");
        mainMenuPanel.SlideIn();
    }

    public void OnSettingsPressed()
    {
        mainMenuPanel.SlideOut();
        settingsPanel.SlideIn();
    }

    public void OnSettingsBackPressed()
    {
        settingsPanel.SlideOut();
        mainMenuPanel.SlideIn();
    }

    public void OnPlayButtonPressed()
    {
        // GetTree().ChangeScene("res://Scenes/game_window.tscn");
        GetTree().ChangeScene("res://Scenes/LevelSelect.tscn");
    }
}
