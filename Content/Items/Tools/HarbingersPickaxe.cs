using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Api;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Items.Tools
{
    public class HarbingersPickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.crit = 2;
			Item.knockBack = 2;
			Item.width = 24; // Hitbox Width
            Item.height = 24; // Hitbox Height
			Item.attackSpeedOnlyAffectsWeaponAnimation = true; // Melee speed affects how fast the tool swings for damage purposes, but not how fast it can dig

			Item.useTime = 18; // Speed before reuse
            Item.useAnimation = 18; // Animation Speed
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = BuyPrice.RarityGreen; // 10 | 00 | 00 | 00 : Platinum | Gold | Silver | Bronze
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.pick = 55; // Pick Power - Higher Value = Better
            Item.tileBoost += 4;
        }
	}
}
