using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public static class GlobalModifiers
	{
		/// <summary>
		/// The <see cref="DamageClass"/> to use for ranged thrower <see cref="ModItem"/>s and <see cref="ModProjectile"/>s
		/// </summary>
		public static DamageClass DamageClass_ThrowingRanged { get; set; }
		/// <summary>
		/// The <see cref="DamageClass"/> to use for melee thrower <see cref="ModItem"/>s and <see cref="ModProjectile"/>s (like boomerangs)
		/// </summary>
		public static DamageClass DamageClass_ThrowingMelee { get; set; }

		/// <summary>
		/// Sets the default values 
		/// </summary>
		public static void SetStaticDefaults()
		{
			DamageClass_ThrowingRanged = DamageClass.Ranged;
			DamageClass_ThrowingMelee = DamageClass.Melee;
		}
	}
}
