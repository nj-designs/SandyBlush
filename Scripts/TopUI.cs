using Godot;
using System;

public class TopUI : TextureRect
{
    private Label scoreLabel;
    private Label counterLabel;

    private int currentScore = 0;
    //private int current_count = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreLabel = (Label)GetParent().FindNode("score_label");
        counterLabel = (Label)GetParent().FindNode("CounterLabel");

        OnUpdateScore(0);
    }

    void OnUpdateScore(int score_delta)
    {
        GD.Print(String.Format("Update score {0}", score_delta));
        currentScore += score_delta;
        scoreLabel.Text = currentScore.ToString();
    }

    void OnUpdateCounter(int currentCnt)
    {
        // current_count += cnt_delta;
        counterLabel.Text = currentCnt.ToString();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
