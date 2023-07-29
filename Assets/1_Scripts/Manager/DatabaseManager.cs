using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class DatabaseManager : MonoBehaviour
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
        public string userID;
        public int gold;
        public int score;
        //public Inventory inventory;
        public bool IsCompleteStory;
        public bool IsCompleteTutorial;
        public string nickname;
        //public PVPModeGrace pvpModeGrace;
        //public StoryModeGrace storyModeGrace;

        public User(string userID, int gold, int score, bool IsCompleteStory, bool IsCompleteTutorial, string nickname)
        {
            this.userID = userID;
            this.gold = gold;
            this.score = score;
            //this.inventory = inventory;
            this.IsCompleteStory = IsCompleteStory;
            this.IsCompleteTutorial= IsCompleteTutorial;
            this.nickname = nickname;
            //this.pvpModeGrace= pvpModeGrace;
            //this.storyModeGrace= storyModeGrace;
        }
    }

    DatabaseReference reference;

    public void Init()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateNewUser(string nickname)
    {
        writeNewUser(Managers.GoogleSignIn.GetUID(), 0, 0, false, false, nickname);
    }

    private void writeNewUser(string userID, int gold, int score, bool IsCompleteStory, bool IsCompleteTutorial,string nickname)
    {
        User user = new User(userID, gold, score, IsCompleteStory, IsCompleteTutorial, nickname);

        string json = JsonUtility.ToJson(user);

        reference.Child("Users").Child(userID).SetRawJsonValueAsync(json);
    }

    public string readData(string userID, string key)
    {
        return readUser(userID, key);
    }

    private string readUser(string userId, string key)
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
                    return data.Value;
                }
            }
            return "error";
        });
        return "error";
    }
}
