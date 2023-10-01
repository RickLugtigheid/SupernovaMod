using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Content.Items.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
    public class AccessoryPlayer : ModPlayer
	{
		#region [Flags]

		public bool hasHeartOfTheJungle = false;
		public bool hasBagOfFungus		= false;
		public bool hasInfernalEmblem	= false;

		#endregion

		/* Buffs */
		private int _buffTypeHellfireRing = ModContent.BuffType<Content.Buffs.Rings.HellfireRingBuff>();
		public bool HasBuffHellfireRing => Player.HasBuff(_buffTypeHellfireRing);

		/* Minions */
		public bool hasMinionVerglasFlake = false;
		public bool hasMinionCarnageOrb = false;
		public bool hasMinionHairbringersKnell = false;

		/* Reset */
		public override void ResetEffects()
		{
			base.ResetEffects();

			hasHeartOfTheJungle	= false;
			hasBagOfFungus		= false;
			hasInfernalEmblem	= false;

			hasMinionVerglasFlake		= false;
			hasMinionCarnageOrb			= false;
			hasMinionHairbringersKnell	= false;
		}

		#region [OnHit methods]

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */ => OnAnyHitNpc(target, damageDone, hit);
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */ => OnAnyHitNpc(target, damageDone, hit, proj.type);

		private void OnAnyHitNpc(NPC target, int damageDone, NPC.HitInfo hit, int? projType = null)
		{
			if (HasBuffHellfireRing && Main.rand.NextBool(3) && (projType != null && projType != ProjectileID.InfernoFriendlyBlast))
			{
				// Get a random position within our enemy sprite
				Vector2 position = target.Center + new Vector2(Main.rand.Next(-target.width, target.width), Main.rand.Next(-target.height, target.height));

				// Spawn a fire blast at the position with a max of 20 damage
				Projectile.NewProjectile(Player.GetSource_FromThis(), position, Vector2.Zero, ProjectileID.InfernoFriendlyBlast, (damageDone / 2) % 20, hit.Knockback, Player.whoAmI);
			}
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			OnHitByAny(npc, hurtInfo);

			if (hasBagOfFungus)
			{
				BagOfFungus_OnHitByNPC();
			}

			if (hasInfernalEmblem)
			{
				InfernalEmblem_OnHitByNPC();
			}
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			OnHitByAny(proj, hurtInfo);
		}

		private void OnHitByAny(Entity entity, Player.HurtInfo hurtInfo)
		{
			if (hasHeartOfTheJungle)
			{
				HeartOfTheJungle_OnHitByAny();
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			ModifyHitByAny(npc, ref modifiers);
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
			ModifyHitByAny(proj, ref modifiers);
		}
		public void ModifyHitByAny(Entity entity, ref Player.HurtModifiers modifiers)
		{
			if (Player.GetModPlayer<ArmorPlayer>().coldArmor && !Player.HasBuff<Content.Buffs.Cooldowns.ColdArmorCooldown>())
			{
				Player.AddBuff(ModContent.BuffType<Content.Buffs.Cooldowns.ColdArmorCooldown>(), 10 * 60);
				SoundEngine.PlaySound(TerrariaRandom.NextSoundIceStruck(), Player.Center);

				// Add dust effect
				for (int j = 0; j < 5; j++)
				{
					int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Ice);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].velocity *= 1.5f;
				}

				// Reduce the final damage by 25%
				modifiers.FinalDamage *= .75f;
			}
		}

		#endregion

		private void HeartOfTheJungle_OnHitByAny()
		{
			// Check each accessory slot for the HeartOfTheJungle accessory
			//
			for (int i = 3; i < 8 + Player.extraAccessorySlots; i++)
			{
				// Check if slot 'i' contains the HeartOfTheJungle accessory
				//
				Item item = Player.armor[i];
				if (item.type == ModContent.ItemType<HeartOfTheJungle>())
				{
					// OnHit the HeartOfTheJungle should consume enery and heal the player
					//
					(item.ModItem as HeartOfTheJungle).ConsumeEnergy(Player);
				}
			}
		}

		private void BagOfFungus_OnHitByNPC()
		{
			// Trigger this effect 1 in 3 times (3 because we start at 0)
			//
			if (!Main.rand.NextBool(2))
			{
				return;
			}

			// Summon '5' Mushroom projectiles around the player
			//
			for (int e = 0; e < 5; e++)
			{
				int i = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X, Player.Center.Y, 1 - Main.rand.Next(-Player.width, Player.width) / 2, Main.rand.Next(-Player.height, Player.height) / 2, ProjectileID.Mushroom, Main.rand.Next(5, 15), 0f, Player.whoAmI, 3f, 3f);
				Main.projectile[i].hostile = false;
				Main.projectile[i].friendly = true;
			}
		}

		private void InfernalEmblem_OnHitByNPC()
		{
			// Summon between '2' and '4' MolotovFire projectiles around the player
			//
			for (int e = 0; e < Main.rand.Next(2, 4); e++)
			{
				short anyMolotovFireID = TerrariaRandom.NextProjectileIDMolotovFire();
				int i = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X, Player.Center.Y, 1 - Main.rand.Next(-5, 5), 1 - Main.rand.Next(-5, 5), anyMolotovFireID, Main.rand.Next(3, 12), 0f, Player.whoAmI, 2f, 2f);
				Main.projectile[i].hostile = false;
				Main.projectile[i].friendly = true;
			}
		}
	}
}
