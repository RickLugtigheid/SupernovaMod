using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Bosses.StoneMantaRay
{
    public class SurgestoneSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Surgestone Sword");
            Tooltip.SetDefault("Summons a tornado at the target after a few strikes");
        }
        int i;
		public override void SetDefaults()
		{
			item.damage = 23;
			item.melee = true;
            item.crit = 4;
            item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = Rarity.Orange;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.scale *= 1.12f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            i++;
            if(i > 6)
            {
                Vector2 perturbedSpeed = new Vector2(0, 0) * .4f;
                Projectile.NewProjectile(target.position.X, target.position.Y, perturbedSpeed.X, perturbedSpeed.Y, 656, item.damage / 2, 4f, player.whoAmI);
                i = 0;
            }
        }
    }
}
