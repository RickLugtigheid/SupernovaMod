using log4net;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod
{
    public class Supernova : Mod
	{
		public static Effect ShaderShockwave { get; private set; }

		public static ILog Log { get; private set; }

		public static bool DebugMode => ModContent.GetInstance<Common.Configs.SupernovaModConfig>().debugMode;

		public override void Load()
		{
			Log = Logger;

			if (Main.netMode != NetmodeID.Server)
			{
				LoadShaders();
			}
		}

		public override void Unload()
		{
			UnloadShaders();
		}

		private void LoadShaders()
		{
			ShaderShockwave = ModContent.Request<Effect>(GetEffectPath("ShockwaveEffect"), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(new Ref<Effect>(ShaderShockwave), "Shockwave"), EffectPriority.VeryHigh);
			Filters.Scene["Shockwave"].Load();
		}
		private void UnloadShaders()
		{
			ShaderShockwave = null;
		}

		public override void PostSetupContent()
		{
			// Check if the user has downloaded the bossChecklist mod
			//
			/*if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				bossChecklist.Call("AddBoss", 1.8f,
					new List<int> { ModContent.NPCType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingerOfAnnihilation>() },
					this,
					"Harbinger of Annihilation",
					() => SupernovaBosses.downedHarbingerOfAnnihilation,
					0, //ModContent.NPCType<Npcs.PreHardmode.CosmicAnomaly>(),
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					"Kill a Cosmic Anomaly in Space"
				);

				bossChecklist.Call("AddBoss", 3.7f,
					new List<int> { ModContent.NPCType<Content.Npcs.Bosses.FlyingTerror.FlyingTerror>() },
					this,
					"Flying Terror",
					() => SupernovaBosses.downedFlyingTerror,
					ModContent.ItemType<Content.Items.Misc.HorridChunk>(),
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorTuft>()
					},
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorInABottle>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.BlunderBuss>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorCleaver>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorKnife>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorRecurve>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorTome>(),
					},
					"Use a [i:" + ModContent.ItemType<Content.Items.Misc.HorridChunk>() + "] at night"
				);
			}*/


			base.PostSetupContent();
		}

		#region Statics
		public static string GetEffectPath(string effectName) => $"SupernovaMod/Assets/Effects/{effectName}";
		public static string GetTexturePath(string textureName) => $"SupernovaMod/Assets/Textures/{textureName}";
		#endregion
	}
}