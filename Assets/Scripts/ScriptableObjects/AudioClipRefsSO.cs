using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    [CreateAssetMenu(menuName = "ScriptableObject/AudioClipRefsSO")]
    public class AudioClipRefsSO : ScriptableObject
    {
        public AudioClip[] chop;
        public AudioClip[] delivery_fail;
        public AudioClip[] delivery_success;
        public AudioClip[] footstep;
        public AudioClip[] object_drop;
        public AudioClip[] object_pickup;
        public AudioClip[] pan_sizzle_loop;
        public AudioClip[] trash;
        public AudioClip[] warning;
    }
}
