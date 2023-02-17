using System;
using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Bosses.FlyingTerror
{
    public class BlunderBuss : SupernovaGunItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Blunder Buss");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-13, 0);
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.expertOnly = true;
            Item.damage = 14;
            Item.width = 74;
            Item.crit = 7;
            Item.height = 22;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.noMelee = true;
            Item.knockBack = 5.4f;
            Item.value = Item.buyPrice(0, 9, 21, 0);
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item38;
            Item.shootSpeed = 6;
            Item.expert = true;
            Item.scale = .75f;

            Gun.spread = 8;
            Gun.recoil = .8f;

            Gun.handlePosition.Y = 2;
            Gun.handlePosition.X -= 12;

            Gun.style = GunStyle.Shotgun;
            Gun.useStyle = GunUseStyle.PumpAction;
            Gun.shotgunMinShots = 3;
            Gun.shotgunMaxShots = 5;
        }
    }
}