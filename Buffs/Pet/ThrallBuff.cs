using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class ThrallBuff : BasePetBuff<ThrallPet>
	{
		protected override (string, string) BuffInfo => ("Lil' Leonard", "'Grrr...'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.thrallPet = true;
	}
}