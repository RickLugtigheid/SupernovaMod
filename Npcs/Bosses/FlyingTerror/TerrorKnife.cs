using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class TerrorKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Knife");
        }

        public override void SetDefaults()
        {
            item.thrown = true; // Set this to true if the weapon is throwable.
            //item.maxStack = 1; // Makes it so the weapon stacks.
            item.damage = 14;
            item.crit = 5;
            item.knockBack = 0.5f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 13;
            item.useTime = 13;
            item.width = 30;
            item.height = 30;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.buyPrice(0, 7, 0, 0); // Another way to handle value of item.
            item.rare = ItemRarityID.Green;
            item.shootSpeed = 14f;
            item.shoot = mod.ProjectileType("TerrorKniveProj");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(16));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("TerrorWing"));
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
