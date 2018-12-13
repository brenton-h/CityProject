using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour {

    public AudioClip Clip;
    public AudioClip Clip2;

    private AudioSource source;

	void Start () {
        source = GetComponent<AudioSource>();
	}
    bool play = false;
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying) {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
                if (play) {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                        source.PlayOneShot(Clip2);
                    } else {
                        source.PlayOneShot(Clip);
                    }
                } else Invoke("startClip", .9f);
                
            } else play = false;
        }
	}


    void startClip() {
        play = true;
    }
}
