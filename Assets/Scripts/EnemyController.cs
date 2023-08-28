using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRb;
    public float moveSpeed;
    private Vector3 _moveDirection;

    // Enemey skeleton will chase the player if the player is within range
    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;

    // Enemy coward will run away from the player if the player is within range
    [Header("Run Away")]
    public bool shouldRunAway;
    public float runAwayRange;

    // Enemy wanderer will wander around the map
    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    // Enemy patroller will patrol between points
    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("Shooting")]
    public bool shouldShoot;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float shootRange;

    [Header("Variables")]
    public Animator animator;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    public SpriteRenderer theBody;

    private void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }

    private void Update()
    {
        if (theBody.isVisible && PlayerController.Instance.gameObject.activeInHierarchy)
        {
            _moveDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                _moveDirection = PlayerController.Instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if (wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        // move the enemy
                        _moveDirection = wanderDirection;

                        if (wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if (pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if (pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    _moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.2f)
                    {
                        currentPatrolPoint++;

                        if (currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < runAwayRange)
            {
                _moveDirection = transform.position - PlayerController.Instance.transform.position;
            }

            //else
            //{
            //    _moveDirection = Vector3.zero;
            //}

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
