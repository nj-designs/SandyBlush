using Godot;
using System;

public class GoalPrefab : TextureRect
{
    int currentNumber;
    int maxValue;
    String goalValue = "";
    Texture goalTexture;

    // References to local nodes
    Label goalLabel;
    TextureRect textureRect;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        goalLabel = (Label)FindNode("GoalLabel");
        textureRect = (TextureRect)FindNode("GoalTextureRect");
    }

    public void SetGoalValues(int max, Texture texture, String value)
    {
        textureRect.Texture = texture;
        maxValue = max;
        goalValue = value;
        goalLabel.Text = String.Format("{0}/{1}", currentNumber, maxValue);
    }

    public void UpdateGoalValues(String colour)
    {
        if (colour == goalValue)
        {
            if (currentNumber < maxValue)
            {
                currentNumber++;
                goalLabel.Text = String.Format("{0}/{1}", currentNumber, maxValue);
            }
        }
    }
}
