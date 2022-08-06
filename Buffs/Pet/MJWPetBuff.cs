using Microsoft.Xna.Framework;
using SpiritMod.GlobalClasses.Players;
using SpiritMod.Items.Sets.MoonWizardDrops.MJWPet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class MJWPetBuff : BasePetBuff<MJWPetProjectile>
	{
		protected override (string, string) BuffInfo => ("Moon Jelly Lightbulb", "'No installation necessary!'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.mjwPet = true;
	}
}