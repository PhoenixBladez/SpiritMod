using Terraria.ModLoader;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	class Bandit : BasePortrait
	{
		public override int ID => ModContent.NPCType<NPCs.Town.Rogue>(); 

		public Bandit() : base(null) { }
	}
}
