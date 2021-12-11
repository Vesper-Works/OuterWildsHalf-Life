using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HalfLifeOverhaul
{
    static class MeshPatcher
    {
        private static bool _loaded = false;

        public static GameObject ickyPrefab;
        public static GameObject hevPrefab;
        public static GameObject nihilanthPrefab;
        public static GameObject tauCannonPrefab;
        public static GameObject vortiguantPrefab;
        public static GameObject headcrabPrefab;
        public static GameObject[] scientistPrefabs = new GameObject[4];
        public static GameObject gmanPrefab;
        public static GameObject barneyPrefab;
        public static GameObject suitGordonPrefeb;
        public static GameObject gordonPrefab;

        private static Shader standardShader = Shader.Find("Standard");

        public static void OnStart()
        {
            if (_loaded) return;

            var bundle = MainBehaviour.instance.ModHelper.Assets.LoadBundle("hlmodels");

            ickyPrefab = LoadPrefab(bundle, "Assets/Prefabs/icky.prefab");
            hevPrefab = LoadPrefab(bundle, "Assets/Prefabs/HEV_suit.prefab");
            nihilanthPrefab = LoadPrefab(bundle, "Assets/Prefabs/nihilanth.prefab");
            tauCannonPrefab = LoadPrefab(bundle, "Assets/Prefabs/tau_cannon.prefab");
            vortiguantPrefab = LoadPrefab(bundle, "Assets/Prefabs/vortiguant.prefab");
            headcrabPrefab = LoadPrefab(bundle, "Assets/Prefabs/headcrab.prefab");
            scientistPrefabs[0] = LoadPrefab(bundle, "Assets/Prefabs/scientist1.prefab"); // "Einstein"
            scientistPrefabs[1] = LoadPrefab(bundle, "Assets/Prefabs/scientist2.prefab"); // "Luther"
            scientistPrefabs[2] = LoadPrefab(bundle, "Assets/Prefabs/scientist3.prefab"); // "Nerd"
            scientistPrefabs[3] = LoadPrefab(bundle, "Assets/Prefabs/scientist4.prefab"); // "Slick"
            barneyPrefab = LoadPrefab(bundle, "Assets/Prefabs/barney.prefab");
            gordonPrefab = LoadPrefab(bundle, "Assets/Prefabs/gordon_nosuit.prefab");
            suitGordonPrefeb = LoadPrefab(bundle, "Assets/Prefabs/gordon_suit.prefab");
            gmanPrefab = LoadPrefab(bundle, "Assets/Prefabs/gman.prefab");

            // TODO: Replace solanum with gman

            // TODO: Replace owl mummies with vortigaunts

            // TODO: Put headcrabs on the traveller's heads

            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<JellyfishController>("Awake", typeof(MeshPatcher), nameof(MeshPatcher.OnJellyFishControllerAwake));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<AnglerfishController>("Awake", typeof(MeshPatcher), nameof(MeshPatcher.OnAnglerfishControllerAwake));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<GhostController>("Initialize", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostControllerInitialize));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<GhostAction>("EnterAction", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostActionEnterAction));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<GhostBrain>("Die", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostBrainDie));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPrefix<GhostEffects>("SetEyeGlow", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostEffectsSetEyeGlow));

            LoadManager.OnCompleteSceneLoad += MeshPatcher.PatchMeshes;

            _loaded = true;
        }

        private static GameObject LoadPrefab(AssetBundle bundle, string path)
        {
            var prefab = bundle.LoadAsset<GameObject>(path);

            // Repair materials             
            foreach(var skinnedMeshRenderer in prefab.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                foreach (var mat in skinnedMeshRenderer.materials)
                {
                    mat.shader = standardShader;
                    mat.renderQueue = 2000;
                }
            }

            prefab.SetActive(false);

            return prefab;
        }

        public static void PatchMeshes(OWScene _, OWScene currentScene)
        {
            if (currentScene != OWScene.SolarSystem) return;

            ReplaceJellyfish();
            ReplaceAnglerfish();
            ReplaceShip();
            ReplaceTools();
        }

        private static void ReplaceJellyfish()
        {
            // Bramble island frozen jellyfish
            var brambleIsland = GameObject.Find("BrambleIsland_Body/Sector_BrambleIsland/Geo_BrambleIsland/BatchedGroup");
            GameObject.Destroy(brambleIsland.transform.Find("BatchedMeshRenderers_21").gameObject);
            GameObject.Destroy(brambleIsland.transform.Find("BatchedMeshRenderers_22").gameObject);
            GameObject islandNihilanth = GameObject.Instantiate(nihilanthPrefab, brambleIsland.transform);
            islandNihilanth.transform.localPosition = new Vector3(-35, 25, 22);
            islandNihilanth.transform.localScale = Vector3.one * 0.02f;
            islandNihilanth.transform.localRotation = new Quaternion(-0.15f, -0.69f, 0f, 0.70f);
            GameObject.Destroy(islandNihilanth.GetComponent<Animator>());
            islandNihilanth.SetActive(true);

            // Dark bramble frozen jellyfish
            GameObject brambleFrozen = GameObject.Find("DarkBramble_Body/Sector_DB/Geometry_DB/OtherComponentsGroup");
            var brambleFrozenJelly = brambleFrozen.transform.Find("Beast_DB_FrozenJellyfish_v3").gameObject;
            GameObject brambleNihilanth = GameObject.Instantiate(nihilanthPrefab, brambleFrozen.transform);
            brambleNihilanth.transform.localPosition = new Vector3(-11f, -551.3993f, 1.12f);
            brambleNihilanth.transform.localScale = Vector3.one * 0.02f;
            brambleNihilanth.transform.localRotation = new Quaternion(0.0647f, 0.7471f, -0.6575f, 0.0735f);
            brambleNihilanth.GetComponent<Animator>().speed = 0f; // Should pause the animation but keep the head open
            brambleNihilanth.SetActive(true);
            GameObject.Destroy(brambleFrozenJelly);

            // Quantum moon frozen jellyfish
            GameObject quantumMoonFrozen = GameObject.Find("QuantumMoon_Body/Sector_QuantumMoon/State_DB/Interactables_DBState/DBState_QuantumObjects/DB_JellyfishinIce/Jelly_Rock_Geo/OtherComponentsGroup");
            GameObject jelly = quantumMoonFrozen.transform.Find("Beast_GD_Jellyfish_v4").gameObject;
            GameObject quantumNihilanth = GameObject.Instantiate(nihilanthPrefab, quantumMoonFrozen.transform);
            quantumNihilanth.transform.localPosition = new Vector3(0.78f, 11.27f, 1.27f);
            quantumNihilanth.transform.localScale = Vector3.one * 0.01f;
            quantumNihilanth.transform.localRotation = jelly.transform.localRotation;
            GameObject.Destroy(quantumNihilanth.GetComponent<Animator>());
            quantumNihilanth.SetActive(true);
            GameObject.Destroy(jelly);
        }

        private static void ReplaceAnglerfish()
        {
            // Museum angler fish
            GameObject tank = GameObject.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/AnglerFishTankPivot").gameObject;
            GameObject tankFish = tank.transform.Find("Beast_Anglerfish").gameObject;
            GameObject tankIcky = GameObject.Instantiate(ickyPrefab, tank.transform);
            tankIcky.transform.localPosition = new Vector3(-0.2f, 2f, -6.6f);
            tankIcky.transform.localRotation = tankFish.transform.localRotation;
            tankIcky.transform.localScale = tankFish.transform.localScale * 1.5f;
            tankIcky.GetComponent<Animator>().SetBool("Lurking", true);
            tankIcky.SetActive(true);
            GameObject.Destroy(tankFish.transform.Find("Beast_Anglerfish").gameObject);
        }

        private static void ReplaceShip()
        {
            // HEV suit
            var hangingSuits = new GameObject[]
            {
                GameObject.Find("Ship_Body/Module_Supplies/Systems_Supplies/ExpeditionGear/EquipmentGeo/Props_HEA_PlayerSuit_Hanging"),
                GameObject.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Interactables_ZeroGCave/SpaceSuit/Props_HEA_PlayerSuit_Hanging")
            };
            foreach(var hangingSuit in hangingSuits)
            {
                foreach (var mr in hangingSuit.GetComponentsInChildren<MeshRenderer>())
                {
                    GameObject.Destroy(mr);
                }
                var jacket = hangingSuit.transform.Find("PlayerSuit_Jacket").gameObject;
                var HEVsuit = GameObject.Instantiate(hevPrefab, jacket.transform);
                HEVsuit.transform.localPosition = new Vector3(-0.3f, -1f, 0f);
                HEVsuit.transform.localScale = Vector3.one * 0.025f;
                HEVsuit.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.up);
                HEVsuit.SetActive(true);
            }

            // Put a headcrab on the shelf why not
            var shelf = GameObject.Find("Ship_Body/Module_Supplies/Systems_Supplies");
            var headcrab = GameObject.Instantiate(headcrabPrefab, shelf.transform);
            headcrab.name = "Lamarr";
            headcrab.transform.localPosition = new Vector3(2.34f, 1.86f, -1.1f);
            headcrab.transform.localScale = Vector3.one * 0.022f;
            headcrab.transform.localRotation = Quaternion.AngleAxis(-35, Vector3.up);
            headcrab.SetActive(true);

            // TODO: Replace healing station with one from half life

            // TODO: Replace probe and probe launcher on shelves with half life versions
        }

        private static void ReplaceTools()
        {
            // Tau cannon
            var signalscope = GameObject.Find("Player_Body/PlayerCamera/Signalscope/Props_HEA_Signalscope");
            var signalscopePrepass = GameObject.Find("Player_Body/PlayerCamera/Signalscope/Props_HEA_Signalscope/Props_HEA_Signalscope_Prepass");
            GameObject.Destroy(signalscope.GetComponent<MeshRenderer>());
            GameObject.Destroy(signalscopePrepass.GetComponent<MeshRenderer>());
            var tauCannon = GameObject.Instantiate(tauCannonPrefab, signalscope.transform);
            tauCannon.transform.localPosition = new Vector3(-0.14f, -0.15f, 0.15f);
            tauCannon.transform.localScale = Vector3.one * 0.012f;
            tauCannon.SetActive(true);

            // TODO: Probe launcher to rocket launcher

            // TODO: Translator to... something

            // TODO: Replace props in the word that have these models
        }


        #region Jellyfish patches
        private static void OnJellyFishControllerAwake(JellyfishController __instance)
        {
            var jellyfish = __instance.gameObject;

            var nihilanth = GameObject.Instantiate(nihilanthPrefab, jellyfish.transform);
            nihilanth.transform.localPosition = new Vector3(0, 11.5f, 0);
            nihilanth.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            nihilanth.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.right);
            nihilanth.SetActive(true);

            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_fluff").gameObject);
            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_inner").gameObject);
            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_innerskirt").gameObject);
            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_legs").gameObject);
            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_midskirt").gameObject);
            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_top").gameObject);
            GameObject.Destroy(jellyfish.transform.Find("Beast_GD_Jellyfish_v4/jellyfish_topskirt").gameObject);
        }
        #endregion Jellyfish patches

        #region Anglerfish patches
        private static void OnAnglerfishControllerAwake(AnglerfishController __instance)
        {
            var anglerfish = __instance.gameObject;

            foreach (var toDestroy in anglerfish.GetComponentsInChildren<Renderer>())
            {
                GameObject.Destroy(toDestroy);
            }

            var icky = GameObject.Instantiate(ickyPrefab, anglerfish.transform);
            icky.name = "Icky";
            icky.transform.localPosition = Vector3.zero;
            icky.transform.localScale = Vector3.one * 2f;
            icky.AddComponent<IckyController>();
            icky.SetActive(true);
        }
        #endregion Anglerfish patches

        #region Vortiguant patches
        private static void OnGhostControllerInitialize(GhostController __instance)
        {
            var ghostBird = __instance.gameObject;

            var vortiguant = GameObject.Instantiate(vortiguantPrefab, ghostBird.transform);
            vortiguant.transform.localPosition = Vector3.back * 0.2f;
            vortiguant.transform.localScale = Vector3.one * 0.05f;
            vortiguant.name = "Vortiguant";
            vortiguant.AddComponent<VortiguantController>();
            vortiguant.SetActive(true);

            // Need to disappear this but doing it breaks a lot of stuff
            foreach(var r in __instance.transform.Find("Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP").GetComponentsInChildren<Renderer>())
            {
                GameObject.Destroy(r);                
            }
            foreach (var owr in __instance.transform.Find("Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP").GetComponentsInChildren<OWRenderer>())
            {
                GameObject.Destroy(owr);
            }
        }

        private static void OnGhostActionEnterAction(GhostAction __instance, GhostController ____controller)
        {
            var vortiguantController = ____controller.gameObject.GetComponentInChildren<VortiguantController>();
            vortiguantController.OnChangeAction(__instance.GetName());
        }

        private static void OnGhostBrainDie(GhostBrain __instance)
        {
            var vortiguantController = __instance.gameObject.GetComponentInChildren<VortiguantController>();
            vortiguantController.OnDie();
        }

        private static bool OnGhostEffectsSetEyeGlow(GhostEffects __instance, float __0, GhostController ____controller)
        {
            ____controller.gameObject.GetComponentInChildren<VortiguantController>().SetEyeGlow(__0);

            return false;
        } 
        #endregion
    }
}
