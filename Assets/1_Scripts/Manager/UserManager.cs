using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager
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
        public string userId;
        public int coin;
        public int score;
        //public Inventory inventory;
        public bool IsCompleteStory;
        public bool IsCompleteTutorial;
        public string nickname;
        //public PVPModeGrace pvpModeGrace;
        //public StoryModeGrace storyModeGrace;

        public User(string userId, int coin, int score, bool IsCompleteStory, bool IsCompleteTutorial, string nickname)
        {
            this.userId = userId;
            this.coin = coin;
            this.score = score;
            //this.inventory = inventory;
            this.IsCompleteStory = IsCompleteStory;
            this.IsCompleteTutorial= IsCompleteTutorial;
            this.nickname = nickname;
            //this.pvpModeGrace= pvpModeGrace;
            //this.storyModeGrace= storyModeGrace;
        }
    }

    public User user {get;set;}

    public void Init()
    {
        
    }

    public void InitUser(string userId, int coin, int score, bool IsCompleteStory, bool IsCompleteTutorial, string nickname)
    {
        user = new User(userId, coin, score, IsCompleteStory, IsCompleteTutorial, nickname);
    }

    public void AddUserCoin(int coin)
    {
        user.coin += coin;
    }
}
