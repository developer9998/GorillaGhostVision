using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using GorillaTagScripts;
using HarmonyLib;
using Utilla.Attributes;

namespace GorillaGhostVision
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0"), ModdedGamemode]
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource PluginLogSource;

        private Harmony harmonyInstance;

        private bool inModdedRoom;

        public void Awake()
        {
            PluginLogSource = Logger;
        }

        public void OnEnabled() => CheckValidity();

        public void OnDisabled() => CheckValidity();

        [ModdedGamemodeJoin]
        public void OnModdedJoined()
        {
            inModdedRoom = true;
            CheckValidity();
        }

        [ModdedGamemodeLeave]
        public void OnModdedLeft()
        {
            inModdedRoom = false;
            CheckValidity();
        }

        public void CheckValidity()
        {
            bool valid = enabled && inModdedRoom;
            if (valid)
            {
                harmonyInstance ??= Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, Constants.GUID);

                Refresh();
                return;
            }

            bool invalid = !enabled || !inModdedRoom;
            if (invalid)
            {
                if (harmonyInstance is not null)
                {
                    harmonyInstance.UnpatchSelf();
                    harmonyInstance = null;
                }

                Refresh();
                return;
            }
        }

        public void Refresh()
        {
            if (NetworkSystem.Instance.InRoom)
            {
                GorillaAmbushManager ambushManager = GorillaGameManager.instance is GorillaGameManager gameManager && gameManager is GorillaAmbushManager tempAmbushManager && tempAmbushManager.isGhostTag ? tempAmbushManager : null;

                List<VRRig> vrRigs = [];
                VRRigCache.Instance.GetAllUsedRigs(vrRigs);

                foreach(VRRig rig in vrRigs)
                {
                    if (!rig.isLocal && rig.TryGetComponent(out GRPlayer component))
                    {
                        component.RefreshPlayerVisuals();
                    }
                    ambushManager?.UpdatePlayerAppearance(rig);
                }
            }
        }
    }
}
