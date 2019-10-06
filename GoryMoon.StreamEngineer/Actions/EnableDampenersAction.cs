﻿using GoryMoon.StreamEngineer.Data;
using Sandbox.Game.Entities;

namespace GoryMoon.StreamEngineer.Actions
{
    public class EnableDampenersAction: BaseAction
    {
        public override void Execute(Data.Data data)
        {
            SessionHandler.EnqueueAction(() =>
            {
                var player = Utils.GetPlayer();
                if (player != null)
                {
                    var controlledEntity = player.Controller.ControlledEntity;

                    if (!controlledEntity.EnabledDamping)
                    {
                        controlledEntity.SwitchDamping();
                    }
                }
            });
        }
    }
    
    
}