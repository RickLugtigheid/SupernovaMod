using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Prefixes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SupernovaMod.Content.Items.Rings.BaseRings
{
    public enum RingType
    {
        Misc = -1,
        Projectile,
    }
    public abstract class SupernovaRingItem : ModItem
    {
        /// <summary>
        /// Damage for <see cref="RingType.Projectile"/> type rings.
        /// </summary>
        public int damage;
        public float damageBonusMulti = 1;

        /// <summary>
        /// Cooldown regen multiplier
        /// </summary>
        public float coolRegen = 1;

		/// <summary>
		/// Ring cooldown to aply when ring is activated
		/// </summary>
		public int Cooldown { get; private set; }

        /// <summary>
        /// The ring animation length in time
        /// </summary>
        public virtual int MaxAnimationFrames { get; } = 1;
        /// <summary>
        /// Ring cooldown without any modifiers
        /// </summary>
        public abstract int BaseCooldown { get; }
        /// <summary>
        /// The type of ring
        /// </summary>
        public virtual RingType RingType { get; } = RingType.Misc;
        /// <summary>
        /// When the ring is activated
        /// </summary>
        /// <param name="player">Player that activated the ring</param>
        public virtual void RingActivate(Player player, float ringPowerMulti) { }
        /// <summary>
        /// When the ring is cooling down
        /// </summary>
        /// <param name="curentCooldown">Curent seconds left of the Cooldown</param>
        /// <param name="player">Player that activated the ring</param>
        [Obsolete("Method no longer used. Use update or a ring buff instead")]
        public virtual void OnRingCooldown(int curentCooldown, Player player) { }
        /// <summary>
        /// Checks if the ring can be activated
        /// <para>This method is for doing custom checks before activating</para>
        /// </summary>
        /// <param name="player">Player that tries to activate the ring</param>
        /// <returns></returns>
        public virtual bool CanRingActivate(RingPlayer player) => true;

        /// <summary>
        /// Update the use animation for our ring.
        /// </summary>
        /// <returns>Animation done</returns>
		public virtual void RingUseAnimation(Player player)
        {

        }

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.accessory = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
            // Insert after 'ItemName'
            tooltips.Insert(1, new TooltipLine(Mod, "CooldownTime", GetDurationText() + " cooldown"));

            // Add damage for Projectile type rings
            if (RingType == RingType.Projectile)
            {
				tooltips.Insert(2, new TooltipLine(Mod, "Damage", Math.Round(damage * damageBonusMulti) + " damage"));
			}

            // Add cool regen bonus text
            //
            if (coolRegen != 1)
            {
                TooltipLine coolRegenBonus = new TooltipLine(Mod, "RingCoolRegenBonus", string.Empty);
                if (coolRegen > 1)
                {
					coolRegenBonus.Text = $"+{Math.Round(coolRegen * 100) - 100}% cooldown";
					coolRegenBonus.IsModifier = true;
                    coolRegenBonus.IsModifierBad = true;
				}
				else
                {
					coolRegenBonus.Text = $"-{100 - Math.Round(coolRegen * 100)}% cooldown";
					coolRegenBonus.IsModifier = true;
				}
				tooltips.Add(coolRegenBonus);
			}
            // Add damage bonus text
            if (damageBonusMulti != 1)
            {
				TooltipLine damageBonus = new TooltipLine(Mod, "RingDamageBonus", string.Empty);
				if (damageBonusMulti > 1)
				{
					damageBonus.Text = $"+{Math.Round(damageBonusMulti * 100) - 100}% damage";
                    damageBonus.IsModifier = true;
				}
				else
				{
					damageBonus.Text = $"-{100 - Math.Round(damageBonusMulti * 100)}% damage";
					damageBonus.IsModifier = true;
					damageBonus.IsModifierBad = true;
				}
				tooltips.Add(damageBonus);
			}
		}

        private string GetDurationText()
        {
            if (Cooldown == 0)
            {
                Cooldown = BaseCooldown;
            }
			Cooldown = (int)(Cooldown * coolRegen);

			int seconds = Cooldown / 60;
            int minutes = seconds / 60;

            if (minutes > 0)
            {
                return minutes + " minute" + (minutes > 1 ? 's' : string.Empty);
            }
            return seconds + " seconds";
		}

		public override void UpdateInventory(Player player)
		{
			Cooldown = BaseCooldown;
			Cooldown = (int)(Cooldown * player.GetModPlayer<ResourcePlayer>().ringCoolRegen);
			Item.RebuildTooltip();
		}
		public override void UpdateEquip(Player player)
		{
			Cooldown = BaseCooldown;
			Cooldown = (int)(Cooldown * player.GetModPlayer<ResourcePlayer>().ringCoolRegen);
			Item.RebuildTooltip();
		}

        // Rings an only be equiped in the ring slot
        //
		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
            if (slot != ModContent.GetInstance<SupernovaRingSlot>().Type)
                return false;
            return true;
		}

		#region Reforge Methods
		public override bool PreReforge()
		{
            coolRegen = 1;
			damageBonusMulti = 1;
			return true;
		}

		// Make rings only be able to get ring prefixes
		//
		private static int[] _ringPrefixes = ModContent.GetContent<RingPrefix>().Select(pre => pre.Type).ToArray();
		public override int ChoosePrefix(UnifiedRandom rand) => Main.rand.NextFromList(_ringPrefixes);
		#endregion
	}
}
