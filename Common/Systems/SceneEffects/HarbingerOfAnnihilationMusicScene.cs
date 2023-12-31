using SupernovaMod.Content.Npcs.HarbingerOfAnnihilation;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.SceneEffects
{
	public class HarbingerOfAnnihilationMusicScene : ModMusicSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

		public override int NPCType => ModContent.NPCType<HarbingerOfAnnihilation>();
		public override int? SupernovaMusic => Supernova.Instance.GetMusicFromMusicMod("HarbingerOfAnnihilation");
		public override int VanillaMusic => MusicID.Boss4;
	}
}
