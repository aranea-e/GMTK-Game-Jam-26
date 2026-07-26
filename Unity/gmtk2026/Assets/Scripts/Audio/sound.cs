using UnityEngine.Audio;
using UnityEngine;

    /*
    *   Code block taken from: 
    *   https://www.youtube.com/watch?v=6OT43pvUyfY
    */

[System.Serializable]
public class sound {
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(.1f, 3f)]
    public float pitch = 1.0f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}
