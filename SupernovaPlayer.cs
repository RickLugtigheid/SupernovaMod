using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Supernova.UI;

namespace Supernova
{
	public class SupernovaPlayer : ModPlayer
	{
		public static float ringCooldownDecrease = 1f;

		/* Accessories */
		public bool aBagOfFungus = false;
		public bool aInfernalEmblem = false;

		/* Minions */
		public bool minionHairbringersKnell = false;
		public bool minionVerglasFlake = false;
		public bool minionCarnageOrb = false;

		/*  Hit Events */
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) => OnHit(target, damage, knockback, crit, proj: proj);
		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) => OnHit(target, damage, knockback, crit, item);
		public void OnHit(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
		{

		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if(aBagOfFungus && Main.rand.Next(2) == 0)
				for (int e = 0; e < 5; e++)
				{
					int i = Projectile.NewProjectile(player.Center.X, player.Center.Y, 1 - Main.rand.Next(-player.width, player.width) / 2, Main.rand.Next(-player.height, player.height) / 2, ProjectileID.Mushroom, Main.rand.Next(5, 15), 0f, player.whoAmI, 3f, 3f);
					Main.projectile[i].hostile = false;
					Main.projectile[i].friendly = true;
				}
			if(aInfernalEmblem)
				for (int e = 0; e < Main.rand.Next(2, 5); e++)
				{
					int i = Projectile.NewProjectile(player.Center.X, player.Center.Y, 1 - Main.rand.Next(-5, 5), 1 - Main.rand.Next(-5, 5), ProjectileID.MolotovFire3, Main.rand.Next(3, 12), 0f, player.whoAmI, 2f, 2f);
					Main.projectile[i].hostile = false;
					Main.projectile[i].friendly = true;
				}
		}

		/* Equip Events */
		public override void PostUpdateEquips()
		{
			ModItem item = player.GetModPlayer<RingSlotPlayer>().EquipSlot.Item.modItem;

			// Check if item is a ring
			if (item != null && item.GetType().IsSubclassOf(typeof(RingBase)))
			{
				RingBase ring = (RingBase)item;
				// Check if we can activate the ring
				if (ring != null && ring.RingCanActivate(player) && !player.HasBuff(mod.BuffType("RingCooldown")) && Supernova.ringAbilityButton.JustPressed)
				{
					// Call on ring activate event
					ring?.OnRingActivate(player);

					// Add cooldown debuff
					player.AddBuff(mod.BuffType("RingCooldown"), (int)Math.Ceiling(ring.cooldown * ringCooldownDecrease), true);
				}

				// When we have the cooldown debuff
				if (player.HasBuff(mod.BuffType("RingCooldown")) && player.buffTime[player.FindBuffIndex(mod.BuffType("RingCooldown"))] != 0)
					// Call on ring cooldown event
					ring?.OnRingCooldown(player.buffTime[player.FindBuffIndex(mod.BuffType("RingCooldown"))], player);
			}
		}

		/* Reset */
		public override void ResetEffects()
		{
			base.ResetEffects();

			// Accessories
			aBagOfFungus = false;

			// Minions
			minionHairbringersKnell = false;
			minionVerglasFlake = false;
			minionCarnageOrb = false;
		}
	}
}
