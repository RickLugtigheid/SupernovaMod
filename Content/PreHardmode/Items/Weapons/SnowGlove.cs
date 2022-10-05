using Supernova.Api.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
	public class SnowGlove : ModShotgun
    {
		public override float SpreadAngle => 4;
		public override int MinShots => 3;
		public override int MaxShots => 3;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Glove");
        }

        public override void SetDefaults()
        {
            
            //item.maxStack = 1; // Makes it so the weapon stacks.
            Item.damage = 8;
            Item.crit = 3;
            Item.knockBack = 2f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 23;
            Item.useTime = 23;
            Item.width = 32;
            Item.height = 32;
            //item.consumable = true; // Makes it so one is taken from stack after use.
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 4, 46, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.SnowBallFriendly;
            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SnowBlock, 70);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
