using HalfLifeOverhaul.Controllers;
using HalfLifeOverhaul.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = HalfLifeOverhaul.Util.Logger;
using Random = UnityEngine.Random;

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
        public static GameObject[] vortigauntMummyPrefabs = new GameObject[11];
        public static GameObject rpgPrefab;
        public static GameObject satchelPrefab;
        public static GameObject protozoaPrefab;
        public static GameObject forkliftPrefab;
        public static GameObject barnaclePrefab;
        public static GameObject gusPrefab;
        public static GameObject crowbarPrefab;

        private static Shader standardShader = Shader.Find("Standard");
        private static Shader transparentShader = Shader.Find("Outer Wilds/Environment/Ship/Cockpit Glass");

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
            for(int i = 0; i < vortigauntMummyPrefabs.Length; i++)
            {
                vortigauntMummyPrefabs[i] = LoadPrefab(bundle, $"Assets/Prefabs/vortiguant_mummy{i+1}.prefab");
            }
            rpgPrefab = LoadPrefab(bundle, "Assets/Prefabs/rpg.prefab");
            satchelPrefab = LoadPrefab(bundle, "Assets/Prefabs/satchel.prefab");
            protozoaPrefab = LoadPrefab(bundle, "Assets/Prefabs/protozoa.prefab", true);
            forkliftPrefab = LoadPrefab(bundle, "Assets/Prefabs/forklift.prefab");
            barnaclePrefab = LoadPrefab(bundle, "Assets/Prefabs/barnacle.prefab");
            gusPrefab = LoadPrefab(bundle, "Assets/Prefabs/gus.prefab");
            crowbarPrefab = LoadPrefab(bundle, "Assets/Prefabs/crowbar.prefab");

            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<JellyfishController>("Awake", typeof(MeshPatcher), nameof(MeshPatcher.OnJellyFishControllerAwake));

            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<AnglerfishController>("Awake", typeof(MeshPatcher), nameof(MeshPatcher.OnAnglerfishControllerAwake));

            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<GhostController>("Initialize", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostControllerInitialize));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<GhostAction>("EnterAction", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostActionEnterAction));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<GhostBrain>("Die", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostBrainDie));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPrefix<GhostEffects>("SetEyeGlow", typeof(MeshPatcher), nameof(MeshPatcher.OnGhostEffectsSetEyeGlow));

            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("Awake", typeof(MeshPatcher), nameof(MeshPatcher.OnSolanumAnimControllerAwake));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("StartConversation", typeof(MeshPatcher), nameof(MeshPatcher.OnStartConversation));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("EndConversation", typeof(MeshPatcher), nameof(MeshPatcher.OnEndConversation));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("PlayGestureToWordStones", typeof(MeshPatcher), nameof(MeshPatcher.OnGesture));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("PlayGestureToCairns", typeof(MeshPatcher), nameof(MeshPatcher.OnGesture));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("StartWritingMessage", typeof(MeshPatcher), nameof(MeshPatcher.OnGesture));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("PlayRaiseCairns", typeof(MeshPatcher), nameof(MeshPatcher.OnRaiseCairns));
            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<SolanumAnimController>("StopWatchingPlayer", typeof(MeshPatcher), nameof(MeshPatcher.OnStopWatchingPlayer));

            MainBehaviour.instance.ModHelper.HarmonyHelper.AddPostfix<PlayerCharacterController>("Awake", typeof(MeshPatcher), nameof(MeshPatcher.OnPlayerCharacterControllerAwake));

            LoadManager.OnCompleteSceneLoad += MeshPatcher.PatchMeshes;

            _loaded = true;
        }

        private static GameObject LoadPrefab(AssetBundle bundle, string path, bool isTransparent = false)
        {
            GameObject prefab = null;
            try
            {
                prefab = bundle.LoadAsset<GameObject>(path);
                
                // Repair materials             
                foreach (var skinnedMeshRenderer in prefab.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    foreach (var mat in skinnedMeshRenderer.materials)
                    {
                        mat.shader = isTransparent? transparentShader : standardShader;
                        //mat.renderQueue = 2000;
                    }
                }
                

                prefab.SetActive(false);
            }
            catch(Exception)
            {
                Logger.LogWarning($"Couldn't load {path}");
            }


            return prefab;
        }

        #region PatchMeshes

        public static void PatchMeshes(OWScene _, OWScene currentScene)
        {
            if (currentScene != OWScene.SolarSystem) return;

            try
            {
                ReplaceJellyfish();
                ReplaceAnglerfish();
                ReplaceShip();
                ReplaceTools();
                ReplaceMummies();
                ReplaceProbes();
                PlaceBarnacles();
                PlaceProtozoa();
                PlaceGus();
                ReplaceAxe();
            }
            catch(Exception e)
            {
                Logger.LogError($"Couldn't finish patching meshes. {e.Message}, {e.StackTrace}");
            }
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

            /*
            var satchel = GameObject.Instantiate(satchelPrefab, shelf.transform);
            satchel.name = "Satchel";
            satchel.transform.localPosition = new Vector3(0, 0, 0);
            satchel.transform.localScale = Vector3.one * 0.022f;
            satchel.transform.localRotation = Quaternion.AngleAxis(-35, Vector3.up);
            satchel.SetActive(true);

            var rpg = GameObject.Instantiate(rpgPrefab, shelf.transform);
            rpg.name = "RPG";
            rpg.transform.localPosition = new Vector3(0, 0, 0);
            rpg.transform.localScale = Vector3.one * 0.022f;
            rpg.transform.localRotation = Quaternion.AngleAxis(-35, Vector3.up);
            rpg.SetActive(true);
            */
            // TODO: Replace healing station with one from half life

        }

        private static void ReplaceTools()
        {
            // Tau cannon
            var signalscopePrepass = GameObject.Find("Player_Body/PlayerCamera/Signalscope/Props_HEA_Signalscope/Props_HEA_Signalscope_Prepass");
            GameObject.Destroy(signalscopePrepass.GetComponent<MeshRenderer>());

            string[] signalscopes = new string[]
            {
                "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/OtherComponentsGroup/Village_UnderLaunchTowerProps/LaunchTowerSequoiaProps/WorkBench2/Props_HEA_Signalscope (1)",
                "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/OtherComponentsGroup/Village_UnderLaunchTowerProps/LaunchTowerSequoiaProps/WorkBench2/Props_HEA_Signalscope (2)",
            };

            foreach(var signalscopePath in signalscopes)
            {
                var signalscope = GameObject.Find(signalscopePath);
                GameObject.Destroy(signalscope.GetComponent<MeshRenderer>());
                foreach(var childMeshRenderer in signalscope.GetComponentsInChildren<MeshRenderer>())
                {
                    GameObject.Destroy(childMeshRenderer);
                }
                var tauCannon = GameObject.Instantiate(tauCannonPrefab, signalscope.transform);
                tauCannon.transform.localPosition = new Vector3(-0.15f, -0.1f, 0.3f);
                tauCannon.transform.localScale = Vector3.one * 0.012f;
                tauCannon.SetActive(true);
            }

            var playerSignalscope = GameObject.Find("Player_Body/PlayerCamera/Signalscope/Props_HEA_Signalscope");
            GameObject.Destroy(playerSignalscope.GetComponent<MeshRenderer>());
            foreach (var childMeshRenderer in playerSignalscope.GetComponentsInChildren<MeshRenderer>())
            {
                GameObject.Destroy(childMeshRenderer);
            }
            var playerTauCannon = GameObject.Instantiate(tauCannonPrefab, playerSignalscope.transform);
            playerTauCannon.transform.localPosition = new Vector3(-0.14f, -0.15f, 0.15f);
            playerTauCannon.transform.localScale = Vector3.one * 0.012f;
            playerTauCannon.SetActive(true);

            /*
            // Probe launcher to rocket launcher
            string[] probeLaunchers = new string[]
            {
                "TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Interactables_ScoutTutorial/TutorialProbeLauncher_Base/VerticalPivot/Launcher/Props_HEA_ProbeLauncher/",
                "TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/TutorialCamera_Base/VerticalPivot/Launcher/Props_HEA_ProbeLauncher/",
                "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/OtherComponentsGroup/Village_UnderLaunchTowerProps/LaunchTowerSequoiaProps/Props_HEA_ProbeLauncher (1)/",
                "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/OtherComponentsGroup/Village_UnderLaunchTowerProps/LaunchTowerSequoiaProps/Props_HEA_ProbeLauncher (2)/",
                "BrittleHollow_Body/Sector_BH/Sector_Crossroads/Props_Crossroads/OtherComponentsGroup/Reibeck_Camp/Props_HEA_ProbeLauncher/",
                "TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Props_ZeroGCave/OtherComponentsGroup/LowerCave/MinePlatForm_B/Props_HEA_ProbeLauncher/",
                //"Ship_Body/Module_Supplies/Systems_Supplies/ExpeditionGear/EquipmentGeo/Props_HEA_ProbeLauncher/",
                //"Player_Body/PlayerCamera/ProbeLauncher/Props_HEA_ProbeLauncher/",
            };

            foreach (var probeLauncherPath in probeLaunchers)
            {
                var probeLauncher = GameObject.Find(probeLauncherPath);
                GameObject.Destroy(probeLauncher.GetComponent<MeshRenderer>());
                foreach (var childMeshRenderer in probeLauncher.GetComponentsInChildren<MeshRenderer>())
                {
                    GameObject.Destroy(childMeshRenderer);
                }
                var rpg = GameObject.Instantiate(rpgPrefab, probeLauncher.transform);
                rpg.transform.localPosition = new Vector3(-0.14f, -0.15f, 0.15f);
                rpg.transform.localScale = Vector3.one * 0.012f;
                rpg.SetActive(true);
            }
            */

            // Player probe launcher
            var playerProbeLauncher = GameObject.Find("Player_Body/PlayerCamera/ProbeLauncher/Props_HEA_ProbeLauncher/");
            GameObject.Destroy(playerProbeLauncher.GetComponent<MeshRenderer>());
            foreach (var childMeshRenderer in playerProbeLauncher.GetComponentsInChildren<MeshRenderer>())
            {
                GameObject.Destroy(childMeshRenderer);
            }
            var playerRPG = GameObject.Instantiate(rpgPrefab, playerProbeLauncher.transform);
            playerRPG.transform.localPosition = new Vector3(-0.8f, -0.2f, 0.9f);
            playerRPG.transform.localScale = Vector3.one * 0.05f;
            playerRPG.transform.localRotation = Quaternion.Euler(270, 0, 0);
            playerRPG.SetActive(true);

            var probeLauncherPrepass = GameObject.Find("Player_Body/PlayerCamera/Signalscope/Props_HEA_Signalscope/Props_HEA_Signalscope_Prepass");
            GameObject.Destroy(probeLauncherPrepass.GetComponent<MeshRenderer>());

            // Ship probe launcher prop
            var shipProbeLauncher = GameObject.Find("Ship_Body/Module_Supplies/Systems_Supplies/ExpeditionGear/EquipmentGeo/Props_HEA_ProbeLauncher/");
            GameObject.Destroy(shipProbeLauncher.GetComponent<MeshRenderer>());
            foreach (var childMeshRenderer in shipProbeLauncher.GetComponentsInChildren<MeshRenderer>())
            {
                GameObject.Destroy(childMeshRenderer);
            }
            var shipRPG = GameObject.Instantiate(rpgPrefab, shipProbeLauncher.transform);
            shipRPG.transform.localPosition = new Vector3(0.66f, -0.12f, -1.5f);
            shipRPG.transform.localScale = Vector3.one * 0.045f;
            shipRPG.transform.localRotation = Quaternion.Euler(318.9286f, 267.8683f, 267.8439f);
            shipRPG.SetActive(true);

            // TODO: Translator to... something

            // TODO: Replace props in the word that have these models

            /*
            // Hide in world objects
            GameObject.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_11").SetActive(false);
            GameObject.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_12").SetActive(false);
            GameObject.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_9").SetActive(false);
           

            // Put HEV suit
            var suit = GameObject.Instantiate(hevPrefab, GameObject.Find("TimberHearth_Body").transform);
            suit.transform.localScale = Vector3.one * 0.05f;
            suit.transform.localPosition = new Vector3(-12.0f, -49.6f, 181.4f);
            suit.transform.localRotation = Quaternion.FromToRotation(suit.transform.TransformDirection(Vector3.down), -suit.transform.localPosition.normalized);
            suit.transform.Rotate(new Vector3(0, 180, 0));
            //suit.transform.LookAt(forklift.transform.parent.TransformDirection(Vector3.forward), suit.transform.localPosition.normalized);
            suit.SetActive(true);
            */
        }

        private static void ReplaceProbes()
        {
            // TODO
            var shipSupplyProbe = GameObject.Find("Ship_Body/Module_Supplies/Systems_Supplies/ExpeditionGear/EquipmentGeo/Props_HEA_Probe_STATIC/");
            GameObject.Destroy(shipSupplyProbe.GetComponentInChildren<MeshRenderer>());
            var shipSatchel = GameObject.Instantiate(satchelPrefab, shipSupplyProbe.transform);
            shipSatchel.transform.localPosition = new Vector3(0.7f, 1.05f, 0.78f);
            shipSatchel.transform.localScale = Vector3.one * 0.045f;
            shipSatchel.transform.localRotation = Quaternion.Euler(337.1081f, 275.5102f, 155.7031f);
            shipSatchel.SetActive(true);

            // Player probe
            var playerProbe = GameObject.Find("Probe_Body/CameraPivot/Geometry/Props_HEA_Probe_ANIM/Props_HEA_Probe/");
            playerProbe.SetActive(false);
            var playerSatchel = GameObject.Instantiate(satchelPrefab, playerProbe.transform.parent);
            playerSatchel.transform.localPosition = new Vector3(1.35f, 0.8f, -0.25f);
            playerSatchel.transform.localScale = Vector3.one * 0.045f;
            playerSatchel.transform.localRotation = Quaternion.Euler(340.6183f, 347.8522f, 136.6455f);
            playerSatchel.SetActive(true);
        }

        private static void ReplaceMummies()
        {
            var mummyCircle1 = "RingWorld_Body/Sector_RingInterior/Sector_Zone1/Sector_DreamFireHouse_Zone1/Interactables_DreamFireHouse_Zone1/DreamFireChamber/MummyCircle";
            ReplaceMummyCircle(GameObject.Find(mummyCircle1));

            var mummyCircle2 = "RingWorld_Body/Sector_RingInterior/Sector_Zone2/Sector_DreamFireLighthouse_Zone2_AnimRoot/Interactibles_DreamFireLighthouse_Zone2/DreamFireChamber/MummyCircle";
            ReplaceMummyCircle(GameObject.Find(mummyCircle2));

            var mummyCircle3 = "RingWorld_Body/Sector_RingInterior/Sector_Zone3/Sector_HiddenGorge/Sector_DreamFireHouse_Zone3/Interactables_DreamFireHouse_Zone3/DreamFireChamber_DFH_Zone3/MummyCircle";
            ReplaceMummyCircle(GameObject.Find(mummyCircle3));
        }

        private static void ReplaceMummyCircle(GameObject mummyCircle)
        {
            var permutation = vortigauntMummyPrefabs.OrderBy(x => (int)Random.Range(0, vortigauntMummyPrefabs.Length)).ToArray();

            int index = 0;
            foreach (Transform child in mummyCircle.transform)
            {
                var mummy = child.Find("Prefab_IP_SleepingMummy_v2")?.Find("Mummy_IP_Anim");
                if(mummy != null)
                {
                    GameObject vortigauntMummy = GameObject.Instantiate(permutation[index++ % permutation.Length], mummy.parent);
                    vortigauntMummy.transform.localScale = Vector3.one * 0.05f;
                    vortigauntMummy.transform.localPosition = new Vector3(0f, 0f, 0.4f);
                    vortigauntMummy.transform.localRotation = Quaternion.Euler(350, 0, 0);
                    vortigauntMummy.SetActive(true);
                    mummy.gameObject.SetActive(false);
                }
            }
        }

        private static void PlaceBarnacles()
        {
            PlaceBarnacle(-104.8f, -49.5f, 77.5f);
            PlaceBarnacle(-106.6f, -26.2f, 84.2f);
            PlaceBarnacle(-83.8f, 64.2f, 95.5f);
            PlaceBarnacle(-92.4f, 77.9f, 103.6f);
            PlaceBarnacle(-80.2f, 111.8f, 83.0f);
            PlaceBarnacle(-78.5f, -135.3f, -36.8f);
            PlaceBarnacle(-89.2f, 108.7f, 77.5f);
            PlaceBarnacle(-87.7f, 102.6f, 86.7f);
            PlaceBarnacle(103.5f, 83.6f, -26.8f);
            PlaceBarnacle(24.7f, -89.9f, -83.2f);
            PlaceBarnacle(22.2f, -121.7f, -74.5f);
            PlaceBarnacle(6.7f, -142.0f, -11.7f);
            PlaceBarnacle(14.5f, -125.7f, 41.9f);
            PlaceBarnacle(40.7f, -137.2f, -10.8f);
        }

        private static void PlaceBarnacle(float x, float y, float z)
        {
            var barnacle = GameObject.Instantiate(barnaclePrefab, GameObject.Find("CaveTwin_Body").transform);
            barnacle.transform.localScale = Vector3.one * 0.03f;
            barnacle.transform.localPosition = new Vector3(x, y, z);
            barnacle.transform.localRotation = Quaternion.FromToRotation(barnacle.transform.TransformDirection(Vector3.down), -barnacle.transform.localPosition.normalized);
            
            barnacle.SetActive(true);
        }

        private static void PlaceProtozoa()
        {
            // Inside
            for (int i = 0; i < 8; i++)
            {
                PlaceProtozoan(Random.Range(0, 360), Random.Range(-90, 90), Random.Range(160, 180));
            }
            // Outside
            for (int i = 0; i < 16; i++)
            {
                PlaceProtozoan(Random.Range(0, 360), Random.Range(-90, 90), Random.Range(310, 340));
            }
        }

        private static void PlaceProtozoan(float longitude, float latitude, float r)
        {
            var protozoan = GameObject.Instantiate(protozoaPrefab);
            protozoan.transform.localScale = Vector3.one * 0.04f;
            protozoan.GetComponent<Animator>().speed = Random.Range(0.4f, 0.6f);
            var controller = protozoan.AddComponent<ProtozoanController>();
            controller.PlaceAt(GameObject.Find("BrittleHollow_Body").transform, r, longitude, latitude);
            protozoan.SetActive(true);
        }

        private static void PlaceGus()
        {
            var forklift = GameObject.Instantiate(forkliftPrefab, GameObject.Find("TimberHearth_Body").transform);
            forklift.transform.localScale = Vector3.one * 0.03f;
            forklift.transform.localPosition = new Vector3(-6.1f, -45.0f, 181.5f);
            forklift.transform.localRotation = Quaternion.FromToRotation(forklift.transform.TransformDirection(Vector3.down), -forklift.transform.localPosition.normalized);
            forklift.transform.Rotate(new Vector3(0, 180, 0));
            forklift.SetActive(true);
        }

        private static void ReplaceAxe()
        {
            var wrist = GameObject.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Villager_HEA_Marl_ANIM_StareDwn/Marl_Skin_01:tall_rig_b_v01:TrajectorySHJnt/Marl_Skin_01:tall_rig_b_v01:ROOTSHJnt/Marl_Skin_01:tall_rig_b_v01:Spine_01SHJnt/Marl_Skin_01:tall_rig_b_v01:Spine_02SHJnt/Marl_Skin_01:tall_rig_b_v01:Spine_TopSHJnt/Marl_Skin_01:tall_rig_b_v01:RT_Arm_ClavicleSHJnt/Marl_Skin_01:tall_rig_b_v01:RT_Arm_ShoulderSHJnt/Marl_Skin_01:tall_rig_b_v01:RT_Arm_ElbowSHJnt/Marl_Skin_01:tall_rig_b_v01:RT_Arm_WristSHJnt/");
            wrist.transform.Find("Props_HEA_Hatchet").gameObject.SetActive(false);

            var crowbar = GameObject.Instantiate(crowbarPrefab, wrist.transform);
            crowbar.transform.localScale = Vector3.one * 0.04f;
            crowbar.transform.localPosition = new Vector3(0.1f, -0.5f, -0.6f);
            crowbar.transform.localRotation = Quaternion.Euler(290, 150, 180);
            crowbar.SetActive(true);
        }

        #endregion PatchMeshes

        #region patches

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

        #region Gman patches
        private static void OnSolanumAnimControllerAwake(SolanumAnimController __instance)
        {
            var solanum = __instance.gameObject;

            var gman = GameObject.Instantiate(gmanPrefab, solanum.transform);
            gman.transform.localPosition = Vector3.zero;
            gman.transform.localScale = Vector3.one * 0.030f;
            gman.name = "Gman";
            gman.AddComponent<GmanController>();

            solanum.transform.Find("Nomai_Mesh:Mesh").gameObject.SetActive(false);

            //Get rid of the "black eyes fix" quads
            foreach(Transform child in Utility.SearchInChildren(solanum.transform, "Nomai_Rig_v01:Neck_TopSHJnt").transform)
            {
                child.gameObject.SetActive(false);
            }

            // Gets rid of the staff
            Utility.SearchInChildren(solanum.transform, "Nomai_Rig_v01:LF_Arm_WristSHJnt").gameObject.SetActive(false);
            gman.SetActive(true);
        }

        private static void OnStartConversation(SolanumAnimController __instance)
        {
            __instance.gameObject.GetComponentInChildren<GmanController>().TriggerAnim(GmanController.GmanState.Yes);
        }

        private static void OnEndConversation(SolanumAnimController __instance)
        {
            __instance.gameObject.GetComponentInChildren<GmanController>().TriggerAnim(GmanController.GmanState.No);
        }

        private static void OnGesture(SolanumAnimController __instance)
        {
            __instance.gameObject.GetComponentInChildren<GmanController>().TriggerAnim(GmanController.GmanState.Brush);
        }

        private static void OnRaiseCairns(SolanumAnimController __instance)
        {
            __instance.gameObject.GetComponentInChildren<GmanController>().TriggerAnim(GmanController.GmanState.LookDown);
        }

        private static void OnStopWatchingPlayer(SolanumAnimController __instance)
        {
            __instance.gameObject.GetComponentInChildren<GmanController>().TriggerAnim(GmanController.GmanState.LookAround);
        }
        #endregion Gman patches

        private static void OnPlayerCharacterControllerAwake(PlayerCharacterController __instance)
        {
            //__instance.gameObject.AddComponent<ObjectSpawner>();
        }

        #endregion patches
    }
}
