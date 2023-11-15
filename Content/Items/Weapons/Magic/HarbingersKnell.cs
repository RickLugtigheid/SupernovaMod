using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class HarbingersKnell : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
        {
            Item.damage = 20;
            Item.mana = 7;
            Item.width = 20;
            Item.height = 20;

            Item.useTime = 48;
            Item.useAnimation = 48;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = BuyPrice.RarityGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item101; // Item78
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.CelestialCystal>();
            Item.shootSpeed = 8;

            Item.DamageType = DamageClass.Magic;
        }
    }
}
