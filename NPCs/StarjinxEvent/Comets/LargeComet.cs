using System.Collections.Generic;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class LargeComet : SmallComet
    {
		protected override string Size => "Large";
		protected override float BeamScale => 1.25f;
		protected override List<int>[] WaveTypes => new[] {
			new List<int>() { NPCID.Vampire, NPCID.WyvernHead, NPCID.Raven },
			new List<int>() { NPCID.ShadowFlameApparition },
			new List<int>() { NPCID.StardustCellBig, NPCID.SolarCorite, NPCID.NebulaBrain, NPCID.VortexRifleman } };
		protected override int[] WaveSizes => new[] { 6, 15, 4 };
		public override void SetStaticDefaults() => DisplayName.SetDefault("Large Starjinx Comet");

		public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 20;
            npc.width = 62;
            npc.height = 58;
        }
    }
}