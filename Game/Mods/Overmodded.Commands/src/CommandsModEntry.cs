//
// Overmodded (SANDBOX) Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Mods.API;

namespace Overmodded.Commands
{
    public class CommandsModEntry : ModEntry
    {
        /// <inheritdoc />
        protected override void OnLoad()
        {
            RegisterBehaviour(new CommandsPlayer());
        }
    }
}
