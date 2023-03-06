using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Accessories;
using Terraria;
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
	}
}
