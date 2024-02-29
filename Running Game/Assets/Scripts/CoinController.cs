using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController
{
    private const int CAPACITY = 200;
    private const float JUMP_COIN_HEIGHT = 2.5f;
    private const float COIN_SIZE = 1;
    private const float HALF = 0.5f;
    private const float CORRECTION_HALF = 0.25f;
    private const float COIN_INTERVAR = 1.5f;
    //private const int INIT_BRONZE_COIN_IDX = CAPACITY * 0;
    //private const int INIT_SILVER_COIN_IDX = CAPACITY * 1;

    public delegate void ScoreDelegate(int _score);
    public ScoreDelegate scoreEvnet;

    private Player player;
    //private SpriteRenderer bronzeCoin;
    //private SpriteRenderer silverCoin;
    private SpriteRenderer coinSpriteRenderer;
    private Vector2 coinSetVector = Vector2.zero;

    private int setCoinIdx;
    //private int setBronzeCoinIdx;
    //private int setSilverCoinIdx;
    private int collisionCoinIdx;
    private int scoreAmount;
    private int coinListCount;

    private List<Coin> coinList = new List<Coin>();
    //private List<Coin> silverCoinList = new List<Coin>();

    public CoinController(Transform _parent, Player _player, float _reposX, float _inScenePosX)
    {
        player = _player;
        //bronzeCoin = Resources.Load<SpriteRenderer>("Prefab/Coin/BronzeCoin");
        //silverCoin = Resources.Load<SpriteRenderer>("Prefab/Coin/SilverCoin");
        coinSpriteRenderer = Resources.Load<SpriteRenderer>("Prefab/Coin/Coin");

        //CreateCoin(bronzeCoin, _parent, _reposX, _inScenePosX);
        //CreateCoin(silverCoin, _parent, _reposX, _inScenePosX);
        CreateCoin(coinSpriteRenderer, _parent, _reposX, _inScenePosX);


        collisionCoinIdx = 0;
        scoreAmount = 0;
        //setBronzeCoinIdx = INIT_BRONZE_COIN_IDX;
        //setSilverCoinIdx = INIT_SILVER_COIN_IDX;
        setCoinIdx = 0;
        coinListCount = coinList.Count;

        //int idx = SetCoinSquarePattern(new Vector2 (4,4), 4, 4, setSilverCoinIdx + INIT_SILVER_COIN_IDX);
        //setSilverCoinIdx = idx % CAPACITY + INIT_SILVER_COIN_IDX;
    }

    public void UpdateCoin()
    {
        MoveCoin();
        UpdateCollisionCoin();
        //UpdateCurrentCollisionCoin();
        //CheckCollisionCoin();
    }

    private void CreateCoin(SpriteRenderer _coinSprite, Transform _parent, float _reposX, float _inScenePosX)
    {
        for(int i =0; i < CAPACITY; i++)
        {
            coinList.Add(new Coin(_coinSprite, _parent, _reposX, _inScenePosX, i));
        }
    }

    private void MoveCoin()
    {
        for(int i = 0; i < coinListCount; i++)
        {
            coinList[i].Move();
            //silverCoinList[i].Move();
        }
    }


    public void SetCoinPosition(Floor _floor, Obstacle _obstacle)
    {

        SetCoinPattern(_floor, _obstacle);
        SetCoinlinear(_floor, _obstacle);
    }

    private void SetCoinlinear(Floor _floor, Obstacle _obstacle)
    {
        AABB floorAABB = _floor.GetAABB();
        AABB obstacleAABB = _obstacle.GetAABB();

        int coinCount = (int)floorAABB.width;
        for (int i = 0; i < coinCount; i++)
        {
            //float obstaclePosX = _obstacle.GetPos().x;
            float obstaclePosX = obstacleAABB.pos.x;
            //Debug.Log("Cactus Pos = " + obstaclePosX);

            Coin coin = coinList[setCoinIdx];
            coinSetVector = Vector2.zero;
            coinSetVector.x = (floorAABB.pos.x - floorAABB.width * HALF) + (COIN_SIZE * HALF) + i;


            if (obstaclePosX - COIN_INTERVAR < coinSetVector.x && obstaclePosX + COIN_INTERVAR > coinSetVector.x)
            {
                //Debug.Log("Dont Set Coin" + obstaclePosX);
                float floorY = (floorAABB.pos.y + floorAABB.height * HALF) + (COIN_SIZE * HALF);
                coinSetVector.y = floorY + JUMP_COIN_HEIGHT;
                coin.SetPosition(coinSetVector);
                coin.SetVisible(true);

                //setBronzeCoinIdx++;
                //setBronzeCoinIdx = setBronzeCoinIdx % CAPACITY + INIT_BRONZE_COIN_IDX;

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

                //setBronzeCoinIdx++;
                //setBronzeCoinIdx = setBronzeCoinIdx % CAPACITY + INIT_BRONZE_COIN_IDX;

                setCoinIdx++;
                setCoinIdx = setCoinIdx % CAPACITY;

            }

            coin.ChangeCoinType(COIN_TYPE.bronze);
        }

    }

    private void SetCoinPattern(Floor _floor, Obstacle _obstacle)
    {
        float between = _floor.GetBetween();
        AABB floorAABB = _floor.GetAABB();

        Vector2 centerPos = floorAABB.pos;
        centerPos.x = (centerPos.x - floorAABB.width * HALF) - between * HALF;
        centerPos.y += 3;

        SetCoinSquarePattern(centerPos, 3, 2);

    }

    private void SetCoinSquarePattern(Vector2 _centerPos, int _width, int _height)
    {
        int count = _width * _height;
        Vector2 startPos = _centerPos;
        //startPos.x = (_centerPos.x - _width * HALF) + COIN_SIZE * HALF;
        //startPos.y = (_centerPos.y + _height * HALF) - COIN_SIZE * HALF;

        for (int i =0; i < count; i++)
        {
            //int idx = (_startCoinIdx + i) % CAPACITY + _initIdx;

            Coin curCoin = coinList[setCoinIdx];

            startPos.x = (_centerPos.x - _width * HALF) + COIN_SIZE * HALF;
            startPos.y = (_centerPos.y + _height * HALF) - COIN_SIZE * HALF;

            startPos.y -= i / _width;
            startPos.x += i % _width;

            curCoin.SetPosition(startPos);
            curCoin.SetVisible(true);
            curCoin.ChangeCoinType(COIN_TYPE.silver);

            setCoinIdx++;
            setCoinIdx = setCoinIdx % CAPACITY;
        }
    }

    private void UpdateCollisionCoin()
    {
        //int count = coinList.Count;
        for(int i =0; i < coinListCount; i++)
        {
            Coin curCoin = coinList[i];

            bool isInScene = curCoin.IsInScene();
            if (isInScene)
            {
                AABB coinAABB = curCoin.GetAABB();
                float coinPosX = coinAABB.pos.x;
                float coinPosY = coinAABB.pos.y;
                float coinWidth = coinAABB.width;
                float coinHeight = coinAABB.height;

                if (coinPosX - coinWidth * CORRECTION_HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
                    coinPosX + coinWidth * CORRECTION_HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
                    coinPosY - coinHeight * CORRECTION_HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
                    coinPosY + coinHeight * CORRECTION_HALF > player.GetPlayerPos().y - CORRECTION_HALF)
                {
                    Debug.Log("Coin Collision~~~~~~~~~~~");

                    curCoin.SetVisible(false);
                    curCoin.SetIsInScene(false);

                    //collisionCoinIdx++;
                    //collisionCoinIdx = collisionCoinIdx % CAPACITY;


                    if(COIN_TYPE.bronze == curCoin.GetCoinType())
                    {
                        scoreAmount += (int)COIN_TYPE.bronze;
                        scoreEvnet?.Invoke(scoreAmount);
                    }
                    if(COIN_TYPE.silver == curCoin.GetCoinType())
                    {
                        scoreAmount += (int)COIN_TYPE.silver;
                        scoreEvnet?.Invoke(scoreAmount);
                    }

                }
            }
        }
    }

    private void UpdateCurrentCollisionCoin()
    {
        Coin curBronzeCoin = coinList[collisionCoinIdx];
        if (player.GetPlayerPos().x - HALF > curBronzeCoin.GetPos().x + (COIN_SIZE * HALF))
        {
            collisionCoinIdx++;
            collisionCoinIdx = collisionCoinIdx % CAPACITY;
        }
    }

    private void CheckCollisionCoin()
    {
        AABB curCoin = coinList[collisionCoinIdx].GetAABB();
        float coinPosX = curCoin.pos.x;
        float coinPosY = curCoin.pos.y;
        float coinWidth = curCoin.width;
        float coinHeight = curCoin.height;

        if (coinPosX - coinWidth * CORRECTION_HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
            coinPosX + coinWidth * CORRECTION_HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
            coinPosY - coinHeight * CORRECTION_HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
            coinPosY + coinHeight * CORRECTION_HALF > player.GetPlayerPos().y - CORRECTION_HALF)
        {
            //Debug.Log("Coin Collision~~~~~~~~~~~");

            coinList[collisionCoinIdx].SetVisible(false);
            collisionCoinIdx++;
            collisionCoinIdx = collisionCoinIdx % CAPACITY;

            scoreAmount++;
            scoreEvnet?.Invoke(scoreAmount);
        }

    }



}
