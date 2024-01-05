namespace LethalChaos;

public class SpeedBoost : EventBase
{
    public static bool IsActivated;
    
    public override string Text { get; } = "Speed Boost";
    public override Phase Phase { get; } = Phase.MoonLanding;

    public override void Activate()
    {
        IsActivated = true;
    }
}