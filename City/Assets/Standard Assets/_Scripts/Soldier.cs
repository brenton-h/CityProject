using UnityEngine;

public class Soldier {

    public static GameObject Prefab { get; set; }

    public GameObject GameObject { get; set; }

    public SoldierState State { get; set; }

    public bool IsPassive { get; set; }

    private float HP = 100;
    public float HealthPoints {
        get { return HP; }
        set {
            if (value <= 0) {
                HP = 0;
                Death();
            } else HP = value;
        }
    }

    public bool IsDead { get { return HP == 0; } }

    public Soldier(bool passive = true) {
        IsPassive = passive;
        HealthPoints = 100;
    }

    public void Death() {
        RagdollCharacter rc = GameObject.GetComponent<RagdollCharacter>();
    }

    public static void SpawnSoldier(Vector3 pos, SoldierState state, bool passive=true) {
        Soldier s = new Soldier {
            GameObject = Object.Instantiate(Prefab, pos, Quaternion.identity),
            IsPassive = passive,
            State = state,
            HealthPoints = 100
        };
    }
}

public enum SoldierState { Idle, Patrol, Pursuing }
