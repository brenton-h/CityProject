using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditController : MonoBehaviour {

    public GameObject[] Points = new GameObject[3];
    public bool RandomMovement = false;
    private Animator Anim;
    private Transform CurrentPoint, playerTransform;
    private Vector3 Direction;
    private long PointIndex = -1;

	void Start() {
        Anim = GetComponent<Animator>();
        CurrentPoint = GetNextPoint();
        Direction = CalcDirection(CurrentPoint.position);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Anim.SetBool("Walk", true);
	}
	
	void Update() {
		if (Vector3.Distance(transform.position, CurrentPoint.position) <= 2f) {
            CurrentPoint = GetNextPoint();
        } else {
            if (PlayerIsClose() && SelfIsInArea()) {
                if (CheckToStop()) {
                    Anim.SetBool("Walk", false);
                } else {
                    Anim.SetBool("Walk", true);
                }
                Direction = CalcDirection(playerTransform.position);
            } else Direction = CalcDirection(CurrentPoint.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), 2f * Time.deltaTime);
        }
	}

    Transform GetNextPoint() {
        if (!RandomMovement) {
            PointIndex++;
            Transform t = Points[PointIndex % Points.Length].transform;
            return t;
        } else {
            int x = Random.Range(0, Points.Length - 1);
            return Points[x].transform;
        }
    }

    Vector3 CalcDirection(Vector3 va) {
        float x = va.x - transform.position.x,
            z = va.z - transform.position.z;
        return new Vector3(x, 0, z).normalized;
    }

    private bool PlayerIsClose() {
        if (playerTransform.position.x < 150f || playerTransform.position.z > -150f) {
            return false;
        } else if (Vector3.Distance(Vector3NoY(transform.position), Vector3NoY(playerTransform.position)) <= 10f) {
            return true;
        } else return false;
    }

    private bool CheckToStop() {
        return (Vector3.Distance(Vector3NoY(transform.position), Vector3NoY(playerTransform.position)) < 2f);
    }

    private bool SelfIsInArea() {
        return (transform.position.x > 150f && transform.position.z < -150f);
    }

    private Vector3 Vector3NoY(Vector3 v3) {
        return new Vector3(v3.x, 0, v3.z);
    }
}
