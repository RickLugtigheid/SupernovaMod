using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Prefixes
{
    public class RingPrefix : ModPrefix
    {
		public override PrefixCategory Category => PrefixCategory.Custom;
		/// <summary>
		/// The ring type this prefix can apply to.
		/// <para>Set to <see cref="RingType.Misc"/> to apply to all ring types</para>
		/// </summary>
		public virtual RingType RingCategory { get; } = RingType.Misc;

		public RingPrefix(float coolRegenMulti = 1, float projectileDamageMulti = 1, int tier = 0)
        {
            this.coolRegenMulti = coolRegenMulti;
			this.projectileDamageMulti = projectileDamageMulti;
			this.tier = tier;
        }

		public override void Apply(Item item)
		{
            if (!RingPlayer.ItemIsRing(item))
            {
                return;
            }

			SupernovaRingItem ring = item.ModItem as SupernovaRingItem;
			ring.coolRegen *= coolRegenMulti;

			// Apply Projectile type ring buffs
			//
			if (ring.RingType == RingType.Projectile)
			{
				ring.damageBonusMulti = projectileDamageMulti;
			}

			// Apply rarity
			item.rare += tier;

			item.RebuildTooltip();
		}

		public override bool CanRoll(Item item)
		{
			if (!RingPlayer.ItemIsRing(item))
			{
				return false;
			}

			SupernovaRingItem ring = item.ModItem as SupernovaRingItem;

			if (RingCategory != RingType.Misc && ring.RingType != RingCategory)
			{
				return false;
			}

			return true;
		}

		public override void ModifyValue(ref float valueMult)
		{
			float extraValue = 1f + 1f * (this.coolRegenMulti - 1f);
			valueMult *= extraValue;
		}

		internal int tier = 0;
		internal float coolRegenMulti = 1;
		internal float projectileDamageMulti = 1;

		/// <summary>
		/// Load all <see cref="RingPrefix"/>es that can be used by the <paramref name="ring"/>.
		/// </summary>
		/// <param name="ring"></param>
		/// <returns></returns>
		public static int[] LoadPrefixes(SupernovaRingItem ring)
		{
			List<int> prefixes = new List<int>();
			foreach (RingPrefix ringPrefix in ModContent.GetContent<RingPrefix>())
			{
				// Check if this prefix has a RingCategory that the given ring can use
				//
				if (ringPrefix.RingCategory == RingType.Misc || ringPrefix.RingCategory == ring.RingType)
				{
					prefixes.Add(ringPrefix.Type);
				}
			}
			return prefixes.ToArray();
		}
	}
}
