using SupernovaMod.Content.Npcs.Fallen;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.SceneEffects
{
	public class FallenMusicScene : ModMusicSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

		public override int NPCType => ModContent.NPCType<Fallen>();
		public override int? SupernovaMusic => Supernova.Instance.GetMusicFromMusicMod("Fallen");
		public override int VanillaMusic => MusicID.Boss3;
    }
}
