using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

    public GameObject Ethan;

    private Transform playerTransform;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        if (Ethan == null)
        {
            Ethan = GameObject.FindGameObjectWithTag("Player");
        }
        playerTransform = Ethan.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (MiniMapController.CamIsActive) {
            rb.transform.position = new Vector3(playerTransform.position.x, 90f, playerTransform.position.z);
        }
	}
}
