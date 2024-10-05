using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.DamageOverTime
{
    public class BlackFlames : ModBuff
    {
        public override string Texture => "SupernovaMod/Assets/Textures/DebuffTemplate";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // TODO: Add npc version of the debuff!

        public override void Update(Player player, ref int buffIndex)
        {
            player.Supernova().blackFlamesDebuff = true;
        }
    }
}
