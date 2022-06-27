using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
	public class DrainedLunazoa : ModGore
	{
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.timeLeft = 60;
			gore.numFrames = 5;
			ChildSafety.SafeGore[gore.type] = true;
			gore.frame = (byte)Main.rand.Next(gore.numFrames);
		}
		public override bool Update(Gore gore)
		{
			gore.position += gore.velocity;
			gore.velocity = Vector2.Lerp(gore.velocity, -Vector2.UnitY, 0.03f);
			gore.frameCounter++;
			if (gore.frameCounter > 7) {
				gore.frameCounter = 0;
				gore.frame++;

				if (gore.frame >= gore.numFrames) 
					gore.frame = 0;
			}
			gore.rotation = gore.velocity.ToRotation() + MathHelper.PiOver2;
			gore.timeLeft = Math.Min(gore.timeLeft, 60); //setting gore timeleft to be lower in the onspawn hook doesnt actually work???
			gore.timeLeft--;
			if(gore.timeLeft < 30)
				gore.velocity = Vector2.Lerp(gore.velocity, -Vector2.UnitY, 0.03f);

			if (gore.timeLeft <= 0) {
				gore.alpha += 15;
				if (gore.alpha >= 255)
					gore.active = false;
			}

			return false;
		}
	}
}