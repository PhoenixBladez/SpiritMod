using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class HarpyPetBuff : BasePetBuff<HarpyPet>
	{
		protected override (string, string) BuffInfo => ("Waning Gibbous", "The Moonlit Faerie will protect you");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.harpyPet = true;
	}
}