using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Buffs
{
    public class RingCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;

            DisplayName.SetDefault("Ring Cooldown");
            Description.SetDefault("Waiting for the power of your ring to come back");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = true;
        }
    }
}