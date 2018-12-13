using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player {

    static Player() {
        GameObject = GameObject.FindGameObjectWithTag("Player");
        Weapons = new Weapon[] {
            new Weapon(
                "Ak47",
                new Vector3(0.064f, 0.335f, 0.109f),
                new Vector3(8.169001f, 87.485f, 98.46301f),
                FiringMode.FullAuto,
                .2f
            ),
            new Weapon(
                "M40A3",
                new Vector3(0.065f, 0.334f, 0.067f),
                new Vector3(-72.994f, 25.132f, 152.942f),
                FiringMode.BoltAction,
                .8f
            )
        };
    }

    public static Transform Transform { get { return GameObject.transform; } }

    public static GameObject GameObject { get; private set; }

    public static bool HasWeapon { get { return Weapon != null; } }

    public static Weapon Weapon { get; private set; }

    public static Weapon[] Weapons { get; private set; }

    public static void AddWeapon(string name, GameObject weapon) {
        foreach (Weapon w in Weapons) {
            if (w.Name.ToLower() == name.ToLower()) {
                Weapon = new Weapon(weapon, w);
            }
        }
    }

}


public class Weapon {

    public Weapon(GameObject gobj, Weapon weapon) {
        Name = weapon.Name;
        PositionOffset = weapon.PositionOffset;
        RotationOffset = weapon.RotationOffset;
        GameObject = gobj;
        FireMode = weapon.FireMode;
        ShootPoint = GameObject.transform.Find("ShootPoint").transform;

        ShootPoints = new Transform[] {
            GameObject.transform.Find("ShootPoint0").transform,
            GameObject.transform.Find("ShootPoint").transform
        };

        FireRate = weapon.FireRate;
    }

    public Weapon(string name, Vector3 pos, Vector3 rot, FiringMode mode, float rate) {
        Name = name;
        PositionOffset = pos;
        RotationOffset = rot;
        FireMode = mode;
        FireRate = rate;
    }

    public static GameObject MuzzleFlash { get; set; }

    public static AudioClip GunShot { get; set; }

    public float FireRate { get; private set; }

    public string Name { get; private set; }

    public FiringMode FireMode { get; private set; }

    public Vector3 PositionOffset { get; private set; }

    public Vector3 RotationOffset { get; private set; }

    public Vector3 Position { get { return Transform.position; } }

    public Vector3 Direction {
        get { return (ShootPoints[1].position - ShootPoints[0].position).normalized; }
    }

    public Transform Transform { get { return GameObject.transform; } }

    public Transform ShootPoint { get; private set; }

    public Transform[] ShootPoints { get; private set; }

    public GameObject GameObject { get; private set; }

    public void Fire(GameObject Bullet) {
        GameObject.GetComponent<AudioSource>().PlayOneShot(GunShot);

        GameObject bulletInstance = Object.Instantiate(Bullet, ShootPoints[1].position, Player.Transform.rotation);

        bulletInstance.GetComponent<BulletController>().Destination = MouseAim.Destination;

        //flashMuzzle(Player.Weapon.ShootPoint.position);
        //Vector3 dir = bullet.GetComponent<BulletController>().Direction;
        bulletInstance.GetComponent<Rigidbody>().AddForce(500f * Direction);
        bulletInstance.GetComponent<BulletController>().DestroySelf(3);
        GameObject muzzleFlash = Object.Instantiate(MuzzleFlash, ShootPoints[1].position, Quaternion.identity);
        Object.Destroy(muzzleFlash, 0.1f);
    }
}

public enum FiringMode { FullAuto, SemiAuto, BoltAction }

public interface IMovable {
    Animator Animator { get; }
    
}

