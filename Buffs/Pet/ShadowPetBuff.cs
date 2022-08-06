using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class ShadowPetBuff : BasePetBuff<ShadowPet>
	{
		protected override (string, string) BuffInfo => ("Shadow Pup", "'Awww'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.shadowPet = true;
	}
}