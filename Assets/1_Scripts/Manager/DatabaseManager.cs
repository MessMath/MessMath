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
using System;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class DatabaseManager : MonoBehaviour
{
    public DatabaseReference reference { get; set; }
    public void Init()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateNewUser(string nickname)
    {
        UnityEngine.Debug.Log("CreateNewUser");
        WriteNewUser(Managers.GoogleSignIn.GetUID(), 0, 0, null, false, false, false, false, nickname);
        Managers.DBManager.SetCoin(10000);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("inventory").Child("obtainedClothes").SetValueAsync("");
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("inventory").Child("obtainedGraces").SetValueAsync("");
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("inventory").Child("obtainedCollections").SetValueAsync("");
    }

    private void WriteNewUser(string userId, int coin, int score, UserManager.Inventory inventory, bool isCompletedStory, bool isCompletedTutorial, bool isCompletedDiagnosis, bool isKilledWitch, string nickname)
    {
        Managers.UserMng.InitUser(userId, coin, score, inventory, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, isKilledWitch, nickname, null, null, "^_^", "");

        string json = JsonUtility.ToJson(Managers.UserMng);
        UnityEngine.Debug.Log("WriteNewUser");
        UnityEngine.Debug.Log(json);

        reference.Child("Users").Child(userId).SetRawJsonValueAsync(json);
    }

    public async void SignInUser(string UID)
    {
        //if(ReadDataAsync(UID, "UID").ToString() == "NotExist")
        //{
        //    Managers.UI.ShowPopupUI<UI_GetNicknamePopup>();
        //}
        //else
        //{
        string userId = await ReadDataAsync(UID, "UID");
        int coin = int.Parse(await ReadDataAsync(UID, "coin"));
        int score = int.Parse(await ReadDataAsync(UID, "score"));
        Inventory inventory = new Inventory();
        inventory.obtainedGraces = await ReadDataAsync(UID, "obtainedGraces");
        inventory.obtainedClothes = await ReadDataAsync(UID, "obtainedClothes");
        inventory.obtainedCollections = await ReadDataAsync(UID, "obtainedCollections");
        bool isCompletedStory = bool.Parse(await ReadDataAsync(UID, "isCompletedStory"));
        bool isCompletedTutorial = bool.Parse(await ReadDataAsync(UID, "isCompletedTutorial"));
        bool isCompletedDiagnosis = bool.Parse(await ReadDataAsync(UID, "isCompletedDiagnosis"));
        bool isKilledWitch = bool.Parse(await ReadDataAsync(UID, "isKilledWitch"));
        string nickname = await ReadDataAsync(UID, "nickname");
        OneOnOneModeGrace oneOnOneModeGrace = new OneOnOneModeGrace();
        oneOnOneModeGrace = oneOnOneModeGrace.Parse(await ReadDataAsync(UID, "oneOnOneModeGrace"));
        StoryModeGrace storyModeGrace = new StoryModeGrace();
        storyModeGrace = storyModeGrace.Parse(await ReadDataAsync(UID, "storyModeGrace"));
        string message = await ReadDataAsync(UID, "message");
        string myClothes = await ReadDataAsync(UID, "myClothes");

        Managers.UserMng.InitUser(userId, coin, score, inventory, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, isKilledWitch, nickname, oneOnOneModeGrace, storyModeGrace, message, myClothes);
        //}
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
            UnityEngine.Debug.Log(snapshot.ChildrenCount);

            foreach (DataSnapshot data in snapshot.Children)
            {
                if (data.Key == key)
                {
                    result = data.Value.ToString();
                    UnityEngine.Debug.Log(result);
                    break;
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("NotExist");
            result = "NotExist";
        }

        return result;
    }

    private async Task<string> ReadInventoryAsync(string userId, string key)
    {
        string result = "";

        DataSnapshot snapshot = await reference.Child("Users").Child(userId).Child("inventory").GetValueAsync();

        if (snapshot.Exists)
        {
            UnityEngine.Debug.Log(snapshot.ChildrenCount);

            foreach (DataSnapshot data in snapshot.Children)
            {
                if (data.Key == key)
                {
                    result = data.Value.ToString();
                    UnityEngine.Debug.Log(result);
                    break;
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("NotExist");
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
            UnityEngine.Debug.Log(snapshot.ChildrenCount);

            foreach (DataSnapshot data in snapshot.Children)
            {
                if (data.Key == userId)
                {
                    Managers.Game.IsExisted = true;
                }
            }

            return Managers.Game.IsExisted;
        }
        else
        {
            UnityEngine.Debug.Log("error");
        }

        return Managers.Game.IsExisted;
    }

    public List<string> ParseObtanined(string str)
    {
        List<string> result = new List<string>();
        foreach (string s in str.Split(","))
        {
            result.Add(s);
        }
        return result;
    }

    async public Task<string> GetNickName(string userId)
    {
        return await ReadUserAsync(userId, "nickname");
    }

    async public Task<string> GetUserMessage(string userId)
    {
        return await ReadUserAsync(userId, "message");
    }

    public async Task<int> GetCoin(string userId)
    {
        return int.Parse(await ReadUserAsync(userId, "coin"));
    }

    public async Task<int> GetScore(string userId)
    {
        return int.Parse(await ReadUserAsync(userId, "score"));
    }

    public async Task<bool> GetIsCompletedDiagnosis(string userId)
    {
        return Convert.ToBoolean(await ReadUserAsync(userId, "isCompletedDiagnosis"));
    }

    public async Task<bool> GetIsCompletedStory(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isCompletedStory"));
    }

    public async Task<bool> GetIsKilledWitch(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isKilledWitch"));
    }

    public async Task<bool> GetIsCompletedTutorial(string userId)
    {
        return bool.Parse(await ReadUserAsync(userId, "isCompletedTutorial"));
    }

    public async Task<string> GetMyClothes(string userId)
    {
        return await ReadUserAsync(userId, "myClothes");
    }

    public async Task<string> GetObtainedClothes(string userId)
    {
        return await ReadInventoryAsync(userId, "obtainedClothes");
    }

    public async Task<string> GetObtainedGraces(string userId)
    {
        return await ReadInventoryAsync(userId, "obtainedGraces");
    }

    public async Task<string> GetObtainedCollections(string userId)
    {
        return await ReadInventoryAsync(userId, "obtainedCollections");
    }

    public void SetNickname(string nickname)
    {
        Managers.UserMng.SetNickname(nickname);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("nickname").SetValueAsync(Managers.UserMng.nickname);
    }

    public void SetUserMessage(string message)
    {
        Managers.UserMng.SetUserMessage(message);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("message").SetValueAsync(Managers.UserMng.message);
    }

    public void SetOneOnOneGrace(string grace1, string grace2, string grace3)
    {
        Managers.UserMng.SetOneOnOneGrace(grace1, grace2, grace3);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("OneOnOneGrace").Child("1").SetValueAsync(Managers.UserMng.oneOnOneModeGrace.grace1);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("OneOnOneGrace").Child("2").SetValueAsync(Managers.UserMng.oneOnOneModeGrace.grace2);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("OneOnOneGrace").Child("3").SetValueAsync(Managers.UserMng.oneOnOneModeGrace.grace3);
    }

    public void SetStoryGrace(string grace1, string grace2, string grace3)
    {
        Managers.UserMng.SetStoryGrace(grace1, grace2, grace3);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("StoryGrace").Child("1").SetValueAsync(Managers.UserMng.storyModeGrace.grace1);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("StoryGrace").Child("2").SetValueAsync(Managers.UserMng.storyModeGrace.grace2);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("StoryGrace").Child("3").SetValueAsync(Managers.UserMng.storyModeGrace.grace3);
    }

    public void SetCoin(int coin)
    {
        Managers.UserMng.SetUserCoin(coin);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("coin").SetValueAsync(Managers.UserMng.coin);
    }

    public void SetScore(int score)
    {
        Managers.UserMng.SetUserScore(score);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("score").SetValueAsync(Managers.UserMng.score);
    }

    public void SetIsCompletedDiagnosis(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedDiagnosis(isCompleted);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("isCompletedDiagnosis").SetValueAsync(Managers.UserMng.isCompletedDiagnosis);
    }

    public async void SetIsCompletedStory(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedStory(isCompleted);
        await reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("isCompletedStory").SetValueAsync(Managers.UserMng.isCompletedStory);
    }

    public void SetIsKilledWitch(bool isKilled)
    {
        Managers.UserMng.SetUseIsKilledWitch(isKilled);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("isKilledWitch").SetValueAsync(Managers.UserMng.isKilledWitch);
    }

    public async void SetIsCompletedTutorial(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedTutorial(isCompleted);
        await reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("isCompletedTutorial").SetValueAsync(Managers.UserMng.isCompletedTutorial);
    }

    public void SetMyClothes(string myClothes)
    {
        Managers.UserMng.SetUserMyClothes(myClothes);
        reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("myClothes").SetValueAsync(Managers.UserMng.myClothes);
    }

    public async void SetObtainedClothes(string clothes)
    {
        var tmp = await GetObtainedClothes(Managers.GoogleSignIn.GetUID());
        tmp += clothes + ",";

        await reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("inventory").Child("obtainedClothes").SetValueAsync(tmp);
    }

    //public List<string> GetObtainedClothes(string userId)
    //{
    //    return StringToList(ReadUser(userId, "obtainedClothes"));
    //}

    public async void SetObtainedGraces(string graces)
    {
        var tmp = await GetObtainedGraces(Managers.GoogleSignIn.GetUID());
        tmp += graces + ",";

        await reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("inventory").Child("obtainedGraces").SetValueAsync(tmp);
    }

    public async void SetObtainedCollections(string collections)
    {
        var tmp = await GetObtainedCollections(Managers.GoogleSignIn.GetUID());
        tmp += collections + ",";
        await reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).Child("inventory").Child("obtainedCollections").SetValueAsync(tmp);
    }

}
