using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Buffs
{
    public class GallantCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;

            DisplayName.SetDefault("Gallant Cooldown");
            Description.SetDefault("Waiting for your 'Gallant' to cooldown");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
        }
    }
}