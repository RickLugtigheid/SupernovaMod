using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons.Melee
{
	public class VerglasSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			DisplayName.SetDefault("Verglas Splitter");
        }
		public override void SetDefaults()
		{
			Item.damage = 30;
            Item.crit = 3;
            Item.width = 40;
			Item.height = 40;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 10;
            Item.value = Item.buyPrice(0, 9, 47, 0); // Another way to handle value of item.
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useAnimation = 34;
			Item.useTime = 34;
            Item.DamageType = DamageClass.Ranged;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// There is a 30% chance that the sword will shoot a frost bolt
			//
			if (Main.rand.NextFloat() <= .30)
			{
				type = ProjectileID.FrostBoltSword;
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
				return false;
			}

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                //Emit dusts when the sword is swung 
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost, Scale: 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
            target.AddBuff(BuffID.Frostburn, 80);
			base.OnHitNPC(player, target, damage, knockBack, crit);
		}
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 8);
            recipe.AddIngredient(ItemID.IceBlade);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
