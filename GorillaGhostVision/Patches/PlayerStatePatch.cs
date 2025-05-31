using HarmonyLib;

namespace GorillaGhostVision.Patches
{
    [HarmonyPatch(typeof(GRPlayer), "get_State")] // Crucial to patch out the getter of the State property, and not the state field (as if you could do that)
    public class PlayerStatePatch
    {
        public static bool OverrideLocalState = false;

        public static bool Prefix(GRPlayer __instance, ref GRPlayer.GRPlayerState __result)
        {
            if (OverrideLocalState && __instance.vrRig.isLocal)
            {
                Plugin.PluginLogSource.LogInfo("Local player state replaced with ghost state");

                __result = GRPlayer.GRPlayerState.Ghost;
                return false;
            }

            return true;
        }
    }
}
