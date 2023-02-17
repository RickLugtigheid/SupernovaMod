using Supernova.Common.Players;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.Items.Rings.BaseRings
{
    public abstract class SupernovaRingItem : ModItem
    {
        /// <summary>
        /// The ring animation length in time
        /// </summary>
        public virtual int MaxAnimationFrames { get; } = 1;
        /// <summary>
        /// Ring cooldown to aply when ring is activated
        /// </summary>
        public abstract int Cooldown { get; }
        /// <summary>
        /// When the ring is activated
        /// </summary>
        /// <param name="player">Player that activated the ring</param>
        public virtual void RingActivate(Player player) { }
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
    }
}
