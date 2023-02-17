using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Npcs.Bosses.FlyingTerror
{
    public class TerrorKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Terror Knife");
        }

        public override void SetDefaults()
        {
            //item.maxStack = 1; // Makes it so the weapon stacks.
            Item.damage = 14;
            Item.crit = 5;
            Item.knockBack = 0.5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 7, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<TerrorKniveProj>();
            Item.DamageType = DamageClass.Throwing;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
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
