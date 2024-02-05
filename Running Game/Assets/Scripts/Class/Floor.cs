using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct AABB
{
    public Vector2 pos;
    public float width;
    public float height;
}

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
        Initialized();
    }

    private void Initialized()
    {
        if (isStartFloor)
        {
            middleSingleFloor.size = new Vector2(START_FLOOR_SIZE, 1);
        }
        else
        {
            float randomWidth = Random.Range(0, 5);
            middleSingleFloor.size = new Vector2((int)randomWidth, 1);
        }

        ResetFloorPartsPos();
        SetFloorWidth();
    }

    public void MoveFloor()
    {

        if (floor.position.x + width * 0.5f < repositionX)
        {
            SetFloorVisible(false);
        }
        else
        {
            floor.transform.Translate(Time.deltaTime * moveSpeed * -0.5f, 0, 0);
        }

    }

    public void SetFloorPosition(Vector2 _position)
    {
        floor.position = _position;
    }

    public void SetFloorSize(Vector2 _size)
    {
        middleSingleFloor.size = _size;
        ResetFloorPartsPos();
        SetFloorWidth();
    }

    public void SetFloorVisible(bool _visible)
    {
        floor.gameObject.SetActive(_visible);
    }

    public bool GetFloorVisible()
    {
        return floor.gameObject.activeSelf;
    }

    public float GetFloorWidth()
    {
        return width;
    }

    public float GetXPos()
    {
        return floor.position.x;
    }

    public AABB GetAABB()
    {
        AABB aabb;
        aabb.pos = floor.position;
        aabb.width = width;
        aabb.height = 1; // 지금은 높이가 무조건 1 임시 데이터
        return aabb;
    }


    private void ResetFloorPartsPos()
    {
        leftSingleFloor.transform.localPosition = new Vector2(-((middleSingleFloor.size.x * 0.5f) + (leftSingleFloor.size.x * 0.5f)), 0);
        rightSingleFloor.transform.localPosition = new Vector2((middleSingleFloor.size.x * 0.5f) + (leftSingleFloor.size.x * 0.5f), 0);
    }

    private void SetFloorWidth()
    {
        width = middleSingleFloor.size.x + leftSingleFloor.size.x + rightSingleFloor.size.x;
    }

}
