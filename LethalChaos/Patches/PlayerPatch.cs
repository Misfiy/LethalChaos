using GameNetcodeStuff;
using HarmonyLib;

namespace LethalChaos.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerPatch
{
    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    private static void OnUpdated(PlayerControllerB __instance)
    {
        __instance.movementSpeed = SpeedBoost.IsActivated ? 7.5f : 4.5f;
        __instance.climbSpeed = SpeedBoost.IsActivated ? 9 : 3;
    }
}