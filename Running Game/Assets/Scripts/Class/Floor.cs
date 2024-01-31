using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor
{
    private Transform floorPerant;
    private Transform repositionFloorStandard;
    private Transform[] floorChlidrenArr;
    private int childCount;
    private float width;
    private float height;
    private int moveSpeed;

    public Floor(Transform _floor, Transform _repositionFloorStandard)
    {
        floorPerant = _floor;
        repositionFloorStandard = _repositionFloorStandard;
        Initialized();
    }

    public void MoveFloor()
    {
        floorPerant.Translate(moveSpeed * Time.deltaTime * -1, 0, 0);

        if(floorPerant.position.x + width < repositionFloorStandard.position.x)
        {
            floorPerant.position = new Vector2(10, -1);
        }
    }

    private void Initialized()
    {
        moveSpeed = floorPerant.GetComponentInChildren<SpriteRenderer>().sortingOrder;
        childCount = floorPerant.childCount;
        floorChlidrenArr = floorPerant.GetComponentsInChildren<Transform>();

        float compare = -100;
        for (int i =0; i < childCount; i++)
        {
            float yPos = floorChlidrenArr[i].position.y;
            if (compare != yPos)
            {
                height++;
                compare = yPos;
            }
        }

        width = childCount / height;

        floorChlidrenArr = null;

        Debug.Log(width);
    }
}
