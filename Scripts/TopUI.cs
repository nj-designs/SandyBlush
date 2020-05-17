using Godot;
using System;

public class TopUI : TextureRect
{
    private Label scoreLabel;

    private int currentScore = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreLabel = (Label)GetParent().FindNode("score_label");
        OnUpdateScore(0);
    }

    void OnUpdateScore(int score_delta)
    {
        GD.Print(String.Format("Update score {0}", score_delta));
        currentScore += score_delta;
        scoreLabel.Text = currentScore.ToString();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
