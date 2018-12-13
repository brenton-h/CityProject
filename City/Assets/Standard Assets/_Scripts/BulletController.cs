using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed = 10f;
    public float force = 500f;
    
    private Rigidbody rb;
	
    public static Vector3 Direction { get; set; }
    private Vector3 dir;
    public Vector3 Destination { get; set; }

	void Start() {
        dir = Direction;
        rb = GetComponent<Rigidbody>();
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit)) {
            Direction = (raycastHit.point - transform.position).normalized;
        } else {
            Direction = ray.direction;
        }   */
    }
	
	void Update() {
        rb.AddForce(dir * force);
        rb.velocity.Set(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            if (Vector3.Distance(transform.position, Destination) < 1f) {
                //DestroySelf();
            }
        
	}

    void OnCollisionEnter(Collision c) {
        print("hit");
        if (c.gameObject.tag == "Car") {
            try {
                ExplosiveCar ec = c.gameObject.GetComponent<ExplosiveCar>();
                if (ec.health <= 1) {
                    ec.StartSequence(3f);
                } else ec.health--;
            } catch (System.NullReferenceException) { }
        } else if (c.gameObject.tag != "Player") {
            PhysicMaterial p = c.collider.material;
            string m = p.name;
            m = m.ToLower().Split(' ')[0];
            if (m == "road") {
                //print("Hit a road");
            } else if (m == "brick") {
            } else if (m == "stone") {
            } else if (m == "grass") {
            } else if (m == "dirt") {
            } else if (m == "") {
            }
        } DestroySelf();        
    }

    public void DestroySelf(float delay=-1) {
        try {
            if (delay <= 0) {
                print("Bullet destroyed");
                Destroy(gameObject, .05f);
            } else Destroy(gameObject, delay);
        } catch (System.NullReferenceException) { print("Bullet destroyed"); }
    }
}
