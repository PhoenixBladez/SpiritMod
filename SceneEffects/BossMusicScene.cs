using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class BossMusicScene : ModSceneEffect
	{
		public override int Music => GetBossMusic();
		public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

		private int GetBossMusic()
		{
			var config = ModContent.GetInstance<SpiritMusicConfig>();

			if (NPC.AnyNPCs(NPCID.SkeletronPrime) && config.SkeletronPrimeMusic)
				return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SkeletronPrime");
		}

		public override bool IsSceneEffectActive(Player player)
		{
			return NPC.AnyNPCs(NPCID.SkeletronPrime);
		}
	}
}
