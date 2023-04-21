using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Transform GunArm;
    public float MoveSpeed;
    public Rigidbody2D TheRb;
    public Animator Animator;

    public GameObject BulletToFire;
    public Transform FirePoint;
    public float TimeBetweenShots;
    private float _shotCounter;

    private Vector2 _moveInput;
    private Camera _theCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _theCamera = Camera.main;
    }

    private void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        _moveInput.Normalize();

        TheRb.velocity = _moveInput * MoveSpeed;

        var mousePos = Input.mousePosition;
        var screenPoint = _theCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            GunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            GunArm.localScale = Vector3.one;
        }

        // rotate gun arm
        var offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        GunArm.rotation = Quaternion.Euler(0, 0, angle);

        // firing bullets
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(BulletToFire, FirePoint.position, FirePoint.rotation);
            _shotCounter = TimeBetweenShots;
        }

        if (Input.GetMouseButton(0))
        {
            _shotCounter -= Time.deltaTime;

            if (_shotCounter <= 0)
            {
                Instantiate(BulletToFire, FirePoint.position, FirePoint.rotation);

                _shotCounter = TimeBetweenShots;
            }
        }

        Animator.SetBool("isMoving", _moveInput != Vector2.zero);
    }
}