using SpiritMod.GlobalClasses.Players;
using SpiritMod.Items.BossLoot.MoonWizardDrops.MJWPet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class MJWPetBuff : BasePetBuff<MJWPetProjectile>
	{
		protected override (string, string) BuffInfo => ("Moon Jelly Lightbulb", "'No installation necessary!'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.mjwPet = true;
		protected override bool IsLightPet => true;
	}
}