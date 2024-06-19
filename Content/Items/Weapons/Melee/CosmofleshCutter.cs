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
			Item.damage = 56;
			Item.crit = 2;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4;
			Item.value = BuyPrice.RarityGreen;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.DamageType = DamageClass.Melee;
			Item.scale = 1.2f;
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
	}
}
