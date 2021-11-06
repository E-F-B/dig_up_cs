using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NCMB;
using NCMB.Tasks;

namespace TGJ2021.Score
{
    public class ScoreManager
    {
        public static int RankLimit = 100;
        
        public async UniTask SaveScore(PlayerScore score)
        {
            var scoreObject = score.ToNCMBObject();
            await scoreObject.SaveTaskAsync();
        }

        public async UniTask<List<PlayerScore>> LoadScore(int max)
        {
            var query = new NCMBQuery<NCMBObject>(nameof(PlayerScore))
                .OrderByDescending("Point");

            query.Limit = max;
            var result = await query.FindTaskAsync();
            return result.Select(ncmb => new PlayerScore(ncmb)).ToList();
        }
    }
}