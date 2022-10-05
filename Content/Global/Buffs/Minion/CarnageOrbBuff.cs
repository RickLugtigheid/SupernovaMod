using Supernova.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Buffs.Minion
{
    public class CarnageOrbBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Orb");
            Description.SetDefault("Summons a orb that will steal the life essence from your foes.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Global.Minions.CarnageOrb>()] > 0)
			{
                modPlayer.hasMinionCarnageOrb = true;
            }

            if (!modPlayer.hasMinionCarnageOrb)
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