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

    public void readData(string userID, string key)
    {
        readUser(userID, key);
    }

    private void readUser(string userID, string key)
    {
        reference.Child(userID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("error");
            }
    
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                
                Debug.Log(snapshot.ChildrenCount);

                foreach (DataSnapshot data in snapshot.Children)
                {
                    if (data.ToString() == key)
                        Debug.Log(data.Value);
                }
            }
        });
    }
}
