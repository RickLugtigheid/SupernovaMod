using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;
using SupernovaMod.Api;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
	public class Mechromite : ModNPC // ModNPC is used for Custom NPCs 
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 8;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new()
			{
				// Influences how the NPC looks in the Bestiary 
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once 
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] { 
				// Sets the spawning conditions of this NPC that is listed in the bestiary. 
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime, 
 
 
				// Sets the description of this NPC that is listed in the bestiary. 
				new FlavorTextBestiaryInfoElement(""),
			});
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 40;
			NPC.defense = 21;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath37;
			NPC.value = BuyPrice.RarityWhite;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.AnomuraFungus;  //NPC behavior 
			AnimationType = NPCID.Harpy;

			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation. 
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
			NPC.frame.Y = frame * frameHeight;

			NPC.spriteDirection = -NPC.direction;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode)
			{
				return SpawnCondition.OverworldDay.Chance * 0.3f;
			}
			return 0;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.MechroDrive>(), 3, maximumDropped: 2));
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(BuffID.Electrified, Main.rand.Next(10, 60));
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				for (int num925 = 0; num925 < 4; num925++)
				{
					int num915 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 1.5f);
					Main.dust[num915].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
				}
				for (int i = 0; i < 12; i++)
				{
					Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(-40, 40), 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.Electric, -diff, Scale: Main.rand.Next(1, 2)).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Electric, -diff, Scale: Main.rand.Next(1, 2)).noGravity = true;
				}
				for (int num921 = 0; num921 < 2; num921++)
				{
					Vector2 position16 = NPC.position + new Vector2(NPC.width * Main.rand.Next(100) / 100f, NPC.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f;
					Vector2 vector33 = default;
					int num920 = Gore.NewGore(NPC.GetSource_Death(), position16, vector33, Main.rand.Next(61, 64), 1f);
					Main.gore[num920].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
					Gore gore2 = Main.gore[num920];
					gore2.velocity *= 0.3f;
					Gore expr_79A3_cp_0 = Main.gore[num920];
					expr_79A3_cp_0.velocity.X = expr_79A3_cp_0.velocity.X + Main.rand.Next(-10, 11) * 0.05f;
					Gore expr_79D3_cp_0 = Main.gore[num920];
					expr_79D3_cp_0.velocity.Y = expr_79D3_cp_0.velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
				}
			}
			else
			{
				int num915 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, .7f);
				Main.dust[num915].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
			}
		}
	}
}
