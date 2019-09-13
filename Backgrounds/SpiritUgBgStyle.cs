using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Backgrounds
{
	public class SpiritUgBgStyle : ModUgBgStyle
	{
        public override bool ChooseBgStyle() => NPC.downedMechBossAny && Main.LocalPlayer.GetSpiritPlayer().ZoneSpirit;

        public override void FillTextureArray(int[] textureSlots)
		{
            for (int i = 0; i <= 3; i++)
            {
                textureSlots[i] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG" + i.ToString());
            }
		}
	}
}