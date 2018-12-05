using UnityEngine;
using System.Collections;


public class PlaySounds2 : MonoBehaviour {
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip WindSound;

    private AudioClip currentMusic;
    private AudioSource audioAudioSource;

    public static bool StartMusic { get; set; }
    private Section currentSection;

    void Start() {
        StartMusic = false;
        audioAudioSource = GetComponent<AudioSource>();
        currentMusic = music1;
        currentSection = Section.City;//LayerController.Self.ClosestPoint;
    }

    void Update() {
        if (StartMusic) {
            if (!audioAudioSource.isPlaying) {
                audioAudioSource.PlayOneShot(currentMusic);
            }
            if (currentSection != LayerController.Self.ClosestPoint) {
                if (LayerController.Self.ClosestPoint == Section.Forrest) { // if entering the forrest
                    currentMusic = music2;
                    audioAudioSource.Stop();
                    audioAudioSource.PlayOneShot(currentMusic);
                } else if (currentSection == Section.Forrest) { // if exiting the forrest
                    currentMusic = music1;
                    audioAudioSource.Stop();
                    audioAudioSource.PlayOneShot(currentMusic);
                }
                currentSection = LayerController.Self.ClosestPoint;
            }
        }// else StartCoroutine(Start_Music());
        //if (LayerController.Self.ClosestPoint == Section.Park) StartMusic = true;
        



            /*
            if (!audioAudioSource.isPlaying) {
                audioAudioSource.PlayOneShot(currentMusic);
            } else {
                if (LayerController.Self.ClosestPoint != currentSection) {

                    if (LayerController.Self.ClosestPoint == Section.Forrest) {
                        currentMusic = music2;
                        audioAudioSource.Stop();
                        audioAudioSource.PlayOneShot(currentMusic);
                    }

                }

            }
            //print(currentSection);
            currentSection = LayerController.Self.ClosestPoint;*/
        }

    IEnumerator Start_Music() { yield return new WaitForSeconds(15); StartMusic = true; }

}
