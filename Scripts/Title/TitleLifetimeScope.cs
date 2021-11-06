using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TGJ2021.Titles
{
    public class TitleLifetimeScope : LifetimeScope
    {

        [SerializeField] private RectTransform _rankingRoot;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TitleSequencer>(Lifetime.Scoped);
            builder.RegisterComponent(_rankingRoot);
        }
    }
}
