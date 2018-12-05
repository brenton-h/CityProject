using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

	public GameObject Prop;
	public Vector3 OffSet;
	public Vector3 Force;
	public Transform PlayerLeftHand, PlayerRightHand;
	public float compensationYAngle = 0f;

	private GameObject thrownObject;

	public GameObject PlayerRifle { get; set; }
	public Vector3 OriginRiflePos { get; set; }

	public void Prepare() {
        //PlayerRifle.transform.parent = PlayerLeftHand;
        //OriginRiflePos = PlayerRifle.transform.position;
		thrownObject = Instantiate(Prop, PlayerRightHand.position, PlayerRightHand.rotation) as GameObject;

		if(thrownObject.GetComponent<Rigidbody>())
			// IF the Prop prefab has a Rigidbody component, THEN destroy such component, making it kinematic
			Destroy(thrownObject.GetComponent<Rigidbody>());

		// Disable the Prop's Sphere Collider component, to avoid collisions with the Character Controller
		thrownObject.GetComponent<SphereCollider>().enabled = false;		

		// name the Prop as 'projectile'
		thrownObject.name = "projectile";

		// make Prop a child of the character's PlayerRightHand
		thrownObject.transform.parent = PlayerRightHand;

		// adjust the Prop's position according to the OffSet variable 
		thrownObject.transform.localPosition = OffSet;

		// reset the porp's rotation
		thrownObject.transform.localEulerAngles = new Vector3(0, 0, 0);
	}

	public void Throw() {
		// Vector3 variable for getting the character's transform rotation
		Vector3 dir = transform.rotation.eulerAngles;

		// Adjust Y-axis of 'dir' Vector to compensate character's direction, if necessary
		dir.y += compensationYAngle;

		// set Prop's transform rotation equal to 'dir' (character's rotation plus Y-Axis compensation)
		thrownObject.transform.rotation = Quaternion.Euler(dir);

		// Dettach Prop from character's hand, making it an independent object
		thrownObject.transform.parent = null;		

		// Enable  Prop's Sphere Collider component
		thrownObject.GetComponent<SphereCollider>().enabled = true;		

		// Add Rigidbody component to Prop
		Rigidbody rig = thrownObject.AddComponent<Rigidbody>();

		// Collider variable for getting the Prop's collider
		Collider PropCollider = thrownObject.GetComponent<Collider> ();

		// Collider variable for getting the character's collider
		Collider col = GetComponent<Collider> ();

		// Ignore collision between Prop and character's colliders
		Physics.IgnoreCollision(PropCollider, col);

		// Add Force to Prop, throwing it
		rig.AddRelativeForce(Force);
	}

	public void ResetRiflePos() {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<BasicController2>().HasRifle) {
            //PlayerRifle.transform.parent = PlayerRightHand;
            //PlayerRifle.transform.position = OriginRiflePos;
        }
		    

	}
}
