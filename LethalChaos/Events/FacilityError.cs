using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LethalChaos;

public class FacilityError : EventBase
{
    public override string Text { get; } = "Facility Error";
    public override Phase Phase { get; } = Phase.Exploration;
    public override void Activate()
    {
        RoundManager.Instance.StartCoroutine(DoActivation());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DoActivation()
    {
        Landmine[] landmines = Object.FindObjectsOfType<Landmine>();
        Turret[] turrets = Object.FindObjectsOfType<Turret>();
        
        foreach (Landmine landmine in landmines)
            landmine.ToggleMine(false);
        
        foreach (Turret turret in turrets)
            turret.ToggleTurretEnabled(false);
        
        RoundManager.Instance.TurnOnAllLights(false);

        yield return new WaitForSeconds(15);
        
        foreach (Landmine landmine in landmines)
            landmine.ToggleMine(true);

        foreach (Turret turret in turrets)
            turret.ToggleTurretEnabled(true);
        
        RoundManager.Instance.TurnOnAllLights(true);
        
        isUsed = false;
    }
}