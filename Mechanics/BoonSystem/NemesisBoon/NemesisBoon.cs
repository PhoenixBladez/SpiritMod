using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.Mechanics.BoonSystem.NemesisBoon
{
	public class NemesisBoon : Boon
	{
		public override bool CanApply => true;

		private float counter;

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Blue.ToVector3() * 0.3f);
			counter += 0.025f;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture("SpiritMod/Mechanics/BoonSystem/NemesisBoon/NemesisBoon");

			DrawSigil(spriteBatch, tex, counter);
		}
	}
}