using SpiritMod.Items.BossLoot.DuskingDrops.DuskingPet;

namespace SpiritMod.Buffs.Pet
{
	public class DuskingPetBuff : BasePetBuff<DuskingPetProjectile>
	{
		protected override (string, string) BuffInfo => ("Minor Shadowflame", "'You see something inside of the flames...'");
		protected override bool IsLightPet => true;
	}
}