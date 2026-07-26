using UnityEngine;
using System;
using UnityEngine.Audio;

    /*
    *   Code block taken from: 
    *   https://www.youtube.com/watch?v=6OT43pvUyfY
    */

public class audioManager : MonoBehaviour {
    public sound[] sounds;

    void Awake() {
        if (FindObjectsOfType<audioManager>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(this);
            foreach (sound s in sounds) {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        
    }

    void Start() {
    }

    public void play (string name) {
        sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void stop (string name) {
        sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void changeVolume (string name, float v) {
        sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = v;
    }

    public void changeVolume (float v) {
        foreach (sound s in sounds) {
            s.source.volume = v;
        }
    }
}
