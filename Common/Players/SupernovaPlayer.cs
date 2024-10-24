using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SupernovaMod
{
    public static class SupernovaPlayerUtils
    {
        public static SupernovaPlayer Supernova(this Player player) => player.GetModPlayer<SupernovaPlayer>();
    }
}
namespace SupernovaMod.Common.Players
{
	public class SupernovaPlayer : ModPlayer
	{
        public bool blackFlamesDebuff = false;

        public override void ResetEffects()
        {
			blackFlamesDebuff = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (blackFlamesDebuff)
            {
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                // Player.lifeRegenTime used to increase the speed at which the player reaches its maximum natural life regeneration
                // So we set it to 0, and while this debuff is active, it never reaches it
                Player.lifeRegenTime = 0;
                // lifeRegen is measured in 1/2 life per second. Therefore, this effect causes (16 / 2 = ) 8 life lost per second
                Player.lifeRegen -= 16;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (blackFlamesDebuff && Main.rand.Next(5) < 3)
            {
                int dust13 = Dust.NewDust(Player.position - new Vector2(2f, 2f), Player.width, Player.height, ModContent.DustType<Content.Dusts.BlackFlamesDust>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 0, default(Color), 1f);
                Main.dust[dust13].noGravity = true;
                Main.dust[dust13].velocity *= 1.1f;
                Dust dust27 = Main.dust[dust13];
                dust27.velocity.Y = dust27.velocity.Y + 0.25f;
                if (Utils.NextBool(Main.rand, 2))
                {
                    Main.dust[dust13].noGravity = false;
                    Main.dust[dust13].scale *= 0.5f;
                }
            }
        }
    }
}