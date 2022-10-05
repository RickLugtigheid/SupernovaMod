using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Content.PreHardmode.Bosses.StoneMantaRay
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
			Item.damage = 23;
            Item.crit = 4;
            Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale *= 1.12f;

            Item.DamageType = DamageClass.Melee;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            i++;
            if(i > 6)
            {
                Vector2 perturbedSpeed = new Vector2(0, 0) * .4f;
                Projectile.NewProjectile(Item.GetSource_FromAI(), target.position.X, target.position.Y, perturbedSpeed.X, perturbedSpeed.Y, 656, Item.damage / 2, 4f, player.whoAmI);
                i = 0;
            }
        }
    }
}
