namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class MediumComet : SmallComet
    {
		protected override string Size => "Medium";
		protected override float BeamScale => 1f;
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