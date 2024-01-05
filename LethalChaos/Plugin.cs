using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LethalChaos;

[BepInPlugin(modGUID, modName, modVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string modGUID = "Misfiy.LethalChaos";
    private const string modName = "Lethal Chaos";
    private const string modVersion = "1.0.0";
    
    private readonly Harmony harmony = new(modGUID);

    public static ManualLogSource Logging { get; private set; } = null!;

    private void Awake()
    {
        Logging = Logger;
        
        EventBase.Register();
        
        harmony.PatchAll();
    }
}