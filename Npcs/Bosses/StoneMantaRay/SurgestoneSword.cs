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
        }
        int i;
		public override void SetDefaults()
		{
			item.damage = 23;
			item.melee = true;
            item.crit = 8;
            item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 4;
			item.UseSound = SoundID.Item1;
            item.useTurn = true;
            item.autoReuse = true;
            item.scale *= 1.16f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            i++;
            if(i > 4)
            {
                Vector2 perturbedSpeed = new Vector2(0, 0) * .4f;
                Projectile.NewProjectile(player.position.X, player.position.Y, perturbedSpeed.X, perturbedSpeed.Y, 656, 18, 4f, player.whoAmI);
                i = 0;
            }
        }
    }
}
