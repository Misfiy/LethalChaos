using System.Collections;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LethalChaos.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    private static Coroutine? _coroutine;

    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    [HarmonyPrefix]
    private static void OnLevelLoad(RoundManager __instance, ref SelectableLevel newLevel)
    {
        Plugin.Logging.LogInfo("Level Load: " + newLevel.PlanetName);
        
        foreach (SpawnableMapObject spawnableMapObject in RoundManager.Instance.currentLevel.spawnableMapObjects)
        {
            if (spawnableMapObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null)
                Variables.Landmine = spawnableMapObject.prefabToSpawn;
        }
        
        EventBase.ResetUses();
        
        EventBase.CurrentPhase = newLevel.PlanetName.Contains("Gordi") ? Phase.Company : Phase.Exploration;
        
        if (_coroutine != null) 
            __instance.StopCoroutine(_coroutine);

        Plugin.Logging.LogInfo(EventBase.CurrentPhase);
        if (EventBase.CurrentPhase != Phase.Exploration || EventBase.CurrentPhase == Phase.Company)
            return;
        
        _coroutine = __instance.StartCoroutine(EventCoroutine(newLevel, EventBase.CurrentPhase));
    }

    [HarmonyPatch(nameof(RoundManager.SpawnScrapInLevel))]
    [HarmonyPrefix]
    private static void StartScrapSpawn(RoundManager __instance)
    {
        __instance.scrapAmountMultiplier *= Utility.CurrentScrapAmountMultiplier;
        __instance.scrapValueMultiplier *= Utility.CurrentScrapValueMultiplier;
    }

    [HarmonyPatch(nameof(RoundManager.SpawnScrapInLevel))]
    [HarmonyPostfix]
    private static void FinishedScrapSpawn(RoundManager __instance)
    {
        __instance.scrapAmountMultiplier /= Utility.CurrentScrapAmountMultiplier;
        __instance.scrapValueMultiplier /= Utility.CurrentScrapValueMultiplier;
        
        foreach (EventBase ev in EventBase.GetFromPhase(EventBase.CurrentPhase == Phase.Company ? Phase.Company : Phase.MoonLanding))
        {
            if (Random.Range(1, 100) > 25) continue;
            ev.Activate();
            ev.isUsed = true;
            HUDManager.Instance.AddTextToChatOnServer($"{ev.Text} has been triggered!");
            Plugin.Logging.LogInfo($"{ev.Text} has been triggered!");
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private static IEnumerator EventCoroutine(SelectableLevel level, Phase phase)
    {
        Plugin.Logging.LogInfo("Starting EventCoroutine");

        while (RoundManager.Instance.currentLevel == level && EventBase.CurrentPhase == phase)
        {
            yield return new WaitForSeconds(30);
            Plugin.Logging.LogInfo("Running random event..");
            
            EventBase ev = EventBase.GetRandomForPhase();

            if (ev != null)
            {
                ev.Activate();
                ev.isUsed = true;
            
                Plugin.Logging.LogInfo($"{ev.Text} has been triggered!");
                HUDManager.Instance.DisplayTip("Random Event!", $"{ev.Text} has been triggered!", true);
            }
        }

        Plugin.Logging.LogInfo("Finished EventCoroutine");
    }
}