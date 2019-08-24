using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public abstract class PhantomMinionINFO : Minion
	{
		protected float idleAccel = 0.05f;
		protected float spacingMult = 4f;
		protected float viewDist = 100f;       //minion view Distance
		protected float chaseDist = 25f;       //how far the minion can go
		protected float chaseAccel = 5;
		protected float inertia = 1f;

		public virtual void CreateDust()
		{

		}

		public override void SelectFrame()
		{

		}

		public override void Behavior()
		{

		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

	}
}