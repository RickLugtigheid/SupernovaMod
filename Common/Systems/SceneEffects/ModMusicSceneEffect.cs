using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.SceneEffects
{
	public abstract class ModMusicSceneEffect : ModSceneEffect
	{
		/// <summary>
		/// The NPC type this scene effect must be used for
		/// </summary>
		public abstract int NPCType { get; }

		/// <summary>
		/// The Supernova music to use.
		/// </summary>
		/// <remarks>When null <see cref="VanillaMusic"/> is used instead (so when the Supernova music mod is not installed / loaded)</remarks>
		public abstract int? SupernovaMusic { get; }
		/// <summary>
		/// The Vanilla music to use when the Supernova music mod is not installed / loaded.
		/// </summary>
		public abstract int VanillaMusic { get; }

		public virtual int MusicDistance => 5000;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual bool PreSetSceneEffect() => true;
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual int GetMusic()
		{
			if (SupernovaMusic == null)
			{
				return VanillaMusic;
			}
			return SupernovaMusic.Value;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual bool SetSceneEffect()
		{
			if (!PreSetSceneEffect())
			{
				return false;
			}
			if (SupernovaMusic == null && VanillaMusic == -1)
			{
				return false;
			}
			Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
			int musicDist = MusicDistance * 2;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (!npc.active)
				{
					continue;
				}

				bool inList = false;
				if (npc.type == NPCType)
				{
					inList = true;
				}

				if (inList)
				{
					Rectangle npcRect = new Rectangle((int)npc.Center.X - this.MusicDistance, (int)npc.Center.Y - this.MusicDistance, musicDist, musicDist);
					if (screenRect.Intersects(npcRect))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override int Music => GetMusic();
		public override bool IsSceneEffectActive(Player player) => SetSceneEffect();
	}
}
