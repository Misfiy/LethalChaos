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
        __instance.movementSpeed = SpeedBoost.IsActivated ? 9 : 4.5f;
        __instance.climbSpeed = SpeedBoost.IsActivated ? 8 : 3;

        if (SpeedBoost.IsActivated)
        {
            __instance.sprintMeter = 1;
        }
    }
}