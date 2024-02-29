using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COIN_TYPE
{
    bronze = 1,
    silver = 10,
    gold = 100
}
public class Coin
{
    private const int CREATE_POS_X = 5;
    private const int CREATE_POS_Y = 5;
    private const float HALF = 0.5f;
    private const string COIN_GRADE = "CoinGrade";

    private float width = 1;
    private float speed;
    private float repositionX;
    private float inScenePosX;

    private SpriteRenderer coinRenderer;
    private Transform coin;
    private Vector2 coinPos;
    private COIN_TYPE coinType;
    private Animator anim;

    private bool isInScene;

    public Coin(SpriteRenderer _obstacle, Transform _parent, float _rePosX, float _inScenePosX, int _idx)
    {
        coinRenderer = GameObject.Instantiate<SpriteRenderer>(_obstacle, _parent);
        coin = coinRenderer.transform;

        coinPos = coin.position = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        coin.name = coin.name + _idx;

        anim = coinRenderer.GetComponent<Animator>();

        speed = coinRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isInScene = false;
        //coinType = _coinType;

        SetVisible(false);
    }

    public void Move()
    {
        if (coin.position.x + width * HALF < repositionX)
        {
            isInScene = false;
            SetVisible(false);
        }
        else
        {
            coinPos.x += Time.deltaTime * speed * -HALF;
            coin.position = coinPos;
        }

        if(coin.position.x < inScenePosX && coin.gameObject.activeSelf)
        {
            isInScene = true;
        }
    }

    public void SetPosition(Vector2 _pos)
    {
        coinPos = _pos;
        coin.position = coinPos;
    }

    public void SetVisible(bool _visible)
    {
        coin.gameObject.SetActive(_visible);
    }

    public Vector2 GetPos()
    {
        return coinPos;
    }

    public AABB GetAABB()
    {
        AABB aabb;
        aabb.pos = coinPos;
        aabb.width = 1;
        aabb.height = 1; // 지금은 높이가 무조건 1 임시 데이터
        return aabb;
    }

    public bool IsInScene()
    {
        return isInScene;
    }

    public void SetIsInScene(bool _isInScene)
    {
        isInScene = _isInScene;
    }

    public COIN_TYPE GetCoinType()
    {
        return coinType;
    }

    public void ChangeCoinType(COIN_TYPE _coinType)
    {
        coinType = _coinType;
        anim.SetInteger(COIN_GRADE ,(int)coinType);
    }
}
