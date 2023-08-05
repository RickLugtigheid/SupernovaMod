using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.FlyingTerror
{
	public class TerrorSpirit : ModNPC
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.ClothiersCurse}";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terror Spirit");
			Main.npcFrameCount[NPC.type] = Main.projFrames[ProjectileID.ClothiersCurse];
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = NPCAIStyleID.CursedSkull;
			AIType = NPCID.CursedSkull;

			NPC.CloneDefaults(NPCID.CursedSkull);
			NPC.lifeMax = 45;
			NPC.damage = 25;
			NPC.defense = 6;
			NPC.value = 0;
		}

		public override void AI()
		{
			if (!Main.rand.NextBool(3))
			{
				Dust dust22 = Main.dust[Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Shadowflame, 0f, -2f, 0, default(Color), 1f)];
				dust22.noGravity = true;
				dust22.fadeIn = 0.5f;
				dust22.alpha = 200;
			}

			// Fix overlap with other npcs
			//
			float overlapVelocity = 0.08f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];

				if (i != NPC.whoAmI && other.active && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width * 2)
				{
					if (NPC.position.X < other.position.X)
					{
						NPC.velocity.X -= overlapVelocity;
					}
					else
					{
						NPC.velocity.X += overlapVelocity;
					}

					if (NPC.position.Y < other.position.Y)
					{
						NPC.velocity.Y -= overlapVelocity;
					}
					else
					{
						NPC.velocity.Y += overlapVelocity;
					}
				}
			}

			base.AI();
		}
	}
}
