﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Summon
{
	public class HarbingersKnell : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			// DisplayName.SetDefault("Harbingers Knell");
            // Tooltip.SetDefault("Summons a Omen to fight for you.");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.mana = 10;
            Item.width = 20;
            Item.height = 20;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = Item.buyPrice(0, 4, 80, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Projectiles.Summon.OmenMinion>();
            Item.shootSpeed = 1f;
            Item.buffType = ModContent.BuffType<Buffs.Summon.HarbingersKnellBuff>();
            Item.buffTime = 3600;

            Item.DamageType = DamageClass.Summon;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => player.altFunctionUse != 2;

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.altFunctionUse == 2)
                player.MinionNPCTargetAim(true);
            return base.UseItem(player);
        }
    }
}
