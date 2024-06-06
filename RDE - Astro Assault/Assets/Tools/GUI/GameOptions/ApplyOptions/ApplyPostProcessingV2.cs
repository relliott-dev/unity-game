using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

namespace RDE
{
    public partial class GameOptions : MonoBehaviour
    {
        [Header("PostProcessing Profiles")]
        public List<PostProcessProfile> postProcessing;

        public void SetGFXMotionBlur(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<MotionBlur>() != null)
                    {
                        postProcessing[i].GetSetting<MotionBlur>().active = value;
                    }
                }
            }
        }

        public void SetGFXBloom(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<Bloom>() != null)
                    {
                        postProcessing[i].GetSetting<Bloom>().active = value;
                    }
                }
            }
        }

        public void SetGFXChromaticAbberation(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<ChromaticAberration>() != null)
                    {
                        postProcessing[i].GetSetting<ChromaticAberration>().active = value;
                    }
                }
            }
        }

        public void SetGFXVignette(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<Vignette>() != null)
                    {
                        postProcessing[i].GetSetting<Vignette>().active = value;
                    }
                }
            }
        }

        public void SetGFXAutoExposure(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<AutoExposure>() != null)
                    {
                        postProcessing[i].GetSetting<AutoExposure>().active = value;
                    }
                }
            }
        }

        public void SetGFXGamma(OptionItem item)
        {
            float value;
            if (float.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<ColorGrading>() != null)
                    {
                        Vector4 temp = new Vector4(0f, 0f, 0f, value);
                        postProcessing[i].GetSetting<ColorGrading>().gamma.value = temp;
                    }
                }
            }
        }

        public void SetGFXContrast(OptionItem item)
        {
            float value;
            if (float.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<ColorGrading>() != null)
                    {
                        FloatParameter temp1 = new FloatParameter { value = value, overrideState = true };
                        postProcessing[i].GetSetting<ColorGrading>().contrast = temp1;
                    }
                }
            }
        }

        public void SetGFXAmbientOcclusion(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<AmbientOcclusion>() != null)
                    {
                        postProcessing[i].GetSetting<AmbientOcclusion>().active = value;
                    }
                }
            }
        }

        public void SetGFXScreenSpaceReflections(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<ScreenSpaceReflections>() != null)
                    {
                        postProcessing[i].GetSetting<ScreenSpaceReflections>().active = value;
                    }
                }
            }
        }

        public void SetGFXDepthOfField(OptionItem item)
        {
            bool value;
            if (bool.TryParse(GetOptionValue(item), out value))
            {
                for (int i = 0; i < postProcessing.Count; i++)
                {
                    if (postProcessing[i].GetSetting<DepthOfField>() != null)
                    {
                        postProcessing[i].GetSetting<DepthOfField>().active = value;
                    }
                }
            }
        }

        public void SetGFXAntiAliasingType(OptionItem item)
        {
            string aaType = GetOptionValue(item);

            Camera[] foundCams = FindObjectsOfType<Camera>();
            for (var i = 0; i < foundCams.Length; i++)
            {
                if (foundCams[i].GetComponent<PostProcessLayer>() != null)
                {
                    if (aaType == "None")
                    {
                        foundCams[i].GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.None;
                    }
                    if (aaType == "MSAA" || aaType == "Normal")
                    {
                        foundCams[i].GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    }
                    if (aaType == "TXAA" || aaType == "Ultra" || aaType == "Normal")
                    {
                        foundCams[i].GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                    }
                    if (aaType == "FAA" || aaType == "Low")
                    {
                        foundCams[i].GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                    }
                }
            }
        }
    }
}