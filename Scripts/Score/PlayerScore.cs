using System;
using NCMB;

namespace TGJ2021.Score
{
    public class PlayerScore
    {
        public string PlayerName { get; }
        public int Point { get; }
        
        public string UserId { get; }

        public DateTime Date { get; }

        public PlayerScore(string name, int point, string userId)
        {
            PlayerName = name;
            Point = point;
            UserId = userId;
            Date = DateTime.Now;
        }

        public PlayerScore(NCMBObject ncmbObject)
        {
            PlayerName = (string)ncmbObject[nameof(PlayerName)];
            var p = ncmbObject[nameof(Point)];
            Point = int.Parse(p.ToString());
            UserId = (string)ncmbObject[nameof(UserId)];
            Date = ncmbObject.CreateDate.Value.Date.ToLocalTime();
        }

        public NCMBObject ToNCMBObject()
        {
            var ncmb = new NCMBObject(nameof(PlayerScore));
            ncmb[nameof(PlayerName)] = PlayerName;
            ncmb[nameof(Point)] = Point;
            ncmb[nameof(UserId)] = UserId;
            return ncmb;
        }
    }
}