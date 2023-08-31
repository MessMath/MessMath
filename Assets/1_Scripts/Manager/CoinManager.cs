using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager
{
    public int GetCoin()
    {
        return Managers.UserMng.GetCoin();
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
        Managers.DBManager.SetCoin(GetCoin() - price);
        if (GetCoin() <= 0) Managers.DBManager.SetCoin(0);
    }
}
