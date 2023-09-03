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

    public string UID;
    public int coin;
    public int score;
    public Inventory inventory = new Inventory();
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
        oneOnOneModeGrace = _oneOnOneModeGrace;
        storyModeGrace = _storyModeGrace;
    }

    public void Init()
    {        
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

    public void SetNickname(string _nickname)
    {
        nickname = _nickname;
    }

    public void SetUserMessage(string _messsage)
    {
        message = _messsage;
    }

    public void SetOneOnOneGrace(string grace1 = null, string grace2 = null, string grace3 = null)
    {
        oneOnOneModeGrace.grace1 = grace1;
        oneOnOneModeGrace.grace2 = grace2;
        oneOnOneModeGrace.grace2 = grace3;
    }
    public void SetStoryGrace(string grace1 = null, string grace2 = null, string grace3 = null)
    {
        storyModeGrace.grace1 = grace1;
        storyModeGrace.grace2 = grace2;
        storyModeGrace.grace3 = grace3;
    }
    public void SetUserCoin(int _coin)
    {
        coin = _coin;
    }
    public void SetUserScore(int _score)
    {
        score = _score;
    }

    public void SetUserIsCompletedStory(bool isCompleted)
    {
        isCompletedStory = isCompleted;
    }

    public void SetUserIsCompletedTutorial(bool isCompleted)
    {
        isCompletedTutorial = isCompleted;
    }

    public void SetUserIsCompletedDiagnosis(bool isCompleted)
    {
        isCompletedDiagnosis = isCompleted;
    }

    public void SetUseIsKilledWitch(bool isKilled)
    {
        isKilledWitch = isKilled;
    }

    public void SetUserMyClothes(string _myClothes)
    {
        myClothes = _myClothes;
    }

    public void SetUserObtainedClothes(string clothes)
    {
        inventory.obtainedClothes += clothes + ",";
    }

    public void SetUserObtainedGraces(string graces)
    {
        inventory.obtainedGraces += graces + ",";
    }

    public void SetUserObtainedCollections(string collections)
    {
        inventory.obtainedCollections += collections + ",";
    }

    public string GetNickname()
    {
        return nickname;
    }

    public int GetCoin()
    {
        return coin;
    }

    public int GetScore()
    {
        return score;
    }
    public List<string> GetObtainedClothes()
    {
        return StringToList(inventory.obtainedClothes);
    }

    public List<string> GetObtainedGraces()
    {
        return StringToList(inventory.obtainedGraces);
    }

    public List<string> GetObtainedCollections()
    {
        return StringToList(inventory.obtainedCollections);
    }

    public bool GetIsCompletedStory()
    {
        return isCompletedStory;
    }

    public bool GetIsCompletedTutorial()
    {
        return isCompletedTutorial;
    }

    public bool GetIsCompletedDiagnosis()
    {
        return isCompletedDiagnosis;
    }

    public bool GetIsKilledWitch()
    {
        return isKilledWitch;
    }

    public string GetMyClothes()
    {
        return myClothes;
    }

    public OneOnOneModeGrace GetOneOnOneModeGrace()
    {
        return oneOnOneModeGrace;
    }
    public StoryModeGrace GetStoryModeGrace()
    {
        return storyModeGrace;
    }

    public string GetMessage()
    {
        return message;
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
