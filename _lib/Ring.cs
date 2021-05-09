using Terraria;
using Terraria.ModLoader;

namespace Supernova
{
	public abstract class RingBase : ModItem
	{
		public const string RING_HELP = "\nPut in the ring slot to make it work.";
		/// <summary>
		/// Ring cooldown to aply when ring is activated
		/// </summary>
		public abstract int cooldown { get; }
		/// <summary>
		/// When the ring is activated
		/// </summary>
		/// <param name="player">Player that activated the ring</param>
		public abstract void OnRingActivate(Player player);
		/// <summary>
		/// When the ring is cooling down
		/// </summary>
		/// <param name="curentCooldown">Curent seconds left of the Cooldown</param>
		/// <param name="player">Player that activated the ring</param>
		public virtual void OnRingCooldown(int curentCooldown, Player player)
		{

		}
		/// <summary>
		/// Checks if the ring can be activated
		/// <para>This method is for doing custom checks before activating</para>
		/// </summary>
		/// <param name="player">Player that tries to activate the ring</param>
		/// <returns></returns>
		public virtual bool RingCanActivate(Player player) => true;
	}
}
