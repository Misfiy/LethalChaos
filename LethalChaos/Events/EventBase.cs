using System.Reflection;

namespace LethalChaos;

public abstract class EventBase
{
    // public static Phase CurrentPhase => StartOfRound.Instance.inShipPhase
    //     ? RoundManager.Instance.currentLevel.PlanetName.Contains("Gordi") 
    //         ? Phase.Company 
    //         : Phase.Exploration 
    //     : Phase.Orbiting;

    public static Phase CurrentPhase = Phase.Orbiting;
    
    public bool isUsed;
    
    public abstract string Text { get; }
    public abstract Phase Phase { get; }
    public virtual bool IsUseable => !isUsed;
    public abstract void Activate();
    
    public static HashSet<EventBase> Events { get; } = new();

    public static List<EventBase> GetFromPhase(Phase phase) =>
        Events.Where(e => e.IsUseable && e.Phase == phase).ToList();
    
    public static EventBase GetRandomForPhase() => 
        Events.Where(e => e.IsUseable && e.Phase == CurrentPhase).ToList().GetRandom();


    public static void ResetUses()
    {
        foreach (EventBase ev in Events)
            ev.isUsed = false;

        SpeedBoost.IsActivated = false;
    }
    
    public static void Register()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        foreach (Type type in assembly.GetTypes())
        {
            if (type.BaseType != typeof(EventBase)) continue;

            EventBase eventBase = (EventBase)Activator.CreateInstance(type);
            Events.Add(eventBase);
        }
    }
}