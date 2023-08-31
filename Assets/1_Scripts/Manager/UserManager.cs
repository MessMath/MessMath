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
        public int coin;
        public int score;
        public Inventory inventory;
        public bool isCompletedStory;
        public bool isCompletedTutorial;
        public bool isCompletedDiagnosis;
        public string nickname;
        public OneOnOneModeGrace oneOnOneModeGrace;
        public StoryModeGrace storyModeGrace;
        public string message;
        //public string obtainedCollections;
        //public string obtainedClothes;
        //public string obtainedGraces;

        public User(string UID, int coin, int score, Inventory inventory ,bool isCompletedStory, bool isCompletedTutorial,
            bool isCompletedDiagnosis, string nickname, OneOnOneModeGrace oneOnOneModeGrace, StoryModeGrace storyModeGrace, string message)
        {
            this.UID = UID;
            this.coin = coin;
            this.score = score;
            this.isCompletedStory = isCompletedStory;
            this.isCompletedTutorial= isCompletedTutorial;
            this.isCompletedDiagnosis = isCompletedDiagnosis;
            this.nickname = nickname;
            this.oneOnOneModeGrace = oneOnOneModeGrace;
            this.storyModeGrace= storyModeGrace;
            this.message = message;
            this.inventory = new Inventory();
            this.oneOnOneModeGrace = new OneOnOneModeGrace();
            this.storyModeGrace= new StoryModeGrace();
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
    public void SetUserScore(int score)
    {
        user.score = score;
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
        user.inventory.obtainedClothes += clothes + ",";
    }

    public void SetUserObtainedGraces(string graces)
    {
        user.inventory.obtainedGraces += graces + ",";
    }

    public void SetUserObtainedCollections(string collections)
    {
        user.inventory.obtainedCollections += collections + ",";
    }

    public string GetNickname()
    {
        return user.nickname;
    }

    public int GetCoin()
    {
        return user.coin;
    }

    public int GetScore()
    {
        return user.score;
    }
    public List<string> GetObtainedClothes()
    {
        return StringToList(user.inventory.obtainedClothes);
    }

    public List<string> GetObtainedGraces()
    {
        return StringToList(user.inventory.obtainedGraces);
    }

    public List<string> GetObtainedCollections()
    {
        return StringToList(user.inventory.obtainedCollections);
    }

    public bool GetIsCompletedStory()
    {
        return user.isCompletedStory;
    }

    public bool GetIsCompletedTutorial()
    {
        return user.isCompletedTutorial;
    }

    public bool GetIsCompletedDiagnosis()
    {
        return user.isCompletedDiagnosis;
    }

    public OneOnOneModeGrace GetOneOnOneModeGrace()
    {
        return user.oneOnOneModeGrace;
    }
    public StoryModeGrace GetStoryModeGrace()
    {
        return user.storyModeGrace;
    }

    public string GetMessage()
    {
        return user.message;
    }

    private List<string> StringToList(string data)
    {
        if (data == null)
            return null;
        string[] lines = data.Split(',');
        List<string> list = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            list.Add(lines[i]);
        }

        return list;
    }

}
