using SupernovaMod.Common.Systems;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.GlobalItems
{
	public class GlobalItemMutator : GlobalItem
	{
		public override void SetDefaults(Item entity)
		{
			base.SetDefaults(entity);

			UpdateThrowingClass(entity);
			if (Supernova.Instance.HasModCalamity)
			{
				UpdateCalamityIntergration(entity);
			}
		}

		private void UpdateCalamityIntergration(Item entity)
		{
			try
			{
				if (entity.DamageType == DamageClass.Throwing)
				{
					entity.DamageType = ModLoader.GetMod("CalamityMod").Find<DamageClass>("RogueDamageClass");
				}
			}
			catch (Exception ex)
			{
				Supernova.Instance.Logger.Error("UpdateCalamityIntergration("+entity.Name+"): " + ex.GetType().Name + ' ' + ex.Message);
			}
		}

		/// <summary>
		/// Updates a item to use a none throwing class class, since this class has been deprecated / removed.
		/// </summary>
		/// <param name="entity"></param>
		private void UpdateThrowingClass(Item entity)
		{
			// Ignore none throwing class items
			//
			if (entity.DamageType != DamageClass.Throwing)
			{
				return;
			}

			// Ignore none SupernovaMod items
			//
			if (entity.ModItem.Mod.Name != Mod.Name)
			{
				return;
			}

			// Check if the thorium mod or Calamity mod is used.
			// If so their respective thrower class should be used.
			//
			if (Supernova.Instance.HasModCalamity || Supernova.Instance.HasModThorium)
			{
				return;
			}

			// Convert any consumable thrown item to ranged class
			//
			if (entity.consumable)
			{
				entity.DamageType = DamageClass.Ranged;
			}
			// Convert any none consumable thrown item to melee class,
			// since these are most likely boomerangs.
			//
			else
			{
				entity.DamageType = DamageClass.Melee;
			}
		}
	}
}
