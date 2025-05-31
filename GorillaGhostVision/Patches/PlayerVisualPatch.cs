using HarmonyLib;

namespace GorillaGhostVision.Patches
{
    [HarmonyPatch(typeof(GRPlayer), nameof(GRPlayer.RefreshPlayerVisuals)), HarmonyWrapSafe]
    public class PlayerVisualPatch
    {
        public static void Prefix(GRPlayer __instance)
        {
            if (!__instance.vrRig.isLocal && __instance.state == GRPlayer.GRPlayerState.Ghost)
            {
                Plugin.PluginLogSource.LogInfo($"Enabling PlayerStatePatch for {__instance.vrRig.Creator.NickName} (always show skeleton)");

                PlayerStatePatch.OverrideLocalState = true;
            }
        }

        public static void Postfix(GRPlayer __instance)
        {
            if (!__instance.vrRig.isLocal && __instance.state == GRPlayer.GRPlayerState.Ghost)
            {
                Plugin.PluginLogSource.LogInfo($"Disabling PlayerStatePatch for {__instance.vrRig.Creator.NickName} (intended behaviour)");

                PlayerStatePatch.OverrideLocalState = false;
            }
        }
    }
}
