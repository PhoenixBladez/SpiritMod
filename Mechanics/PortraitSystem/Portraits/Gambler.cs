using Terraria.ModLoader;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	public class Gambler : BasePortrait
	{
		public override int ID => ModContent.NPCType<NPCs.Town.Gambler>();

		public Gambler() : base(null) { }
	}
}
