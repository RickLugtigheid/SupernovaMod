using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class CarnageRifle : SupernovaGunItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Carnage Rifle");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -1.5f);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 41;
            Item.width = 58;
            Item.crit = 4;
            Item.height = 20;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.knockBack = 6.4f;
            Item.autoReuse = false;
            Item.value = BuyPrice.RarityOrange;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item36;
            Item.shootSpeed = 14;

            Gun.recoil = .8f;
            Gun.useStyle = GunUseStyle.PumpAction;
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