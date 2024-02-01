using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    private const int FLOOR_CAPICITER = 10;

    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform floorParent;

    private List<Floor> floorList = new List<Floor>();
    private Floor startFloor;
    private Floor lastFloor;
    private Player player;
    private SpriteRenderer leftFloorPart;
    private SpriteRenderer middleFloorPart;
    private SpriteRenderer rightFloorPart;

    private void Awake()
    {
        leftFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left");
        middleFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle");
        rightFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right");

        //startFloor = new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true);
    }

    private void Start()
    {
        CreateFloor();
        SetFloorPosition();
    }

    private void Update()
    {
        //startFloor.MoveFloor();
        MoveFloors();
    }

    private void CreateFloor()
    {
        floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true));
        for(int i =0; i < FLOOR_CAPICITER; i++)
        {
            floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent));
        }
    }
    private void SetFloorPosition()
    {
        int count = floorList.Count;
        for(int i =0; i < count; i++)
        {
            if( i == 0)
            {
                floorList[i].SetFloorPosition(new Vector2(0, -1));
            }
            else
            {
                float randomY = Random.Range(-2, 2);
                floorList[i].SetFloorPosition(new Vector2(lastFloor.GetXPos() + (lastFloor.GetFloorWidth() * 0.5f) + 2 + (floorList[i].GetFloorWidth() * 0.5f), randomY));
            }
            lastFloor = floorList[i];
        }
    }
    private void MoveFloors()
    {
        int count = floorList.Count;
        for (int i = 0; i < count; i++)
        {
            floorList[i].MoveFloor();
        }
    }
}
