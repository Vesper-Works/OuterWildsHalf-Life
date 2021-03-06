using HarmonyLib;
using OWML.ModHelper;
using OWML.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HalfLifeOverhaul
{
    internal class AnimatedWater : MonoBehaviour
    {
        private Material material;

        private void Start()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            material.mainTextureOffset += new Vector2(0, Time.deltaTime * 0.25f);
        }
    }
    public class MainBehaviour : ModBehaviour
    {
        public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, AudioClip> Sounds = new Dictionary<string, AudioClip>();
        public static MainBehaviour instance { get; set; }

        private void Start()
        {
            instance = this;
            TitleScreenAnimation titleScreenAnimation = FindObjectOfType<TitleScreenAnimation>();
            TypeExtensions.SetValue(titleScreenAnimation, "_fadeDuration", 0);
            TypeExtensions.SetValue(titleScreenAnimation, "_gamepadSplash", false);
            TypeExtensions.SetValue(titleScreenAnimation, "_introPan", false);
            TypeExtensions.Invoke(titleScreenAnimation, "FadeInTitleLogo", new object[0]);
            TitleAnimationController titleAnimationController = UnityEngine.Object.FindObjectOfType<TitleAnimationController>();
            TypeExtensions.SetValue(titleAnimationController, "_logoFadeDelay", 0.001f);
            TypeExtensions.SetValue(titleAnimationController, "_logoFadeDuration", 0.001f);
            TypeExtensions.SetValue(titleAnimationController, "_optionsFadeDelay", 0.001f);
            TypeExtensions.SetValue(titleAnimationController, "_optionsFadeDuration", 0.001f);
            TypeExtensions.SetValue(titleAnimationController, "_optionsFadeSpacing", 0.001f);

            MeshPatcher.OnStart();
            SkeletonSwapper.OnStart();

            PatchAudio(0, true);
            LoadManager.OnCompleteSceneLoad += PatchAudio;
            LoadManager.OnCompleteSceneLoad += PatchTextures;
        }

        private void PatchTextures(OWScene originalScene, OWScene loadScene)
        {

            //All specifics should go here, regioned by their astral body
            #region Timber Hearth
            var mat = Resources.FindObjectsOfTypeAll<MeshRenderer>().First(x => x.name == "TH_Surface").material;
            mat.shader = Shader.Find("Standard");
            mat.SetFloat("_Glossiness", 0);
            mat.SetFloat("_Metallic", 0);
            mat.SetTexture("_MainTex", LoadTexture("THGrass.png"));//_MainTex
            mat.SetTexture("_Overlay1Tex", LoadTexture("RichDirt.png"));//Dirt
            mat.SetTexture("_Overlay2Tex", LoadTexture("THGrass.png"));//Grass
            mat.SetTexture("_Overlay3Leaves", LoadTexture("Cactus.png"));//_Overlay3Leaves
            #endregion

            #region EmberTwinPatch

            var emberTwinSurfaceGeometry = GameObject.Find("CaveTwin_Body/Sector_CaveTwin/Geometry_CaveTwin/OtherComponentsGroup/Geometry_Surface");
            var emberTwinUndergroundGeometry = GameObject.Find("CaveTwin_Body/Sector_CaveTwin/Geometry_CaveTwin/OtherComponentsGroup/Geometry_Underground");

            foreach (var mesh in emberTwinSurfaceGeometry.GetComponentsInChildren<MeshRenderer>())
            {
                var material = mesh.material;
                material.SetFloat("_Glossiness", 0);
                material.SetFloat("_Metallic", 0.5f);
                material.SetTexture("_MainTex", LoadTexture("HTrocks.png"));//_MainTex
                material.SetTexture("_BumpMap", LoadTexture("HTrocks.png"));//_BumpMap
                material.SetTexture("_SurfaceAlbedoTex", LoadTexture("AridGround.png"));
            }

            foreach (var mesh in emberTwinUndergroundGeometry.GetComponentsInChildren<MeshRenderer>())
            {
                var material = mesh.material;
                material.SetFloat("_Glossiness", 0);
                material.SetFloat("_Metallic", 0.5f);
                material.SetTexture("_MainTex", LoadTexture("HTrocks.png"));//_MainTex
                material.SetTexture("_BumpMap", LoadTexture("AridGround.png"));//_BumpMap
                material.SetTexture("_SurfaceAlbedoTex", LoadTexture("AridGround.png"));
            }

            var temp = GameObject.Find("CaveTwin_Body/SandSphere_Rising/SandSphere").GetComponent<TessellatedSphereRenderer>();

            Shader shader = temp.sharedMaterials[0].shader;

            temp.sharedMaterials[0].SetFloat("_Glossiness", 0);
            temp.sharedMaterials[0].SetFloat("_Metallic", 0.5f);
            for (int i = 0; i < shader.GetPropertyCount(); i++)
            {
                if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
                {
                    temp.sharedMaterial.SetTexture(shader.GetPropertyName(i), LoadTexture("WavySand.png"));
                }
            }

            shader = temp.sharedMaterials[1].shader;

            temp.sharedMaterials[1].SetFloat("_Glossiness", 0);
            temp.sharedMaterials[1].SetFloat("_Metallic", 0.5f);

            for (int i = 0; i < shader.GetPropertyCount(); i++)
            {
                if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
                {
                    temp.sharedMaterial.SetTexture(shader.GetPropertyName(i), LoadTexture("WavySand.png"));
                }
            }

            #endregion

            #region AsheTwin

            temp = GameObject.Find("TowerTwin_Body/SandSphere_Draining/SandSphere").GetComponent<TessellatedSphereRenderer>();

            shader = temp.sharedMaterials[0].shader;

            temp.sharedMaterials[0].SetFloat("_Glossiness", 0);
            temp.sharedMaterials[0].SetFloat("_Metallic", 0);
            for (int i = 0; i < shader.GetPropertyCount(); i++)
            {
                if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
                {
                    temp.sharedMaterial.SetTexture(shader.GetPropertyName(i), LoadTexture("RockySand.png"));
                }
            }

            shader = temp.sharedMaterials[1].shader;

            temp.sharedMaterials[1].SetFloat("_Glossiness", 0);
            temp.sharedMaterials[1].SetFloat("_Metallic", 0);
            for (int i = 0; i < shader.GetPropertyCount(); i++)
            {
                if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
                {
                    temp.sharedMaterial.SetTexture(shader.GetPropertyName(i), LoadTexture("RockySand.png"));
                }
            }

            GameObject.Find("SandFunnel_Body/ScaleRoot/Geo_SandFunnel/Effects_HT_SandColumn/SandColumn_Exterior").GetComponent<MeshRenderer>().material.mainTexture = LoadTexture("WavySand.png");
            GameObject.Find("SandFunnel_Body/ScaleRoot/Geo_SandFunnel/Effects_HT_SandColumn/SandColumn_Interior").GetComponent<MeshRenderer>().material.mainTexture = LoadTexture("WavySand.png");

            #endregion

            foreach (var material in Resources.FindObjectsOfTypeAll<Material>()) //Kind of cursed, but go through each material in the project, replacing their textures.
            {

                if (material.mainTexture != null)
                {
                    if (material.ToString().ToLower().Contains("trunk") ||
                    material.ToString().ToLower().Contains("tree") ||
                    material.ToString().ToLower().Contains("bark") ||
                    material.ToString().ToLower().Contains("vine"))
                    {

                        SetTextures(material, LoadTexture("RichTree.png"));
                    }
                    if (material.ToString().ToLower().Contains("leaves") || material.ToString().ToLower().Contains("leaf"))
                    {
                        SetTextures(material, LoadTexture("Cactus.png"));
                    }
                    if (material.ToString().ToLower().Contains("cactus"))
                    {
                        SetTextures(material, LoadTexture("Cactus.png"));
                    }
                    if (material.ToString().ToLower().Contains("plank") ||
                        material.ToString().ToLower().Contains("cabin"))
                    {
                        SetTextures(material, LoadTexture("Tree.png"));
                    }
                    if (material.ToString().ToLower().Contains("cliff") ||
                          material.ToString().ToLower().Contains("rock") ||
                          material.ToString().ToLower().Contains("stone") ||
                          material.ToString().ToLower().Contains("terrain_th_geyser"))
                    {
                        SetTextures(material, LoadTexture("MarbleClifface.png"));
                    }
                    if (material.ToString().ToLower().Contains("bh"))
                    {
                        SetTextures(material, LoadTexture("BHSurface.png"));
                    }
                    if (material.ToString().ToLower().Contains("ice") && !material.ToString().ToLower().Contains("coreslice"))
                    {
                        SetTextures(material, LoadTexture("Ice.png"));
                    }
                    if (material.ToString().ToLower().Contains("metal"))
                    {
                        SetTextures(material, LoadTexture("Metal.png"));
                    }
                    if (material.ToString().ToLower().Contains("snow"))
                    {
                        SetTextures(material, LoadTexture("Snow.png"));
                    }
                    if (material.ToString().ToLower().Contains("hct_surface"))
                    {
                        //material.SetTexture("_MainTex", LoadTexture("HTrocks.png"));
                        //material.SetTexture("_SurfaceAlbedoTex", LoadTexture("AridGround.png"));
                    }

                    if (material.ToString().ToLower().Contains("craterfloor"))
                    {
                        SetTextures(material, LoadTexture("THGrass.png"));
                        //material.gameObject.AddComponent<AnimatedWater>();
                    }
                }
            }

            foreach (var item in Resources.FindObjectsOfTypeAll<MeshRenderer>()) //This targets specifically water so I can add the animation to it.
            {
                if (item.ToString().ToLower().Contains("water") ||
            item.ToString().ToLower().Contains("ocean") ||
            item.ToString().ToLower().Contains("tornado"))
                {
                    SetTextures(item.material, LoadTexture("Water.png"));
                    item.gameObject.AddComponent<AnimatedWater>();
                }
            }

            foreach (var text in Resources.FindObjectsOfTypeAll<UnityEngine.UI.Text>()) //Make all UI text yellow
            {
                text.color = Color.yellow;
            }
        }

        private void SetTextures(Material mat, Texture2D texture)
        {
            mat.color = Color.white;
            mat.shader = Shader.Find("Standard");
            mat.SetFloat("_Glossiness", 0);
            mat.SetFloat("_Metallic", 0.5f);
            mat.SetTexture("_MainTex", texture);

        }
        public static Texture2D LoadTexture(string path)
        {
            if (instance.Textures.ContainsKey(path)) { return instance.Textures[path]; } //Texture already loaded, get from memory

            //Else, load the image from file
            Texture2D texture = null;

            byte[] imageBytes;
            string imagePath = instance.ModHelper.Manifest.ModFolderPath + path;
            if (File.Exists(imagePath))
            {
                texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                imageBytes = File.ReadAllBytes(imagePath);
                texture.LoadImage(imageBytes);
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.filterMode = FilterMode.Point;

                instance.Textures.Add(path, texture);
                return texture;
            }
            else
            {
                instance.ModHelper.Console.WriteLine("Cannot find file: " + imagePath);
                return LoadTexture("MissingTexture.png");
            }
        }
        private void PatchAudio(OWScene originalScene, OWScene loadScene)
        {
            StartCoroutine(PatchAudio(1, false)); //Have to load audio with delay for some reason
        }
        private IEnumerator PatchAudio(float delay, bool justLoadAudio)
        {
            yield return new WaitForSecondsRealtime(delay);

            //In OuterWilds, everything that makes a sound is an AudioType

            //This array is all the audio types that we're going to change
            AudioType[] audioTypesToPatch =
{
                AudioType.MovementDirtFootstep,
                AudioType.MovementMetalFootstep,
                AudioType.MovementGlassFootsteps,
                AudioType.MovementGrassFootstep,
                AudioType.MovementIceFootstep,
                AudioType.MovementSnowFootstep,
                AudioType.MovementStoneFootstep,
                AudioType.MovementNomaiMetalFootstep,
                AudioType.MovementSandFootstep,
                AudioType.MovementWoodFootstep,
                AudioType.MovementShallowWaterFootstep,
                AudioType.LandingSand,
                AudioType.HT_InsideSandfall_Ship_LP,
                AudioType.HT_InsideSandfall_Suit_LP,
                AudioType.HT_SandColumnEnd_LP,
                AudioType.HT_SandColumnStart_LP,
                AudioType.HT_SandfallSmallBottom_LP,
                AudioType.HT_SandRiver_LP,
                AudioType.ImpactLowSpeed,
                AudioType.ImpactMediumSpeed,
                AudioType.ImpactHighSpeed,
                AudioType.ImpactUnderwater,
                AudioType.PlayerBreathing_LowOxygen_LP,
                AudioType.PlayerBreathing_LowOxygen_LP,
                AudioType.PlayerSuitCriticalWarning,
                AudioType.Death_BigBang,
                AudioType.Death_Crushed,
                AudioType.Death_Digestion,
                AudioType.Death_Energy,
                AudioType.Death_Instant,
                AudioType.Death_Lava,
                AudioType.Death_Self,
                AudioType.Death_TimeLoop,
                AudioType.ElectricShock,
                AudioType.ShipDamageSingleElectricSpark,
                AudioType.ShipDamageElectricSparking_LP,
                AudioType.ShipCabinUseMedkit,
                AudioType.ShipCabinUseRefueller,
                AudioType.PlayerSuitWearSuit,
                AudioType.BH_SubsurfaceAmbience_LP,
                AudioType.BH_SurfaceAmbience_LP,
                AudioType.BlackHoleAmbience_LP,
                AudioType.CometAmbience_LP,
                AudioType.DB_Ambience_LP,
                AudioType.DB_Ambient,
                AudioType.EyeAmbience_LP,
                AudioType.GD_CaveAmbience_LP,
                AudioType.GD_CoreAmbient_LP,
                AudioType.GD_RainAmbient_LP,
                AudioType.GD_UnderwaterAmbient_LP,
                AudioType.HT_CaveAmbientBig_LP,
                AudioType.HT_CaveAmbientSmall_LP,
                AudioType.HT_SurfaceAmbience_LP,
                AudioType.NomaiComputerAmbient,
                AudioType.NomaiGravCrystalAmbient_LP,
                AudioType.NomaiGravCrystalFlickerAmbient_LP,
                AudioType.NomaiGravityCannonAmbient_LP,
                AudioType.NomaiRecorderAmbient_LP,
                AudioType.NomaiRemoteCameraAmbient_LP,
                AudioType.NomaiTractorBeamAmbient_LP,
                AudioType.QM_Ambient,
                AudioType.QuantumAmbience_LP,
                AudioType.ShipCabinAmbience,
                AudioType.ShipCabinComputerAmbient_LP,
                AudioType.ShipCockpitLandingCamAmbient_LP,
                AudioType.ShipLogAmbience_LP,
                AudioType.Sun_Ambience_LP,
                AudioType.TH_CanyonAmbienceDay_LP,
                AudioType.TH_CanyonAmbienceNight_LP,
                AudioType.TH_HiAltitudeAmbienceDay_LP,
                AudioType.TH_HiAltitudeAmbienceNight_LP,
                AudioType.TH_MuseumAmbience_LP,
                AudioType.TH_UnderwaterAmbience_LP,
                AudioType.TH_ZeroGCaveAmbient_LP,
                AudioType.TimeLoopDevice_Ambient,
                AudioType.VesselAmbience_LP,
                AudioType.WhiteHoleAmbience_LP,
                AudioType.PlayerBreathing_LP,
                AudioType.PlayerSuitJetpackBoost,
                AudioType.PlayerSuitJetpackThrustTranslational_LP,
                AudioType.PlayerSuitJetpackOxygenPropellant_LP,
                AudioType.ShipThrustAfterburn_LP,
                AudioType.ShipThrustIgnition,
                //AudioType.ShipThrustRotational,
                AudioType.ShipThrustTranslational_LP,
                AudioType.ShipReentryBurn_LP,
                AudioType.ToolFlashlightOn,
                AudioType.ToolFlashlightOff,
                AudioType.Menu_UpDown,
                AudioType.Menu_ChangeTab,
                AudioType.Menu_LeftRight,
                AudioType.Menu_Pause,
                AudioType.Menu_RebindKey,
                AudioType.Menu_Unpause,
                AudioType.Menu_SliderIncrement,
                AudioType.Menu_RebindKey,
                AudioType.Menu_ResetDefaults,
               
                //
                //
                AudioType.TH_Village,
                AudioType.Travel_Theme, //Space
                AudioType.SunStation,
                AudioType.EndOfTime,
                AudioType.EYE_EndOfGame,
                AudioType.FinalCredits,

            };

            //All the names of the files we're gonna use, the indexes MUST match. Use multiple files for variation (I think)
            string[][] audioFilesToUse =
            {
                 new string[]{ "pl_dirt1.wav", "pl_dirt2.wav","pl_dirt3.wav","pl_dirt4.wav" },
                 new string[]{ "pl_metal1.wav", "pl_metal2.wav", "pl_metal3.wav", "pl_metal4.wav" },
                  new string[]{ "pl_metal1.wav", "pl_metal2.wav", "pl_metal3.wav", "pl_metal4.wav" },
                 new string[]{ "pl_dirt1.wav", "pl_dirt2.wav","pl_dirt3.wav","pl_dirt4.wav" },
                 new string[]{ "pl_step1.wav", "pl_step2.wav", "pl_step3.wav", "pl_step4.wav" },
                 new string[]{ "pl_step1.wav", "pl_step2.wav", "pl_step3.wav", "pl_step4.wav"},
                 new string[]{ "pl_tile1.wav", "pl_tile2.wav", "pl_tile3.wav", "pl_tile4.wav"},
                 new string[]{ "pl_metal1.wav", "pl_metal2.wav", "pl_metal3.wav", "pl_metal4.wav"},
                 new string[]{ "pl_dirt1.wav", "pl_dirt2.wav","pl_dirt3.wav","pl_dirt4.wav" },
                 new string[]{ "pl_step1.wav", "pl_step2.wav", "pl_step3.wav", "pl_step4.wav" },
                 new string[]{ "pl_wade1.wav", "pl_wade2.wav", "pl_wade3.wav", "pl_wade4.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "sandfall1.wav", "sandfall2.wav" },
                 new string[]{ "pl_jumpland2.wav" },
                 new string[]{ "pl_fallpain1.wav","pl_fallpain2.wav","pl_fallpain3.wav" },
                 new string[]{ "scream01.wav" },
                 new string[]{ "pl_slosh1.wav","pl_slosh2.wav","pl_slosh3.wav","pl_slosh4.wav" },
                 new string[]{ "breathe1.wav" },
                 new string[]{ "breathe1.wav" },
                 new string[]{ "health_critical.wav", },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "flatline.wav" },
                 new string[]{ "shock_damage.wav" },
                 new string[]{ "spark1.wav" },
                 new string[]{ "spark1.wav","spark2.wav","spark3.wav","spark4.wav","spark5.wav","spark6.wav" },
                 new string[]{ "medshotno1.wav" },
                 new string[]{ "airtank1.wav" },
                 new string[]{ "gunpickup1.wav" },
                 new string[]{ "rocket_groan4LP.wav"},
                 new string[]{ "distantmortar1LP.wav" },
                 new string[]{ "dronemachine3LP.wav" },
                 new string[]{ "aliencave1LP.wav" },
                 new string[]{ "DBLP.wav" },
                 new string[]{ "disgusting.wav", "deadsignal1.wav", "deadsignal2.wav","alienclicker1.wav" },
                 new string[]{ "port_suckin1.wav", "port_suckout1.wav" },
                 new string[]{ "des_wind2LP.wav" },
                 new string[]{ "labdrone2LP.wav" },
                 new string[]{ "waterfall2LP.wav" },
                 new string[]{ "alien_purrmachineLP.wav" },
                 new string[]{ "aliencave1LP.wav" },
                 new string[]{ "squirm2LP.wav" },
                 new string[]{ "wind2LP.wav" },
                 new string[]{ "computalk1.wav", "computalk2.wav" },
                 new string[]{ "labdrone1LP.wav" },
                 new string[]{ "hawk1.wav"},
                 new string[]{ "mechwhineLP.wav" },
                 new string[]{ "techamb1LP.wav" },
                 new string[]{ "techamb2LP.wav" },
                 new string[]{ "zapmachineLP.wav" },
                 new string[]{ "alienwind1.wav", "alienwind2.wav" },
                 new string[]{ "alienwind1LP.wav", "alienwind2LP.wav" },
                 new string[]{ "mechwhine.wav" },
                 new string[]{ "littlemachineLP.wav" },
                 new string[]{ "crtnoiseLP.wav" },
                 new string[]{ "littlemachineLP.wav" },
                 new string[]{ "bigwarningLP.wav" },
                 new string[]{ "wind2LP.wav" },
                 new string[]{ "wind2LP.wav" },
                 new string[]{ "wind2LP.wav"},
                 new string[]{ "wind2LP.wav"},
                 new string[]{ "alien_beaconLP.wav"},
                 new string[]{ "aliencave1LP.wav"},
                 new string[]{ "pounderLP.wav"},
                 new string[]{ "pounder.wav"},
                 new string[]{ "alien_twowLP.wav"},
                 new string[]{ "screammachineLP.wav"},
                 new string[]{ "breathe1LP.wav"},
                 new string[]{ "rocketflame1LP.wav"},
                 new string[]{ "rocketflame1LP.wav"},
                 new string[]{ "rocket_steam1.wav"},
                 new string[]{ "rocket1LP.wav"},
                 new string[]{ "egon_windup2.wav"},
                 //new string[]{ "cbar_miss1.wav"},
                 new string[]{ "rocketflame1LP.wav"},
                 new string[]{ "egon_off1LP.wav"},
                 new string[]{ "flashlight1.wav"},
                 new string[]{ "flashlight1.wav"},
                 new string[]{ "buttonclickrelease.wav"},
                 new string[]{ "buttonrollover.wav"},
                 new string[]{ "buttonclick.wav"},
                 new string[]{ "buttonclick.wav"},
                 new string[]{ "buttonclick.wav"},
                 new string[]{ "buttonclickrelease.wav"},
                 new string[]{ "buttonrollover.wav"},
                 new string[]{ "buttonclick.wav"},
                 new string[]{ "buttonclick.wav"},
            //
            //
                 new string[]{ "Half-Life17.mp3"},
                 new string[]{ "Prospero01.mp3"},
                 new string[]{ "Half-Life05.mp3"},
                 new string[]{ "Half-Life10.mp3"},
                 new string[]{ "Half-Life13.mp3"},
                 new string[]{ "Half-Life17.mp3"},
            };



            if (justLoadAudio)
            {
                for (int i = 0; i < audioFilesToUse.Length; i++)
                {
                    GetClips(audioFilesToUse[i]);
                }
                yield break;
            }

            //The audio dictionary is a dictionary containing all of the sounds, matched to the int value of the AudioType enum
            Dictionary<int, AudioLibrary.AudioEntry> audioDictionary = ((Dictionary<int, AudioLibrary.AudioEntry>)AccessTools.Field(typeof(AudioManager), "_audioLibraryDict").GetValue(Locator.GetAudioManager()));

            for (int i = 0; i < audioFilesToUse.Length; i++)
            {
                try
                {
                    audioDictionary[(int)audioTypesToPatch[i]] = new AudioLibrary.AudioEntry(audioTypesToPatch[i], GetClips(audioFilesToUse[i]), 0.5f);
                }
                catch
                {
                    audioDictionary.Add((int)audioTypesToPatch[i], new AudioLibrary.AudioEntry(audioTypesToPatch[i], GetClips(audioFilesToUse[i]), 0.5f));
                }
            }
        }
        private AudioClip[] GetClips(string[] names)
        {
            AudioClip[] clips = new AudioClip[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                clips[i] = GetClip(names[i]);
            }
            return clips;
        }
        private AudioClip GetClip(string name)
        {
            if (instance.Sounds.ContainsKey(name)) { return instance.Sounds[name]; } //If it's already loaded, give the one in memory
            AudioClip audioClip = ModHelper.Assets.GetAudio(name); //Else load it
            instance.Sounds.Add(name, audioClip);
            return audioClip;
        }
    }
}
