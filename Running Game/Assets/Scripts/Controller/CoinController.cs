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
    private const int COIN_SQUARE_PATTURN_FLOOR_BETWEEN = 4;
    private const int COIN_SQUARE_PATTURN_WIDTH_MIN = 1;
    private const int COIN_SQUARE_PATTURN_WIDTH_MAX = 6;
    private const int COIN_SQUARE_PATTURN_HEIGHT_MIN = 1;
    private const int COIN_SQUARE_PATTURN_HEIGHT_MAX = 4;

    public delegate void ScoreDelegate(int _score);
    public ScoreDelegate onScoreEvnet;

    private Player player;
    private SpriteRenderer coinSpriteRenderer;
    private Vector2 coinSetVector = Vector2.zero;

    private int setCoinIdx;
    private int scoreAmount;
    private int coinListCount;

    private List<Coin> coinList = new List<Coin>();

    public CoinController(Transform _parent, Player _player, float _reposX, float _inScenePosX)
    {
        player = _player;

        coinSpriteRenderer = Resources.Load<SpriteRenderer>("Prefab/Coin/Coin");

        CreateCoin(coinSpriteRenderer, _parent, _reposX, _inScenePosX);


        scoreAmount = 0;
        setCoinIdx = 0;
        coinListCount = coinList.Count;

    }

    public void FixedUpdateCoin()
    {
        MoveCoin();
        UpdateCollisionCoin();
        MoveInMagnetRangeCoin();
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
        }
    }

    private void MoveInMagnetRangeCoin()
    {
        if (player.IsMagnet())
        {
            for (int i = 0; i < coinListCount; i++)
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

                    Vector2 playerPos = player.GetPlayerPos();
                    int magnetRange = player.GetMagnetRange();

                    if (coinPosX - coinWidth * CORRECTION_HALF < playerPos.x + magnetRange &&
                        coinPosX + coinWidth * CORRECTION_HALF > playerPos.x - magnetRange &&
                        coinPosY - coinHeight * CORRECTION_HALF < playerPos.y + magnetRange &&
                        coinPosY + coinHeight * CORRECTION_HALF > playerPos.y - magnetRange)
                    {
                        Debug.Log("On Magnet Range");
                        curCoin.MoveToTarget(player);
                    }
                }
            }
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
            float obstaclePosX = obstacleAABB.pos.x;

            Coin coin = coinList[setCoinIdx];
            coinSetVector = Vector2.zero;
            coinSetVector.x = (floorAABB.pos.x - floorAABB.width * HALF) + (COIN_SIZE * HALF) + i;


            if (obstaclePosX - COIN_INTERVAR < coinSetVector.x && obstaclePosX + COIN_INTERVAR > coinSetVector.x)
            {
                float floorY = (floorAABB.pos.y + floorAABB.height * HALF) + (COIN_SIZE * HALF);
                coinSetVector.y = floorY + JUMP_COIN_HEIGHT;
                coin.SetPosition(coinSetVector);
                coin.SetVisible(true);

                setCoinIdx++;
                setCoinIdx = setCoinIdx % CAPACITY;

            }
            else
            {
                coinSetVector.y = (floorAABB.pos.y + floorAABB.height * HALF) + (COIN_SIZE * HALF);
                coin.SetPosition(coinSetVector);
                coin.SetVisible(true);

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
        centerPos.y += COIN_SQUARE_PATTURN_FLOOR_BETWEEN;

        int width = GetRandomValue(COIN_SQUARE_PATTURN_WIDTH_MIN, (int)between);
        //int width = (int)between;
        int height = GetRandomValue(COIN_SQUARE_PATTURN_HEIGHT_MIN, COIN_SQUARE_PATTURN_HEIGHT_MAX);
        SetCoinSquarePattern(centerPos, width, height);

    }

    private void SetCoinSquarePattern(Vector2 _centerPos, int _width, int _height)
    {
        int count = _width * _height;
        Vector2 startPos = _centerPos;

        for (int i =0; i < count; i++)
        {
            Coin curCoin = coinList[setCoinIdx];

            startPos.x = (_centerPos.x - _width * HALF) + COIN_SIZE * HALF;
            startPos.y = (_centerPos.y + _height * HALF) - COIN_SIZE * HALF;

            startPos.y -= i / _width;
            startPos.x += i % _width;

            curCoin.SetPosition(startPos);
            curCoin.SetVisible(true);
            if(count <= 3)
            {
                curCoin.ChangeCoinType(COIN_TYPE.gold);
            }
            else if(6 >= count && count > 3)
            {
                curCoin.ChangeCoinType(COIN_TYPE.silver);
            }
            else
            {
                curCoin.ChangeCoinType(COIN_TYPE.bronze);
            }


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
                    //Debug.Log("Coin Collision~~~~~~~~~~~");

                    curCoin.SetVisible(false);
                    curCoin.SetIsInScene(false);

                    scoreAmount += (int)curCoin.GetCoinType();
                    onScoreEvnet?.Invoke(scoreAmount);
                }
            }
        }
    }

    private void NomalCollisionCoinCalculation(Coin _coin)
    {
        AABB coinAABB = _coin.GetAABB();
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

            _coin.SetVisible(false);
            _coin.SetIsInScene(false);

            scoreAmount += (int)_coin.GetCoinType();
            onScoreEvnet?.Invoke(scoreAmount);
        }
    }

    private void MagnetItemCollisionCoinCalculation(Coin _coin)
    {
        AABB coinAABB = _coin.GetAABB();
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

            _coin.SetVisible(false);
            _coin.SetIsInScene(false);

            scoreAmount += (int)_coin.GetCoinType();
            onScoreEvnet?.Invoke(scoreAmount);
        }
    }

    public int GetRandomValue(int _min, int _max)
    {
        return Random.Range(_min, _max);
    }

}
