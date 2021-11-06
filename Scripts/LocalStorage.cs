using System;
using UnityEngine;

namespace TGJ2021
{
    public class LocalStorage
    {
        public const string UserIdKey = "user_id";
        public const string HiScoreKey = "hiscore";
        public const string UserNameKey = "user_name";

        public bool HasAccount => !String.IsNullOrEmpty(LoadUserId());
        
        public string LoadUserId() => PlayerPrefs.GetString(UserIdKey, String.Empty);

        public void CreateUserId() => PlayerPrefs.SetString(UserIdKey, Guid.NewGuid().ToString());

        public string LoadUserName() => PlayerPrefs.GetString(UserNameKey, "名無しの盗掘者");

        public void SaveUserName(string name) => PlayerPrefs.SetString(UserNameKey, name);

        public void SaveHiScore(int score) => PlayerPrefs.SetInt(HiScoreKey, score);
        public int LoadHiScore() => PlayerPrefs.GetInt(HiScoreKey, 100);
    }
}