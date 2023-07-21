using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor;

    public int distanceToEnd;
    public Direction selectedDirection;
    public float xOffset = 18f, yOffset = 10f;

    public enum Direction
    {
        up,
        right,
        down,
        left
    }

    public Transform generatorPoint;
    public LayerMask whatIsRoom;

    private GameObject endRoom;
    private List<GameObject> layoutRoomObjects = new();

    public RoomPrefabs rooms;

    private void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (var i = 0; i < distanceToEnd; i++)
        {
            var newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }
}

[Serializable]
public class RoomPrefabs
{
    public GameObject singleUp,
        singleDown,
        singleLeft,
        singleRight,
        doubleUpDown,
        doubleLeftRight,
        doubleUpRight,
        doubleDownRight,
        doubleDownLeft,
        doubleUpLeft,
        tripleUpRightDown,
        tripleRightDownLeft,
        tripleDownLeftUp,
        tripleLeftUpRight;
}