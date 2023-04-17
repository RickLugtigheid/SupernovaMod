using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
    public class AccessoryPlayer : ModPlayer
	{
		/* Accessories */
		public bool accHeartOfTheJungle = false;
		public bool accBagOfFungus = false;
		public bool accInfernalEmblem = false;

		/* Buffs */
		private int _buffTypeHellfireRing = ModContent.BuffType<Content.Buffs.Rings.HellfireRingBuff>();
		public bool HasBuffHellfireRing => Player.HasBuff(_buffTypeHellfireRing);

		/* Minions */
		public bool hasMinionVerglasFlake = false;
		public bool hasMinionCarnageOrb = false;
		public bool hasMinionHairbringersKnell = false;

		/* Misc */
		public bool coldArmor = false;

		/* Reset */
		public override void ResetEffects()
		{
			base.ResetEffects();

			accBagOfFungus		= false;
			accInfernalEmblem = false;
			accHeartOfTheJungle = false;

			hasMinionVerglasFlake		= false;
			hasMinionCarnageOrb			= false;
			hasMinionHairbringersKnell	= false;

			coldArmor = false;
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) => OnAnyHitNpc(target, damage, knockback, crit);
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) => OnAnyHitNpc(target, damage, knockback, crit, proj.type);

		private void OnAnyHitNpc(NPC target, int damage, float knockback, bool crit, int? projType = null)
		{
			if (HasBuffHellfireRing && Main.rand.NextBool(3) && (projType != null && projType != ProjectileID.InfernoFriendlyBlast))
			{
				// Get a random position within our enemy sprite
				Vector2 position = target.Center + new Vector2(Main.rand.Next(-target.width, target.width), Main.rand.Next(-target.height, target.height));

				// Spawn a fire blast at the position with a max of 20 damage
				Projectile.NewProjectile(Player.GetSource_FromThis(), position, Vector2.Zero, ProjectileID.InfernoFriendlyBlast, (damage / 2) % 20, knockback, Player.whoAmI);
			}
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			OnHitByAny(npc, damage, crit);
			if (accBagOfFungus & Main.rand.NextBool(2))
			{
				Invoke_BagOfFungus();
			}
			if (accInfernalEmblem)
			{
				Invoke_InfernalEmblem();
			}
		}
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			OnHitByAny(proj, damage, crit);
		}

		private void OnHitByAny(Entity entity, int damage, bool crit)
		{
			if (accHeartOfTheJungle)
			{
				for (int i = 3; i < 8 + Player.extraAccessorySlots; i++)
				{
					Item item = Player.armor[i];
					if (item.type == ModContent.ItemType<HeartOfTheJungle>())
					{
						Invoke_HeartOfTheJungle(item.ModItem as HeartOfTheJungle);
					}
				}
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			ModifyHitByAny(npc, ref damage, ref crit);
		}
		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
		{
			ModifyHitByAny(proj, ref damage, ref crit);
		}
		public void ModifyHitByAny(Entity entity, ref int damage, ref bool crit)
		{
			if (coldArmor && !Player.HasBuff<Content.Buffs.Cooldowns.ColdArmorCooldown>())
			{
				Player.AddBuff(ModContent.BuffType<Content.Buffs.Cooldowns.ColdArmorCooldown>(), 10 * 60);
				SoundEngine.PlaySound(GetRandomIceStruckSound(), Player.Center);

				// Add dust effect
				for (int j = 0; j < 5; j++)
				{
					int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Ice);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].velocity *= 1.5f;
				}

				damage = (int)(damage * .75f);
			}
		}

		void Invoke_BagOfFungus()
		{
			for (int e = 0; e < 5; e++)
			{
				int i = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X, Player.Center.Y, 1 - Main.rand.Next(-Player.width, Player.width) / 2, Main.rand.Next(-Player.height, Player.height) / 2, ProjectileID.Mushroom, Main.rand.Next(5, 15), 0f, Player.whoAmI, 3f, 3f);
				Main.projectile[i].hostile = false;
				Main.projectile[i].friendly = true;
			}
		}
		void Invoke_InfernalEmblem()
		{
			for (int e = 0; e < Main.rand.Next(2, 5); e++)
			{
				int i = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X, Player.Center.Y, 1 - Main.rand.Next(-5, 5), 1 - Main.rand.Next(-5, 5), ProjectileID.MolotovFire3, Main.rand.Next(3, 12), 0f, Player.whoAmI, 2f, 2f);
				Main.projectile[i].hostile = false;
				Main.projectile[i].friendly = true;
			}
		}
		void Invoke_HeartOfTheJungle(HeartOfTheJungle item)
		{
			item.ConsumeEnergy(Player);
		}

		private SoundStyle GetRandomIceStruckSound()
		{
			switch (Main.rand.Next(0, 2))
			{
				default:
					return SoundID.Item48;
				case 1:
					return SoundID.Item49;
				case 2:
					return SoundID.Item50;
			}
		}
	}
}
