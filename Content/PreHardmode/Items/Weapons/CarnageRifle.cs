using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class CarnageRifle : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Rifle");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }

        public override void SetDefaults()
        {
            Item.damage = 41;
            Item.width = 40;
            Item.crit = 4;
            Item.height = 20;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 6.4f;
            Item.autoReuse = false;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = 10; //idk why but all the guns in the vanilla source have this
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;

            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 7);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}