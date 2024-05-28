using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using DynamicData.Binding;
using Prism.Events;
using Prism.Regions;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;

namespace App.ViewModels
{
    internal class AppViewModel : ReactiveObject
    {
        private readonly IRegionManager _regionManager;

        public AppViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            var eventAggregator1 = eventAggregator;
            _regionManager = regionManager;
        }

    }
}
