using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class CoinManager
{
    public async Task<int> GetCoin()
    {
        return await Managers.DBManager.GetCoin(Managers.GoogleSignIn.GetUID());
    }

    public async Task<bool> CheckPurchase(int price)
    {
        if(price <= await GetCoin())
        {
            Purchase(price);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async void Purchase(int price)
    {
        Managers.DBManager.SetCoin(await GetCoin() - price);
        if (await GetCoin() <= 0) Managers.DBManager.SetCoin(0);
    }
}
