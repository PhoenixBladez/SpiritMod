using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
	public class StarjinxGore : ModGore
	{
		public override void OnSpawn(Gore gore) {
			gore.numFrames = 3;
			gore.frame = (byte)Main.rand.Next(3);
			gore.timeLeft = 10;
			gore.position -= new Vector2(11, 11);
			ChildSafety.SafeGore[gore.type] = true;
		}

		public override Color? GetAlpha(Gore gore, Color lightColor)
		{
			Color color = SpiritMod.StarjinxColor(Main.GlobalTime * 0.8f);
			color.A = (byte)gore.alpha;
			float lerpamount = gore.alpha / 250f;
			return Color.Lerp(color, Color.Transparent, lerpamount);
		}

		public override bool Update(Gore gore)
		{
			gore.light = 0.5f;
			gore.position += gore.velocity;
			gore.rotation += 0.2f;
			gore.alpha += 6;

			if (gore.alpha > 250)
				gore.active = false;
			return false;
		}
	}
}