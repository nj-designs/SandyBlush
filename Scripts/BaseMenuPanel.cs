using Godot;

public class BaseMenuPanel : CanvasLayer
{

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void SlideIn()
    {
        animationPlayer.Play("SlideIn");
    }

    public void SlideOut()
    {
        animationPlayer.PlayBackwards("SlideIn");
    }
}
