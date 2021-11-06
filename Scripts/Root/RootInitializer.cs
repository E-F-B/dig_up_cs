using System.Threading;
using Cysharp.Threading.Tasks;
using Rewired;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace TGJ2021
{
    public class RootInitializer : IInitializable, IAsyncStartable
    {
        private readonly FadeManager _fadeManager;
        private readonly LocalStorage _localStorage;

        public RootInitializer(FadeManager fadeManager, LocalStorage storage)
        {
            _fadeManager = fadeManager;
            _localStorage = storage;
        }
        
        public void Initialize()
        {
            if (!_localStorage.HasAccount)
            {
                _localStorage.CreateUserId();
            }
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => ReInput.isReady, cancellationToken: cancellation);
            await SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Title"));
        }
    }
}