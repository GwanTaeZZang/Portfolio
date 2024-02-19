using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController
{
    private const int CAPACITY = 100;
    private const int COIN_INTERVAR = 1;
    private const float COIN_SIZE = 1;
    private const float HALF = 0.5f;

    private Player player;
    private SpriteRenderer bronzeCoin;
    private Vector2 coinSetVector = Vector2.zero;

    private int setCoinIdx = 0;


    private List<Coin> bronzeCoinList = new List<Coin>();

    public CoinController(Transform _parent, Player _player, float _reposX)
    {
        player = _player;
        bronzeCoin = Resources.Load<SpriteRenderer>("Prefab/Coin/BronzeCoin");


        CreateCoin(bronzeCoin, _parent, _reposX);
    }

    public void UpdateCoin()
    {
        MoveCoin();
    }

    private void CreateCoin(SpriteRenderer _coinSprite, Transform _parent, float _reposX)
    {
        for(int i =0; i < CAPACITY; i++)
        {
            bronzeCoinList.Add(new Coin(_coinSprite, _parent, _reposX, setCoinIdx));
            setCoinIdx++;
        }
        setCoinIdx = 0;
    }

    private void MoveCoin()
    {
        int count = bronzeCoinList.Count;
        for(int i = 0; i < count; i++)
        {
            bronzeCoinList[i].Move();
        }
    }

    public void SetCoinPosition(Floor _floor)
    {
        AABB floorAABB = _floor.GetAABB();

        int coinCount = (int)floorAABB.width;
        for(int i =0; i < coinCount; i++)
        {
            coinSetVector = Vector2.zero;
            //Debug.Log(setCoinIdx);
            Coin coin = bronzeCoinList[setCoinIdx];
            coinSetVector.x = (floorAABB.pos.x - floorAABB.width * HALF) + (COIN_SIZE * HALF) + i;
            coinSetVector.y = (floorAABB.pos.y + floorAABB.height * HALF) + (COIN_SIZE * HALF);
            coin.SetPosition(coinSetVector);
            coin.SetVisible(true);

            setCoinIdx++;
            setCoinIdx = setCoinIdx % CAPACITY;
        }
    }
}
