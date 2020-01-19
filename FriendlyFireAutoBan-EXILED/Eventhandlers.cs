using EXILED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyFireAutoBan_EXILED
{
    public class EventHandlers
    {
        public FFAB plugin;
        public int damageThreshold;
        public int banMins;

        public Dictionary<ReferenceHub, int> playerTable = new Dictionary<ReferenceHub, int>();

        public EventHandlers(FFAB plugin, int damage, int ban) 
        {
            this.plugin = plugin;
            damageThreshold = damage;
            banMins = ban;
        }

        public void OnPlayerHurt(ref PlayerHurtEvent ev)
        {
            if (!playerTable.ContainsKey(ev.Attacker) || !playerTable.ContainsKey(ev.Player))
            {
                Plugin.Error("Player hurt event failed, maybe they just joined?");
                return;
            }

            if (ev.Attacker == null || ev.Player == null)
                return;
            if (!ev.Player.characterClassManager.isClient || !ev.Attacker.characterClassManager.isClient)
                return;

            Team attackerT = Plugin.GetTeam(ev.Attacker.characterClassManager.CurClass);
            Team attackedT = Plugin.GetTeam(ev.Player.characterClassManager.CurClass);

            if (attackedT == attackerT)
            {
                if (playerTable[ev.Attacker] >= damageThreshold)
                    GameCore.Console.singleton.TypeCommand($"/ban " + ev.Attacker.queryProcessor.PlayerId + " " + banMins, new FFABSender());
                else
                    playerTable[ev.Attacker] += (int) ev.Info.Amount;
            }
        }

        internal void WaitingForPlayers() => playerTable.Clear();

        internal void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (ev.Player.characterClassManager.isClient)
                playerTable.Add(ev.Player, 0);
        }

        internal void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            if (ev.Player.characterClassManager.isClient)
                playerTable.Remove(ev.Player);
        }
    }
}
