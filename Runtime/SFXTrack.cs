using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Varollo.SFXManager
{
    /// <summary>
    /// A SFXTrack object holds a Audio Source and Sounds and applies their configurations.
    /// </summary>
    [Serializable]
    public class SFXTrack
    {
        public string Name;
        public AudioMixerGroup AudioMixerGroup;
        public List<Sound> Sounds = new List<Sound>();
        [HideInInspector] public AudioSource audioSource;

        /// <summary>
        /// Play a sound on this track
        /// </summary>
        public void PlaySound(Sound sound)
        {
            CheckForTrackInstance();

            AssignAudio(sound);

            audioSource.Play();
        }

        private void AssignAudio(Sound sound)
        {
            audioSource.bypassReverbZones = true;
            audioSource.outputAudioMixerGroup = AudioMixerGroup;

            audioSource.clip = sound.Clip;
            audioSource.priority = sound.Priority;
            audioSource.volume = sound.Volume;
            audioSource.pitch = sound.Pitch;
            audioSource.loop = sound.Loop;
            audioSource.panStereo = sound.PanStereo;
        }

        private void CheckForTrackInstance()
        {
            if (!audioSource)
            {
                CreateTrackInstance();
            }
        }

        private void CreateTrackInstance()
        {
            var trackInstance = new GameObject($"[SFX] {Name} Track");
            audioSource = trackInstance.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(audioSource);
        }
    }
}
