using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : Item
{
    private SpriteRenderer magnetSpriteRenderer;

    public override void Initialized(SpriteRenderer _item, Transform _parent, float _rePosX, float _inScenePosX)
    {
        magnetSpriteRenderer = GameObject.Instantiate<SpriteRenderer>(_item, _parent);
        item = magnetSpriteRenderer.transform;
        itemPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        item.position = itemPos;

        speed = magnetSpriteRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isCollision = false;

        SetVisible(false);
    }

    public override void SetPosition(Floor _floor)
    {
        width = magnetSpriteRenderer.bounds.size.x;
        height = magnetSpriteRenderer.bounds.size.y;

        // 아이템 생성 위치를 블럭 위로 지정했을 때 로직
        AABB floorAABB = _floor.GetAABB();
        float randomPosMin = (floorAABB.pos.x - floorAABB.width * HALF);
        float randomPosMax = (floorAABB.pos.x + floorAABB.width * HALF);

        itemPos.x = GetRandomValue(randomPosMin, randomPosMax);
        itemPos.y = floorAABB.pos.y + ITEM_POSY;

        SetVisible(true);
    }

    public override void CollisionItem(Player _player)
    {
        ItemModel item = ItemManager.getInstance.GetItemModel(ITEM_TYPE.magnet);
        int range = item.level / 10 + 3;
        float time = item.level + 4;
        _player.SetMagnetEffect(true, range, time);

        Debug.Log("Collision Magnet Item");
        Debug.Log("Magnet Item Level = " + item.level);
        Debug.Log("Magnet Range = " + range);
        Debug.Log("Magnet Time = " + time);

    }

}
