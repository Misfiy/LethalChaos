using HarmonyLib;

namespace LethalChaos.Patches;

[HarmonyPatch(typeof(LungProp))]
public class LungPropPatch
{
    [HarmonyPatch("DisconnectFromMachinery")]
    [HarmonyPrefix]
    private static void DisconnectPrefix(LungProp __instance)
    {
        __instance.scrapValue = (int)(__instance.scrapValue * Utility.CurrentScrapValueMultiplier);
    }
}