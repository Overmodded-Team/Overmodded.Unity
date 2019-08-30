//
// Overmodded (SANDBOX) Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Statistics;
using Overmodded.Mods.API;
using Overmodded.Mods.API.Commands;
using Overmodded.Mods.API.Core.Gameplay;

namespace Overmodded.Commands
{
    public class CommandsPlayer : ModBehaviour
    {
        [Command("kill", ExecMode = CmdExecMode.OnlyAsServer, Description = "Kill your self...")]
        public void OnKill(ModCommandSender sender)
        {
            if (!sender.IsPlayer)
                return;

            sender.PlayerEntity.ServerKillEntity(new MDamageResult(
                new MDamageRequest(new MDamageData(sender.PlayerEntity), sender.PlayerEntity),
                new MDamageInfo(
                    (int) sender.PlayerEntity.StatisticManager.GetStatistic(StatisticName.Health).FirstValue), true));
        }

        [Command("respawn", ExecMode = CmdExecMode.OnlyAsServer, Description = "Respawn your self...")]
        public void OnRespawn(ModCommandSender sender)
        {
            if (!sender.IsPlayer)
                return;

            // instead of ServerRespawnEntity we should use RestartCharacter with resurrect set to true
            sender.PlayerEntity.RestartCharacter(true);
            //sender.PlayerEntity.ServerRespawnEntity();
        }
    }
}
