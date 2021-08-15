using System.Collections.Generic;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class MediumComet : SmallComet
    {
		protected override string Size => "Medium";
		protected override float BeamScale => 1f;
		protected override List<int>[] WaveTypes => new[] {
			new List<int>() { NPCID.Corruptor, NPCID.Vulture, NPCID.Raven, NPCID.Slimer, NPCID.GiantBat },
			new List<int>() { NPCID.WyvernHead } };
		protected override int[] WaveSizes => new[] { 6, 1 };

		public override void SetStaticDefaults() => DisplayName.SetDefault("Medium Starjinx Comet");

		public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 15;
            npc.width = 42;
            npc.height = 40;
        }
    }
}