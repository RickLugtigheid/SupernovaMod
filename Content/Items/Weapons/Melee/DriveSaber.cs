using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;
using SupernovaMod.Common;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
	public class DriveSaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			//Tooltip.SetDefault("Fires a short-range bolt of energy\nInflicts Electrified and deals 10% more damage to Electrified enemies");
		}
		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.crit = 3;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = BuyPrice.RarityLightRed;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileID.MagnetSphereBolt;
			Item.shootSpeed = 5;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.MechroDrive>(), 3);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ModContent.ItemType<Materials.BrokenSwordShards>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(6))
			{
				// Emit dusts when the sword is swung  
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Electric, Scale: .6f);
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage = (int)(damage * .85f);
		}

		public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			// 40% chance to inflict Electrified 
			// 
			if (Main.rand.NextChance(.4f))
			{
				target.AddBuff(BuffID.Electrified, Main.rand.Next(1, 3) * 60);
			}
		}
	}
}
