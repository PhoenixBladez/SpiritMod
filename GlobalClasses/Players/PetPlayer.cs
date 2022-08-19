using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Players
{
	public class PetPlayer : ModPlayer
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
		public bool starplatePet = false;

		public Dictionary<int, bool> pets = new();

		public override void ResetEffects()
		{
			foreach (int item in pets.Keys)
				pets[item] = false;
		}

		public void PetFlag(Projectile projectile)
		{
			var modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();

			if (!modPlayer.pets.ContainsKey(projectile.type))
				modPlayer.pets.Add(projectile.type, true);

			if (Player.dead)
				modPlayer.pets[projectile.type] = false;

			if (modPlayer.pets[projectile.type])
				projectile.timeLeft = 2;
		}
	}
}
