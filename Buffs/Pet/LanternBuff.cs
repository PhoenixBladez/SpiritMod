using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class LanternBuff : BasePetBuff<Lantern>
	{
		protected override (string, string) BuffInfo => ("Lantern Power Battery", "'It illuminates the way!'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.lanternPet = true;
	}
}