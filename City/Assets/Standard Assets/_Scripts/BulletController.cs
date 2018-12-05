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
                DestroySelf();
            }
        
	}

    void OnCollisionEnter(Collision c) {
        DestroySelf();
    }

    public void DestroySelf(int delay=-1) {
        try {
            if (delay <= 0) {
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("Player").GetComponent<MouseAim>().flashMuzzle(transform.position);
            } else Destroy(gameObject, delay);
        } catch (System.NullReferenceException) { print("Bullet destroyed"); }
    }
}
