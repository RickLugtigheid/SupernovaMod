using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class HarpoonSword : ModItem
    {
        private Asset<Texture2D> _textureShoot;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Harpoon Blade");
            // Tooltip.SetDefault("Right click to shoot a harpoon at your enemies");
        }

        private int _swordDamage = 24;
        public override void SetDefaults()
        {
            Item.damage = _swordDamage;
            Item.crit = 4;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            SetDefaultsSword();

            Item.DamageType = DamageClass.Melee;

            //_textureShoot = ModContent.Request<Texture2D>(Texture + "_Harpoon");
		}

        private void SetDefaultsSword()
        {
            Item.autoReuse = true;
            // Left Click has no projectile
            Item.shootSpeed = 0f;
            Item.shoot = ProjectileID.None;
            Item.UseSound = SoundID.Item1;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.damage = _swordDamage;
        }
        private void SetDefaultsHarpoon()
        {
            Item.autoReuse = false;
            Item.shoot = ProjectileID.Harpoon;
            Item.damage = 28;
            Item.shootSpeed = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;

            Item.UseSound = SoundID.Item10;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                SetDefaultsHarpoon();
            }
            else
            {
                SetDefaultsSword();
            }
            base.UseStyle(player, heldItemFrame);
        }

		/*public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
            if (Item.useStyle == ItemUseStyleID.Shoot)
            {
                spriteBatch.Draw(_textureShoot.Value, Item.position, lightColor);
                return false;
            }
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}*/

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 7);
            //recipe.anyIronBar = true;
            recipe.acceptedGroups.AddRange(new[] { RecipeGroupID.IronBar });
            recipe.AddIngredient(ItemID.Harpoon);
            recipe.AddIngredient(ItemID.SharkFin);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
