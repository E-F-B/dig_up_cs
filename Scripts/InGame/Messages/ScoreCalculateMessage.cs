namespace TGJ2021.InGame.Messages
{
    public class ScoreCalculateMessage
    {
        public int currentScore;
        public int hiScore;

        public bool IsNewRecord => currentScore > hiScore;
    }
}