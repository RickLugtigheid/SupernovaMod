using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.DreamlandsNPCs
{
    public class EldritchSlime : ModNPC // ModNPC is used for Custom NPCs 
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
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
				//BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HorrorSpace, // TODO 
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime, 
 
				// Sets the description of this NPC that is listed in the bestiary. 
				new FlavorTextBestiaryInfoElement(""),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 32;
            NPC.damage = 25;
            NPC.defense = 7;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 200f;
            NPC.knockBackResist = .3f;
            NPC.aiStyle = NPCAIStyleID.Slime;
            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            Color color = new Color(116, 139, 126);
            if (NPC.life > 0)
            {
                for (int k = 0; k < 12; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny, hit.HitDirection, -.5f, 0, color, 0.8f);
                }
            }
            else
            {
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny, 1.5f * hit.HitDirection, -1, 0, color, 0.8f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.SlimeStaff, 10000));
        }
    }
}
