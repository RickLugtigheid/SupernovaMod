using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Rings
{
    public class GemRing : RingBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gem Ring");
            Tooltip.SetDefault("When you click the Ring Ability button" +
                "\n the ring will shoot all the gem staf projectiles\n" +
                "\n put in slot 0 to make it work");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Green;
            item.value = Item.buyPrice(0, 5, 0, 0);
        }
		public override int cooldown => 1350;
		public override void OnRingActivate(Player player)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 position = player.Center;
				Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
				int ShootDamage = 17;
				int Type = 121;
				float ShootKnockback = 3.6f;

				Vector2 diff = Main.MouseWorld - player.Center;

				diff.Normalize();

				Projectile.NewProjectile(position.X, position.Y, diff.X * 10, diff.Y * 10, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(position.X, position.Y, diff.X * 9, diff.Y * 9, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type1 = 122;
				Projectile.NewProjectile(position.X, position.Y, diff.X * 8, diff.Y * 8, Type1, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type2 = 123;
				Projectile.NewProjectile(position.X, position.Y, diff.X * 7, diff.Y * 7, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(position.X, position.Y, diff.X * 6, diff.Y * 6, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type3 = 124;
				Projectile.NewProjectile(position.X, position.Y, diff.X * 5, diff.Y * 5, Type3, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type4 = 125;
				Projectile.NewProjectile(position.X, position.Y, diff.X * 4, diff.Y * 4, Type4, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type5 = 126;
				Projectile.NewProjectile(position.X, position.Y, diff.X * 3, diff.Y * 3, Type5, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
			}
		}
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GoldenRingMold"));

            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}