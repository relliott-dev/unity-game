using System.Collections.Generic;
using UnityEngine;

namespace RDE
{
    public partial class GameOptions : MonoBehaviour
    {
        public void SetAudioSpeakerMode(OptionItem item)
        {
            string value = GetOptionValue(item);

            AudioConfiguration config = AudioSettings.GetConfiguration();

            if (value == "Mono")
                config.speakerMode = AudioSpeakerMode.Mono;
            if (value == "Stereo")
                config.speakerMode = AudioSpeakerMode.Stereo;
            if (value == "Quad Speakers")
                config.speakerMode = AudioSpeakerMode.Quad;
            if (value == "5.0 Surround Sound")
                config.speakerMode = AudioSpeakerMode.Surround;
            if (value == "5.1 SubWoofer")
                config.speakerMode = AudioSpeakerMode.Mode5point1;
            if (value == "7.1 Surround Sound")
                config.speakerMode = AudioSpeakerMode.Mode7point1;
            if (value == "Prologic")
                config.speakerMode = AudioSpeakerMode.Prologic;
        }

        public void SetAudioMusicVolume(OptionItem item)
        {
            if (float.TryParse(GetOptionValue(item), out float value))
            {
                AudioMixerManager.instance.MusicVolume = value;
            }
        }

        public void SetAudioSFXVolume(OptionItem item)
        {
            if (float.TryParse(GetOptionValue(item), out float value))
            {
                AudioMixerManager.instance.SFXVolume = value;
            }
        }

        public void GetMicrophones()
        {
            List<string> microphoneListTemp = new List<string>();
            for (int i = 0; i < Microphone.devices.Length; i++)
            {
                microphoneListTemp.Add(Microphone.devices[i]);
            }

            if (microphoneList != null)
            {
                microphoneList.ClearOptions();
                microphoneList.AddOptions(microphoneListTemp);
            }

            SetAudioNewMicrophone();
        }

        public void SetAudioNewMicrophone()
        {
            if (microphoneList != null)
            {
                chosenMicrophone = GetOptionValueByName(microphoneList.gameObject.name);
                if (string.IsNullOrWhiteSpace(chosenMicrophone) && Microphone.devices.Length > 0)
                {
                    chosenMicrophone = Microphone.devices[0];
                }

                for (var i = 0; i < microphoneList.options.Count; i++)
                {
                    if (microphoneList.options[i].text.ToLower() == chosenMicrophone.ToString().ToLower())
                    {
                        microphoneList.value = i;
                        break;
                    }
                }
            }
        }
    }
}