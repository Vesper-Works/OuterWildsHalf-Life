using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HalfLifeOverhaul
{
    public static class SkeletonSwapper
    {
        public static void SwapSkeletons(OWScene _, OWScene currentScene)
        {
            if (currentScene != OWScene.SolarSystem) return;

            ReplaceHearthians();
        }

        private static void ReplaceHearthians()
        {
            // Tall rigs
            var slate = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_StartingCamp/Characters_StartingCamp/Villager_HEA_Slate/Villager_HEA_Slate_ANIM_LogSit/Slate_Skin_01:Slate_Mesh:Villager_HEA_Slate";
            var slatePrefix = "Slate_Skin_01:tall_rig_b_v01:";
            SwapSkeletonToScientist(slate, slatePrefix, MeshPatcher.scientistPrefabs[0]);

            var hornfels = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1)/Villager_HEA_Hornfels_ANIM_Working/Hornfels_Skin_01:Hornfels_Mesh:Villager_HEA_Hornfels";
            var hornfelsPrefix = "Hornfels_Skin_01:tall_rig_b_v01:";
            SwapSkeletonToScientist(hornfels, hornfelsPrefix, MeshPatcher.scientistPrefabs[1]);

            var marl = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Villager_HEA_Marl_ANIM_StareDwn/Marl_Skin_01:Marl_Mesh:Villager_HEA_Marl";
            var marlPrefix = "Marl_Skin_01:tall_rig_b_v01:";
            SwapSkeletonToScientist(marl, marlPrefix, MeshPatcher.scientistPrefabs[2]);

            var porphy = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/Villager_HEA_Porphy_ANIM_Taste/Porphy_Skin_01:Porphy_Mesh:Villager_HEA_Porphy";
            var prophyPrefix = "Porphy_Skin_01:tall_rig_b_v01:";
            SwapSkeletonToScientist(porphy, prophyPrefix, MeshPatcher.scientistPrefabs[3]);

            // Player rigs
            var hal = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Character_HEA_Hal_Museum/Villager_HEA_Hal_ANIM_Museum/hal_skin:HEA_Villagers:Villager_HEA_Hal";
            var halPrefix = "hal_skin:player_rig_v01:";

            // Traveler rigs (might be the same as player rig since the player has a traveler rig)
            // There are a lot of copies of the player mesh for nomai projection pools so we'll just search for all of them
            var player = "player_mesh_noSuit:Traveller_HEA_Player";
            var playerSuit = "Traveller_Mesh_v01:Traveller_Geo";
            var playerPrefix = "Traveller_Rig_v01:";

            var esker = "Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker/Villager_HEA_Esker_ANIM_Rocking/rutile_skin:Esker_Mesh:Villager_HEA_Esker";
            var eskerPrefix = "rutile_skin:Traveller_Rig_v01:";

            var tuff = "TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/Villager_HEA_Tuff_ANIM_Mine/player_v01:Traveller_Mesh_v01:Villager_HEA_Tuff";
            var tuffPrefix = "player_v01:Traveller_Rig_v01:";

            var rutile = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/Villager_HEA_Rutile_ANIM_Rocking/rutile_skin:Traveller_Mesh_v01:Villager_HEA_Rutile";
            var rutilePrefix = "rutile_skin:Traveller_Rig_v01:";

            // Child rigs
            var tephra = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_Hidden/Villager_HEA_Tephra (1)/Villager_HEA_Tephra_ANIM_SitIdle/Tephra_Skin_01:Tephra_Mesh:Villager_HEA_Tephra";
            var tephra2 = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/Villager_HEA_Tephra_ANIM_SitIdle/Tephra_Skin_01:Tephra_Mesh:Villager_HEA_Tephra";
            var tephraPrefix = "Tephra_Skin_01:Child_Rig_V01:";

            var galena = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_Hidden/Villager_HEA_Galena (1)/Villager_HEA_Galena_ANIM_Idle/Galena_Skin_01:Galena_Mesh:Villager_HEA_Galena";
            var galena2 = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/Villager_HEA_Galena_ANIM_Idle/Galena_Skin_01:Galena_Mesh:Villager_HEA_Galena";
            var galenaPrefix = "Galena_Skin_01:Child_Rig_V01:";

            var mica = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/Villager_HEA_Mica_ANIM/Village_HEA_Mica:Mica_Mesh:Mica_Mesh";
            var micaPrefix = "Village_HEA_Mica:Child_Rig:";

            var arkose = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Arkose_GhostMatter/Villager_HEA_Arkose_ANIM_RockThrow/Arkose_Skin_01:Arkose_Mesh:Villager_HEA_Arkose";
            var arkosePrefix = "Arkose_Skin_01:Child_Rig_V01:";

            var moraine = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Moraine/Villager_HEA_Moraine_ANIM_Idle/Moraine_Mesh:Villager_HEA_Moraine";
            var morainePrefix = "Child_Rig_V01:";

            // Gneiss ?
            var gneiss = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/Villager_HEA_Gneiss_ANIM_Tuning/Gneiss:Mesh:Villager_HEA_Gneiss/";
            var gneissPrefix = "Gneiss:Rig:";

            // Short rigs
            var spinel = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Villager_HEA_Spinel_ANIM_Fishing/Spinel_Skin:Spinel_Mesh:Villager_HEA_Spinel/";
            var spinelPrefix = "Spinel_Skin:Short_Rig_V01:";

            var gossan = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan/Villager_HEA_Gossan_ANIM_Polish/Gossan_Skin:Gossan_Mesh:Villager_HEA_Gossan";
            var gossanPrefix = "Gossan_Skin:Short_Rig_V01:";

            var tektite = "TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Tektite/Villager_HEA_Tektite_ANIM_Idle/Tektite_Skin:Tektite_Mesh:Villager_HEA_Tektite";
            var tektite2 = "TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Villager_HEA_Tektite_ANIM_Idle/Tektite_Skin:Tektite_Mesh:Villager_HEA_Tektite";
            var tektitePrefix = "Tektite_Skin:Short_Rig_V01:";
        }

        private static void SwapSkeletonToScientist(string originalModelPath, string bonePrefix, GameObject prefab)
        {
            var originalModel = GameObject.Find(originalModelPath);

            var newModel = GameObject.Instantiate(prefab, originalModel.transform.parent.transform);
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localScale = Vector3.one * 0.03f;
            newModel.SetActive(true);

            foreach(var skinnedMeshRenderer in newModel.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                var bones = skinnedMeshRenderer.bones;
                for (int i = 0; i < bones.Length; i++)
                {
                    var bone = bones[i];
                    if (bone.name.Equals("Bone01")) continue;

                    bone.parent = Utility.SearchInChildren(originalModel.transform.parent.parent, bonePrefix + bonesToBones[bones[i].name]);
                    bone.localScale = Vector3.one * 3.5f;

                    bone.localPosition = bonesLocalPositon[bone.name];

                    // Rotations are really weird
                    if (bone.name.Equals("Bip01 L Foot")) bone.localRotation = Quaternion.Euler(new Vector3(90, 270, 0));
                    else if (bone.name.Equals("Bip01 R Foot")) bone.localRotation = Quaternion.Euler(new Vector3(90, 90, 0));
                    else if (bone.name.Equals("Bip02 L Foot")) bone.localRotation = Quaternion.Euler(90, 270, 0);
                    else if (bone.name.Equals("Bip02 R Foot")) bone.localRotation = Quaternion.Euler(0, 0, 270);
                    else if (bone.name.Contains(" R ")) bone.localRotation = Quaternion.Euler(new Vector3(180f, 0f, 180f));
                    else if (bone.name.Contains(" L ")) bone.localRotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
                    else bone.localRotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
                }

                skinnedMeshRenderer.rootBone = Utility.SearchInChildren(originalModel.transform.parent.parent, bonePrefix + "TrajectorySHJnt");
                skinnedMeshRenderer.updateWhenOffscreen = true;
            }

            // Disappear old model
            originalModel.SetActive(false);
        }

        private static Dictionary<string, string> bonesToBones = new Dictionary<string, string>()
        {
            // Spine
            { "Bip01", "TrajectorySHJnt" },
            { "Bip01 Pelvis", "ROOTSHJnt" },
            { "Bip01 Spine", "ROOTSHJnt" },
            { "Bip01 Spine1", "Spine_01SHJnt" },
            { "Bip01 Spine2", "Spine_02SHJnt" },
            { "Bip01 Spine3", "Spine_TopSHJnt" },
            { "Bip01 Neck", "Neck_01SHJnt" },
            { "Bip01 Head", "Neck_01SHJnt" },

            // Left leg
            { "Bip01 L Leg", "LF_Leg_HipSHJnt" },
            { "Bip01 L Leg1", "LF_Leg_KneeSHJnt" },
            { "Bip01 L Foot", "LF_Leg_BallSHJnt" },

            // Right leg
            { "Bip01 R Leg", "RT_Leg_HipSHJnt" },
            { "Bip01 R Leg1", "RT_Leg_KneeSHJnt" },
            { "Bip01 R Foot", "RT_Leg_BallSHJnt" },

            // Left arm
            { "Bip01 L Arm", "LF_Arm_ClavicleSHJnt" },
            { "Bip01 L Arm1", "LF_Arm_ShoulderSHJnt" },
            { "Bip01 L Arm2", "LF_Arm_ElbowSHJnt" },

            // Right arm
            { "Bip01 R Arm", "RT_Arm_ClavicleSHJnt" },
            { "Bip01 R Arm1", "RT_Arm_ShoulderSHJnt" },
            { "Bip01 R Arm2", "RT_Arm_ElbowSHJnt" },

            // Left hand
            { "Bip01 L Hand", "LF_Arm_WristSHJnt" },
            { "Bip01 L Finger1", "LF_Finger_01_01SHJnt" },
            { "Bip01 L Finger11", "LF_Finger_01_02SHJnt" },
            { "Bip01 L Finger12", "LF_Finger_01_03SHJnt" },

            // Right hand
            { "Bip01 R Hand", "RT_Arm_WristSHJnt" },
            { "Bip01 R Finger1", "RT_Finger_01_01SHJnt" },
            { "Bip01 R Finger11", "RT_Finger_01_02SHJnt" },
            { "Bip01 R Finger12", "RT_Finger_01_03SHJnt" },

            // RIG 2 //

            // Spine
            { "Bip02", "TrajectorySHJnt" },
            { "Bip02 Pelvis", "ROOTSHJnt" },
            { "Bip02 Spine", "Spine_01SHJnt" },
            { "Bip02 Spine1", "Spine_02SHJnt" },
            { "Bip02 Spine2", "Spine_TopSHJnt" },
            { "Bip02 Neck", "Neck_01SHJnt" },
            { "Bip02 Head", "Neck_01SHJnt" },
            { "Bone01", "Neck_01SHJnt" },

            // Left leg
            { "Bip02 L Leg", "LF_Leg_HipSHJnt" },
            { "Bip02 L Leg1", "LF_Leg_KneeSHJnt" },
            { "Bip02 L Foot", "LF_Leg_BallSHJnt" },

            // Right leg
            { "Bip02 R Leg", "RT_Leg_HipSHJnt" },
            { "Bip02 R Leg1", "RT_Leg_KneeSHJnt" },
            { "Bip02 R Foot", "RT_Leg_BallSHJnt" },
            // Left arm
            { "Bip02 L Arm", "LF_Arm_ClavicleSHJnt" },
            { "Bip02 L Arm1", "LF_Arm_ShoulderSHJnt" },
            { "Bip02 L Arm2", "LF_Arm_ElbowSHJnt" },

            // Right arm
            { "Bip02 R Arm", "RT_Arm_ClavicleSHJnt" },
            { "Bip02 R Arm1", "RT_Arm_ShoulderSHJnt" },
            { "Bip02 R Arm2", "RT_Arm_ElbowSHJnt" },

            // Left hand
            { "Bip02 L Hand", "LF_Arm_WristSHJnt" },

            // Right hand
            { "Bip02 R Hand", "RT_Arm_WristSHJnt" },
        };

        private static Dictionary<string, Vector3> bonesLocalPositon = new Dictionary<string, Vector3>()
        {
            // Spine
            { "Bip01", Vector3.zero },
            { "Bip01 Pelvis", Vector3.zero },
            { "Bip01 Spine", new Vector3(-0.01f, -0.02f, 0f) },
            { "Bip01 Spine1", new Vector3(-0.2f, -0.1f, 0f) },
            { "Bip01 Spine2", new Vector3(-0.2f, 0f, 0f) },
            { "Bip01 Spine3", new Vector3(-0.05f, -0.05f, 0f) },
            { "Bip01 Neck", new Vector3(0.05f, 0.05f, 0f) },
            { "Bip01 Head", new Vector3(-0.07f, 0.02f, 0f) },

            // Left leg
            { "Bip01 L Leg", new Vector3(-0.1f, 0f, 0.1f) },
            { "Bip01 L Leg1", Vector3.zero },
            { "Bip01 L Foot", new Vector3(0.1f, 0, 0.1f) },

            // Right leg
            { "Bip01 R Leg", new Vector3(0.1f, 0f, 0.1f) },
            { "Bip01 R Leg1", Vector3.zero },
            { "Bip01 R Foot", new Vector3(-0.1f, 0, -0.1f) },

            // Left arm
            { "Bip01 L Arm", new Vector3(-0.05f, 0f, -0.1f) },
            { "Bip01 L Arm1", new Vector3(-0.025f, 0f, -0.1f) },
            { "Bip01 L Arm2", new Vector3(-0.15f, 0f, 0f) },

            // Right arm
            { "Bip01 R Arm", new Vector3(0.05f, 0f, -0.1f) },
            { "Bip01 R Arm1", new Vector3(0.025f, 0.1f, -0.1f) },
            { "Bip01 R Arm2", new Vector3(0.15f, 0f, 0f) },

            // Left hand
            { "Bip01 L Hand", new Vector3(-0.05f, 0f, 0f) },
            { "Bip01 L Finger1", Vector3.zero },
            { "Bip01 L Finger11", Vector3.zero },
            { "Bip01 L Finger12", Vector3.zero },

            // Right hand
            { "Bip01 R Hand", new Vector3(0.05f, 0f, 0f) },
            { "Bip01 R Finger1", Vector3.zero },
            { "Bip01 R Finger11", Vector3.zero },
            { "Bip01 R Finger12", Vector3.zero },
            
            // RIG 2 //

            // Spine
            { "Bip02", Vector3.zero },
            { "Bip02 Pelvis", Vector3.zero },
            { "Bip02 Spine", Vector3.zero },
            { "Bip02 Spine1", Vector3.zero },
            { "Bip02 Spine2", new Vector3(0.1f, -0.05f, 0f) },
            { "Bip02 Neck", Vector3.zero },
            { "Bip02 Head", Vector3.zero },
            { "Bone01", Vector3.zero },

            // Left leg
            { "Bip02 L Leg", Vector3.zero },
            { "Bip02 L Leg1", Vector3.zero },
            { "Bip02 L Foot", new Vector3(0.1f, 0f, 0.1f) },

            // Right leg
            { "Bip02 R Leg", Vector3.zero },
            { "Bip02 R Leg1", Vector3.zero },
            { "Bip02 R Foot", new Vector3(-0.1f, 0f, -0.1f) },

            // Left arm
            { "Bip02 L Arm", Vector3.zero },
            { "Bip02 L Arm1", Vector3.zero },
            { "Bip02 L Arm2", Vector3.zero },

            // Right arm
            { "Bip02 R Arm", Vector3.zero },
            { "Bip02 R Arm1", Vector3.zero },
            { "Bip02 R Arm2", Vector3.zero },

            // Left hand
            { "Bip02 L Hand", Vector3.zero },

            // Right hand
            { "Bip02 R Hand", Vector3.zero },
        };
    }
}
