using HarmonyLib;
using GorillaTagScripts;

namespace GorillaGhostVision.Patches
{
    [HarmonyPatch(typeof(GorillaAmbushManager), nameof(GorillaAmbushManager.UpdatePlayerAppearance))]
    public class GhostTagPatch
    {
        public static void Prefix(GorillaAmbushManager __instance, VRRig rig)
        {
            if (__instance.isGhostTag)
            {
                rig.SetInvisibleToLocalPlayer(false);
            }
        }
    }
}
