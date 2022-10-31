using System;
using Microsoft.Xna.Framework;
using Supernova.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
	public class BlunderBuss : ModShotgun
    {
		public override float SpreadAngle => 8;
		public override int MinShots => 3;
		public override int MaxShots => 5;

		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Blunder Buss");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-13, 0);
        public override void SetDefaults()
        {
            Item.expertOnly = true;
            Item.damage = 14;
            Item.width = 40;
            Item.crit = 7;
            Item.height = 20;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.4f;
            Item.value = Item.buyPrice(0, 9, 21, 0);
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item38;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 8.7f;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;

            Item.expert = true;
            Item.scale = .85f;
        }
    }
}