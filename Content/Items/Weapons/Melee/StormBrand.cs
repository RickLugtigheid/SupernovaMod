using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;
using Terraria.Audio;
using Terraria.DataStructures;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class StormBrand : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Storm Brand");
            // Tooltip.SetDefault("Some description about what this weapon does.");
        }
        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.crit = 3;
            Item.width = 52;
            Item.height = 62;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = BuyPrice.RarityOrange;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
			Item.useTurn = true;

			Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 5.5f;
            Item.shoot      = ProjectileID.ThunderStaffShot;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            SoundEngine.PlaySound(SoundID.Item92, position);
            velocity = velocity.RotatedByRandom(.12f);
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                // Emit dusts when the sword is swung 
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Electric, Scale: .4f);
            }
        }
    }
}
