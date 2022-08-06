using SpiritMod.GlobalClasses.Players;
using SpiritMod.Items.Sets.ScarabeusDrops.ScarabPet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class ScarabPetBuff : BasePetBuff<ScarabPetProjectile>
	{
		protected override (string, string) BuffInfo => ("Tiny Scarab", "'It really loves to roll...'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.scarabPet = true;
	}
}