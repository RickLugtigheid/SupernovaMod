using SupernovaMod.Api;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class HarbingersCrest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = -12;
            Item.expert = true;
            Item.value = BuyPrice.RarityBlue;
            Item.accessory = true;
			Item.damage = 21;
			Item.crit = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
			// Summon the Harbingers Arm
			//
            if (player.whoAmI == Main.myPlayer)
            {
				player.AddBuff(ModContent.BuffType<Buffs.Summon.HarbingersArmBuff>(), 1);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>()] < 1)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>(), Item.damage, 7, player.whoAmI, Item.whoAmI);
				}
			}
		}
		/*public override void UpdateVanity(Player player)
		{
			// Summon the Harbingers Arm
			//
			if (player.whoAmI == Main.myPlayer)
			{
				player.AddBuff(ModContent.BuffType<Buffs.Summon.HarbingersArmBuff>(), 1);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>()] < 1)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>(), 17, 4, player.whoAmI, Item.whoAmI, 1);
				}
			}
		}*/
	}
}
