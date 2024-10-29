using SupernovaMod.Content.Items.Materials;
using SupernovaMod.Content.Projectiles.Magic;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    // TODO: This was accidentally removed but retrieved through source code,
    // so please change the code back to the original state...
    public class StarcoreStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[((ModItem)this).Item.type] = true;
        }

        public override void SetDefaults()
        {
            //IL_00ad: Unknown result type (might be due to invalid IL or missing references)
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[((ModItem)this).Type] = 1;
            ((ModItem)this).Item.damage = 52;
            ((ModItem)this).Item.crit = 1;
            ((Entity)((ModItem)this).Item).width = 28;
            ((Entity)((ModItem)this).Item).height = 34;
            ((ModItem)this).Item.useTime = 34;
            ((ModItem)this).Item.useAnimation = 34;
            ((ModItem)this).Item.useStyle = 5;
            ((ModItem)this).Item.noMelee = true;
            ((ModItem)this).Item.knockBack = 5f;
            ((ModItem)this).Item.value = 1000;
            ((ModItem)this).Item.rare = 5;
            ((ModItem)this).Item.UseSound = SoundID.Item21;
            ((ModItem)this).Item.autoReuse = true;
            ((ModItem)this).Item.shoot = ModContent.ProjectileType<StarcoreBolt>();
            ((ModItem)this).Item.mana = 12;
            ((ModItem)this).Item.shootSpeed = 14f;
            ((ModItem)this).Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe obj = ((ModItem)this).CreateRecipe(1);
            obj.AddIngredient<Starcore>(3);
            obj.AddIngredient(ItemID.HallowedBar, 12);
            obj.AddTile(134);
            obj.Register();
        }
    }

}
