using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
	internal class ResourcePlayer : ModPlayer
	{
		public float lifeEnergy = 0;
		public int lifeEnergyMax2 = 0;
		public float lifeEnergyRegen = .01f;

		public float ringPower = 1;
		public float ringCoolRegen = 1;

		public override void PreUpdate()
		{
			// Reset our max values
			lifeEnergyMax2 = 0;
			lifeEnergyRegen = .001f;
			ringPower = 1;
			ringCoolRegen = 1;

			base.PreUpdate();
		}

		public override void PostUpdateEquips()
		{
			// Update our life energy value
			//
			if (lifeEnergy < lifeEnergyMax2)
			{
				lifeEnergy += lifeEnergyRegen;
			}
		}
	}
}
