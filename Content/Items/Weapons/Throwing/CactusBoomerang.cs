using System;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class CactusBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Cactus Boomerang");
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.crit = 4;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 1, 46, 82);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 7;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.CactusBoomerangProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;

            Item.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
        }
        public override bool CanUseItem(Player player) //this make that you can shoot only 1 boomerang at once
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Cactus, 27);
            recipe.AddIngredient(ItemID.AntlionMandible, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}