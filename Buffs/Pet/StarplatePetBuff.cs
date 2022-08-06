using SpiritMod.GlobalClasses.Players;
using SpiritMod.Items.Sets.StarplateDrops.StarplatePet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class StarplatePetBuff : BasePetBuff<StarplatePetProjectile>
	{
		protected override (string, string) BuffInfo => ("Starplate Miniature", "Looking for something...");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.starplatePet = true;
	}
}