using MessagePipe;
using TGJ2021.InGame.Messages;
using TGJ2021.InGame.Players;
using TGJ2021.InGame.Result;
using TGJ2021.InGame.Rocks;
using TGJ2021.InGame.Scores;
using TGJ2021.InGame.SpellCards;
using TGJ2021.InGame.Timers;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VContainer;
using VContainer.Unity;

namespace TGJ2021.InGame
{
    public class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private BulletFactory _bulletFactory;

        [SerializeField] private PlayerBehaviour _playerBehaviour;

        [SerializeField] private Transform _rockParent;

        [SerializeField] private Transform _rockSpawnPositions;

        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private HiScoreView _hiScoreView;

        [SerializeField] private GameTimerView _gameTimerView;

        [SerializeField] private InGameSettings _gameSettings;

        [SerializeField] private GameEndView _gameEndView;

        [SerializeField] private RockSettings _rockSettings;

        [SerializeField] private ScoreItemView _scoreItemView;

        [SerializeField] private BlastBulletView _blastBulletView;

        [SerializeField] private MomoyoView _momoyoView;

        [SerializeField] private ResultBoardView _resultBoardView;

        [SerializeField] private RectTransform _rankingRoot;

        [SerializeField] private RetryPopupView _retryPopupView;

        [SerializeField] private SpellCardUI _spellCardUI;

        [SerializeField] private BulletDangerView _bulletDangerView;

        [SerializeField] private PostProcessVolume _postProcessVolume;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterMessages(builder);

            builder.RegisterComponent(_bulletFactory).As<IBulletFactory>();
            builder.RegisterEntryPoint<BulletUpdater>(Lifetime.Scoped);

            builder.RegisterEntryPoint<GameSequencer>()
                .WithParameter(_retryPopupView);

            builder.Register<RockFactory>(Lifetime.Scoped)
                .WithParameter(_rockSettings)
                .WithParameter(_rockParent);

            builder.Register<RockSpawner>(Lifetime.Scoped)
                .WithParameter(_rockSpawnPositions);

            builder.Register<ScoreObserver>(Lifetime.Scoped)
                .WithParameter(_scoreView)
                .WithParameter(_hiScoreView)
                .AsImplementedInterfaces();

            builder.Register<GameTimer>(Lifetime.Scoped)
                .WithParameter(_gameTimerView)
                .AsImplementedInterfaces();

            builder.RegisterInstance(_gameSettings);

            builder.RegisterEntryPoint<Player>(Lifetime.Scoped).AsSelf();
            builder.RegisterComponent(_playerBehaviour);

            builder.RegisterComponent(_gameEndView);

            builder.RegisterFactory<Vector3, ScoreItemView>(resolver =>
            {
                return (spawnPosition =>
                {
                    var item = Instantiate(_scoreItemView, spawnPosition, Quaternion.identity);
                    return item;
                });
            }, Lifetime.Scoped);

            builder.Register<ScoreItemSpawner>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MineBlast>(Lifetime.Scoped)
                .WithParameter(_blastBulletView);

            builder.RegisterEntryPoint<SpellCardObserver>(Lifetime.Scoped)
                .WithParameter(_spellCardUI);

            builder.RegisterComponent(_momoyoView);

            builder.Register<ResultBoardScore>(Lifetime.Scoped)
                .WithParameter(_resultBoardView);

            builder.RegisterComponent(_rankingRoot);

            builder.Register<BulletCounter>(Lifetime.Scoped);
            builder.Register<BreakRockCounter>(Lifetime.Scoped);
            builder.RegisterEntryPoint<BulletDangerObserver>(Lifetime.Scoped)
                .WithParameter(_bulletDangerView)
                .WithParameter(_postProcessVolume);
        }

        private void RegisterMessages(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<BulletSpawnMessage>(options);
            builder.RegisterMessageBroker<BulletDeSpawnMessage>(options);
            builder.RegisterMessageBroker<GameStartMessage>(options);
            builder.RegisterMessageBroker<GameEndMessage>(options);
            builder.RegisterMessageBroker<TimeUpMessage>(options);

            builder.RegisterMessageBroker<PlayerDeadMessage>(options);
            builder.RegisterMessageBroker<PlayerReSpawnMessage>(options);

            builder.RegisterMessageBroker<RockBreakMessage>(options);
            builder.RegisterMessageBroker<AddScoreMessage>(options);

            builder.RegisterMessageBroker<SpellCardMessage>(options);
            builder.RegisterMessageBroker<ScoreCalculateMessage>(options);

            builder.RegisterMessageBroker<UshirodoHitMessage>(options);
        }
    }
}