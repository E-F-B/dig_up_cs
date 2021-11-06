using TGJ2021.Ranking;
using TGJ2021.Score;
using TGJ2021.TGJ2021.Music;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TGJ2021
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private FadeManager _fadeManager;
        [SerializeField] private BGMPlayer _bgmPlayer;
        [SerializeField] private RankingBoardView _rankingBoardViewPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_bgmPlayer);
            builder.RegisterComponent(_fadeManager);
            builder.RegisterEntryPoint<RootInitializer>(Lifetime.Scoped);
            builder.Register<LocalStorage>(Lifetime.Singleton);
            builder.Register<ScoreManager>(Lifetime.Singleton);
            builder.Register<RankingBoard>(Lifetime.Singleton)
                .WithParameter(_rankingBoardViewPrefab);
        }
    }
}
