using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>Used to check if a player is currently in the starjinx event, and if so, set up visual effects.</summary>
	class StarjinxPlayer : ModPlayer
	{
		public bool zoneStarjinxEvent = false;
		public Vector2 StarjinxPosition;
		public override void ResetEffects() => zoneStarjinxEvent = false;

		public override void UpdateBiomeVisuals()
		{
			if (zoneStarjinxEvent)
			{
				player.AddBuff(ModContent.BuffType<HighGravityBuff>(), 2);
			}

			player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxSky", zoneStarjinxEvent);
			SpiritMod.starjinxBorderEffect.Parameters["Radius"].SetValue(1500);
			SpiritMod.starjinxBorderEffect.Parameters["NoiseTexture"].SetValue(mod.GetTexture("Textures/Trails/Trail_2"));
			SpiritMod.starjinxBorderShader.UseColor(new Color(255, 166, 252).ToVector3());
			player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxBorder", zoneStarjinxEvent, StarjinxPosition);
			player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxBorderFade", zoneStarjinxEvent, StarjinxPosition);
		}
	}
}
