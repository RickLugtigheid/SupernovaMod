using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public sealed partial class Supernova : Mod
	{
		public override object Call(params object[] args)
		{
			// Check if the first argument (the function name) was given
			//
			if (args == null || args.Length == 0)
			{
				Logger.Error("ModCallError: No function name specified. First argument must be a function name.");
				return null;
			}
			// Enforce the function name to be of type string
			//
			if (!(args[0] is string))
			{
				Logger.Error("ModCallError: Invalid type for function name given. First argument must be a string.");
				return null;
			}

			// Get the function name
			string function = (string)args[0];

			// For making it easy to create new functions we try to load
			// a method in this class that matches 'Call_{function}'
			//
			// Fist try to get the method with name 'Call_{function}'
			MethodInfo? callMethod = GetType().GetMethod($"Call_{function}");
			// Than we check if the method was found
			//
			if (callMethod == null)
			{
				Logger.Error($"ModCallError: Invalid function name '{function}' given. Please check the documentation for all valid ModCall functions.");
				return null;
			}

			// At last we invoke the callMethod with our arguments,
			// and return the returned value.
			return callMethod.Invoke(this, args);
		}

		public object Call_GetDownedBoss(object[] args)
		{
			if (args.Length < 1)
			{
				Logger.Error($"ModCallError: Call({args[0]}) - No boss name specified. First argument of a '{args[0]}' call must be a boss name.");
				return null;
			}
			if (!(args[1] is string))
			{
				Logger.Error($"ModCallError: Call({args[0]}) - Invalid type for boss name given. First argument of a '{args[0]}' call must be a string.");
				return null;
			}

			switch (args[1])
			{
				// Pre-Hardmode
				case "HarbingerOfAnnihilation":
					return DownedSystem.downedHarbingerOfAnnihilation;
				case "Bloodweaver":
					return DownedSystem.downedBloodweaver;
				case "FlyingTerror":
					return DownedSystem.downedFlyingTerror;
				case "StormSovereign":
					return DownedSystem.downedStormSovereign;
				default:
					Logger.Error($"ModCallError: Call({args[0]}) - No boss with name '{args[1]}' found. Please check the documentation for valid boss names.");
					return null;
			}
		}

		#region Argument Extraction methods

		private bool TryGetPlayerArgument(object[] args, out Player player, int playerArgIndex = 1)
		{
			player = null;

			// Check if the argument was given
			//
			if (args.Length < playerArgIndex)
			{
				Logger.Error($"ModCallError: Call({args[0]}) - No Player specified.");
				return false;
			}

			// Check if the Player argument is of type Player
			//
			if (!(args[playerArgIndex] is Player))
			{
				Logger.Error($"ModCallError: Call({args[0]}) - No Player object given for Player argument.");
				return false;
			}
			player = (Player)args[playerArgIndex];

			// Success
			return true;
		}
		private bool TryGetBonusArguments<T>(object[] args, out Player player, out T value)
		{
			player = null;
			value = default;

			// Get the player argument
			//
			if (!TryGetPlayerArgument(args, out player))
			{
				return false;
			}

			// Check if the value argument was given
			//
			if (args.Length < 2)
			{
				Logger.Error($"ModCallError: Call({args[0]}) - No value specified.");
				return false;
			}

			// Check if the value argument is for type float
			//
			if (!(args[2] is T))
			{
				Logger.Error($"ModCallError: Call({args[0]}) - No {typeof(T).Name} object given for value argument.");
				return false;
			}
			value = (T)args[2];

			// Success
			return true;
		}

		#endregion

		#region Ring Calls

		public object Call_IsRingItem(object[] args)
		{
			if (args.Length < 1)
			{
				Logger.Error($"ModCallError: Call({args[0]}) - No Item specified.");
				return null;
			}
			if (!(args[1] is Item))
			{
				Logger.Error($"ModCallError: Call({args[0]}) - Invalid type for Item to check given. First argument of a '{args[0]}' call must be a Item.");
				return null;
			}

			return Players.RingPlayer.ItemIsRing(args[1] as Item);
		}

		public object Call_GetRingCooldownMod(object[] args)
		{
			if (!TryGetPlayerArgument(args, out Player player))
			{
				return null;
			}
			// Get the resource player and return the RingCooldownMod
			return player.GetModPlayer<Players.ResourcePlayer>().ringCoolRegen;
		}
		public object Call_GetRingPower(object[] args)
		{
			if (!TryGetPlayerArgument(args, out Player player))
			{
				return null;
			}
			// Get the resource player and return the RingPower
			return player.GetModPlayer<Players.ResourcePlayer>().ringPower;
		}

		public object Call_BonusRingCooldown(object[] args)
		{
			if (!TryGetBonusArguments(args, out Player player, out float value))
			{
				return null;
			}
			// Get the resource player and add the value to the cooldown regen
			//
			Players.ResourcePlayer resourcePlayer = player.GetModPlayer<Players.ResourcePlayer>();
			resourcePlayer.ringCoolRegen += value;
			return null;
		}
		public object Call_BonusRingCooldownMulti(object[] args)
		{
			if (!TryGetBonusArguments(args, out Player player, out float value))
			{
				return null;
			}
			// Get the resource player and multiply the cooldown regen by the value
			//
			Players.ResourcePlayer resourcePlayer = player.GetModPlayer<Players.ResourcePlayer>();
			resourcePlayer.ringCoolRegen *= value;
			return null;
		}

		public object Call_BonusRingPower(object[] args)
		{
			if (!TryGetBonusArguments(args, out Player player, out float value))
			{
				return null;
			}

			// Get the resource player and add the value to the ring power
			//
			Players.ResourcePlayer resourcePlayer = player.GetModPlayer<Players.ResourcePlayer>();
			resourcePlayer.ringPower += value;
			return null;
		}
		public object Call_BonusRingPowerMulti(object[] args)
		{
			if (!TryGetBonusArguments(args, out Player player, out float value))
			{
				return null;
			}

			// Get the resource player and multiply the ring power by the value
			//
			Players.ResourcePlayer resourcePlayer = player.GetModPlayer<Players.ResourcePlayer>();
			resourcePlayer.ringPower *= value;
			return null;
		}


		#endregion
	}
}
