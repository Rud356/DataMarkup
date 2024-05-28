using Prism.Ioc;
using Prism.Modularity;
using App.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.MarkupProject.Views;

//namespace App.MarkupProject
//{
//    internal class MarkupModule
//    {
//    }
//}
namespace SamplePrism2024.About;

public class MarkupModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<MarkupWindow>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {

    }
}