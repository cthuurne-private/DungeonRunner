using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
            {
                if (PlayerController.Instance.dashCounter > 0)
                {
                    Smash();
                }

                break;
            }
            case "PlayerBullet":
                Smash();
                break;
        }
    }

    public void Smash()
    {
        Destroy(gameObject);

        AudioManager.Instance.PlaySFX(0);

        // show broken pieces
        var piecesToDrop = Random.Range(1, maxPieces);

        for (var i = 0; i < piecesToDrop; i++)
        {
            var randomPiece = Random.Range(0, brokenPieces.Length);

            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        // drop items
        if (shouldDropItem)
        {
            var dropChance = Random.Range(0f, 100f);

            if (dropChance < itemDropPercent)
            {
                var randomItem = Random.Range(0, itemsToDrop.Length);

                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}
