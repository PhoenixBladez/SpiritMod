using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	public class SpiritUgBgStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
		{
			for (int i = 0; i < 4; ++i)
				textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/SpiritBiomeUG" + i);
			textureSlots[4] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/SpiritBiomeUG5");
			textureSlots[5] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/SpiritBiomeUG4");
		}
	}
}