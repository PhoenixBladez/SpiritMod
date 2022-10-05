using SpiritMod.Items.BossLoot.AtlasDrops.AtlasPet;

namespace SpiritMod.Buffs.Pet
{
	public class AtlasPetBuff : BasePetBuff<AtlasPetProjectile>
	{
		protected override (string, string) BuffInfo => ("Atlas Jr.", "He wants to lend a hand!");
	}
}