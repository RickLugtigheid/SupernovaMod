using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Items.Weapons.Melee
{
    public class BlazingFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Blazing Fire");
            Tooltip.SetDefault("Creates a fire aura around you.");
        }
        public override void SetDefaults()
        {
            Item.damage = 18;    //The damage stat for the Weapon.
            Item.crit = 8;
            Item.scale *= 1.56f;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.channel = true;
            Item.useStyle = 100;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(0, 5, 40, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.BlazingFireProj>();
            Item.noUseGraphic = true;

            Item.DamageType = DamageClass.Melee;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ItemID.Wood, 50);
            recipe.AddIngredient(ItemID.Obsidian, 7);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}