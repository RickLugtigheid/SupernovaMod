using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Summon
{
    public class VerglasScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Scepter");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            Item.damage = 20;
            Item.crit = 12;
            Item.width = 28;
            Item.height = 34;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 5;        //this is how the item is holded
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = 1000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.VerglasScepterProj>();
            Item.mana = 8;             //mana use
            Item.shootSpeed = 16f;    //projectile speed when shoot
            Item.DamageType = DamageClass.Magic;
        }
        float muli = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Flip our multi
            muli = -muli;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, muli);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}