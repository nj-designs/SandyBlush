using Godot;
using System;

public class GoalHolder : Node
{
    [Signal]
    delegate void SigCreateGoals(int max, Texture texture, String value);

    [Signal]
    delegate void SigGameWon();

    public override void _Ready()
    {
        CreateGoals();
    }

    private void checkGoals(String goalType)
    {
        foreach (Goal goal in GetChildren())
        {
            goal.CheckGoal(goalType);
        }
        CheckGameWin();
    }

    public void OnSigCheckGoal(String goalType)
    {
        GD.Print("OnSigCheckGoal: ", goalType);
        checkGoals(goalType);
    }

    public void CreateGoals()
    {
        foreach (Goal goal in GetChildren())
        {
            EmitSignal(nameof(SigCreateGoals), goal.MaxNeeded, goal.GoalTexture, goal.GoalString);
        }
    }

    public void CheckGameWin()
    {
        if (IsGoalsMet())
        {
            GD.Print("you Win!!");
            EmitSignal(nameof(SigGameWon));
        }
    }

    public bool IsGoalsMet()
    {
        foreach (Goal goal in GetChildren())
        {
            if (goal.isGoalMet() == false)
            {
                return false;
            }

        }
        return true;
    }
}
