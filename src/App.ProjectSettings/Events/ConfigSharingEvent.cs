using System;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.Events;

public class ConfigSharingEvent : PubSubEvent<Models.Interfaces.IProjectConfigLoader>
{

}
