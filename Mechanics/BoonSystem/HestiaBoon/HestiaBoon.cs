using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.Mechanics.BoonSystem.HestiaBoon
{
	public class HestiaBoon : Boon
	{
		public override bool CanApply => true;
		public override string TexturePath => "SpiritMod/Mechanics/BoonSystem/HestiaBoon/HestiaBoon";

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Orange.ToVector3() * 0.3f);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawBeam(new Color(247, 195, 92, 0), new Color(247, 117, 42, 0));

			DrawBloom(spriteBatch, new Color(247, 195, 92) * 0.5f, 0.5f);

			DrawSigil(spriteBatch);
		}
	}
}