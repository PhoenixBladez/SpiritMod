using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class CaptiveMaskPetBuff : BasePetBuff<CaptiveMaskPet>
	{
		protected override (string, string) BuffInfo => ("Unbound Mask", "'Once more unto the breach!'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.maskPet = true;
	}
}