using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager
{
    /*public class Inventory
    {

    }
    public class PVPModeGrace
    {

    }
    public class StoryModeGrace
    {

    }*/

    public class User
    {
        public string UID;
        public int coin;
        public int score;
        //public Inventory inventory;
        public bool isCompletedStory;
        public bool isCompletedTutorial;
        public bool isCompletedDiagnosis;
        public string nickname;
        //public PVPModeGrace pvpModeGrace;
        //public StoryModeGrace storyModeGrace;

        public User(string UID, int coin, int score, bool isCompletedStory, bool isCompletedTutorial,  bool isCompletedDiagnosis, string nickname)
        {
            this.UID = UID;
            this.coin = coin;
            this.score = score;
            //this.inventory = inventory;
            this.isCompletedStory = isCompletedStory;
            this.isCompletedTutorial= isCompletedTutorial;
            this.isCompletedDiagnosis = isCompletedDiagnosis;
            this.nickname = nickname;
            //this.pvpModeGrace= pvpModeGrace;
            //this.storyModeGrace= storyModeGrace;
        }
    }

    public User user {get;set;}

    public void InitUser(string UID, int coin, int score, bool isCompletedStory, bool isCompletedTutorial, bool isCompletedDiagnosis, string nickname)
    {
        user = new User(UID, coin, score, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname);
    }

    public void SetExistingUser()
    {
        string UID = Managers.GoogleSignIn.GetUID();
        int coin = int.Parse(Managers.DBManager.ReadData(UID, "coin"));
        int score = int.Parse(Managers.DBManager.ReadData(UID, "score"));
        bool isCompletedStory = bool.Parse(Managers.DBManager.ReadData(UID, "isCompletedStory"));
        bool isCompletedTutorial = bool.Parse(Managers.DBManager.ReadData(UID, "isCompletedTutorial"));
        bool isCompletedDiagnosis = bool.Parse(Managers.DBManager.ReadData(UID, "isCompletedDiagnosis"));
        string nickname = Managers.DBManager.ReadData(UID, "nickname");

        InitUser(UID, coin, score, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname);
    }

    public void AddUserCoin(int coin)
    {
        user.coin += coin;
    }

    public void SetIsCompletedStory()
    {
        user.isCompletedStory = true;
    }

    public void isCompletedTutorial()
    {
        user.isCompletedTutorial = true;
    }

    public void isCompletedDiagnosis()
    {
        user.isCompletedDiagnosis = true;
    }

}
