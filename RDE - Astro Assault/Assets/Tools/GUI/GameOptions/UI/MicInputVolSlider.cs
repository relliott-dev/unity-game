using System;
using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    [RequireComponent(typeof(Slider))]
    public class MicInputVolSlider : MonoBehaviour
    {
        [SerializeField] public Slider slider1;
        [SerializeField] public bool checkOnStart = true;
        [SerializeField] public bool isEnabled = false;
        [SerializeField] public float currVolume = 0f;
        [SerializeField] public float minDecibels = -80f;
        [SerializeField] public float maxDecibels = 25f;
        private string currMic;
        private AudioClip recordingClip;
        private int sampleSize = 128;

        void Awake()
        {
            if (GameOptions.instance == null)
            {
                Debug.Log("[MicInputVolSlider] No GameOptions component running.");
                return;
            }
            slider1.maxValue = maxDecibels;
            slider1.minValue = minDecibels;
        }

        void OnEnable()
        {
            currMic = GameOptions.instance.chosenMicrophone;
            if (checkOnStart)
                StartDetectingMic();
        }

        void Update()
        {
            //Detect Mic Change
            if (currMic != GameOptions.instance.chosenMicrophone)
            {
                StopDetectingMic(); //Stop Old One
                currMic = GameOptions.instance.chosenMicrophone;
                StartDetectingMic();
            }

            //Update Slider to Current Volume
            if (isEnabled)
            {
                if (String.IsNullOrWhiteSpace(currMic))
                    return;
                currVolume = GetMicVolume();
                slider1.value = currVolume;
            }
        }

        private float GetMicVolume()
        {
            float volume = 0;
            float[] micData = new float[sampleSize];
            int micPosition = Microphone.GetPosition(currMic) - (sampleSize + 1);
            if (micPosition < 0)
                return 0;

            recordingClip.GetData(micData, micPosition);

            //Calculate dB
            float RefValue = 0.1f;
            float sum = 0f;
            for (int i = 0; i < sampleSize; i++)
            {
                sum += micData[i] * micData[i]; //Sum Up the Samples
            }
            float rmsVal = Mathf.Sqrt(sum / sampleSize); //Square Root of Average
            volume = 20 * Mathf.Log10(rmsVal / RefValue); //Calculate dB
            volume = Mathf.Clamp(volume, -100f, 25f); //Clamp it
            return volume;
        }

        void OnDisable()
        {
            StopDetectingMic();
        }

        private void StopDetectingMic()
        {
            if (String.IsNullOrWhiteSpace(currMic))
                return;

            if (Microphone.IsRecording(currMic))
                Microphone.End(currMic);
            isEnabled = false;
        }

        public void StartDetectingMic()
        {
            if (String.IsNullOrWhiteSpace(currMic))
                return;

            //Start Recording
            recordingClip = Microphone.Start(currMic, true, 999, 44100);
            isEnabled = true;
        }

        /*void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                StartDetectingMic();
            }
            else
            {
                StopDetectingMic();
            }
        }*/
    }
}