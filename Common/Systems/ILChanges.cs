using SupernovaMod.Api.Effects;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public class ILChanges : ModSystem
	{
		private static class Hooks
		{
			public static On_Main.hook_DrawInfernoRings _DrawForegroundParticles;
		}

		public override void OnModLoad()
		{
			// Register hook '_DrawForegroundParticles' as a 'On_Main.hook_DrawInfernoRings' hook
			//
			On_Main.hook_DrawInfernoRings hook_DrawInfernoRings;
			if ((hook_DrawInfernoRings = Hooks._DrawForegroundParticles) == null)
			{
				hook_DrawInfernoRings = (Hooks._DrawForegroundParticles = new On_Main.hook_DrawInfernoRings(ILChanges.DrawForegroundParticles));
			}
			On_Main.DrawInfernoRings += hook_DrawInfernoRings;
		}

		private static void DrawForegroundParticles(On_Main.orig_DrawInfernoRings origin, Main self)
		{
			ParticleSystem.DrawParticles(Main.spriteBatch);
			origin.Invoke(self);
		}

		public override void PostUpdateEverything()
		{
			if (!Main.dedServ)
			{
				//ParticleSystem.RunRandomSpawnAttempts();
				ParticleSystem.UpdateParticles();
			}
		}
	}
}
