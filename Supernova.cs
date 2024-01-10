using log4net;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SupernovaMod.Common.Systems;
using SupernovaMod.Api.Effects;

namespace SupernovaMod
{
    public sealed partial class Supernova : Mod
	{
		public static Supernova Instance { get ; private set; }

		public static Effect ShaderShockwave { get; private set; }

		public static ILog Log { get; private set; }

		public static bool DebugMode => false;//ModContent.GetInstance<Common.Configs.SupernovaModConfig>().debugMode;

		private SupernovaModCalls _calls = new SupernovaModCalls();

		#region Other Mods

		public Mod supernovaMusic;
		public Mod calamity;
		public Mod thorium;
		public Mod bossChecklist;

		public bool HasModSupernovaMusic => supernovaMusic != null;
		public bool HasModCalamity => calamity != null;
		public bool HasModThorium => thorium != null;
		public bool HasModBossChecklist => bossChecklist != null;

		#endregion

		public override object Call(params object[] args) => _calls.Call(args);

		public override void Load()
		{
			Instance = this;
			Log = Logger;

			// Set our global defaults
			//
			GlobalModifiers.SetStaticDefaults();

			// Try load other mods
			//
			supernovaMusic = null;
			ModLoader.TryGetMod("SupernovaMusic", out supernovaMusic);
			calamity = null;
			ModLoader.TryGetMod("CalamityMod", out calamity);
			ModIntegrationsSystem.HandleIntegrationRogueCalamity();
			thorium = null;
			ModLoader.TryGetMod("ThoriumMod", out calamity);
			bossChecklist = null;
			ModLoader.TryGetMod("BossChecklist", out bossChecklist);

			//
			if (Main.netMode != NetmodeID.Server)
			{
				LoadShaders();
			}
			if (!Main.dedServ)
			{
				ParticleSystem.Load();
			}
		}

		public override void Unload()
		{
			// Unload other mod variables
			//
			supernovaMusic = null;
			calamity = null;
			thorium = null;
			bossChecklist = null;

			UnloadShaders();
			ParticleSystem.Unload();
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

		/// <summary>
		/// Gets a song from the Supernova Music mod
		/// </summary>
		/// <param name="songFile"></param>
		/// <returns></returns>
		public int? GetMusicFromMusicMod(string songFile)
		{
			if (!HasModSupernovaMusic)
			{
				return null;
			}
			return new int?(MusicLoader.GetMusicSlot(supernovaMusic, "Assets/Music/" + songFile));
		}

		#region Statics
		public static string GetEffectPath(string effectName) => $"SupernovaMod/Assets/Effects/{effectName}";
		public static string GetTexturePath(string textureName) => $"SupernovaMod/Assets/Textures/{textureName}";
		#endregion
	}
}