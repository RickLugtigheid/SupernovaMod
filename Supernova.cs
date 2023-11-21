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

		public override object Call(params object[] args) => _calls.Call(args);

		public override void Load()
		{
			Instance = this;
			Log = Logger;

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

		#region Statics
		public static string GetEffectPath(string effectName) => $"SupernovaMod/Assets/Effects/{effectName}";
		public static string GetTexturePath(string textureName) => $"SupernovaMod/Assets/Textures/{textureName}";
		#endregion
	}
}