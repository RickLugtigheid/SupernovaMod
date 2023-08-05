using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Cooldowns
{
    public class ReloadDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;

            // DisplayName.SetDefault("Reloading");
            // Description.SetDefault("Waiting for the weapon to be reload.");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}