using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {
    
    public float speed = 1;
    public GameObject[] Points = new GameObject[2];
    public bool RandomMovement = false;
    public bool stayIdle;
    private Transform CurrentDestination;
    private CharacterController controller;
    private Animator animator;
    private Vector3 Direction;
    private int PointIndex = -1;
    void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        CurrentDestination = GetNextPoint();
        animator.SetFloat("Speed", speed, .25f, Time.deltaTime);
    }
	
	void Update() {
		if (controller.isGrounded) {
            if (stayIdle) {
                animator.SetFloat("zSpeed", 0, .25f, Time.deltaTime);
            } else {
                if (Vector3.Distance(transform.position, CurrentDestination.position) <= 2f) {
                    CurrentDestination = GetNextPoint();
                } else {
                    Direction = CalcDirection(CurrentDestination.position);
                    Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, CalcDirection(CurrentDestination.position, false), 1, 0);
                    transform.rotation = Quaternion.LookRotation(new Vector3(rotateDirection.x, 0, rotateDirection.z));
                    //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), 2f * Time.deltaTime);
                    animator.SetFloat("zSpeed", speed, .25f, Time.deltaTime);//Vector3.Distance(transform.position, CurrentDestination.position)
                    animator.SetFloat("Speed", speed, .25f, Time.deltaTime);
                }
            }
        }
	}

    Transform GetNextPoint() {
        if (!RandomMovement) {
            PointIndex++;
            if (PointIndex == Points.Length) PointIndex = 0;
            return Points[PointIndex].transform;
        } else {
            int x = Random.Range(0, Points.Length - 1);
            return Points[x].transform;
        }
    }

    Vector3 CalcDirection(Vector3 v3, bool normalize=true) {
        return normalize ? (Vector3NoY(v3) - Vector3NoY(transform.position)).normalized : (Vector3NoY(v3) - Vector3NoY(transform.position));

    }

    private Vector3 Vector3NoY(Vector3 v3) {
        return new Vector3(v3.x, 0, v3.z);
    }
}
