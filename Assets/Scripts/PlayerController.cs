using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [HideInInspector]
    public bool canMove = true;

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

    public SpriteRenderer bodySpriteRenderer;

    private float _activeMoveSpeed;
    public float dashSpeed = 8f;
    public float dashLength = 0.5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = 0.5f;

    [HideInInspector]
    public float dashCounter;

    private float _dashCoolCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _theCamera = Camera.main;
        _activeMoveSpeed = MoveSpeed;
    }

    private void Update()
    {
        if (canMove && !LevelManager.Instance.isPaused)
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");

            _moveInput.Normalize();

            TheRb.velocity = _moveInput * _activeMoveSpeed;

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
                AudioManager.Instance.PlaySFX(12);
                _shotCounter = TimeBetweenShots;
            }

            if (Input.GetMouseButton(0))
            {
                _shotCounter -= Time.deltaTime;

                if (_shotCounter <= 0)
                {
                    Instantiate(BulletToFire, FirePoint.position, FirePoint.rotation);
                    AudioManager.Instance.PlaySFX(12);
                    _shotCounter = TimeBetweenShots;
                }
            }

            // Dashing
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    _activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    Animator.SetTrigger("dash");
                    AudioManager.Instance.PlaySFX(8);
                    PlayerHealthController.Instance.MakeInvincible(dashInvincibility);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    _activeMoveSpeed = MoveSpeed;
                    _dashCoolCounter = dashCooldown;
                }
            }

            if (_dashCoolCounter > 0)
            {
                _dashCoolCounter -= Time.deltaTime;
            }

            Animator.SetBool("isMoving", _moveInput != Vector2.zero);
        }
        else
        {
            TheRb.velocity = Vector2.zero;
            Animator.SetBool("isMoving", false);
        }
    }
}