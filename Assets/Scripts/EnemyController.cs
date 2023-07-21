using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRb;
    public float moveSpeed;

    public float rangeToChasePlayer;
    private Vector3 _moveDirection;

    public Animator animator;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldShoot;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float shootRange;

    public SpriteRenderer theBody;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (theBody.isVisible && PlayerController.Instance.gameObject.activeInHierarchy)
        {
            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < rangeToChasePlayer)
            {
                _moveDirection = PlayerController.Instance.transform.position - transform.position;
            }
            else
            {
                _moveDirection = Vector3.zero;
            }

            _moveDirection.Normalize();

            theRb.velocity = _moveDirection * moveSpeed;

            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    AudioManager.Instance.PlaySFX(13);
                }
            }
        }
        else
        {
            theRb.velocity = Vector2.zero;
        }

        animator.SetBool("isMoving", _moveDirection != Vector3.zero);
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        AudioManager.Instance.PlaySFX(2);

        Instantiate(hitEffect, transform.position, transform.rotation);

        if (health <= 0)
        {
            Destroy(gameObject);

            AudioManager.Instance.PlaySFX(1);

            var selectedSplatter = Random.Range(0, deathSplatters.Length);
            var rotation = Random.Range(0, 4);

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));
        }
    }
}
