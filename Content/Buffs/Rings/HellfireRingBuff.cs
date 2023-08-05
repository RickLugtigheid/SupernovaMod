using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Rings
{
    public class HellfireRingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;

            // DisplayName.SetDefault("Ring of Hellfire");
            // Description.SetDefault("Attacks now have a chance to cause an fiery explosion.");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
        }
    }
}
