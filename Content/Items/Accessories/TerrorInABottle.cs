using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Api.Effects;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
	public class TerrorInABottle : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.value = BuyPrice.RarityGreen;
			Item.accessory = true;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetJumpState<TerrorExtraJump>().Enable();
		}
	}

	public class TerrorExtraJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new After(BlizzardInABottle);

		public override float GetDurationMultiplier(Player player)
		{
			return 1f;
		}

		public override void UpdateHorizontalSpeeds(Player player)
		{
			// Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			player.runAcceleration *= 1.25f;
			player.maxRunSpeed *= 2f;
			player.jumpSpeedBoost = 1.5f;
		}

		public override void OnStarted(Player player, ref bool playSound)
		{
			// Spawn rings of fire particles
			int offsetY = player.height;
			if (player.gravDir == -1f)
				offsetY = 0;

			offsetY -= 16;

			DrawDust.Ring(
				player.Top + new Vector2(0, offsetY),
				-player.velocity * 0.35f,
				new Vector2((player.width + 2), 5) * 1.5f,
				ModContent.DustType<Dusts.TerrorDust>(),
				50
			);
			/*Vector2 center = player.Top + new Vector2(0, offsetY);
			const int numDusts = 40;
			for (int i = 0; i < numDusts; i++)
			{
				(float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / numDusts));

				float amplitudeX = cos * (player.width + 2) / 2f;
				float amplitudeY = sin * 5;

				Dust dust = Dust.NewDustPerfect(center + new Vector2(amplitudeX, amplitudeY), ModContent.DustType<Dusts.TerrorDust>() *//*DustID.BlueFlare*//*, -player.velocity * 0.35f, Scale: 1f);
				dust.noGravity = true;
			}*/
		}

		public override void ShowVisuals(Player player)
		{
			// Use this hook to trigger effects that should appear throughout the duration of the extra jump
			// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
			int offsetY = player.height - 6;
			if (player.gravDir == -1f)
				offsetY = 6;

			/*DrawDust.Ring(
				player.Top + new Vector2(0, offsetY),
				-player.velocity * 0.35f,
				new Vector2((player.width + 2), 5)
			);*/
		}
	}
}
