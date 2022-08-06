using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class PhantomPetBuff : BasePetBuff<PhantomPet>
	{
		protected override (string, string) BuffInfo => ("Phantom", "'It blends into the night'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.phantomPet = true;
	}
}