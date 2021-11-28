using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class BlunderBuss : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blunder Buss");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-13, 0);
        public override void SetDefaults()
        {
            item.expertOnly = true;
            item.damage = 14;
            item.ranged = true;
            item.width = 40;
            item.crit = 7;
            item.height = 20;
            item.useTime = 42;
            item.useAnimation = 42;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 5.4f;
            item.value = Item.buyPrice(0, 9, 21, 0);
            item.autoReuse = false;
            item.rare = Rarity.Rainbow;
            item.UseSound = SoundID.Item38;
            item.shoot = ProjectileID.Bullet;
            item.shootSpeed = 2.7f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true;

            item.expert = true;
            item.scale = .85f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = Calc.RandomSpread(speedX, speedY, 8, .0325f, 5);
            for (int i = 0; i < Main.rand.Next(3, 5); ++i)
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X - Main.rand.NextFloat(-.5f, .75f), speeds[i].Y - Main.rand.NextFloat(-.5f, .75f), type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}