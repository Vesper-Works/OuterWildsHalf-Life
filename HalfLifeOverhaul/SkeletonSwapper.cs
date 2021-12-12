using HalfLifeOverhaul.Util;
using OWML.Common;
using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = HalfLifeOverhaul.Util.Logger;

namespace HalfLifeOverhaul
{
    public static class SkeletonSwapper
    {
        private static bool _loaded = false;
        public static void OnStart()
        {
            if (_loaded) return;

            LoadManager.OnCompleteSceneLoad += SkeletonSwapper.SwapSkeletons;

            _loaded = true;
        }

        public static void SwapSkeletons(OWScene _, OWScene currentScene)
        {
            if (currentScene == OWScene.SolarSystem)
            { 
                ReplaceHearthians();
                ReplacePlayer();
            }
            if(currentScene == OWScene.EyeOfTheUniverse)
            {
                //ReplacePlayer();
            }
        }

        private static void ReplaceHearthians()
        {
            var tallRigSuffix = "SHJnt";
            var travellerRigSuffix = "_Jnt";
            var shortRigSuffix = "_Jnt";

            // Tall rigs
            var slate = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_StartingCamp/Characters_StartingCamp/Villager_HEA_Slate/Villager_HEA_Slate_ANIM_LogSit/Slate_Skin_01:Slate_Mesh:Villager_HEA_Slate";
            var slatePrefix = "Slate_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(slate, slatePrefix, tallRigSuffix, MeshPatcher.scientistPrefabs[0], 3.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var hornfels = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1)/Villager_HEA_Hornfels_ANIM_Working/Hornfels_Skin_01:Hornfels_Mesh:Villager_HEA_Hornfels";
            var hornfelsPrefix = "Hornfels_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(hornfels, hornfelsPrefix, tallRigSuffix, MeshPatcher.scientistPrefabs[1], 3.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var marl = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Villager_HEA_Marl_ANIM_StareDwn/Marl_Skin_01:Marl_Mesh:Villager_HEA_Marl";
            var marlPrefix = "Marl_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(marl, marlPrefix, tallRigSuffix, MeshPatcher.barneyPrefab, 3.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var porphy = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/Villager_HEA_Porphy_ANIM_Taste/Porphy_Skin_01:Porphy_Mesh:Villager_HEA_Porphy";
            var prophyPrefix = "Porphy_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(porphy, prophyPrefix, tallRigSuffix, MeshPatcher.barneyPrefab, 3.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            // Traveler rigs
            var esker = "Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker/Villager_HEA_Esker_ANIM_Rocking/rutile_skin:Esker_Mesh:Villager_HEA_Esker";
            var eskerPrefix = "rutile_skin:Traveller_Rig_v01:Traveller_";
            SwapSkeleton(esker, eskerPrefix, travellerRigSuffix, MeshPatcher.scientistPrefabs[3], 2.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var hal = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Character_HEA_Hal_Museum/Villager_HEA_Hal_ANIM_Museum/hal_skin:HEA_Villagers:Villager_HEA_Hal";
            var halPrefix = "hal_skin:player_rig_v01:Traveller_";
            SwapSkeleton(hal, halPrefix, travellerRigSuffix, MeshPatcher.scientistPrefabs[2], 2.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var tuff = "TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/Villager_HEA_Tuff_ANIM_Mine/player_v01:Traveller_Mesh_v01:Villager_HEA_Tuff";
            var tuffPrefix = "player_v01:Traveller_Rig_v01:Traveller_";
            SwapSkeleton(tuff, tuffPrefix, travellerRigSuffix, MeshPatcher.barneyPrefab, 2.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var rutile = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/Villager_HEA_Rutile_ANIM_Rocking/rutile_skin:Traveller_Mesh_v01:Villager_HEA_Rutile";
            var rutilePrefix = "rutile_skin:Traveller_Rig_v01:Traveller_";
            SwapSkeleton(rutile, rutilePrefix, travellerRigSuffix, MeshPatcher.barneyPrefab, 2.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            // Short rigs
            var gneiss = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/Villager_HEA_Gneiss_ANIM_Tuning/Gneiss:Mesh:Villager_HEA_Gneiss/";
            var gneissPrefix = "Gneiss:Rig:";
            SwapSkeleton(gneiss, gneissPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var spinel = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Villager_HEA_Spinel_ANIM_Fishing/Spinel_Skin:Spinel_Mesh:Villager_HEA_Spinel/";
            var spinelPrefix = "Spinel_Skin:Short_Rig_V01:";
            SwapSkeleton(spinel, spinelPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var gossan = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan/Villager_HEA_Gossan_ANIM_Polish/Gossan_Skin:Gossan_Mesh:Villager_HEA_Gossan";
            var gossanPrefix = "Gossan_Skin:Short_Rig_V01:";
            SwapSkeleton(gossan, gossanPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var tektite = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Tektite/Villager_HEA_Tektite_ANIM_Idle/Tektite_Skin:Tektite_Mesh:Villager_HEA_Tektite";
            var tektite2 = "TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Villager_HEA_Tektite_ANIM_Idle/Tektite_Skin:Tektite_Mesh:Villager_HEA_Tektite";
            var tektitePrefix = "Tektite_Skin:Short_Rig_V01:";
            SwapSkeleton(tektite, tektitePrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);
            SwapSkeleton(tektite2, tektitePrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            // Child rigs (count as short)
            var tephra = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_Hidden/Villager_HEA_Tephra (1)/Villager_HEA_Tephra_ANIM_SitIdle/Tephra_Skin_01:Tephra_Mesh:Villager_HEA_Tephra";
            var tephra2 = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/Villager_HEA_Tephra_ANIM_SitIdle/Tephra_Skin_01:Tephra_Mesh:Villager_HEA_Tephra";
            var tephraPrefix = "Tephra_Skin_01:Child_Rig_V01:";
            SwapSkeleton(tephra, tephraPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);
            SwapSkeleton(tephra2, tephraPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var galena = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_Hidden/Villager_HEA_Galena (1)/Villager_HEA_Galena_ANIM_Idle/Galena_Skin_01:Galena_Mesh:Villager_HEA_Galena";
            var galena2 = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/Villager_HEA_Galena_ANIM_Idle/Galena_Skin_01:Galena_Mesh:Villager_HEA_Galena";
            var galenaPrefix = "Galena_Skin_01:Child_Rig_V01:";
            SwapSkeleton(galena, galenaPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);
            SwapSkeleton(galena2, galenaPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var mica = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/Villager_HEA_Mica_ANIM/Village_HEA_Mica:Mica_Mesh:Mica_Mesh";
            var micaPrefix = "Village_HEA_Mica:Child_Rig:";
            SwapSkeleton(mica, micaPrefix, shortRigSuffix, MeshPatcher.scientistPrefabs[2], 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var arkose = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Arkose_GhostMatter/Villager_HEA_Arkose_ANIM_RockThrow/Arkose_Skin_01:Arkose_Mesh:Villager_HEA_Arkose";
            var arkosePrefix = "Arkose_Skin_01:Child_Rig_V01:";
            SwapSkeleton(arkose, arkosePrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            var moraine = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Moraine/Villager_HEA_Moraine_ANIM_Idle/Moraine_Mesh:Villager_HEA_Moraine";
            var morainePrefix = "Child_Rig_V01:";
            SwapSkeleton(moraine, morainePrefix, shortRigSuffix, MeshPatcher.scientistPrefabs[3], 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation);

            // Travelers
            var chert = "CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/Traveller_HEA_Chert_ANIM_Chatter_Chipper/Chert_Skin_02:Chert_Mesh:Traveller_HEA_Chert";
            var chertPrefix = "Chert_Skin_02:Child_Rig_V01:";
            SwapSkeleton(chert, chertPrefix, shortRigSuffix, MeshPatcher.suitGordonPrefeb, 1.5f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation, false);
            GameObject.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/Traveller_HEA_Chert_ANIM_Chatter_Chipper/Chert_Skin_02:Chert_Mesh:Traveller_HEA_Chert/Chert_Skin_02:Chert_Mesh:Traveller_HEA_Chert 1").SetActive(false);

            var gabbro = "GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute/gabbro_OW_V02:gabbro_mesh:Gabbro_Geo";
            var gabbroPrefix = "gabbro_OW_V02:gabbro_rig_v01:";
            SwapSkeleton(gabbro, gabbroPrefix, shortRigSuffix, MeshPatcher.suitGordonPrefeb, 3f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation, false);
            
            var feldspar = "DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters/Traveller_HEA_Feldspar/Traveller_HEA_Feldspar_ANIM_Talking";
            var feldsparPrefix = "Feldspar_Skin:Short_Rig_V01:";
            SwapSkeleton(feldspar, feldsparPrefix, shortRigSuffix, MeshPatcher.suitGordonPrefeb, 3f, hearthianBoneMap, hearthianBoneOffsets, hearthianBoneRotation, false);
            
            var riebeck = "BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck/Traveller_HEA_Riebeck_ANIM_Talking";
            var riebeckPrefix = "Riebeck_Rig2:";
            SwapSkeleton(riebeck, riebeckPrefix, "", MeshPatcher.suitGordonPrefeb, riebeckBoneScale, riebeckBoneMap, riebeckBoneOffsets, riebeckBoneRotation, false);
            
            // Gneiss' banjo mesh is unloaded when you leave Timber Hearth so this doesn't work.
            /*
            // Replace riebeck's banjo
            var banjoRoot = "BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck/Traveller_HEA_Riebeck_ANIM_Talking/Riebeck_Rig2:jt_MAINSHJ/Riebeck_Rig2:jt_ZERO/Riebeck_Rig2:jt_ROOTSHJ/Riebeck_Rig2:jt_Spine_01SHJ/Riebeck_Rig2:jt_banjoSHJ/";
            GameObject.Find(banjoRoot + "Traveller_HEA_Riebeck_BanjoStrings (1)").SetActive(false);
            
            var gneissBanjo = GameObject.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/Villager_HEA_Gneiss_ANIM_Tuning/Gneiss:Rig:Trajectory_Jnt/Gneiss:Rig:ROOT_Jnt/Gneiss:Rig:Spine_01_Jnt/Gneiss:Rig:Spine_02_Jnt/Gneiss:Rig:Spine_Top_Jnt/Props_HEA_Banjo");

            //MakeBanjo(GameObject.Find("Player_Body").transform);

            var newBanjo = GameObject.Instantiate(gneissBanjo, GameObject.Find("Player_Body").transform);
            var streamingRenderMeshHandle = gneissBanjo.GetComponent<StreamingRenderMeshHandle>();
            streamingRenderMeshHandle.proxyMesh = gneissBanjo.GetComponent<MeshFilter>().sharedMesh;

            GameObject.Destroy(newBanjo.GetComponent<StreamingRenderMeshHandle>());
            GameObject.Destroy(gneissBanjo.GetComponent<StreamingRenderMeshHandle>());

            newBanjo.transform.localPosition = Vector3.zero;
            //newBanjo.GetComponent<MeshFilter>().mesh = GameObject.Instantiate(newBanjo.GetComponent<MeshFilter>().mesh);
            //newBanjo.GetComponent<MeshFilter>().sharedMesh = GameObject.Instantiate(newBanjo.GetComponent<MeshFilter>().sharedMesh);
            newBanjo.SetActive(true);
            */
        }

        public static void ReplacePlayer()
        {
            var player = "player_mesh_noSuit:Traveller_HEA_Player";
            var playerSuit = "Traveller_Mesh_v01:Traveller_Geo";
            var playerPrefix = "Traveller_Rig_v01:Traveller_";
            foreach (var playerMesh in Utility.FindObjectsWithName(player))
            {
                SwapSkeleton(playerMesh, playerPrefix, "_Jnt", MeshPatcher.gordonPrefab, 30f, hearthianBoneMap, playerBoneOffsets, hearthianBoneRotation, false);
            }
            foreach (var playerSuitMesh in Utility.FindObjectsWithName(playerSuit))
            {
                SwapSkeleton(playerSuitMesh, playerPrefix, "_Jnt", MeshPatcher.suitGordonPrefeb, 30f, hearthianBoneMap, playerBoneOffsets, hearthianBoneRotation, false);
            }

            // Marshmallow stick
            GameObject.Find("Player_Body/RoastingSystem/Stick_Root/Stick_Pivot/Stick_Tip/Props_HEA_RoastingStick/RoastingStick_Arm").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Player_Body/RoastingSystem/Stick_Root/Stick_Pivot/Stick_Tip/Props_HEA_RoastingStick/RoastingStick_Arm_NoSuit").GetComponent<MeshRenderer>().enabled = false;
        }

        private static void SwapSkeleton(string originalModelPath, string bonePrefix, string boneSuffix, GameObject prefab, float scale,
             Func<string, string> boneConversion, Func<string, Vector3> boneOffsets, Func<string, Quaternion> boneRotations, bool isVillager = true)
        {
            var originalModel = GameObject.Find(originalModelPath);
            SwapSkeleton(originalModel, bonePrefix, boneSuffix, prefab, scale, boneConversion, boneOffsets, boneRotations, isVillager);
        }

        private static void SwapSkeleton(string originalModelPath, string bonePrefix, string boneSuffix, GameObject prefab, Func<string, float> boneScale,
        Func<string, string> boneConversion, Func<string, Vector3> boneOffsets, Func<string, Quaternion> boneRotations, bool isVillager = true)
        {
            var originalModel = GameObject.Find(originalModelPath);
            SwapSkeleton(originalModel, bonePrefix, boneSuffix, prefab, boneScale, boneConversion, boneOffsets, boneRotations, isVillager);
        }

        private static void SwapSkeleton(GameObject originalModel, string bonePrefix, string boneSuffix, GameObject prefab, float scale,
            Func<string, string> boneConversion, Func<string, Vector3> boneOffsets, Func<string, Quaternion> boneRotations, bool isVillager = true)
        {
            SwapSkeleton(originalModel, bonePrefix, boneSuffix, prefab, (_) => scale, boneConversion, boneOffsets, boneRotations, isVillager);
        }

        private static void SwapSkeleton(GameObject originalModel, string bonePrefix, string boneSuffix, GameObject prefab, Func<string, float> boneScale, 
            Func<string, string> boneConversion, Func<string, Vector3> boneOffsets, Func<string, Quaternion> boneRotations, bool isVillager = true)
        {
            var newModel = GameObject.Instantiate(prefab, originalModel.transform.parent.transform);
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localScale = Vector3.one * 0.03f;
            newModel.SetActive(true);

            // Disappear existing mesh renderers
            foreach(var skinnedMeshRenderer in originalModel.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (!skinnedMeshRenderer.name.Contains("Props_HEA_Jetpack"))
                {
                    skinnedMeshRenderer.sharedMesh = null;

                    var owRenderer = skinnedMeshRenderer.gameObject.GetComponent<OWRenderer>();
                    if(owRenderer != null) owRenderer.enabled = false;

                    var streamingMeshHandle = skinnedMeshRenderer.gameObject.GetComponent<StreamingMeshHandle>();
                    if (streamingMeshHandle != null) GameObject.Destroy(streamingMeshHandle);
                }
            }

            var skinnedMeshRenderers = newModel.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                var bones = skinnedMeshRenderer.bones;
                for (int i = 0; i < bones.Length; i++)
                {
                    var bone = bones[i];
                    var newBone = boneConversion(bone.name);
                    if (newBone == null) continue;

                    var newParent = Utility.SearchInChildren(originalModel.transform.parent, bonePrefix + newBone + boneSuffix);

                    if (newParent == null) Logger.LogWarning($"Couldn't find parent for {bone.name} in {skinnedMeshRenderer.name}");
                    else bone.parent = newParent;

                    bone.localScale = boneScale(bone.name) * Vector3.one;
                    bone.localPosition = boneScale(bone.name) * boneOffsets(bone.name);
                    bone.localRotation = boneRotations(bone.name);
                }

                skinnedMeshRenderer.rootBone = Utility.SearchInChildren(originalModel.transform.parent, bonePrefix + boneConversion("rootBone") + boneSuffix);
                if (skinnedMeshRenderer.rootBone == null) Logger.LogError($"Couldn't find root bone for {skinnedMeshRenderer.name}");
                skinnedMeshRenderer.quality = SkinQuality.Bone4;
                skinnedMeshRenderer.updateWhenOffscreen = true;

                // Reparent the skinnedMeshRenderer to the original object.
                skinnedMeshRenderer.transform.parent = originalModel.transform;

                if(isVillager)
                {
                    try
                    {
                        var animController = Utility.SearchComponentInParents<CharacterAnimController>(skinnedMeshRenderer.transform);
                        var fieldInfo = typeof(CharacterAnimController).GetField("_skinRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
                        fieldInfo.SetValue(animController, null);

                        var characterLightingRigController = Utility.SearchComponentInParents<CharacterLightingRigController>(skinnedMeshRenderer.transform);
                        characterLightingRigController.enabled = false;
                    }
                    catch(Exception e)
                    {
                        Logger.LogError($"Couldn't fix CharacterAnimController for {originalModel.name}: {e.Message}, {e.StackTrace}");
                    }
                }
            }
            GameObject.Destroy(newModel);
        }

        private static string hearthianBoneMap(string name)
        {
            string newBone;
            if (name.EndsWith("rootBone")) newBone = "Trajectory";
            else if (name.Equals("Bip01") || name.Equals("Bip02")) newBone = "Trajectory";
            else if (name.EndsWith("Pelvis")) newBone = "ROOT";
            else if (name.Equals("Bip01 Spine1")) newBone = "Spine_01";
            else if (name.Equals("Bip01 Spine2")) newBone = "Spine_02";
            else if (name.Equals("Bip02 Spine")) newBone = "Spine_01";
            else if (name.Equals("Bip02 Spine1")) newBone = "Spine_02";
            else if (name.EndsWith("Neck")) newBone = "Neck_01";
            else if (name.EndsWith("Head")) newBone = "Neck_Top";
            else if (name.EndsWith("L Leg")) newBone = "LF_Leg_Hip";
            else if (name.EndsWith("L Leg1")) newBone = "LF_Leg_Knee";
            else if (name.EndsWith("L Foot")) newBone = "LF_Leg_Ball";
            else if (name.EndsWith("R Leg")) newBone = "RT_Leg_Hip";
            else if (name.EndsWith("R Leg1")) newBone = "RT_Leg_Knee";
            else if (name.EndsWith("R Foot")) newBone = "RT_Leg_Ball";
            else if (name.EndsWith("L Arm")) newBone = "LF_Arm_Clavicle";
            else if (name.EndsWith("L Arm1")) newBone = "LF_Arm_Shoulder";
            else if (name.EndsWith("L Arm2")) newBone = "LF_Arm_Elbow";
            else if (name.EndsWith("R Arm")) newBone = "RT_Arm_Clavicle";
            else if (name.EndsWith("R Arm1")) newBone = "RT_Arm_Shoulder";
            else if (name.EndsWith("R Arm2")) newBone = "RT_Arm_Elbow";
            else if (name.EndsWith("L Hand")) newBone = "LF_Arm_Wrist";
            else if (name.EndsWith("R Hand")) newBone = "RT_Arm_Wrist";
            else newBone = null;
            return newBone;
        }

        private static Vector3 hearthianBoneOffsets(string name)
        {
            Vector3 offset;
            if (name.EndsWith("L Foot")) offset = new Vector3(0.03f, 0, 0.03f);
            else if (name.EndsWith("R Foot")) offset = new Vector3(-0.03f, 0, -0.03f);
            else if (name.EndsWith("Head")) offset = new Vector3(0.05f, 0.033f, 0f);
            else if (name.EndsWith("L Arm")) offset = new Vector3(0, 0.03f, -0.03f);
            else if (name.EndsWith("R Arm")) offset = new Vector3(0, -0.03f, 0.03f);
            else offset = Vector3.zero;
            return offset;
        }

        private static Quaternion hearthianBoneRotation(string name)
        {
            Quaternion localRotation;
            if (name.Contains("L Foot")) localRotation = Quaternion.Euler(new Vector3(90, 270, 0));
            else if (name.Equals("Bip01 R Foot")) localRotation = Quaternion.Euler(new Vector3(90, 90, 0));
            else if (name.Equals("Bip02 R Foot")) localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
            else if (name.Contains("L Leg")) localRotation = Quaternion.Euler(90, 0, 0);
            else if (name.Contains("R Leg")) localRotation = Quaternion.Euler(90, 180, 0);
            else if (name.Contains(" L ")) localRotation = Quaternion.Euler(270, 0, 0);
            else if (name.Contains(" R ")) localRotation = Quaternion.Euler(270, 180, 0);
            else localRotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
            return localRotation;
        }

        private static Vector3 playerBoneOffsets(string name)
        {
            if (name.Contains("Head")) return new Vector3(0.3f, -0.1f, -0.15f) / 30f;
            else return hearthianBoneOffsets(name);
        }

        private static string riebeckBoneMap(string name)
        {
            string newBone;
            if (name.EndsWith("rootBone")) newBone = "jt_ZERO";
            else if (name.Equals("Bip01") || name.Equals("Bip02")) newBone = "jt_ZERO";
            else if (name.EndsWith("Pelvis")) newBone = "jt_ROOTSHJ";
            else if (name.Equals("Bip01 Spine")) newBone = "jt_ROOTSHJ";
            else if (name.Equals("Bip01 Spine1")) newBone = "jt_Spine_01SHJ";
            else if (name.Equals("Bip01 Spine2")) newBone = "jt_Spine_02SHJ";
            else if (name.Equals("Bip01 Spine3")) newBone = "jt_Spine_TopSHJ";
            else if (name.Equals("Bip02 Spine")) newBone = "jt_Spine_01SHJ";
            else if (name.Equals("Bip02 Spine1")) newBone = "jt_Spine_02SHJ";
            else if (name.EndsWith("Neck")) newBone = "jt_Neck_01SHJ";
            else if (name.EndsWith("Head")) newBone = "jt_Neck_TopSHJ";
            else if (name.EndsWith("L Leg")) newBone = "jt_l_Leg_HipSHJ";
            else if (name.EndsWith("L Leg1")) newBone = "jt_l_Leg_KneeSHJ";
            else if (name.EndsWith("L Foot")) newBone = "jt_l_Leg_AnkleSHJ";
            else if (name.EndsWith("R Leg")) newBone = "jt_r_Leg_HipSHJ";
            else if (name.EndsWith("R Leg1")) newBone = "jt_r_Leg_KneeSHJ";
            else if (name.EndsWith("R Foot")) newBone = "jt_r_Leg_AnkleSHJ";
            else if (name.EndsWith("L Arm")) newBone = "jt_l_Arm_ClavicleSHJ";
            else if (name.EndsWith("L Arm1")) newBone = "jt_l_Arm_ShoulderSHJ";
            else if (name.EndsWith("L Arm2")) newBone = "jt_l_Arm_ElbowSHJ";
            else if (name.EndsWith("R Arm")) newBone = "jt_r_Arm_ClavicleSHJ";
            else if (name.EndsWith("R Arm1")) newBone = "jt_r_Arm_ShoulderSHJ";
            else if (name.EndsWith("R Arm2")) newBone = "jt_r_Arm_ElbowSHJ";
            else if (name.EndsWith("L Hand")) newBone = "jt_l_Arm_WristSHJ";
            else if (name.EndsWith("R Hand")) newBone = "jt_r_Arm_WristSHJ";
            else newBone = null;
            return newBone;
        }

        private static Vector3 riebeckBoneOffsets(string name)
        {
            Vector3 offset;
            if (name.EndsWith("Spine3")) offset = new Vector3(-0.075f, 0f, 0f);
            else if (name.EndsWith("Spine2")) offset = new Vector3(-0.025f, 0f, 0f);
            else if (name.EndsWith("Spine1")) offset = new Vector3(-0.025f, 0f, 0f);
            else if (name.EndsWith("Spine")) offset = new Vector3(0f, 0.025f, 0.025f);
            else if (name.EndsWith("Pelvis")) offset = new Vector3(0f, 0f, 0.025f);
            else offset = Vector3.zero;
            return offset;
        }

        private static Quaternion riebeckBoneRotation(string name)
        {
            Quaternion localRotation;
            if (name.Contains("L Foot")) localRotation = Quaternion.Euler(new Vector3(90, 300, 0));
            else if (name.Contains("R Foot")) localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            else if (name.Contains("L Leg")) localRotation = Quaternion.Euler(90, 0, 0);
            else if (name.Contains("R Leg")) localRotation = Quaternion.Euler(90, 0, 0);
            else if (name.EndsWith("L Arm")) localRotation = Quaternion.Euler(270, 0, 0);
            else if (name.EndsWith("R Arm")) localRotation = Quaternion.Euler(90, 0, 180);
            else if (name.Contains(" L ")) localRotation = Quaternion.Euler(0, 180, 180);
            else if (name.Contains(" R ")) localRotation = Quaternion.Euler(0, 0, 180);
            else if (name.EndsWith("Spine") || name.EndsWith("Pelvis")) localRotation = Quaternion.Euler(0, 270, 270);
            else localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            return localRotation;
        }

        private static float riebeckBoneScale(string name)
        {
            float scale;
            if (name.Contains("Leg") || name.Contains("Foot")) scale = 3f;
            else scale = 4f;
            return scale;
        }
    }
}
