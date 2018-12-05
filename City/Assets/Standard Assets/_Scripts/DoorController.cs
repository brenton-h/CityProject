using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DoorController : MonoBehaviour {

    public GameObject Ethan;

    //private bool open = false;
    Vector3 originalPos;

	// Use this for initialization
	void Start () {
        originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (Ethan == null)
        {
           
        }
        Ethan = GameObject.FindGameObjectWithTag("Player");


    }
	
	// Update is called once per frame
	//void Update () {
		/*float d = Vector3.Distance(transform.position, Ethan.transform.position);
        
        if (d <= 2)
        {
            if (!open) {
                transform.position = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
				open = true;
            }
            
        } else if (open)
        {
            transform.position = originalPos;
            open = false;
        }*/
    //}

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Player") { OpenDoor(); }
    }

    void OpenDoor() {
        transform.position = new Vector3(transform.position.x, transform.position.y - 5f, transform.position.z);
        Invoke("CloseDoor", 60f);
    }

    void CloseDoor() {
        transform.position = originalPos;
    }
}
