using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;
using System;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.Bloodmoon
{
	public class BloodCaster : ModNPC // ModNPC is used for Custom NPCs
    {
		protected Player target;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Caster");
            //Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 36;
            NPC.damage = 10;
            NPC.defense = 7;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath36;
            NPC.value = 1000f;
            NPC.knockBackResist = .5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
        }

        /*public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }*/

        private int[] _ownedNpcs = new int[3];

		public override void AI()
		{
            ref float timer = ref NPC.ai[0];
            ref float attackPointer = ref NPC.ai[1];
			timer++;

			target = Main.player[NPC.target];

			if (attackPointer == 0)
            {
                if (timer % 90 == 0)
                {
                    ShootToPlayer(ProjectileID.BloodShot, 24);
                }

                if (timer >= 230)
                {
                    timer = 0;
                    attackPointer++;
                }
            }
            else if (attackPointer == 1)
            {
                for (int i = 0; i < _ownedNpcs.Length; i++)
                {
                    int index = _ownedNpcs[i];
                    if (index == 0 || !Main.npc[index].active || Main.npc[index].timeLeft > 1)
                    {
						_ownedNpcs[i] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 50, ModContent.NPCType<Gazer>());
                    }
                }
                attackPointer++;
			}
            else
            {
                attackPointer = 0;
            }
		}

		private void ShootToPlayer(int type, int damage, float velocityMulti = 1, float rotationMulti = 1)
		{
			SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

			Vector2 position = new Vector2(NPC.Center.X + (NPC.width / 2 + 25) * NPC.direction, NPC.Center.Y + NPC.height - 50);
			float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
			rotation *= rotationMulti;

			Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * velocityMulti;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, 0);

			for (int x = 0; x < 5; x++)
			{
				int dust = Dust.NewDust(position, 25, 25, DustID.Blood, velocity.X, velocity.Y, 80, default, Main.rand.NextFloat(.9f, 1.6f));
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= Main.rand.NextFloat(.3f, .5f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.QuarionShard>(), 4, maximumDropped: 2));
            base.ModifyNPCLoot(npcLoot);
        }
    }
}
