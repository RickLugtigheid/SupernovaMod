using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class CosmofleshCutter : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
			Item.damage = 83;
			Item.crit = 1;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4.5f;
			Item.value = BuyPrice.RarityLightRed;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.DamageType = DamageClass.Melee;
			Item.scale = 1.25f;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                // Emit dusts when the sword is swung 
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.CorruptionThorns);
            }
        }
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			// Calculate the position our target will be at after knockback
			Vector2 targetPositon = target.Center;
			targetPositon.X += Item.knockBack * hit.HitDirection;

			// Get the ground position the target is on or above
			//
			Vector2? groundPosition = SupernovaUtils.GetGroundTileFromPostion(targetPositon);
			if (!groundPosition.HasValue)
			{
				return;
			}
			//
			Vector2 velocity = -Vector2.UnitY * Main.rand.Next(5, 9);
			velocity = velocity.RotatedByRandom(.32f);
			// Spawn our projectile and make melee projectile
			//
			Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_OnHit(target), groundPosition.Value, velocity, ModContent.ProjectileType<Projectiles.Magic.EldrichTentacle>(), Item.damage, Item.knockBack);
			proj.DamageType = DamageClass.MeleeNoSpeed;
		}

		public override void AddRecipes()
		{
			// Corruption recipe
			//
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 8);
			recipe.AddIngredient<Materials.EldritchEssence>(20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			// Crimson recipe
			//
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 8);
			recipe.AddIngredient<Materials.EldritchEssence>(20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
