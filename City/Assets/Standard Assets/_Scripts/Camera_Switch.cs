using UnityEngine;
public class Camera_Switch : MonoBehaviour
{
    public Camera[] cameras = new Camera[3];
    public bool changeAudioListener = true;
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            EnableCamera(cameras[0], true);
            EnableCamera(cameras[1], false);
            EnableCamera(cameras[2], false);
        }
        if (Input.GetKeyDown("1"))
        {
            EnableCamera(cameras[0], false);
            EnableCamera(cameras[1], true);
            EnableCamera(cameras[2], false);
        }
        if (Input.GetKeyDown("2"))
        {
            EnableCamera(cameras[0], false);
            EnableCamera(cameras[1], false);
            EnableCamera(cameras[2], true);
        }
    }
    private void EnableCamera(Camera cam, bool
    enabledStatus)
    {
        cam.enabled = enabledStatus;
        if (changeAudioListener)
            cam.GetComponent<AudioListener>().enabled =
            enabledStatus;
    }
}