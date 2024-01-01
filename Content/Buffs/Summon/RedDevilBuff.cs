using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Summon
{
    public class RedDevilBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}