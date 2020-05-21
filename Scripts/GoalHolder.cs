using Godot;
using System;

public class GoalHolder : Node
{
    private void checkGoals(String goalType)
    {
        foreach (Goal goal in GetChildren())
        {
            goal.CheckGoal(goalType);
        }
    }

    public void OnSigCheckGoal(String goalType)
    {
        GD.Print("OnSigCheckGoal: ", goalType);
        checkGoals(goalType);
    }
}
