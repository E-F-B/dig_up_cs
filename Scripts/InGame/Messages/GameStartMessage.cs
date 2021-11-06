using System.Threading;

namespace TGJ2021.InGame.Messages
{
    public class GameStartMessage
    {
        /// <summary>
        /// ゲーム実行中のトークン
        /// </summary>
        public CancellationToken InGameLoopToken;
    }
}