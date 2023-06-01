using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlessingManager
{
    GameObject player;
    PlayerControllerCCF playerController;
    bool tempBlessed = false;

    public void Init()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerController = player.GetComponent<PlayerControllerCCF>();
    }

    public IEnumerator tempBlessing()
    {
        if(playerController.Hp == 1 && tempBlessed == false)
        {
            tempBlessed = true;
            yield return new WaitForSeconds(1);
            playerController.Hp += 1;
            Debug.Log("tempBlessed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}
