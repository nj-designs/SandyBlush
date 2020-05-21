using Godot;
using System;

public class Goal : Node
{
    // Goal Information
    [Export]
    public Texture GoalTexture;
    [Export]
    public int MaxNeeded;
    [Export]
    public String GoalString;

    private int numberCollected = 0;

    public void CheckGoal(String goalType)
    {
        if (goalType == GoalString)
        {
            UpdateGoal();
        }
    }

    private void UpdateGoal()
    {
        if (numberCollected < MaxNeeded)
        {
            numberCollected++;
        }
        if (numberCollected == MaxNeeded)
        {
            GD.Print("Goal Met");
        }
    }
}
