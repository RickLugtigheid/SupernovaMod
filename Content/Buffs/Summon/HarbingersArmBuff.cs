using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Summon
{
    public class HarbingersArmBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Harbingers Arm");
            // Description.SetDefault("A arm ripped from the Harbinger that will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}