using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class VerglasScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Verglas Scepter");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            Item.damage = 22;
            Item.crit = 2;
            Item.width = 28;
            Item.height = 34;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = 1000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.VerglasScepterProj>();
            Item.mana = 3;
            Item.shootSpeed = 14f;    //projectile speed when shoot
            Item.DamageType = DamageClass.Magic;
        }
        float muli = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Flip our multiplier
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