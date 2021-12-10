using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.NPCs;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoon : Boon
	{
		public override bool CanApply => true;

		private float counter;

		private float projectileCounter;

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Violet.ToVector3() * 0.3f);
			counter += 0.025f;
			if (projectileCounter % 300 == 0)
			{
				for (int i = 0; i < 3; i++)
					Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<HecateBoonRune>(), 0, 0, npc.target, npc.whoAmI, i); 
			}
			projectileCounter++;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture("SpiritMod/Mechanics/BoonSystem/HecateBoon/HecateBoon");

			DrawSigil(spriteBatch, tex, counter);
		}
	}
}