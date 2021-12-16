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

		bool initialized = false;

		private Projectile proj;

		public override string TexturePath => "SpiritMod/Mechanics/BoonSystem/NemesisBoon/NemesisBoon";

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Blue.ToVector3() * 0.3f);
			if (!initialized)
			{
				proj = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<NemesisBoonSword>(), NPCUtils.ToActualDamage(npc.damage), 5, 255);
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
			DrawBeam(new Color(134, 247, 245, 0), new Color(72, 165, 232, 0));

			DrawBloom(spriteBatch, new Color(76, 218, 237) * 0.33f, 0.5f);

			DrawSigil(spriteBatch);
		}
	}
}