using UnityEngine;
using System;

/* ----------------------------------------
 * class to demonstrate how to create a Picture-in-Picture effect
 * using two cameras. This script should be attached to a secondary camera
 * featuring a higher Depth level than the main camera.
 */
public class MiniMapController : MonoBehaviour {

    private bool setView;
    public static bool CamIsActive { get; private set; }
    public static MiniMapController Self;
    public enum HorizontalAlignment { Left, Center, Right };
    public enum VerticalAlignment { Top, Center, Bottom };
    public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
    public VerticalAlignment verticalAlignment = VerticalAlignment.Top;
    public float widthPercentage = 0.82f;
    public float heightPercentage = 0.5f;

    public GameObject arrow;
    private Rigidbody arrowRb;
    private Transform playerTransform;
    private Camera Cam;
    private Vector2 Origin, Size;

    void Start() {
        Self=this;
        setView = false;
        Cam = GetComponent<Camera>();
        Cam.enabled = false;
        CamIsActive = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // every frame update Camera properties
    void Update() {
        if (Input.GetKeyDown("m")) {
            Cam.enabled = !Cam.enabled;
            CamIsActive = Cam.enabled;
            if (Cam.enabled) {
                ActivateCamera();
            } else arrow.SetActive(false);
        }
        if (CamIsActive) {
            arrow.transform.position = new Vector3(playerTransform.position.x, 90f, playerTransform.position.z);
        }
    }

    // based on horizontal and vertical alignment
    // create and returrn a 2D (x,y) bottom left origin for Camera's viewport 

    private Vector2 CalcOrigin()
    {
        float originX = 0;
        float originY = 0;

        switch (horizontalAlignment) {
            case HorizontalAlignment.Right: originX = 1 - widthPercentage; break;
            case HorizontalAlignment.Center: originX = 0.5f - (0.5f * widthPercentage); break;
            case HorizontalAlignment.Left: default: originX = 0; break;
        }

        switch (verticalAlignment) {
            case VerticalAlignment.Top: originY = 1 - heightPercentage; break;
            case VerticalAlignment.Center: originY = 0.5f - (0.5f * heightPercentage); break;
            case VerticalAlignment.Bottom:
            default: originY = 0; break;
        }

        return new Vector2(originX, originY);
    }

    void ActivateCamera() {
        Origin = CalcOrigin();
        Size = new Vector2(widthPercentage, heightPercentage);
        Cam.rect = new Rect(Origin, Size);
        arrow.SetActive(true);
    }
}
