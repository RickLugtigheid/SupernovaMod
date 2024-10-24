using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using SupernovaMod.Api;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class MechroRailgun : SupernovaGunItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 80;
            Item.crit = 2;
			Item.knockBack = 7;
            Item.ArmorPenetration = 5;
			Item.width = 90;
			Item.height = 26;
            Item.useAnimation = 62;
            Item.useTime = 62;
            Item.autoReuse = true;
            Item.value = BuyPrice.RarityPink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item92;
			Item.shootSpeed = 14;

            Gun.recoil = .6f;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
            type = ModContent.ProjectileType<Projectiles.Ranged.RailgunBolt>();

			EffectsPlayer effectPlayer = player.GetModPlayer<EffectsPlayer>();
			if (effectPlayer.ScreenShakePower < 5f)
			{
				effectPlayer.ScreenShakePower = 1.5f;
			}
		}


        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.MechroDrive>(), 3);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
			recipe.AddIngredient(ModContent.ItemType<Materials.HiTechFirearmManual>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}