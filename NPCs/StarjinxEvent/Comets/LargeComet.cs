namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class LargeComet : SmallComet
    {
		protected override string Size => "Large";
		protected override float BeamScale => 1.25f;
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