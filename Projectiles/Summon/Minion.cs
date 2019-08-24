using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public abstract class Minion : ModProjectile
	{
		public override void AI()
		{
			CheckActive();
			Behavior();
			SelectFrame();
		}

		public abstract void CheckActive();

		public abstract void Behavior();

		public abstract void SelectFrame();
	}
}