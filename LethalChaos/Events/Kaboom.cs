using Object = UnityEngine.Object;

namespace LethalChaos;

public class Kaboom : EventBase
{
    public override string Text { get; } = "Kaboom!";
    public override Phase Phase { get; } = Phase.Exploration;
    public override void Activate()
    {
        foreach (Landmine landmine in Object.FindObjectsOfType<Landmine>())
            landmine.Detonate();
    }
}