using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager
{
    int coin = 100;

    public int GetCoin()
    {
        return coin;
    }

    public bool CheckPurchase(int price)
    {
        if(price <= coin)
        {
            Purchase(price);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Purchase(int price)
    {
        coin -= price;
        if(coin <= 0) coin = 0;
    }
}
