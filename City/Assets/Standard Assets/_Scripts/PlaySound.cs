using UnityEngine;


public class PlaySounds : MonoBehaviour {
    public AudioClip music1;
    public AudioClip music2;


    private AudioClip currentMusic;
    private AudioSource audioAudioSource;

    private Section currentSection;

    void Start() {
        audioAudioSource = GetComponent<AudioSource>();
        currentMusic = music1;
        currentSection = LayerController.Self.ClosestPoint;
    }

    void Update() {
        if (!audioAudioSource.isPlaying) {
            audioAudioSource.PlayOneShot(currentMusic);
        } else {
            if (LayerController.Self.ClosestPoint != currentSection) {
                
                if (LayerController.Self.ClosestPoint == Section.Forrest && currentSection != Section.Forrest) {
                    currentMusic = music2;
                    audioAudioSource.PlayOneShot(currentMusic);
                }
                currentSection = LayerController.Self.ClosestPoint;
            }
            
        }
    }

}
