using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class HauntedBookPetBuff : BasePetBuff<HauntedBookPet>
	{
		protected override (string, string) BuffInfo => ("Haunted Tome", "'Haunted, yet comforting'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.bookPet = true;
	}
}