using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;

namespace SpiritMod.Buffs.Pet
{
	public class JellyfishBuff : BasePetBuff<JellyfishPet>
	{
		protected override (string, string) BuffInfo => ("Boop", "'The Jellyfish is helping you relax'");
		public override void SetPetFlag(Player player, PetPlayer petPlayer) => petPlayer.jellyfishPet = true;
	}
}