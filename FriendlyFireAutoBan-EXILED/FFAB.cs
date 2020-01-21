using EXILED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyFireAutoBan_EXILED
{
    public class FFAB : EXILED.Plugin
    {
        public EventHandlers EventHandlers;

        public override string getName => "FFAB";

        private string version = "2.0b";

        public override void OnDisable()
        {
            // d i s a b l e
            Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
            Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
            Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
            Events.WaitingForPlayersEvent -= EventHandlers.WaitingForPlayers;

            EventHandlers = null;

            Info("FFAB Disabled!");
        }

        public override void OnEnable()
        {
            if (!Config.GetBool("ffab_enable", true))
                return;
            // e n a b l e
            int ban = Config.GetInt("ffab_banMins", 120);
            int threshhold = Config.GetInt("ffab_damageThreshold", 450);

            EventHandlers = new EventHandlers(this, threshhold, ban);

            Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
            Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
            Events.WaitingForPlayersEvent += EventHandlers.WaitingForPlayers;
            Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;


            Info("FFAB Enabled! Current Version: " + version);
        }

        public override void OnReload()
        {

        }
    }
}
