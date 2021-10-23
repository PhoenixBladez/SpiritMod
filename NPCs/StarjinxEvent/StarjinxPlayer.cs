using SpiritMod.Buffs;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>Used to check if a player is currently in the starjinx event, and if so, set up visual effects.</summary>
	class StarjinxPlayer : ModPlayer
	{
		public bool zoneStarjinxEvent = false;

		public override void ResetEffects() => zoneStarjinxEvent = false;

		public override void UpdateBiomeVisuals()
		{
			if (zoneStarjinxEvent)
				player.AddBuff(ModContent.BuffType<HighGravityBuff>(), 2);

			player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxSky", zoneStarjinxEvent);
		}
	}
}
