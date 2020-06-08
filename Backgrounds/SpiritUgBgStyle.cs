using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Backgrounds
{
    public class SpiritUgBgStyle : ModUgBgStyle
    {
        public override bool ChooseBgStyle() => NPC.downedMechBossAny && Main.LocalPlayer.GetSpiritPlayer().ZoneSpirit;

        public override void FillTextureArray(int[] textureSlots) {
            textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG0");
            textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG1");
            textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG2");
            textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG3");
            textureSlots[4] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG5");
            textureSlots[5] = mod.GetBackgroundSlot("Backgrounds/SpiritBiomeUG4");
        }
    }
}