using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LevelManager.Instance.LevelEnd());
        }
    }
}
