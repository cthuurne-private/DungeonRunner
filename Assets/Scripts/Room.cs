using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;
    public GameObject[] doors;

    [HideInInspector]
    public bool roomActive;

    private void Start()
    {
        
    }

    private void Update()
    {
    }

    public void OpenDoors()
    {
        foreach (var door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.Instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                foreach (var door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomActive = false;
        }
    }
}
