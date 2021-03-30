using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Supernova.Npcs.Bosses.StoneMantaRay
{
    public class StoneRayChild : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("StoneRayChild");
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {

            npc.width = 44;
            npc.height = 44;
            npc.damage = 40;
            npc.defense = 14;
            npc.lifeMax = 50;
            npc.value = 600f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles. 
            //aiType = NPCID.Harpy;  //npc behavior
            animationType = NPCID.Harpy;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;

            npc.spriteDirection = npc.direction;
        }
    }
}