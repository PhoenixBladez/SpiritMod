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

		bool initialized = false;

		private Projectile proj;

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Blue.ToVector3() * 0.3f);
			counter += 0.025f;
			if (!initialized)
			{
				proj = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<NemesisBoonSword>(), Main.expertMode ? (int)(npc.damage / 4) : npc.damage, 5, 255);
				proj.ai[0] = npc.whoAmI;
				initialized = true;
			}
		}

		public override void OnDeath()
		{
			if (proj != null && proj.active)
				proj.ai[1] = 1;

			DropOlympium(Main.rand.Next(3,6));
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture("SpiritMod/Mechanics/BoonSystem/NemesisBoon/NemesisBoon");

			DrawSigil(spriteBatch, tex, counter);
		}
	}
}