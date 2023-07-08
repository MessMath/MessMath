using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager
{
    public int GetCoin()
    {
        if (PlayerPrefs.HasKey("Coin"))
            return PlayerPrefs.GetInt("Coin");
        else { PlayerPrefs.SetInt("Coin", 0); return PlayerPrefs.GetInt("Coin"); }
    }

    public bool CheckPurchase(int price)
    {
        if(price <= GetCoin())
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
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - price);
        if (GetCoin() <= 0) PlayerPrefs.SetInt("Coin", 0);
    }
}
