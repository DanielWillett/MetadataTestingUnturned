using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using UnityEngine;

namespace MetadataTesting
{
    public class Main : RocketPlugin
    {
        protected override void Load()
        {
            Rocket.Core.Logging.Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Green); //SELECT vars FROM playerstats WHERE Username LIKE %blaz%;
        }
        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log($"{Name} has been unloaded!", ConsoleColor.DarkGreen);
        }
    }
}