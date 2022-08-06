using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class SwordPetBuff : BasePetBuff<SwordPet>
	{
		protected override (string, string) BuffInfo => ("Possessed Blade", "'Is this a dagger I see in front of me?'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.swordPet = true;
	}
}