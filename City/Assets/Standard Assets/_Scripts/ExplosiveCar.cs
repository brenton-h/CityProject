using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCar : MonoBehaviour {
    public AudioClip Clip;
    public GameObject ExplodeParticles;
    public GameObject Fire;
    private GameObject fireInstance;
    private AudioSource source;
    private DeadlyObject deadlyObject;
    private bool Armed = true;
    public float radius = 10f;
    public float power = 300.0F;
    public float upwards = 3.0f;

    public int health { get; set; }

    void Start () {
        health = 5;
        source = GetComponent<AudioSource>();
        deadlyObject = GetComponent<DeadlyObject>();
	}

    public void StartSequence(float time=5f, bool spawnFire=true) {
        if (!Armed) return;
        else Armed = false;
        if (spawnFire) {
            fireInstance = Instantiate(Fire, transform.position + (Vector3.up * 1.5f), Quaternion.identity);
            fireInstance.transform.localScale *= 2f;
            Invoke("DestroyFire", time * .9f);
        }
        Invoke("Explode", time);
        
        
    }

    void DestroyFire() { Destroy(fireInstance); }

    public void Explode() {
        print("Exploded");
        source.PlayOneShot(Clip);
        Instantiate(ExplodeParticles, transform.position, Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        // For each Collider found in the list
        foreach (Collider col in colliders) {
            // Get Rigidbody component of game object
            Rigidbody rb = col.GetComponent<Rigidbody>();

            deadlyObject.HitObject(col);

            if (rb != null)
                // IF game object features a rigidbody component, add explosion force passing variables set by user as parameters for strength, origin, radius and upwards modifier 
                rb.AddExplosionForce(power, explosionPos, radius, upwards);
            

        }
        Rigidbody carRb = GetComponent<Rigidbody>();
        carRb.velocity.Set(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        fireInstance = Instantiate(Fire, transform.position + (Vector3.up * 1.1f), Quaternion.identity);
        fireInstance.transform.localScale *= 10f;
    }
}
