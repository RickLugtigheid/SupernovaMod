using Terraria;
using Terraria.ModLoader;

namespace Supernova.Buffs
{
    public class QuarionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.SetDefault("Quarion Potion");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage += 0.72f;
            player.magicDamage += 0.72f;
            player.rangedDamage += 0.72f;
            player.thrownDamage += 0.72f;
            player.statLifeMax2 /= 2;
            player.moveSpeed += 10.35f;
            player.moveSpeed *= 2f;
            player.statDefense /= 2;
        }
    }
}