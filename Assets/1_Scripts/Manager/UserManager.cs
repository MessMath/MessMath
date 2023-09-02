using Firebase.Database;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserManager;

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

        public OneOnOneModeGrace Parse(string str)
        {
            OneOnOneModeGrace oneOnOneModeGrace = new OneOnOneModeGrace();
            string[] tmp = str.Split(',');
            if (tmp[0] == "null")
            {
                oneOnOneModeGrace.grace1 = "";
            }
            else
            {
                oneOnOneModeGrace.grace1 = tmp[0];
            }
            if (tmp[1] == "null")
            {
                oneOnOneModeGrace.grace2 = "";
            }
            else
            {
                oneOnOneModeGrace.grace2 = tmp[1];
            }
            if (tmp[2] == "null")
            {
                oneOnOneModeGrace.grace3 = "";
            }
            else
            {
                oneOnOneModeGrace.grace3 = tmp[2];
            }

            return oneOnOneModeGrace;
        }
    };
    public class StoryModeGrace
    {
        public string grace1;
        public string grace2;
        public string grace3;

        public StoryModeGrace Parse(string str)
        {
            StoryModeGrace storyModeGrace = new StoryModeGrace();
            string[] tmp = str.Split(',');
            if (tmp[0] == "null")
            {
                storyModeGrace.grace1 = "";
            }
            else
            {
                storyModeGrace.grace1 = tmp[0];
            }
            if (tmp[1] == "null")
            {
                storyModeGrace.grace2 = "";
            }
            else
            {
                storyModeGrace.grace2 = tmp[1];
            }
            if (tmp[2] == "null")
            {
                storyModeGrace.grace3 = "";
            }
            else
            {
                storyModeGrace.grace3 = tmp[2];
            }
            return storyModeGrace;
        }
    }

    public class User
    {
        private static User instance = null;
        public User Instance()
        {
            if (instance == null)
            {
                instance = new User();
            }
            return instance;
        }

        public string UID;
        public int coin;
        public int score;
        public Inventory inventory;
        public bool isCompletedStory;
        public bool isCompletedTutorial;
        public bool isCompletedDiagnosis;
        public bool isKilledWitch;
        public string nickname;
        public OneOnOneModeGrace oneOnOneModeGrace;
        public StoryModeGrace storyModeGrace;
        public string message;
        public string myClothes;
        //public string obtainedCollections;
        //public string obtainedClothes;
        //public string obtainedGraces;

        public void InitUser(string _UID, int _coin, int _score, Inventory _inventory, bool _isCompletedStory,
        bool _isCompletedTutorial, bool _isCompletedDiagnosis, bool _isKilledWitch, string _nickname, OneOnOneModeGrace _oneOnOneModeGrace, StoryModeGrace _storyModeGrace, string _message, string _myClothes)
        {
            UID = _UID;
            coin = _coin;
            score = _score;
            isCompletedStory = _isCompletedStory;
            isCompletedTutorial = _isCompletedTutorial;
            isCompletedDiagnosis = _isCompletedDiagnosis;
            isKilledWitch = _isKilledWitch;
            nickname = _nickname;
            oneOnOneModeGrace = _oneOnOneModeGrace;
            storyModeGrace = _storyModeGrace;
            message = _message;
            myClothes = _myClothes;
            if (_inventory == null)
            {
                inventory = new Inventory();
            }
            else
            {
                inventory = _inventory;
            }

            oneOnOneModeGrace = _oneOnOneModeGrace;
            storyModeGrace = _storyModeGrace;
        }
    }

    public User user;

    public void Init()
    {
        user = user.Instance();
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

    public void SetUseIsKilledWitch(bool isKilled)
    {
        user.isKilledWitch = isKilled;
    }

    public void SetUserMyClothes(string myClothes)
    {
        user.myClothes = myClothes;
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

    public bool GetIsKilledWitch()
    {
        return user.isKilledWitch;
    }

    public string GetMyClothes()
    {
        return user.myClothes;
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
