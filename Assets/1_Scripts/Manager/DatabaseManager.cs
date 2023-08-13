using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Firebase;
using Firebase.Database;
using Firebase.Unity;

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
        //WriteNewUser(Managers.GoogleSignIn.GetUID(), 0, 0, false, false, false, nickname);
        WriteNewUser(nickname, 0, 0, false, false, false);
    }

    private void WriteNewUser(string userId, int coin, int score, bool isCompletedStory, bool isCompletedTutorial, bool isCompletedDiagnosis/*, string nickname*/)
    {
        Managers.UserMng.InitUser(userId, coin, score, null, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis/*, nickname*/, null, null, "안녕하세요!");

        string json = JsonUtility.ToJson(Managers.UserMng.user);
        Debug.Log("WriteNewUser");
        Debug.Log(json);

        reference.Child("Users").Child(userId).SetRawJsonValueAsync(json);
    }

    public string ReadData(string userId, string key)
    {
        return ReadUser(userId, key);
    }

    private string ReadUser(string userId, string key)
    {
        //reference의 자식(userEmail)를 task로 받음
        reference.Child("Users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("error");
                return "error";
            }
            //task가 성공적이면
            else if (task.IsCompleted)
            {
                // DataSnapshot 변수를 선언하여 task의 결과 값을 반환
                DataSnapshot snapshot = task.Result;
                // snapshot의 자식 개수를 확인
                Debug.Log(snapshot.ChildrenCount);

                //foreach문으로 각각 데이터를 IDictionary로 변환해 각 이름에 맞게 변수 초기화
                foreach (DataSnapshot data in snapshot.Children)
                {
                    if(data.Key == key)
                        Debug.Log(data.Value);
                    return data.Value.ToString();
                }
            }
            return "error";
        });
        return "error";
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
    public int GetCoin(string userId)
    {
        return int.Parse(ReadUser(userId, "coin"));
    }

    public void SetScore(int score)
    {
        Managers.UserMng.SetUserScore(score);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("score").SetValueAsync(Managers.UserMng.user.score);
    }

    public void SetIsCompletedDiagnosis(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedDiagnosis(isCompleted);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isCompletedDiagnosis").SetValueAsync(Managers.UserMng.user.isCompletedDiagnosis);
    }
    public bool GetIsCompletedDiagnosis(string userId)
    {
        return bool.Parse(ReadUser(userId, "isCompletedDiagnosis"));
    }

    public void SetIsCompletedStory(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedStory(isCompleted);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isCompletedStory").SetValueAsync(Managers.UserMng.user.isCompletedStory);
    }
    public bool GetIsCompletedStory(string userId)
    {
        return bool.Parse(ReadUser(userId, "isCompletedStory"));
    }

    public void SetIsCompletedTutorial(bool isCompleted)
    {
        Managers.UserMng.SetUserIsCompletedTutorial(isCompleted);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("isCompletedTutorial").SetValueAsync(Managers.UserMng.user.isCompletedTutorial);
    }
    public bool GetIsCompletedTutorial(string userId)
    {
        return bool.Parse(ReadUser(userId, "isCompletedTutorial"));
    }

    public void SetObtainedClothes(string clothes)
    {
        Managers.UserMng.SetUserObtainedClothes(clothes);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("Inventory").Child("ObtainedClothes").SetValueAsync(Managers.UserMng.user.inventory.obtainedClothes);
    }
    //public List<string> GetObtainedClothes(string userId)
    //{
    //    return StringToList(ReadUser(userId, "obtainedClothes"));
    //}

    public void SetObtainedGraces(string graces)
    {
        Managers.UserMng.SetUserObtainedGraces(graces);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("Inventory").Child("ObtainedGraces").SetValueAsync(Managers.UserMng.user.inventory.obtainedGraces);
    }

    public void SetObtainedCollections(string collections)
    {
        Managers.UserMng.SetUserObtainedCollections(collections);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("Inventory").Child("ObtainedCollections").SetValueAsync(Managers.UserMng.user.inventory.obtainedCollections);
    }

}
