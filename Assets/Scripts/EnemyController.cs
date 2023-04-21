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

    private void Start()
    {
        
    }

    private void Update()
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

        animator.SetBool("isMoving", _moveDirection != Vector3.zero);

        if (shouldShoot)
        {
            fireCounter -= Time.deltaTime;

            if (fireCounter <= 0 )
            {
                fireCounter = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        Instantiate(hitEffect, transform.position, transform.rotation);

        if (health <= 0)
        {
            Destroy(gameObject);

            var selectedSplatter = Random.Range(0, deathSplatters.Length);
            var rotation = Random.Range(0, 4);

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));
        }
    }
}
