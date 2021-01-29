using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
	public class SandBall : ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			gore.timeLeft = 20;
			ChildSafety.SafeGore[gore.type] = true;
		}
		public override bool Update(Gore gore)
		{
			gore.velocity = Vector2.Lerp(gore.velocity, Vector2.UnitY * 6, 0.06f);
			gore.position += gore.velocity;
			gore.rotation += 0.1f * Math.Sign(gore.velocity.X);
			gore.timeLeft = Math.Min(gore.timeLeft, 20); //setting gore timeleft to be lower in the onspawn hook doesnt actually work???
			gore.timeLeft--;
			if (gore.timeLeft <= 0) {
				gore.alpha += 15;
				if (gore.alpha >= 255)
					gore.active = false;
			}

			return false;
		}
	}
}