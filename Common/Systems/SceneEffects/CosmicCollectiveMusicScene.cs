using SupernovaMod.Content.Npcs.CosmicCollective;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.SceneEffects
{
	public class CosmicCollectiveMusicScene : ModMusicSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

		public override int NPCType => ModContent.NPCType<CosmicCollective>();
		public override int? SupernovaMusic => Supernova.Instance.GetMusicFromMusicMod("CosmicCollective");
		public override int VanillaMusic => MusicID.Boss4;
    }
}
