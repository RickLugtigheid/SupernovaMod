using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
    public class TerrorTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Tome");
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.crit = 14;
            Item.width = 24;
            Item.height = 28;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.mana = 4;             //mana use
            Item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("FirendlyTerrorProj").Type;  //this make the item shoot your projectile
            Item.shootSpeed = 15;    //projectile speed when shoot
            Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TerrorTuft>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}