using SupernovaMod.Api;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class SealOfSamael : ModItem
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
            Item.value = BuyPrice.RarityLightRed;
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
			Item.damage = 66;
            Item.knockBack = 4;
			Item.crit = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
			// Summon a Red Devil minion
			//
            if (player.whoAmI == Main.myPlayer)
            {
				player.AddBuff(ModContent.BuffType<Buffs.Summon.RedDevilBuff>(), 1);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.RedDevil>()] < 1)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Summon.RedDevil>(), Item.damage, 7, player.whoAmI, Item.whoAmI);
				}
			}
		}
	}
}
