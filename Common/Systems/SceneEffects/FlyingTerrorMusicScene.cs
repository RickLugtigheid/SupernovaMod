using SupernovaMod.Content.Npcs.FlyingTerror;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.SceneEffects
{
	public class FlyingTerrorMusicScene : ModMusicSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

		public override int NPCType => ModContent.NPCType<FlyingTerror>();
		public override int? SupernovaMusic => Supernova.Instance.GetMusicFromMusicMod("FlyingTerror");
		public override int VanillaMusic => MusicID.Boss3;
	}
}
