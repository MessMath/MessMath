using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager
{
    //public class Inventory
    //{

    //}

    public class User
    {
        public class OneOnOneModeGrace
        {
            public string grace1;
            public string grace2;
            public string grace3;
        };
        public class StoryModeGrace
        {
            public string grace1;
            public string grace2;
            public string grace3;
        }

        public string UID;
        public int coin;
        public int score;
        //public Inventory inventory;
        public bool isCompletedStory;
        public bool isCompletedTutorial;
        public bool isCompletedDiagnosis;
        public string nickname;
        public string message;

        public OneOnOneModeGrace oneOnOneModeGrace;
        public StoryModeGrace storyModeGrace;

        public User(string UID, int coin, int score, bool isCompletedStory, bool isCompletedTutorial,  bool isCompletedDiagnosis, string nickname, string message)
        {
            this.UID = UID;
            this.coin = coin;
            this.score = score;
            //this.inventory = inventory;
            this.isCompletedStory = isCompletedStory;
            this.isCompletedTutorial= isCompletedTutorial;
            this.isCompletedDiagnosis = isCompletedDiagnosis;
            this.nickname = nickname;
            this.message = message;
            //this.onOneModeGrace= pvpModeGrace;
        }
    }

    public User user {get;set;}

    public void InitUser(string UID, int coin, int score, bool isCompletedStory, bool isCompletedTutorial, bool isCompletedDiagnosis, string nickname, string message)
    {
        user = new User(UID, coin, score, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname, message);
        user.storyModeGrace = new User.StoryModeGrace();
        user.oneOnOneModeGrace = new User.OneOnOneModeGrace();
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
        string message = Managers.DBManager.ReadData(UID, "message");

        InitUser(UID, coin, score, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname, message);
    }

    public void SetNickname(string nickname)
    {
        user.nickname = nickname;
    }

    public void SetUserMessage(string messsage)
    {
        user.message = messsage;
    }

    public void SetOneOnOneGrace(string grace1 = null, string grace2 = null, string grace3 = null)
    {
        user.oneOnOneModeGrace.grace1 = grace1;
        user.oneOnOneModeGrace.grace2 = grace2;
        user.oneOnOneModeGrace.grace2 = grace3;
    }
    public void SetStoryGrace(string grace1 = null, string grace2 = null, string grace3 = null)
    {
        user.storyModeGrace.grace1 = grace1;
        user.storyModeGrace.grace2 = grace2;
        user.storyModeGrace.grace3 = grace3;
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
