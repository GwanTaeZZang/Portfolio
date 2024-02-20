using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController
{
    private const int CAPACITY = 100;
    private const float JUMP_COIN_HEIGHT = 2.5f;
    private const float COIN_SIZE = 1;
    private const float HALF = 0.5f;
    private const float CORRECTION_HALF = 0.25f;
    private const float COIN_INTERVAR = 1.5f;

    private Player player;
    private SpriteRenderer bronzeCoin;
    private Vector2 coinSetVector = Vector2.zero;

    private int setCoinIdx = 0;
    private int collisionCoinIdx;

    private List<Coin> bronzeCoinList = new List<Coin>();

    public CoinController(Transform _parent, Player _player, float _reposX)
    {
        player = _player;
        bronzeCoin = Resources.Load<SpriteRenderer>("Prefab/Coin/BronzeCoin");


        CreateCoin(bronzeCoin, _parent, _reposX);

        collisionCoinIdx = 0;
    }

    public void UpdateCoin()
    {
        MoveCoin();
        UpdateCurrentCollisionCoin();
        CheckCollisionCoin();
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

    public void SetCoinPosition(Floor _floor, Obstacle _obstacle)
    {
        AABB floorAABB = _floor.GetAABB();
        AABB obstacleAABB = _obstacle.GetAABB();

        int coinCount = (int)floorAABB.width;
        for(int i =0; i < coinCount; i++)
        {
            //float obstaclePosX = _obstacle.GetPos().x;
            float obstaclePosX = obstacleAABB.pos.x;
            //Debug.Log("Cactus Pos = " + obstaclePosX);

            Coin coin = bronzeCoinList[setCoinIdx];
            coinSetVector = Vector2.zero;
            coinSetVector.x = (floorAABB.pos.x - floorAABB.width * HALF) + (COIN_SIZE * HALF) + i;


            if (obstaclePosX - COIN_INTERVAR < coinSetVector.x && obstaclePosX + COIN_INTERVAR > coinSetVector.x)
            {
                //Debug.Log("Dont Set Coin" + obstaclePosX);
                float floorY = (floorAABB.pos.y + floorAABB.height * HALF) + (COIN_SIZE * HALF);
                coinSetVector.y = floorY + JUMP_COIN_HEIGHT;
                coin.SetPosition(coinSetVector);
                coin.SetVisible(true);

                setCoinIdx++;
                setCoinIdx = setCoinIdx % CAPACITY;

            }
            else
            {
                //coinSetVector = Vector2.zero;
                //Debug.Log(setCoinIdx);
                //Coin coin = bronzeCoinList[setCoinIdx];
                //coinSetVector.x = (floorAABB.pos.x - floorAABB.width * HALF) + (COIN_SIZE * HALF) + i;
                coinSetVector.y = (floorAABB.pos.y + floorAABB.height * HALF) + (COIN_SIZE * HALF);
                coin.SetPosition(coinSetVector);
                coin.SetVisible(true);

                setCoinIdx++;
                setCoinIdx = setCoinIdx % CAPACITY;
            }
        }
    }


    private void UpdateCurrentCollisionCoin()
    {
        Coin curBronzeCoin = bronzeCoinList[collisionCoinIdx];
        if (player.GetPlayerPos().x - HALF > curBronzeCoin.GetPos().x + (COIN_SIZE * HALF))
        {
            collisionCoinIdx++;
            collisionCoinIdx = collisionCoinIdx % CAPACITY;
        }
    }


    private void CheckCollisionCoin()
    {
        AABB curCoin = bronzeCoinList[collisionCoinIdx].GetAABB();
        float coinPosX = curCoin.pos.x;
        float coinPosY = curCoin.pos.y;
        float coinWidth = curCoin.width;
        float coinHeight = curCoin.height;

        if (coinPosX - coinWidth * CORRECTION_HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
            coinPosX + coinWidth * CORRECTION_HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
            coinPosY - coinHeight * CORRECTION_HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
            coinPosY + coinHeight * CORRECTION_HALF > player.GetPlayerPos().y - CORRECTION_HALF)
        {
            Debug.Log("Coin Collision~~~~~~~~~~~");

            bronzeCoinList[collisionCoinIdx].SetVisible(false);
            collisionCoinIdx++;
            collisionCoinIdx = collisionCoinIdx % CAPACITY;
        }

    }

}
