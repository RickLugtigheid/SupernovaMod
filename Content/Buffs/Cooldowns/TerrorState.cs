using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Cooldowns
{
    public class TerrorState : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }
}