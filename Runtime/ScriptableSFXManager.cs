using System.Collections.Generic;
using UnityEngine;

namespace Varollo.SFXManager
{
    /// <summary>
    /// Scriptable Object to hold Tracks with Sounds.
    /// </summary>
    [CreateAssetMenu(fileName = "SFX Manager", menuName = "Varollo/SFX Manager/New SFX Manager", order = 0)]
    public class ScriptableSFXManager : ScriptableObject
    {
        [SerializeField] private SFXTrack[] tracks;
        [Header("Debug")]
        [SerializeField] private bool enableLogs;

        private Dictionary<string, Dictionary<string, Sound>> SoundDictionary { get; set; }
        private Dictionary<string, SFXTrack> TrackDictionary { get; set; }

        private void CreateSoundDictionary()
        {
            SoundDictionary = new Dictionary<string, Dictionary<string, Sound>>();

            foreach (var track in tracks)
            {
                Dictionary<string, Sound> trackDictionary = new Dictionary<string, Sound>();
                foreach (var sound in track.Sounds)
                {
                    trackDictionary.Add(sound.Name, sound);
                }

                SoundDictionary.Add(track.Name, trackDictionary);
            }
        }

        private void CreateTrackDictionary()
        {
            TrackDictionary = new Dictionary<string, SFXTrack>();

            foreach (var track in tracks)
            {
                TrackDictionary.Add(track.Name, track);
            }
        }

        /// <summary>
        /// Play a Sound from a Track.
        /// </summary>
        public void Play(string trackName, string soundName)
        {
            if (!TryGetSoundByName(trackName, soundName, out var sound)) return;

            if (!TryGetTrack(trackName, out var track)) return;

            track.PlaySound(sound);
        }

        /// <summary>
        /// Play a Sound from a Track.
        /// </summary>
        /// <param name="track_sound">track[ = trackName]_sound[ = soundName]</param>
        public void Play(string track_sound)
        {
            var track = track_sound.Substring(0, track_sound.IndexOf('_'));
            var sound = track_sound.Substring(track_sound.IndexOf('_') + 1);
            Play(track, sound);
        }

        /// <summary>
        /// Returns a Sound from a Track
        /// </summary>
        public Sound GetSoundByName(string trackName, string soundName)
        {
            CheckForSoundDictionary();

            return SoundDictionary[trackName][soundName];
        }

        /// <summary>
        /// Tries to get a sound from a track.
        /// </summary>
        /// <param name="sound">Sound got</param>
        /// <returns>Returns wheater found the sound or not</returns>
        public bool TryGetSoundByName(string trackName, string soundName, out Sound sound)
        {
            CheckForSoundDictionary();

            sound = new Sound();

            if (!SoundDictionary.TryGetValue(trackName, out var track))
            {
                Debug.LogError($"[SFX] No track with name '{trackName}' found on {name}");
                return false;
            }
            if (!track.TryGetValue(soundName, out sound))
            {
                Debug.LogError($"[SFX] No sound with name '{soundName}' found on {name}");
                return false;
            }

            return true;
        }

        private void CheckForSoundDictionary()
        {
            if (SoundDictionary == null)
            {
                CreateSoundDictionary();

                if (enableLogs)
                {
                    Debug.Log($"[SFX] {name}'s sound dictionary generated.");
                }
            }
        }

        private void CheckForTrackDictionary()
        {
            if (TrackDictionary == null)
            {
                CreateTrackDictionary();

                if (enableLogs)
                {
                    Debug.Log($"[SFX] {name}'s track dictionary generated.");
                }
            }
        }

        /// <summary>
        /// Returns a Track.
        /// </summary>
        public SFXTrack GetTrack(string trackName)
        {
            CheckForTrackDictionary();

            return TrackDictionary[trackName];
        }

        /// <summary>
        /// Tries to get a Track.
        /// </summary>
        /// <param name="track">Track got</param>
        /// <returns>Returns wheater found the Track or not</returns>
        public bool TryGetTrack(string trackName, out SFXTrack track)
        {
            CheckForTrackDictionary();

            if (!TrackDictionary.TryGetValue(trackName, out track))
            {
                Debug.LogError($"[SFX] No track with name '{trackName}' found on {name}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add a new track to this SFX Manager instance.
        /// </summary>
        public void AddTrack(SFXTrack track)
        {
            CheckForTrackDictionary();
            TrackDictionary.Add(track.Name, track);
        }

        /// <summary>
        /// Add a new Sound to a track on this SFX Manager Instance.
        /// </summary>
        public void AddSound(Sound sound, string trackName)
        {
            CheckForSoundDictionary();
            if (!TryGetTrack(trackName, out var track))
            {
                SoundDictionary[trackName].Add(sound.Name, sound);
            }

        }

        /// <summary>
        /// Add a new track and sound to this SFX Manager Instance
        /// </summary>
        public void AddSoundToNewTrack(Sound sound, SFXTrack track)
        {
            AddTrack(track);
            AddSound(sound, track.Name);
        }
    }
}