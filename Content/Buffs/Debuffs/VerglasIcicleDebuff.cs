using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.Buffs.Debuffs
{
    public class VerglasIcicleDebuff : ModBuff
    {
        private float _defenceDecreaseMulti = 1;
        // NPC only buff so we'll just assign it a useless buff icon.
        public override string Texture => "Supernova/Assets/Textures/DebuffTemplate";

        /*public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;

            DisplayName.SetDefault("Frozen Armor");
            Description.SetDefault("Reduces defence. Debuff increases with the amount of stuck iciles.");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
        }*/
        public override void Update(NPC npc, ref int buffIndex)
        {
            //if (npc.javelined)
            {
                int stickyProjectiles = 0;
                int projType = ModContent.ProjectileType<Projectiles.Ranged.VerglasIcicle>();
                for (int n = 0; n < 1000; n++)
                {
                    if (
                        Main.projectile[n].active                       // Is the found projectile active?
                        &&
                        Main.projectile[n].type == projType             // Is the found projectile a VerglasIcicle?
                                                                        //&& 
                                                                        //Main.projectile[n].ai[0] == 1f				// Idk
                        &&
                        Main.projectile[n].ai[1] == npc.whoAmI   // Is the found projectile sticking to our npc?
                    )
                    {
                        stickyProjectiles++;
                    }
                }

                int newDefense = npc.defDefense - (int)(stickyProjectiles * _defenceDecreaseMulti);
                if (newDefense < 0)
                {
                    newDefense = 0;
                }
                npc.defense = newDefense;
            }

            base.Update(npc, ref buffIndex);
        }
    }
}