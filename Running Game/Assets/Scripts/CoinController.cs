using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController
{
    private const int CAPACITY = 30;

    private Player player;
    private SpriteRenderer bronzeCoin;


    private List<Coin> bronzeCoinList = new List<Coin>();

    public CoinController(Transform _parent, Player _player, float _reposX)
    {
        player = _player;
        bronzeCoin = Resources.Load<SpriteRenderer>("Prefab/Coin/BronzeCoin");


        CreateCoin(bronzeCoin, _parent, _reposX);
    }

    public void UpdateCoin()
    {

    }

    private void CreateCoin(SpriteRenderer _coinSprite, Transform _parent, float _reposX)
    {
        for(int i =0; i < CAPACITY; i++)
        {
            bronzeCoinList.Add(new Coin(_coinSprite, _parent, _reposX));
        }
    }
}
