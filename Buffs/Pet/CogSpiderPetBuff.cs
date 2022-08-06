using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class CogSpiderPetBuff : BasePetBuff<CogSpiderPet>
	{
		protected override (string, string) BuffInfo => ("Star Spider", "'The stars will give you solace'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.starPet = true;
	}
}