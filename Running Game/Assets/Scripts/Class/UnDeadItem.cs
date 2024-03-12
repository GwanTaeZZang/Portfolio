using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnDeadItem : Item
{
    private SpriteRenderer undeadItemSpriteRenderer;

    public override void Initialized(SpriteRenderer _item, Transform _parent, float _rePosX, float _inScenePosX)
    {
        throw new System.NotImplementedException();
    }

    public override void SetPosition(Floor _floor)
    {
        throw new System.NotImplementedException();
    }
    public override void CollisionItem(Player _player)
    {
        throw new System.NotImplementedException();
    }

}
