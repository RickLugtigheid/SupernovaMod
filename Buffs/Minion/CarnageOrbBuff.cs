using Terraria;
using Terraria.ModLoader;

namespace Supernova.Buffs.Minion
{
    public class CarnageOrbBuff : ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Carnage Orb");
            Description.SetDefault("Summons a orb that will steal the life essence from your foes.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SupernovaPlayer modPlayer = player.GetModPlayer<SupernovaPlayer>();

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.CarnageOrb>()] > 0)
                modPlayer.minionCarnageOrb = true;
            if (!modPlayer.minionCarnageOrb)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}