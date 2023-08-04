using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager
{
    public class Inventory
    {
        public string obtainedClothes {get;set;}
        public string obtainedGraces {get;set;}
        public string obtainedCollections {get;set;}
    }
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

    public class User
    {
        public string UID;
        public int coin { get; set; }
        public int score { get; set; }
        public Inventory inventory { get; set; }
        public bool isCompletedStory { get; set; }
        public bool isCompletedTutorial { get; set; }
        public bool isCompletedDiagnosis { get; set; }
        public string nickname { get; set; }
        public OneOnOneModeGrace oneOnOneModeGrace { get; set; }
        public StoryModeGrace storyModeGrace { get; set; }
        public string message { get; set; }

        public User(string UID, int coin, int score, Inventory inventory ,bool isCompletedStory, bool isCompletedTutorial,
            bool isCompletedDiagnosis, string nickname, OneOnOneModeGrace oneOnOneModeGrace, StoryModeGrace storyModeGrace, string message)
        {
            this.UID = UID;
            this.coin = coin;
            this.score = score;
            this.inventory = inventory;
            this.isCompletedStory = isCompletedStory;
            this.isCompletedTutorial= isCompletedTutorial;
            this.isCompletedDiagnosis = isCompletedDiagnosis;
            this.nickname = nickname;
            this.oneOnOneModeGrace = oneOnOneModeGrace;
            this.storyModeGrace= storyModeGrace;
            this.message = message;
        }
    }

    public User user {get;set;}

    public void InitUser(string UID, int coin, int score, Inventory inventory,bool isCompletedStory,
        bool isCompletedTutorial, bool isCompletedDiagnosis, string nickname, OneOnOneModeGrace oneOnOneModeGrace, StoryModeGrace storyModeGrace, string message)
    {
        user = new User(UID, coin, score, inventory, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname, oneOnOneModeGrace, storyModeGrace, message);
    }

    //public void SetExistingUser()
    //{
    //    string UID = Managers.GoogleSignIn.GetUID();
    //    int coin = int.Parse(Managers.DBManager.ReadData(UID, "coin"));
    //    int score = int.Parse(Managers.DBManager.ReadData(UID, "score"));
    //    bool isCompletedStory = bool.Parse(Managers.DBManager.ReadData(UID, "isCompletedStory"));
    //    bool isCompletedTutorial = bool.Parse(Managers.DBManager.ReadData(UID, "isCompletedTutorial"));
    //    bool isCompletedDiagnosis = bool.Parse(Managers.DBManager.ReadData(UID, "isCompletedDiagnosis"));
    //    string nickname = Managers.DBManager.ReadData(UID, "nickname");

    //    InitUser(UID, coin, score, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname);
    //}

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
    public void SetUserCoin(int coin)
    {
        user.coin = coin;
    }

    public void SetUserIsCompletedStory(bool isCompleted)
    {
        user.isCompletedStory = isCompleted;
    }

    public void SetUserIsCompletedTutorial(bool isCompleted)
    {
        user.isCompletedTutorial = isCompleted;
    }

    public void SetUserIsCompletedDiagnosis(bool isCompleted)
    {
        user.isCompletedDiagnosis = isCompleted;
    }

    public void SetUserObtainedClothes(string clothes)
    {
        user.inventory.obtainedClothes = clothes;
    }

    public void SetUserObtainedGraces(string graces)
    {
        user.inventory.obtainedGraces = graces;
    }

    public void SetUserObtainedCollections(string collections)
    {
        user.inventory.obtainedCollections = collections;
    }



}
