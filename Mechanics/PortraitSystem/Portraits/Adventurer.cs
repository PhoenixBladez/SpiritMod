using Terraria.ModLoader;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	class Adventurer : BasePortrait
	{
		public override int ID => ModContent.NPCType<NPCs.Town.Adventurer>(); 

		public Adventurer() : base(null) { }
	}
}
