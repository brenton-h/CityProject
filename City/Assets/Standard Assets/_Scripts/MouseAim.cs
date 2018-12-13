using UnityEngine;
using System.Collections;
public class MouseAim : MonoBehaviour {
    

    public AudioClip GunShot;

    public GameObject MuzzleFlash;

    public Transform spine;
    public Transform weapon;

    public GameObject crosshairImage;

    public Vector2 xLimit = new Vector2(-40f, 40f);
    public Vector2 yLimit = new Vector2(-40f, 40f);

    private float xAxis = 0f, yAxis = 0f;

    public Transform ShootPoint { get; set; }
    private GameObject player;
    private BasicController2 bc2;
    private Vector3 fromPosition;

    public static Vector3 Direction { get; set; }
    public static Vector3 Destination { get; set; }


    void Awake() {
        Weapon.MuzzleFlash = MuzzleFlash;
        Weapon.GunShot = GunShot;
    }

    void Start() {
        MuzzleFlash.transform.localScale *= 0.1f;
        player = GameObject.FindGameObjectWithTag("Player");
        bc2 = player.GetComponent<BasicController2>();
        crosshairImage.SetActive(false);
    }

    public void LateUpdate() {
        if (Player.HasWeapon) {
            RotateSpine();
            ShowCrosshairIfRaycastHit();
        }
    }

    private void RotateSpine() {
        yAxis += Input.GetAxis("Mouse X");
        yAxis = Mathf.Clamp(yAxis, yLimit.x, yLimit.y);
        xAxis -= Input.GetAxis("Mouse Y");
        xAxis = Mathf.Clamp(xAxis, xLimit.x, xLimit.y);
        Vector3 newSpineRotation = new Vector3(xAxis, yAxis, spine.localEulerAngles.z);
        spine.localEulerAngles = newSpineRotation;
    }

    private void ShowCrosshairIfRaycastHit() {
        //Vector3 weaponForwardDirection = weapon.TransformDirection(Vector3.forward);
        //Vector3 forwardDirection = Player.Weapon.Transform.TransformDirection(Player.Weapon.WeaponForward);
        RaycastHit raycastHit;
        fromPosition = Player.Weapon.Position + Vector3.one;
        if (Physics.Raycast(Player.Weapon.ShootPoints[1].position, Player.Weapon.Direction, out raycastHit)) {
            Vector3 hitLocation = Camera.main.WorldToScreenPoint(raycastHit.point);
            Destination = hitLocation;
            BulletController.Direction = Player.Weapon.Direction;// (raycastHit.point - Player.Weapon.Position).normalized;
            Direction = Player.Weapon.Direction;// (raycastHit.point - Player.Weapon.Position).normalized;
            DisplayPointerImage(hitLocation);
        } else {
            Destination = Vector3.negativeInfinity;
            crosshairImage.SetActive(false);
            BulletController.Direction = Player.Weapon.Direction;
            Direction = Player.Weapon.Direction;
        }
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit)) {
            crosshairImage.transform.position = Camera.main.WorldToScreenPoint(raycastHit.point);
            crosshairImage.SetActive(true);
        } else crosshairImage.SetActive(false);*/
    }
    
    private void DisplayPointerImage(Vector3 hitLocation) {
        crosshairImage.transform.position = hitLocation;
        crosshairImage.SetActive(true);
    }

    public void FireBullet(GameObject Bullet) {
        
        Player.Weapon.GameObject.GetComponent<AudioSource>().PlayOneShot(GunShot);
        //new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 0.5f)
        
        GameObject bulletInstance = Instantiate(Bullet, Player.Weapon.ShootPoint.position,
            Player.Transform.rotation);
       
        bulletInstance.GetComponent<BulletController>().Destination = Destination;
        
        flashMuzzle(Player.Weapon.ShootPoint.position);
        //Vector3 dir = bullet.GetComponent<BulletController>().Direction;
        bulletInstance.GetComponent<Rigidbody>().AddForce(500f * BulletController.Direction);
        bulletInstance.GetComponent<BulletController>().DestroySelf(3);
    }

    public void flashMuzzle(Vector3 pos) {
        GameObject muzzleFlash = Instantiate(MuzzleFlash, pos, Quaternion.identity);
        Destroy(muzzleFlash, 0.1f);
    }
}
