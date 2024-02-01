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
        // Start Floor
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
                // Start Floor
                floorList[i].SetFloorPosition(new Vector2(0, -1));
            }
            else
            {
                SetRandomFloorPos(floorList[i]);
            }
            lastFloor = floorList[i];
        }
    }
    private void MoveFloors()
    {
        int count = floorList.Count;
        for (int i = 0; i < count; i++)
        {
            if (!floorList[i].GetFloorVisible())
            {
                RePosFloor(floorList[i]);

                floorList[i].SetFloorVisible(true);
                lastFloor = floorList[i];
            }
            floorList[i].MoveFloor();
        }
    }
    private void RePosFloor(Floor _floor)
    {
        float randomWidth = Random.Range(0, 3);
        _floor.SetFloorSize(new Vector2((int)randomWidth, 1));

        SetRandomFloorPos(_floor);
    }

    private void SetRandomFloorPos(Floor _floor)
    {
        float randomY = Random.Range(-2, 2);
        _floor.SetFloorPosition(new Vector2(lastFloor.GetXPos() + (lastFloor.GetFloorWidth() * 0.5f) + 2 + (_floor.GetFloorWidth() * 0.5f), randomY));

    }
}
