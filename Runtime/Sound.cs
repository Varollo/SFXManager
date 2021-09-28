using System;
using UnityEngine;

namespace Varollo.SFXManager
{
    /// <summary>
    /// A Sound object represents a AudioClip and it's configurations.
    /// </summary>
    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
        [Space]
        [Range(0, 256)] public int Priority = 128;
        [Range(0, 1)] public float Volume = 1f;
        [Range(-3, 3)] public float Pitch = 1f;
        [Range(-1, 1)] public float PanStereo = 0f;
        [Space]
        public bool Loop = false;
    }
}
