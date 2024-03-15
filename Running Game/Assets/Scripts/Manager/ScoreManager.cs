using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private int coinCoefficient;
    private int score;
    private int totalCoin;
    private int resultCoin;

    public override bool Initialize()
    {
        coinCoefficient = 10;
        score = 0;
        totalCoin = 0;
        return base.Initialize();
    }

    public void SetScore(int _score)
    {
        score = _score;
        resultCoin = score / coinCoefficient;
        CoinCalculate();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetResultCoin()
    {
        return resultCoin;
    }

    public int GetCoin()
    {
        return totalCoin;
    }

    public bool DecreaseCoin(int _amount)
    {
        if(totalCoin - _amount <= 0)
        {
            return false;
        }

        totalCoin -= _amount;
        return true;
    }

    private void CoinCalculate()
    {
        totalCoin += resultCoin;
    }


}
