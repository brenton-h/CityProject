using UnityEngine;
public class BasicController : MonoBehaviour {

    private Animator animator;

    private CharacterController characterController;

    public GameObject Bullet;

    public float transitionTime = 0.25f;

    private float speedLimit = 2f;

    public bool moveDiagonally = true;
    public bool mouseRotate = true;
    public bool keyboardRotate = true;

    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (characterController.isGrounded) {
            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) {
                speedLimit = .5f;
            } else {
                speedLimit = 1f;
            }
            float h = Input.GetAxis("Horizontal"),
                  v = Input.GetAxis("Vertical");

            float xSpeed = h * speedLimit,
                  zSpeed = v * speedLimit;

            float speed = Mathf.Sqrt((h * h) + (v * v));

            if (v != 0 && !moveDiagonally) xSpeed = 0f;

            if (v != 0 && keyboardRotate) {
                this.transform.Rotate(Vector3.up * h, Space.World);
            }

            /*if (Input.GetKey("q")) {
                // rotate left
            } else if (Input.GetKey("e")) {
                // rotate right
            }*/

            if (mouseRotate) {
                this.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Mathf.Sign(v), Space.World);
            }

            animator.SetFloat("zSpeed", zSpeed, transitionTime, Time.deltaTime);
            animator.SetFloat("xSpeed", xSpeed, transitionTime, Time.deltaTime);
            animator.SetFloat("speed", speed, transitionTime, Time.deltaTime);

            //if (Input.GetKeyDown(KeyCode.F)) { animator.SetBool("Grenade", true); } else { animator.SetBool("Grenade", false); }
            if (Input.GetButtonDown("Fire1")) {
                animator.SetBool("Fire", true);
                FireBullet();
            }
            if (Input.GetButtonUp("Fire1")) { animator.SetBool("Fire", false); }
        }
    }

    void FireBullet() {
        //new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 0.5f)
        GameObject bullet = Instantiate(Bullet,
            new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 0.5f),
            transform.rotation);
        
        Vector3 dir = BulletController.Direction;
        bullet.GetComponent<Rigidbody>().AddForce(500f * dir);
        bullet.GetComponent<BulletController>().DestroySelf(3);
    }
}
