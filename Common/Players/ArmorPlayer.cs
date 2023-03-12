using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Accessories;
using SupernovaMod.Content.Projectiles.Ranged.Arrows;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
    public class ArmorPlayer : ModPlayer
	{
		public bool zirconiumArmor = false;
		public bool coldArmor = false;

		/* Reset */
		public override void ResetEffects()
		{
			base.ResetEffects();

			zirconiumArmor = false;
			coldArmor = false;
		}
	}
}
