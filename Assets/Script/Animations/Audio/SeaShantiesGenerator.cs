using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeaShantiesGenerator : MonoBehaviour {

    // Use this for initialization
    public List<AudioClip> clips;
    private AudioSource source;

	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!source.isPlaying)
            ChooseSong();
	}

    private void ChooseSong()
    {
        bool goodSong = false;
        int song = 0;
        while (!goodSong) {
            song = Random.Range(0, clips.Count);
            if (source.clip != clips[song])
                goodSong = true;
        }
        source.clip = clips[song];
        print("SONG " + song);
        source.Play();
    }
}
