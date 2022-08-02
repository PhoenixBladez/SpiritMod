using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Players
{
	internal class PetPlayer : ModPlayer
	{
		public bool starPet = false;
		public bool saucerPet = false;
		public bool bookPet = false;
		public bool swordPet = false;
		public bool shadowPet = false;
		public bool starachnidPet = false;
		public bool thrallPet = false;
		public bool jellyfishPet = false;
		public bool phantomPet = false;
		public bool lanternPet = false;
		public bool maskPet = false;
		public bool harpyPet = false;
		public bool cultFishPet = false;
		public bool briarSlimePet = false;
		public bool scarabPet = false;
		public bool vinewrathPet = false;
		public bool mjwPet = false;

		public override void ResetEffects()
		{
			starPet = false;
			saucerPet = false;
			bookPet = false;
			swordPet = false;
			shadowPet = false;
			starachnidPet = false;
			thrallPet = false;
			jellyfishPet = false;
			phantomPet = false;
			lanternPet = false;
			maskPet = false;
			harpyPet = false;
			scarabPet = false;
			mjwPet = false;
		}
	}
}
