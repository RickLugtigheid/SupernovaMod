using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Buffs.Potions
{
    public class QuarionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.SetDefault("Quarion Potion");
            Description.SetDefault("Greatly increased damage and speed but halved health");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += 0.52f;
            player.GetDamage(DamageClass.Magic) += 0.52f;
            player.GetDamage(DamageClass.Ranged) += 0.52f;
            player.GetDamage(DamageClass.Throwing) += 0.52f;
            player.statLifeMax2 /= 2;
            player.moveSpeed += 2;
            //player.statDefense /= 2;
        }
    }
}