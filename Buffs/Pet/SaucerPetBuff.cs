using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class SaucerPetBuff : BasePetBuff<SaucerPet>
	{
		protected override (string, string) BuffInfo => ("Support Saucer", "'It seems to only provide moral support...'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.saucerPet = true;
	}
}