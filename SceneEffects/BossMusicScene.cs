using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class BossMusicScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SkeletronPrime");
		public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;
		public override bool IsSceneEffectActive(Player player) => NPC.AnyNPCs(NPCID.SkeletronPrime);
	}
}
