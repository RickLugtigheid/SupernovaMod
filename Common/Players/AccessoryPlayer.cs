using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Common.Players
{
	public class AccessoryPlayer : ModPlayer
	{
		public bool hasBagOfFungus = false;
		public bool hasInfernalEmblem = false;

		/* Minions */
		public bool hasMinionVerglasFlake = false;
		public bool hasMinionCarnageOrb = false;
		public bool hasMinionHairbringersKnell = false;

		/* Reset */
		public override void ResetEffects()
		{
			base.ResetEffects();

			hasBagOfFungus		= false;
			hasInfernalEmblem	= false;

			hasMinionVerglasFlake		= false;
			hasMinionCarnageOrb			= false;
			hasMinionHairbringersKnell	= false;
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if (hasBagOfFungus & Main.rand.NextBool(2))
			{
				Invoke_BagOfFungus();
			}
			else if (hasInfernalEmblem)
			{
				Invoke_InfernalEmblem();
			}
		}

		void Invoke_BagOfFungus()
		{
			for (int e = 0; e < 5; e++)
			{
				int i = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X, Player.Center.Y, 1 - Main.rand.Next(-Player.width, Player.width) / 2, Main.rand.Next(-Player.height, Player.height) / 2, ProjectileID.Mushroom, Main.rand.Next(5, 15), 0f, Player.whoAmI, 3f, 3f);
				Main.projectile[i].hostile = false;
				Main.projectile[i].friendly = true;
			}
		}
		void Invoke_InfernalEmblem()
		{
			for (int e = 0; e < Main.rand.Next(2, 5); e++)
			{
				int i = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X, Player.Center.Y, 1 - Main.rand.Next(-5, 5), 1 - Main.rand.Next(-5, 5), ProjectileID.MolotovFire3, Main.rand.Next(3, 12), 0f, Player.whoAmI, 2f, 2f);
				Main.projectile[i].hostile = false;
				Main.projectile[i].friendly = true;
			}
		}
	}
}
