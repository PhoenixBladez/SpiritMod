using Terraria.ModLoader;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	class RuneWizard : BasePortrait
	{
		public override int ID => ModContent.NPCType<NPCs.Town.RuneWizard>(); 

		public RuneWizard() : base(null) { }
	}
}
