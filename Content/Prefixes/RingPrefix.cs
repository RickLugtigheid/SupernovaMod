using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
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
	}
}
