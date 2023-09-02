using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using Unity.VisualScripting;
using System.Threading.Tasks;
using static UserManager;

public class DatabaseManager : MonoBehaviour
{
    public DatabaseReference reference{get;set;}
    public void Init()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateNewUser(string nickname)
    {
        Debug.Log("CreateNewUser");
        WriteNewUser(Managers.GoogleSignIn.GetUID(), 0, 0, null, false, false, false, false, nickname);
        Managers.DBManager.SetCoin(10000);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("inventory").Child("obtainedClothes").SetValueAsync("");
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("inventory").Child("obtainedGraces").SetValueAsync("");
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("inventory").Child("obtainedCollections").SetValueAsync("");
    }

    private void WriteNewUser(string userId, int coin, int score, UserManager.Inventory inventory, bool isCompletedStory, bool isCompletedTutorial, bool isCompletedDiagnosis, bool isKilledWitch, string nickname)
    {
        Managers.UserMng.user.InitUser(userId, coin, score, inventory, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, isKilledWitch, nickname, null, null, "^_^", "");

        string json = JsonUtility.ToJson(Managers.UserMng.user);
        Debug.Log("WriteNewUser");
        Debug.Log(json);

        reference.Child("Users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void SignInUser(string UID)
    {
        if(ReadDataAsync(UID, "UID").ToString() == "NotExist")
        {
            Managers.UI.ShowPopupUI<UI_GetNicknamePopup>();
        }
        else
        {
            string userId = ReadDataAsync(UID, "UID").ToString();
            int coin = int.Parse(ReadDataAsync(UID, "coin").ToString());
            int score = int.Parse(ReadDataAsync(UID, "score").ToString());
            Inventory inventory = new Inventory();
            inventory.obtainedGraces = ReadDataAsync(UID, "obtainedGraces").ToString();
            inventory.obtainedClothes = ReadDataAsync(UID, "obtainedClothes").ToString();
            inventory.obtainedCollections = ReadDataAsync(UID, "obtainedCollections").ToString();
            bool isCompletedStory = bool.Parse(ReadDataAsync(UID, "isCompletedStory").ToString());
            bool isCompletedTutorial = bool.Parse(ReadDataAsync(UID, "isCompletedTutorial").ToString());
            bool isCompletedDiagnosis = bool.Parse(ReadDataAsync(UID, "isCompletedDiagnosis").ToString());
            bool isKilledWitch = bool.Parse(ReadDataAsync(UID, "isKilledWitch").ToString());
            string nickname = ReadDataAsync(UID, "nickname").ToString();
            OneOnOneModeGrace oneOnOneModeGrace = new OneOnOneModeGrace();
            oneOnOneModeGrace = oneOnOneModeGrace.Parse(ReadDataAsync(UID, "oneOnOneModeGrace").ToString());
            StoryModeGrace storyModeGrace = new StoryModeGrace();
            storyModeGrace = storyModeGrace.Parse(ReadDataAsync(UID, "storyModeGrace").ToString());
            string message= ReadDataAsync(UID, "message").ToString();
            string myClothes= ReadDataAsync(UID, "myClothes").ToString();

            Managers.UserMng.user.InitUser(userId, coin, score, inventory, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, isKilledWitch, nickname, oneOnOneModeGrace, storyModeGrace, message, myClothes);
        }
    }

    public async Task<string> ReadDataAsync(string userId, string key)
    {
        return await ReadUserAsync(userId, key);
    }

    private async Task<string> ReadUserAsync(string userId, string key)
    {
        string result = "";

        DataSnapshot snapshot = await reference.Child("Users").Child(userId).GetValueAsync();

        if (snapshot.Exists)
        {
            Debug.Log(snapshot.ChildrenCount);

            foreach (DataSnapshot data in snapshot.Children)
            {
                if (data.Key == key)
                {
                    result = data.Value.ToString();
                    Debug.Log(result);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("NotExist");
            result = "NotExist";
        }

        return result;
    }

    public async Task<bool> CheckUserId(string userId)
    {
        Managers.Game.IsExisted = false;
        DataSnapshot snapshot = await reference.Child("Users").GetValueAsync();

        if (snapshot.Exists)
        {
            Debug.Log(snapshot.ChildrenCount);

            foreach (DataSnapshot data in snapshot.Children)
            {
                Debug.Log("데이터 키값" + data.Key);
                if (data.Key == userId)
                {
                    Managers.Game.IsExisted = true;
                    Debug.Log("0000000000000000000000000000000");
                }
            }

            return Managers.Game.IsExisted;
        }
        else
        {
            Debug.Log("error");
        }

        return Managers.Game.IsExisted;
    }


    public void SetNickname(string nickname)
    {
        Managers.UserMng.SetNickname(nickname);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("nickname").SetValueAsync(Managers.UserMng.user.nickname);
    }

    public void SetUserMessage(string message)
    {
        Managers.UserMng.SetUserMessage(message);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("message").SetValueAsync(Managers.UserMng.user.message);
    }

    public void SetOneOnOneGrace(string grace1, string grace2, string grace3)
    {
        Managers.UserMng.SetOneOnOneGrace(grace1, grace2, grace3);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("OneOnOneGrace").Child("1").SetValueAsync(Managers.UserMng.user.oneOnOneModeGrace.grace1);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("OneOnOneGrace").Child("2").SetValueAsync(Managers.UserMng.user.oneOnOneModeGrace.grace2);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("OneOnOneGrace").Child("3").SetValueAsync(Managers.UserMng.user.oneOnOneModeGrace.grace3);
    }

    public void SetStoryGrace(string grace1, string grace2, string grace3)
    {
        Managers.UserMng.SetStoryGrace(grace1, grace2, grace3);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("StoryGrace").Child("1").SetValueAsync(Managers.UserMng.user.storyModeGrace.grace1);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("StoryGrace").Child("2").SetValueAsync(Managers.UserMng.user.storyModeGrace.grace2);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("StoryGrace").Child("3").SetValueAsync(Managers.UserMng.user.storyModeGrace.grace3);
    }


    public void SetCoin(int coin)
    {
        Managers.UserMng.SetUserCoin(coin);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("coin").SetValueAsync(Managers.UserMng.user.coin);
    }

    public async Task<int> GetCoin(string userId)
    {
        return int.Parse(await ReadUserAsync(userId, "coin"));
    }

    public void SetScore(int score)
    {
        Managers.UserMng.SetUserScore(score);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("score").SetValueAsync(Managers.UserMng.user.score);
    }

    public async Task<int> GetScore(string userId)
    {
        return int.Parse(await ReadUserAsync(userId, "score"));
    }

    public void SetIsCompletedDiagnosis(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedDiagnosis(isCompleted);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isCompletedDiagnosis").SetValueAsync(Managers.UserMng.user.isCompletedDiagnosis);
    }
    public async Task<bool> GetIsCompletedDiagnosis(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isCompletedDiagnosis"));
    }

    public void SetIsCompletedStory(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedStory(isCompleted);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isCompletedStory").SetValueAsync(Managers.UserMng.user.isCompletedStory);
    }

    public async Task<bool> GetIsCompletedStory(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isCompletedStory"));
    }


    public void SetIsKilledWitch(bool isKilled)
    {
        Managers.UserMng.SetUseIsKilledWitch(isKilled);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isKilledWitch").SetValueAsync(Managers.UserMng.user.isKilledWitch);
    }

    public async Task<bool> GetIsKilledWitch(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isKilledStory"));
    }


    public void SetMyClothes(string myClothes)
    {
        Managers.UserMng.SetUserMyClothes(myClothes);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("myClothes").SetValueAsync(Managers.UserMng.user.myClothes);
    }

    public async Task<string> GetMyClothes(string userId)
    {
        return await ReadUserAsync(userId, "myClothes");
    }

    public void SetIsCompletedTutorial(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedTutorial(isCompleted);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isCompletedTutorial").SetValueAsync(Managers.UserMng.user.isCompletedTutorial);
    }
    public async Task<bool> GetIsCompletedTutorial(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isCompletedTutorial"));
    }

    public void SetObtainedClothes(string clothes)
    {
        Managers.UserMng.SetUserObtainedClothes(clothes);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("inventory").Child("obtainedClothes").SetValueAsync(Managers.UserMng.user.inventory.obtainedClothes);
    }
    //public List<string> GetObtainedClothes(string userId)
    //{
    //    return StringToList(ReadUser(userId, "obtainedClothes"));
    //}

    public void SetObtainedGraces(string graces)
    {
        Managers.UserMng.SetUserObtainedGraces(graces);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("inventory").Child("obtainedGraces").SetValueAsync(Managers.UserMng.user.inventory.obtainedGraces);
    }

    public void SetObtainedCollections(string collections)
    {
        Managers.UserMng.SetUserObtainedCollections(collections);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("inventory").Child("obtainedCollections").SetValueAsync(Managers.UserMng.user.inventory.obtainedCollections);
    }

}
