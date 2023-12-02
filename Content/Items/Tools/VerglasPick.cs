using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Items.Tools
{
    public class VerglasPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Verglas Pick");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.crit = 1;
            Item.knockBack = 3;
			Item.width = 24;
            Item.height = 24;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 3, 54);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
			Item.useTurn = true;

			Item.pick = 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(4))
			{
				// Emit dusts when the sword is swung 
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost);
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 80);
		}
		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
		{
			if (hurtInfo.PvP)
            {
                target.AddBuff(BuffID.Frostburn, 80);
            }
		}
	}
}
