using UnityEngine;

namespace RDE
{
    public partial class GameOptions : MonoBehaviour
    {
        private string globalQualityLowName = "Low";
        private string globalQualityMediumName = "Normal";
        private string globalQualityHighName = "High";
        private string globalQualityVeryHighName = "Ultra";

        private void ProjectQuality(string quality)
        {
            for (var i = 0; i < QualitySettings.names.Length; i++)
            {
                if (QualitySettings.names[i].ToString() == quality)
                {
                    QualitySettings.SetQualityLevel(i);
                }
            }
        }

        public void SetGFXProjectQuality(OptionItem item)
        {
            string value = GetOptionValue(item);

            if (value == "Low")
            {
                ProjectQuality(globalQualityLowName);
                SetOptionValueByName("Model Quality", "Low");
                SetOptionValueByName("Texture Quality", "Low");
                SetOptionValueByName("Shadow Quality", "Low");
                SetOptionValueByName("V-Sync", "Off");
                SetOptionValueByName("Render Distance", "Low");
                SetOptionValueByName("Anisotropic Filtering", "Low");
                SetGFXPixelLightCount("Low");
                SetGFXReflectionProbes("Low");
            }
            else if (value == "Medium")
            {
                ProjectQuality(globalQualityMediumName);
                SetOptionValueByName("Model Quality", "Medium");
                SetOptionValueByName("Texture Quality", "Medium");
                SetOptionValueByName("Shadow Quality", "Medium");
                SetOptionValueByName("V-Sync", "1x");
                SetOptionValueByName("Render Distance", "Medium");
                SetOptionValueByName("Anisotropic Filtering", "Medium");
                SetGFXPixelLightCount("Medium");
                SetGFXReflectionProbes("Medium");
            }
            else if (value == "High")
            {
                ProjectQuality(globalQualityHighName);
                SetOptionValueByName("Model Quality", "High");
                SetOptionValueByName("Texture Quality", "High");
                SetOptionValueByName("Shadow Quality", "High");
                SetOptionValueByName("V-Sync", "2x");
                SetOptionValueByName("Render Distance", "High");
                SetOptionValueByName("Anisotropic Filtering", "High");
                SetGFXPixelLightCount("High");
                SetGFXReflectionProbes("High");
            }
            else if (value == "Very High")
            {
                ProjectQuality(globalQualityVeryHighName);
                SetOptionValueByName("Model Quality", "Very High");
                SetOptionValueByName("Texture Quality", "Very High");
                SetOptionValueByName("Shadow Quality", "Very High");
                SetOptionValueByName("V-Sync", "3x");
                SetOptionValueByName("Render Distance", "Very High");
                SetOptionValueByName("Anisotropic Filtering", "Very High");
                SetGFXPixelLightCount("Very High");
                SetGFXReflectionProbes("Very High");
            }
        }

        public void SetGFXModelQuality(OptionItem item)
        {
            string value = GetOptionValue(item);

            if (value == "Low")
            {
                QualitySettings.lodBias = 0.4f;
                QualitySettings.maximumLODLevel = 1;
                QualitySettings.skinWeights = SkinWeights.OneBone;
            }
            else if (value == "Medium")
            {
                QualitySettings.lodBias = 1f;
                QualitySettings.maximumLODLevel = 1;
                QualitySettings.skinWeights = SkinWeights.TwoBones;
            }
            else if (value == "High")
            {
                QualitySettings.lodBias = 2f;
                QualitySettings.maximumLODLevel = 0;
                QualitySettings.skinWeights = SkinWeights.FourBones;
            }
            else if (value == "Very High")
            {
                QualitySettings.lodBias = 3f;
                QualitySettings.maximumLODLevel = 0;
                QualitySettings.skinWeights = SkinWeights.Unlimited;
            }
        }

        public void SetGFXTextureQuality(OptionItem item)
        {
            string value = GetOptionValue(item);

            if (value == "Low")
            {
                QualitySettings.globalTextureMipmapLimit = 2;
                QualitySettings.streamingMipmapsMemoryBudget = 512;
            }
            else if (value == "Medium")
            {
                QualitySettings.globalTextureMipmapLimit = 1;
                QualitySettings.streamingMipmapsMemoryBudget = 1024;
            }
            else if (value == "High")
            {
                QualitySettings.globalTextureMipmapLimit = 0;
                QualitySettings.streamingMipmapsMemoryBudget = 2048;
            }
            else if (value == "Very High")
            {
                QualitySettings.globalTextureMipmapLimit = 0;
                QualitySettings.streamingMipmapsMemoryBudget = SystemInfo.graphicsMemorySize;
            }
        }

        public void SetGFXShadowQuality(OptionItem item)
        {
            string value = GetOptionValue(item);

            if (value == "Low")
            {
                QualitySettings.shadowResolution = ShadowResolution.Low;
                QualitySettings.shadowDistance = 20;
            }
            if (value == "Medium")
            {
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                QualitySettings.shadowDistance = 50;
            }
            if (value == "High")
            {
                QualitySettings.shadowResolution = ShadowResolution.High;
                QualitySettings.shadowDistance = 100;
            }
            if (value == "Very High")
            {
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                QualitySettings.shadowDistance = 200;
            }
        }

        public void SetGFXVSync(OptionItem item)
        {
            string value = GetOptionValue(item);

            if (value == "Off")
                QualitySettings.vSyncCount = 0;
            if (value == "x1")
                QualitySettings.vSyncCount = 1;
            if (value == "x2")
                QualitySettings.vSyncCount = 2;
            if (value == "x3")
                QualitySettings.vSyncCount = 3;
        }

        public void SetGFXRenderDistance(OptionItem item)
        {
            string value = GetOptionValue(item);
            float camDistance = 1000f;
            if (value == "Low")
                camDistance = 800f;
            if (value == "Medium")
                camDistance = 1500f;
            if (value == "High")
                camDistance = 2500f;
            if (value == "Very High")
                camDistance = 5000f;

            var foundCams = FindObjectsOfType<Camera>();
            for (var i = 0; i < foundCams.Length; i++)
            {
                foundCams[i].farClipPlane = camDistance;
            }
        }

        public void SetGFXAnisotropicFiltering(OptionItem item)
        {
            string value = GetOptionValue(item);

            if (value == "Low")
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            if (value == "Medium")
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            if (value == "High")
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            if (value == "Very High")
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }

        public void SetGFXPixelLightCount(string value)
        {
            if (value == "Low")
                QualitySettings.pixelLightCount = 0;
            if (value == "Medium")
                QualitySettings.pixelLightCount = 1;
            if (value == "High")
                QualitySettings.pixelLightCount = 2;
            if (value == "Very High")
                QualitySettings.pixelLightCount = 4;
        }

        public void SetGFXReflectionProbes(string value)
        {
            if (value == "Low")
                QualitySettings.realtimeReflectionProbes = false;
            if (value == "Medium")
                QualitySettings.realtimeReflectionProbes = false;
            if (value == "High")
                QualitySettings.realtimeReflectionProbes = true;
            if (value == "Very High")
                QualitySettings.realtimeReflectionProbes = true;
        }

        public void SetGFXBrightness(OptionItem item)
        {
            float value;
            if (float.TryParse(GetOptionValue(item), out value))
            {
                Screen.brightness = value;
            }
        }
    }
}