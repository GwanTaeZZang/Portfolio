using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor
{
    private const float START_FLOOR_SIZE = 20f;

    private Transform floor;
    private SpriteRenderer leftSingleFloor;
    private SpriteRenderer middleSingleFloor;
    private SpriteRenderer rightSingleFloor;
    private float width;
    private int moveSpeed;
    private bool isStartFloor;
    private bool isMoved;
    private float repositionX;

    public Floor(SpriteRenderer _leftFloorPart, SpriteRenderer _middleFloorPart, SpriteRenderer _rightFloorPart, Transform _floorGroup, bool _isStartFloor = false)
    {
        floor = new GameObject("Floor").transform;
        floor.position = Vector3.zero;
        floor.SetParent(_floorGroup);
        leftSingleFloor = GameObject.Instantiate<SpriteRenderer>(_leftFloorPart, floor);
        middleSingleFloor = GameObject.Instantiate<SpriteRenderer>(_middleFloorPart, floor);
        rightSingleFloor = GameObject.Instantiate<SpriteRenderer>(_rightFloorPart, floor);
        moveSpeed = leftSingleFloor.sortingOrder;
        isStartFloor = _isStartFloor;
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        isMoved = true;
        Initialized();
    }

    public void MoveFloor()
    {
        if(floor.position.x + width * 0.5f < repositionX && isMoved)
        {
            SetFloorVisible(false);
            isMoved = false;
            return;
        }
        else if (isMoved)
        {
            floor.transform.Translate(Time.deltaTime * moveSpeed * -1, 0, 0);
        }
    }

    public void SetFloorPosition(Vector2 _position)
    {
        floor.position = _position;
    }

    public void SetFloorVisible(bool _visible)
    {
        floor.gameObject.SetActive(_visible);
    }

    public float GetFloorWidth()
    {
        return width;
    }

    public float GetXPos()
    {
        return floor.position.x;
    }

    private void Initialized()
    {


        if (isStartFloor)
        {
            //floor.position = new Vector2(0, -1);
            middleSingleFloor.size = new Vector2(START_FLOOR_SIZE, 1);
        }
        else
        {
            //float randomYPos = Random.Range(-2, 3);
            //floor.position = new Vector2(0, randomYPos);
            float randomWidth =  Random.Range(0, 5);
            middleSingleFloor.size = new Vector2((int)randomWidth, 1);
        }

        leftSingleFloor.transform.localPosition = new Vector2(-((middleSingleFloor.size.x * 0.5f) + (leftSingleFloor.size.x * 0.5f)), 0);
        rightSingleFloor.transform.localPosition = new Vector2((middleSingleFloor.size.x * 0.5f) + (leftSingleFloor.size.x * 0.5f), 0);

        width = middleSingleFloor.size.x + leftSingleFloor.size.x + rightSingleFloor.size.x;
    }

}
