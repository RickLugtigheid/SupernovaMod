using Terraria;
using Terraria.ModLoader;

namespace Supernova.Api
{
	public abstract class SupernovaRing : ModItem
	{
		/// <summary>
		/// Ring cooldown to aply when ring is activated
		/// </summary>
		public abstract int Cooldown { get; }
		/// <summary> 
		/// The cooldown as buff cooldown 
		/// </summary> 
		protected int BuffCooldown => Cooldown * 2;
		/// <summary>
		/// When the ring is activated
		/// </summary>
		/// <param name="player">Player that activated the ring</param>
		public virtual void OnRingActivate(Player player) { }
		/// <summary>
		/// When the ring is cooling down
		/// </summary>
		/// <param name="curentCooldown">Curent seconds left of the Cooldown</param>
		/// <param name="player">Player that activated the ring</param>
		public virtual void OnRingCooldown(int curentCooldown, Player player) { }
		/// <summary>
		/// Checks if the ring can be activated
		/// <para>This method is for doing custom checks before activating</para>
		/// </summary>
		/// <param name="player">Player that tries to activate the ring</param>
		/// <returns></returns>
		public virtual bool CanRingActivate(Player player) => true;
	}
}
