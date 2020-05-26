using Godot;
using System;

public class SettingsMgr : Node
{

    private const string _cfgPath = "user://config.ini";

    // Audio Section
    private const string _audioSectionName = "audio";
    private const string _sfxOnKey = "sfxOn";
    private bool _sfxOn = true;
    private const string _musicOnKey = "musicOn";
    private bool _musicOn = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        ConfigFile cfgFile = new ConfigFile();
        Error err = cfgFile.Load(_cfgPath);
        if (err != Error.Ok)
        {
            GD.Print(string.Format("LoadConfig() failed. File:{0} Err:{1}",
                _cfgPath, err.ToString()));
            SaveConfig();
        }
    }

    private void SaveConfig()
    {
        ConfigFile cfgFile = new ConfigFile();
        cfgFile.SetValue(_audioSectionName, _sfxOnKey, _sfxOn);
        cfgFile.SetValue(_audioSectionName, _musicOnKey, _musicOn);

        Error err = cfgFile.Save(_cfgPath);
        GD.Print(string.Format("SaveConfig() File:{0} Err:{1}", _cfgPath, err.ToString()));
    }
}
