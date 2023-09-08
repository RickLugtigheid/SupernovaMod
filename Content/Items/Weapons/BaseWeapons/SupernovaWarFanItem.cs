using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.BaseWeapons
{
    public abstract class SupernovaWarFanItem : ModItem
	{
		protected virtual Vector2 HandlePosition => new Vector2(-15, 1);
		protected virtual float SwingDegree => 1;

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.direction = Math.Sign((Main.MouseWorld - player.Center).X);
			float itemRotation = player.compositeFrontArm.rotation + 1.5707964f * player.gravDir;
			Vector2 itemPosition = player.MountedCenter + itemRotation.ToRotationVector2() * 7f;
			SupernovaUtils.CleanHoldStyle(player, itemRotation, itemPosition, new Vector2(Item.width, Item.height), new Vector2?(HandlePosition), false, false, true);
			base.UseStyle(player, heldItemFrame);
		}

		public override void UseItemFrame(Player player)
		{
			player.direction = Math.Sign((Main.MouseWorld - player.Center).X);
			float animProgress = 1f - player.itemTime / (float)player.itemTimeMax;
			float rotation = (player.Center - Main.MouseWorld).ToRotation() * player.gravDir + 1.5707964f;
			rotation = rotation - (SwingDegree * (float)Math.Pow((double)((0.4f - animProgress) / 0.4f), 2.0) * player.direction);

			if (animProgress < 0.4f)
			{
				// Aply gun recoil
				float recoil = -SwingDegree;
				rotation += recoil * (float)Math.Pow((double)((0.4f - animProgress) / 0.4f), 2.0) * player.direction;
			}
			player.SetCompositeArmFront(true, 0, rotation);
		}
	}
}
