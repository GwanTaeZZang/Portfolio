using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin
{
    protected const int CREATE_POS_X = -5;
    protected const int CREATE_POS_Y = -5;


    private SpriteRenderer coinRenderer;
    private Transform coin;
    private Vector2 coinPos;

    public Coin(SpriteRenderer _obstacle, Transform _parent, float _rePosX)
    {
        coinRenderer = GameObject.Instantiate<SpriteRenderer>(_obstacle, _parent);
        coin = coinRenderer.transform;
        coinPos = coin.position = new Vector2(CREATE_POS_X, CREATE_POS_Y);


    }

    public void Move()
    {

    }
}
