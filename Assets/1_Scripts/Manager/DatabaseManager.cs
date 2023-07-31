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

    /*public bool CheckExistingUser(string userId, GameObject LogTMP)
    {
        reference.Child("Users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                LogTMP.GetComponent<TextMeshProUGUI>().text += "\n Load Faulted";
                return false;
            }
            //task가 성공적이면
            else if (task.IsCompleted)
            {
                LogTMP.GetComponent<TextMeshProUGUI>().text += "\n Load Completed";
                DataSnapshot snapshot = task.Result;

                //foreach문으로 각각 데이터를 IDictionary로 변환해 각 이름에 맞게 변수 초기화
                foreach (DataSnapshot data in snapshot.Children)
                {
                    
                    if(data.Key == "userId")
                    {
                        LogTMP.GetComponent<TextMeshProUGUI>().text += "\nGetUserId";
                        LogTMP.GetComponent<TextMeshProUGUI>().text += "\npersonInfo[userId] = " + data.Value;
                        LogTMP.GetComponent<TextMeshProUGUI>().text += "\nuserId = " + userId;
                        return true;
                    }
                }
                return true;
            }
            return false;
        });
        return false;
    }*/

    public void CreateNewUser(string nickname)
    {
        WriteNewUser(Managers.GoogleSignIn.GetUID(), 0, 0, false, false, false, nickname);
    }

    private void WriteNewUser(string userId, int coin, int score, bool isCompletedStory, bool isCompletedTutorial, bool isCompletedDiagnosis, string nickname)
    {
        Managers.UserMng.InitUser(userId, coin, score, isCompletedStory, isCompletedTutorial, isCompletedDiagnosis, nickname);

        string json = JsonUtility.ToJson(Managers.UserMng.user);

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

    public void AddCoin(int coin)
    {
        Managers.UserMng.AddUserCoin(coin);
        reference.Child("Users").Child(Managers.UserMng.user.UID).Child("Coin").SetValueAsync(Managers.UserMng.user.coin);
    }
}
