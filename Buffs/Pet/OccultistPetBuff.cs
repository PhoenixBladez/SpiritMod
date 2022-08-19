using SpiritMod.Items.BossLoot.OccultistDrops.OccultistPet;

namespace SpiritMod.Buffs.Pet
{
	public class OccultistPetBuff : BasePetBuff<OccultistPetProjectile>
	{
		protected override (string, string) BuffInfo => ("Lil' Occultist", "'Surprisingly mellow'");
	}
}