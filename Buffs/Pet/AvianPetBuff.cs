using SpiritMod.Items.BossLoot.AvianDrops.AvianPet;

namespace SpiritMod.Buffs.Pet
{
	public class AvianPetBuff : BasePetBuff<AvianPet>
	{
		protected override (string, string) BuffInfo => ("Ancient Hatchling", "eg");
	}
}