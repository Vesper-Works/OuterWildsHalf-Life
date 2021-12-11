using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StreamingMeshHandle;

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
            if (currentScene != OWScene.SolarSystem) return;

            ReplaceHearthians();
            ReplacePlayer();
        }

        private static void ReplaceHearthians()
        {
            var tallRigSuffix = "SHJnt";
            var travellerRigSuffix = "_Jnt";
            var shortRigSuffix = "_Jnt";

            // Tall rigs
            var slate = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_StartingCamp/Characters_StartingCamp/Villager_HEA_Slate/Villager_HEA_Slate_ANIM_LogSit/Slate_Skin_01:Slate_Mesh:Villager_HEA_Slate";
            var slatePrefix = "Slate_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(slate, slatePrefix, tallRigSuffix, MeshPatcher.scientistPrefabs[0], 3.5f, hearthianBones, hearthianBoneOffsets);

            var hornfels = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1)/Villager_HEA_Hornfels_ANIM_Working/Hornfels_Skin_01:Hornfels_Mesh:Villager_HEA_Hornfels";
            var hornfelsPrefix = "Hornfels_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(hornfels, hornfelsPrefix, tallRigSuffix, MeshPatcher.scientistPrefabs[1], 3.5f, hearthianBones, hearthianBoneOffsets);

            var marl = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Villager_HEA_Marl_ANIM_StareDwn/Marl_Skin_01:Marl_Mesh:Villager_HEA_Marl";
            var marlPrefix = "Marl_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(marl, marlPrefix, tallRigSuffix, MeshPatcher.barneyPrefab, 3.5f, hearthianBones, hearthianBoneOffsets);

            var porphy = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/Villager_HEA_Porphy_ANIM_Taste/Porphy_Skin_01:Porphy_Mesh:Villager_HEA_Porphy";
            var prophyPrefix = "Porphy_Skin_01:tall_rig_b_v01:";
            SwapSkeleton(porphy, prophyPrefix, tallRigSuffix, MeshPatcher.barneyPrefab, 3.5f, hearthianBones, hearthianBoneOffsets);

            // Traveler rigs
            var hal = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Character_HEA_Hal_Museum/Villager_HEA_Hal_ANIM_Museum/hal_skin:HEA_Villagers:Villager_HEA_Hal";
            var halPrefix = "hal_skin:player_rig_v01:Traveller_";
            SwapSkeleton(hal, halPrefix, travellerRigSuffix, MeshPatcher.scientistPrefabs[2], 2.5f, hearthianBones, hearthianBoneOffsets);

            var esker = "Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker/Villager_HEA_Esker_ANIM_Rocking/rutile_skin:Esker_Mesh:Villager_HEA_Esker";
            var eskerPrefix = "rutile_skin:Traveller_Rig_v01:Traveller_";
            SwapSkeleton(esker, eskerPrefix, travellerRigSuffix, MeshPatcher.scientistPrefabs[3], 2.5f, hearthianBones, hearthianBoneOffsets);

            var tuff = "TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/Villager_HEA_Tuff_ANIM_Mine/player_v01:Traveller_Mesh_v01:Villager_HEA_Tuff";
            var tuffPrefix = "player_v01:Traveller_Rig_v01:Traveller_";
            SwapSkeleton(tuff, tuffPrefix, travellerRigSuffix, MeshPatcher.barneyPrefab, 2.5f, hearthianBones, hearthianBoneOffsets);

            var rutile = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/Villager_HEA_Rutile_ANIM_Rocking/rutile_skin:Traveller_Mesh_v01:Villager_HEA_Rutile";
            var rutilePrefix = "rutile_skin:Traveller_Rig_v01:Traveller_";
            SwapSkeleton(rutile, rutilePrefix, travellerRigSuffix, MeshPatcher.barneyPrefab, 2.5f, hearthianBones, hearthianBoneOffsets);

            // Short rigs
            var gneiss = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/Villager_HEA_Gneiss_ANIM_Tuning/Gneiss:Mesh:Villager_HEA_Gneiss/";
            var gneissPrefix = "Gneiss:Rig:";
            SwapSkeleton(gneiss, gneissPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBones, hearthianBoneOffsets);

            var spinel = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Villager_HEA_Spinel_ANIM_Fishing/Spinel_Skin:Spinel_Mesh:Villager_HEA_Spinel/";
            var spinelPrefix = "Spinel_Skin:Short_Rig_V01:";
            SwapSkeleton(spinel, spinelPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBones, hearthianBoneOffsets);

            var gossan = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan/Villager_HEA_Gossan_ANIM_Polish/Gossan_Skin:Gossan_Mesh:Villager_HEA_Gossan";
            var gossanPrefix = "Gossan_Skin:Short_Rig_V01:";
            SwapSkeleton(gossan, gossanPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBones, hearthianBoneOffsets);

            var tektite = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Tektite/Villager_HEA_Tektite_ANIM_Idle/Tektite_Skin:Tektite_Mesh:Villager_HEA_Tektite";
            var tektite2 = "TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Villager_HEA_Tektite_ANIM_Idle/Tektite_Skin:Tektite_Mesh:Villager_HEA_Tektite";
            var tektitePrefix = "Tektite_Skin:Short_Rig_V01:";
            SwapSkeleton(tektite, tektitePrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBones, hearthianBoneOffsets);
            SwapSkeleton(tektite2, tektitePrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 2f, hearthianBones, hearthianBoneOffsets);

            // Child rigs (count as short)
            var tephra = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_Hidden/Villager_HEA_Tephra (1)/Villager_HEA_Tephra_ANIM_SitIdle/Tephra_Skin_01:Tephra_Mesh:Villager_HEA_Tephra";
            var tephra2 = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/Villager_HEA_Tephra_ANIM_SitIdle/Tephra_Skin_01:Tephra_Mesh:Villager_HEA_Tephra";
            var tephraPrefix = "Tephra_Skin_01:Child_Rig_V01:";
            SwapSkeleton(tephra, tephraPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBones, hearthianBoneOffsets);
            SwapSkeleton(tephra2, tephraPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBones, hearthianBoneOffsets);

            var galena = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_Hidden/Villager_HEA_Galena (1)/Villager_HEA_Galena_ANIM_Idle/Galena_Skin_01:Galena_Mesh:Villager_HEA_Galena";
            var galena2 = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/Villager_HEA_Galena_ANIM_Idle/Galena_Skin_01:Galena_Mesh:Villager_HEA_Galena";
            var galenaPrefix = "Galena_Skin_01:Child_Rig_V01:";
            SwapSkeleton(galena, galenaPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBones, hearthianBoneOffsets);
            SwapSkeleton(galena2, galenaPrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBones, hearthianBoneOffsets);

            var mica = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/Villager_HEA_Mica_ANIM/Village_HEA_Mica:Mica_Mesh:Mica_Mesh";
            var micaPrefix = "Village_HEA_Mica:Child_Rig:";
            SwapSkeleton(mica, micaPrefix, shortRigSuffix, MeshPatcher.scientistPrefabs[2], 1.5f, hearthianBones, hearthianBoneOffsets);

            var arkose = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Arkose_GhostMatter/Villager_HEA_Arkose_ANIM_RockThrow/Arkose_Skin_01:Arkose_Mesh:Villager_HEA_Arkose";
            var arkosePrefix = "Arkose_Skin_01:Child_Rig_V01:";
            SwapSkeleton(arkose, arkosePrefix, shortRigSuffix, MeshPatcher.barneyPrefab, 1.5f, hearthianBones, hearthianBoneOffsets);

            var moraine = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Moraine/Villager_HEA_Moraine_ANIM_Idle/Moraine_Mesh:Villager_HEA_Moraine";
            var morainePrefix = "Child_Rig_V01:";
            SwapSkeleton(moraine, morainePrefix, shortRigSuffix, MeshPatcher.scientistPrefabs[3], 1.5f, hearthianBones, hearthianBoneOffsets);

            // Travelers
            var chert = "CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/Traveller_HEA_Chert_ANIM_Chatter_Chipper/Chert_Skin_02:Chert_Mesh:Traveller_HEA_Chert";
            var chertPrefix = "Chert_Skin_02:Child_Rig_V01:";
            SwapSkeleton(chert, chertPrefix, shortRigSuffix, MeshPatcher.suitGordonPrefeb, 1.5f, hearthianBones, hearthianBoneOffsets);
            GameObject.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/Traveller_HEA_Chert_ANIM_Chatter_Chipper/Chert_Skin_02:Chert_Mesh:Traveller_HEA_Chert/Chert_Skin_02:Chert_Mesh:Traveller_HEA_Chert 1").SetActive(false);

            var gabbro = "GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute/gabbro_OW_V02:gabbro_mesh:Gabbro_Geo";
            var gabbroPrefix = "gabbro_OW_V02:gabbro_rig_v01:";
            SwapSkeleton(gabbro, gabbroPrefix, shortRigSuffix, MeshPatcher.suitGordonPrefeb, 3f, hearthianBones, hearthianBoneOffsets);
            GameObject.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute/gabbro_OW_V02:gabbro_mesh:Gabbro_Geo/gabbro_OW_V02:gabbro_mesh:Gabbro_Main").SetActive(false);

            var feldspar = "DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters/Traveller_HEA_Feldspar/Traveller_HEA_Feldspar_ANIM_Talking";
            var feldsparPrefix = "Feldspar_Skin:Short_Rig_V01:";
            SwapSkeleton(feldspar, feldsparPrefix, shortRigSuffix, MeshPatcher.suitGordonPrefeb, 3f, hearthianBones, hearthianBoneOffsets);
            GameObject.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters/Traveller_HEA_Feldspar/Traveller_HEA_Feldspar_ANIM_Talking/Feldspar_Skin:Feldspar_Mesh:Traveller_HEA_Feldspar").SetActive(false);

            /*
            var riebeck = "BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck/Traveller_HEA_Riebeck_ANIM_Talking";
            SwapSkeleton(riebeck, "", "", MeshPatcher.suitGordonPrefeb, 4f, riebeckBones, hearthianBoneOffsets);
            GameObject.Find("BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck/Traveller_HEA_Riebeck_ANIM_Talking/Riebeck_Rig2:Traveler_Reiback_mesh").SetActive(false);
            */
        }

        public static void ReplacePlayer()
        {
            var player = "player_mesh_noSuit:Traveller_HEA_Player";
            var playerSuit = "Traveller_Mesh_v01:Traveller_Geo";
            var playerPrefix = "Traveller_Rig_v01:Traveller_";
            foreach (var playerMesh in Utility.FindObjectsWithName(player))
            {
                Logger.Log($"Found player mesh at {Utility.GetPath(playerMesh.transform)}");
                SwapSkeleton(playerMesh, playerPrefix, "_Jnt", MeshPatcher.gordonPrefab, 30f, hearthianBones, hearthianBoneOffsets);
            }
            foreach (var playerSuitMesh in Utility.FindObjectsWithName(playerSuit))
            {
                SwapSkeleton(playerSuitMesh, playerPrefix, "_Jnt", MeshPatcher.suitGordonPrefeb, 30f, hearthianBones, hearthianBoneOffsets);
            }

            // Marshmallow stick
            GameObject.Find("Player_Body/RoastingSystem/Stick_Root/Stick_Pivot/Stick_Tip/Props_HEA_RoastingStick/RoastingStick_Arm").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Player_Body/RoastingSystem/Stick_Root/Stick_Pivot/Stick_Tip/Props_HEA_RoastingStick/RoastingStick_Arm_NoSuit").GetComponent<MeshRenderer>().enabled = false;
        }

        private static void SwapSkeleton(string originalModelPath, string bonePrefix, string boneSuffix, GameObject prefab, float scale,
             Dictionary<string, string> boneConversion, Dictionary<string, Vector3> boneOffsets)
        {
            var originalModel = GameObject.Find(originalModelPath);
            SwapSkeleton(originalModel, bonePrefix, boneSuffix, prefab, scale, boneConversion, boneOffsets);
        }

        private static void SwapSkeleton(GameObject originalModel, string bonePrefix, string boneSuffix, GameObject prefab, float scale,
            Dictionary<string, string> boneConversion, Dictionary<string, Vector3> boneOffsets)
        {
            var newModel = GameObject.Instantiate(prefab, originalModel.transform.parent.transform);
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localScale = Vector3.one * 0.03f;
            newModel.SetActive(true);

            // Disappear existing mesh renderers
            foreach(var skinnedMeshRenderer in originalModel.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if(!skinnedMeshRenderer.name.Contains("Props_HEA_Jetpack"))
                    skinnedMeshRenderer.sharedMesh = null;
            }

            var skinnedMeshRenderers = newModel.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (skinnedMeshRenderer.name.Equals("DM_Gordon_Head1")) continue;
                var bones = skinnedMeshRenderer.bones;
                for (int i = 0; i < bones.Length; i++)
                {
                    var bone = bones[i];
                    if (!boneConversion.ContainsKey(bone.name)) continue;
                    try
                    {
                        bone.parent = Utility.SearchInChildren(originalModel.transform.parent, bonePrefix + boneConversion[bone.name] + boneSuffix);
                        if (bone.parent == null) Logger.LogWarning($"Couldn't find parent for {bone.name} in {skinnedMeshRenderer.name}");

                        bone.localScale = Vector3.one * scale;

                        // Have to adjust all heads but the player bc Hearthians got some really long necks
                        if(!bone.name.Contains("Head") || !(prefab == MeshPatcher.gordonPrefab || prefab == MeshPatcher.suitGordonPrefeb))
                            bone.localPosition = scale * (boneOffsets.ContainsKey(bone.name) ? boneOffsets[bone.name] : Vector3.zero);

                        if (bone.name.Contains("L Foot")) bone.localRotation = Quaternion.Euler(new Vector3(90, 270, 0));
                        else if (bone.name.Equals("Bip01 R Foot")) bone.localRotation = Quaternion.Euler(new Vector3(90, 90, 0));
                        else if (bone.name.Equals("Bip02 R Foot")) bone.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
                        else if (bone.name.Contains("L Leg")) bone.localRotation = Quaternion.Euler(90, 0, 0);
                        else if (bone.name.Contains("R Leg")) bone.localRotation = Quaternion.Euler(90, 180, 0);
                        else if (bone.name.Contains(" L ")) bone.localRotation = Quaternion.Euler(270, 0, 0);
                        else if (bone.name.Contains(" R ")) bone.localRotation = Quaternion.Euler(270, 180, 0);
                        else bone.localRotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
                    }
                    catch(Exception)
                    {
                        Logger.LogWarning($"Couldn't find bone ({bones[i]}) for ({skinnedMeshRenderer.name})");
                    }
                }

                skinnedMeshRenderer.rootBone = Utility.SearchInChildren(originalModel.transform.parent, bonePrefix + boneConversion["rootBone"] + boneSuffix);
                if (skinnedMeshRenderer.rootBone == null) Logger.LogError($"Couldn't find root bone for {skinnedMeshRenderer.name}");
                skinnedMeshRenderer.quality = SkinQuality.Bone4;
                skinnedMeshRenderer.updateWhenOffscreen = true;

                // Reparent the skinnedMeshRenderer to the original object.
                skinnedMeshRenderer.transform.parent = originalModel.transform;
            }

            GameObject.Destroy(newModel);
        }

        #region Hearthian bones
        private static Dictionary<string, string> hearthianBones = new Dictionary<string, string>()
        {
            { "rootBone", "Trajectory" },
            // Spine
            { "Bip01", "Trajectory" },
            { "Bip01 Pelvis", "ROOT" },
            //{ "Bip01 Spine", "ROOT" },
            { "Bip01 Spine1", "Spine_01" },
            { "Bip01 Spine2", "Spine_02" },
             //{ "Bip01 Spine3", "Spine_Top" },
            { "Bip01 Neck", "Neck_01" },
            { "Bip01 Head", "Neck_Top" },

            // Left leg
            { "Bip01 L Leg", "LF_Leg_Hip" },
            { "Bip01 L Leg1", "LF_Leg_Knee" },
            { "Bip01 L Foot", "LF_Leg_Ball" },

            // Right leg
            { "Bip01 R Leg", "RT_Leg_Hip" },
            { "Bip01 R Leg1", "RT_Leg_Knee" },
            { "Bip01 R Foot", "RT_Leg_Ball" },

            // Left arm
            { "Bip01 L Arm", "LF_Arm_Clavicle" },
            { "Bip01 L Arm1", "LF_Arm_Shoulder" },
            { "Bip01 L Arm2", "LF_Arm_Elbow" },

            // Right arm
            { "Bip01 R Arm", "RT_Arm_Clavicle" },
            { "Bip01 R Arm1", "RT_Arm_Shoulder" },
            { "Bip01 R Arm2", "RT_Arm_Elbow" },

            // Left hand
            { "Bip01 L Hand", "LF_Arm_Wrist" },

            // Right hand
            { "Bip01 R Hand", "RT_Arm_Wrist" },

            // RIG 2 //

            // Spine
            { "Bip02", "Trajectory" },
            { "Bip02 Pelvis", "ROOT" },
            { "Bip02 Spine", "Spine_01" },
            { "Bip02 Spine1", "Spine_02" },
            //{ "Bip02 Spine2", "Spine_Top" },
            { "Bip02 Neck", "Neck_01" },
            { "Bip02 Head", "Neck_Top" },

            // Left leg
            { "Bip02 L Leg", "LF_Leg_Hip" },
            { "Bip02 L Leg1", "LF_Leg_Knee" },
            { "Bip02 L Foot", "LF_Leg_Ball" },

            // Right leg
            { "Bip02 R Leg", "RT_Leg_Hip" },
            { "Bip02 R Leg1", "RT_Leg_Knee" },
            { "Bip02 R Foot", "RT_Leg_Ball" },
            
            // Left arm
            { "Bip02 L Arm", "LF_Arm_Clavicle" },
            { "Bip02 L Arm1", "LF_Arm_Shoulder" },
            { "Bip02 L Arm2", "LF_Arm_Elbow" },

            // Right arm
            { "Bip02 R Arm", "RT_Arm_Clavicle" },
            { "Bip02 R Arm1", "RT_Arm_Shoulder" },
            { "Bip02 R Arm2", "RT_Arm_Elbow" },

            // Left hand
            { "Bip02 L Hand", "LF_Arm_Wrist" },

            // Right hand
            { "Bip02 R Hand", "RT_Arm_Wrist" },
        };

        private static Dictionary<string, Vector3> hearthianBoneOffsets = new Dictionary<string, Vector3>()
        {
            { "Bip01 L Foot", new Vector3(0.03f, 0, 0.03f) },
            { "Bip01 R Foot", new Vector3(-0.03f, 0, -0.03f) },
            { "Bip02 L Foot", new Vector3(0.03f, 0f, 0.03f) },
            { "Bip02 R Foot", new Vector3(-0.03f, 0f, -0.03f) },
            { "Bip02 Head", new Vector3(0.05f, 0.033f, 0f) },
            { "Bip01 Head", new Vector3(0.05f, 0.033f, 0f) },
            { "Bip01 L Arm", new Vector3(0, 0.03f, -0.03f) },
            { "Bip02 L Arm", new Vector3(0, 0.03f, -0.03f) },
            { "Bip01 R Arm", new Vector3(0, -0.03f, 0.03f) },
            { "Bip02 R Arm", new Vector3(0, -0.03f, 0.03f) },
        };

        #endregion Hearthian bones

        #region Riebeck bones
        private static Dictionary<string, string> riebeckBones = new Dictionary<string, string>()
        {
            { "rootBone", "Riebeck_Rig2:jt_ZERO" },
            // Spine
            { "Bip01", "Riebeck_Rig2:jt_ZERO" },
            { "Bip01 Pelvis", "Riebeck_Rig2:jt_ROOTSHJ" },
            //{ "Bip01 Spine", "ROOT" },
            { "Bip01 Spine1", "Riebeck_Rig2:jt_Spine_01SHJ" },
            { "Bip01 Spine2", "Riebeck_Rig2:jt_Spine_02SHJ" },
             //{ "Bip01 Spine3", "Spine_Top" },
            { "Bip01 Neck", "Riebeck_Rig2:jt_Neck_01SHJ" },
            { "Bip01 Head", "Riebeck_Rig2:jt_Neck_TopSHJ" },

            // Left leg
            { "Bip01 L Leg", "Riebeck_Rig2:jt_l_Leg_HipSHJ" },
            { "Bip01 L Leg1", "Riebeck_Rig2:jt_l_Leg_KneeSHJ" },
            { "Bip01 L Foot", "Riebeck_Rig2:jt_l_Leg_AnkleSHJ" },

            // Right leg
            { "Bip01 R Leg", "Riebeck_Rig2:jt_r_Leg_HipSHJ" },
            { "Bip01 R Leg1", "Riebeck_Rig2:jt_r_Leg_KneeSHJ" },
            { "Bip01 R Foot", "Riebeck_Rig2:jt_r_Leg_AnkleSHJ" },

            // Left arm
            { "Bip01 L Arm", "Riebeck_Rig2:jt_l_Arm_ClavicleSHJ" },
            { "Bip01 L Arm1", "Riebeck_Rig2:jt_l_Arm_ShoulderSHJ" },
            { "Bip01 L Arm2", "Riebeck_Rig2:jt_l_Arm_ElbowSHJ" },

            // Right arm
            { "Bip01 R Arm", "Riebeck_Rig2:jt_r_Arm_ClavicleSHJ" },
            { "Bip01 R Arm1", "Riebeck_Rig2:jt_r_Arm_ShoulderSHJ" },
            { "Bip01 R Arm2", "Riebeck_Rig2:jt_r_Arm_ElbowSHJ" },

            // Left hand
            { "Bip01 L Hand", "Riebeck_Rig2:jt_l_Arm_WristSHJ" },

            // Right hand
            { "Bip01 R Hand", "Riebeck_Rig2:jt_r_Arm_WristSHJ" },
        };
        #endregion Riebeck bones
    }
}
