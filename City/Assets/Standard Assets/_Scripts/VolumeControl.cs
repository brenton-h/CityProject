using UnityEngine;
using UnityEngine.Audio;
public class VolumeControl : MonoBehaviour {
    public GameObject panel;
    public AudioMixer myMixer;
    private bool isPaused = false;
    void Start() {
        panel.SetActive(false);
        ON_CHANGE_OverallVol(0.09f); ON_CHANGE_MusicVol(0.55f); ON_CHANGE_FxVol(0.9f);
    }
    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            panel.SetActive(!panel.activeInHierarchy);
            if (isPaused) Time.timeScale = 1.0f; else Time.timeScale = 0.0f;
            isPaused = !isPaused;
        }
    }
    public void ON_CHANGE_OverallVol(float vol) { myMixer.SetFloat("OverallVolume", Mathf.Log10(vol) * 20f); }
    public void ON_CHANGE_MusicVol(float vol) { myMixer.SetFloat("MusicVolume", Mathf.Log10(vol) * 20f); }
    public void ON_CHANGE_FxVol(float vol) { myMixer.SetFloat("FxVolume", Mathf.Log10(vol) * 20f); }
}