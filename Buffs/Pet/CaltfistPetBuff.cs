using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class CaltfistPetBuff : BasePetBuff<Caltfist>
	{
		protected override (string, string) BuffInfo => ("Cultfish", "This little bugger lights the way!");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.cultFishPet = true;
	}
}