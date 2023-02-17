using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Bosses.StoneMantaRay
{
    public class StoneRayChild : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("StoneRayChild");
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 44;
            NPC.damage = 40;
            NPC.defense = 14;
            NPC.lifeMax = 50;
            NPC.value = 600f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 6;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            //AIType = NPCID.Harpy;  //npc behavior
            AnimationType = NPCID.Harpy;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }
    }
}