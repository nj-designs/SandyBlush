using Godot;
using System;

public class SettingsPanel : BaseMenuPanel
{
    [Signal]
    delegate void SoundChanged();

    [Signal]
    delegate void BackButton();

    public void OnButton1Pressed()
    {
        GD.Print("SettingsPanel.OnButton2Pressed()");
        EmitSignal(nameof(SoundChanged));
    }

    public void OnButton2Pressed()
    {
        GD.Print("SettingsPanel.OnButton2Pressed()");
        EmitSignal(nameof(BackButton));
    }
}
