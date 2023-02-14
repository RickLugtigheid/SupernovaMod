using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Supernova.Common.Systems;
//using Supernova.Content.Global.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Supernova
{
	public class Supernova : Mod
	{
		public static ILog Log { get; private set; }
		public override void Load()
		{
			Log = Logger;
		}

		public override void PostSetupContent()
		{
			// Check if the user has downloaded the bossChecklist mod
			//
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				bossChecklist.Call("AddBoss", 1.8f,
					new List<int> { ModContent.NPCType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingerOfAnnihilation>() },
					this,
					"Harbinger of Annihilation",
					() => SupernovaBosses.downedHarbingerOfAnnihilation,
					0, //ModContent.NPCType<Npcs.PreHardmode.CosmicAnomaly>(),
					new List<int> {
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					new List<int> {
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					"Kill a Cosmic Anomaly in Space"
				);

				bossChecklist.Call("AddBoss", 3.7f,
					new List<int> { ModContent.NPCType<Content.PreHardmode.Bosses.FlyingTerror.FlyingTerror>() },
					this,
					"Flying Terror",
					() => SupernovaBosses.downedFlyingTerror,
					ModContent.ItemType<Content.PreHardmode.Items.Misc.HorridChunk>(),
					new List<int> {
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.TerrorTuft>()
					},
					new List<int> {
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.TerrorInABottle>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.BlunderBuss>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.TerrorCleaver>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.TerrorKnife>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.TerrorRecurve>(),
						ModContent.ItemType<Content.PreHardmode.Bosses.FlyingTerror.TerrorTome>(),
					},
					"Use a [i:" + ModContent.ItemType<Content.PreHardmode.Items.Misc.HorridChunk>() + "] at night"
				);
			}


			base.PostSetupContent();
		}
	}
}