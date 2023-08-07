using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.StatDebuffs
{
    public class VerglasIcicleDebuff : ModBuff
    {
        private float _defenceDecreaseMulti = 1;
        // NPC only buff so we'll just assign it a useless buff icon.
        public override string Texture => "SupernovaMod/Assets/Textures/DebuffTemplate";

        public override void Update(NPC npc, ref int buffIndex)
        {
            //if (npc.javelined)
            {
                int stickyProjectiles = 0;
                int projType = ModContent.ProjectileType<Projectiles.Ranged.VerglasIcicle>();

				// Find all sticking VerglasIcicles
                //
				for (int n = 0; n < 1000; n++)
                {
                    if (
                        Main.projectile[n].active                       // Is the found projectile active?
                        &&
                        Main.projectile[n].type == projType             // Is the found projectile a VerglasIcicle?
                        &&
                        Main.projectile[n].ai[1] == npc.whoAmI          // Is the found projectile sticking to our npc?
                    )
                    {
                        stickyProjectiles++;
                    }
                }

                // Calculate the new defense
                //
                int newDefense = npc.defense - (int)(stickyProjectiles * _defenceDecreaseMulti);
                if (newDefense > 0)
                {
                    npc.defDefense = newDefense;
				}
            }
        }
    }
}