using Godot;
using System;

public class LevelButton : Node2D
{
    // Level Stuff
    [Export]
    public int LevelNumber;
    [Export]
    public String LevelToLoad;
    [Export]
    public bool LevelEnabled;
    [Export]
    public bool ScoreGoalMet;

    // Texture Stuff
    [Export]
    public Texture BlockedTexture;
    [Export]
    public Texture OpenTexture;
    [Export]
    public Texture GoalMetTexture;
    [Export]
    public Texture GoalNotMetTexture;

    private Label label;
    private TextureButton button;
    private Sprite starSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        label = GetNode<Label>("TextureButton/Label");
        button = GetNode<TextureButton>("TextureButton");
        starSprite = GetNode<Sprite>("Sprite");
        Setup();
    }

    private void Setup()
    {
        label.Text = LevelNumber.ToString();
        button.TextureNormal = LevelEnabled ? OpenTexture : BlockedTexture;
        starSprite.Texture = ScoreGoalMet ? GoalMetTexture : GoalNotMetTexture;
    }

    public void OnButtonPressed()
    {
        if (LevelEnabled)
        {
            GetTree().ChangeScene(LevelToLoad);
        }
    }
}
