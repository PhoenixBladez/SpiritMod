using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class SwarmTelegraph : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.hide = true;
			Projectile.timeLeft = 360;
			Projectile.tileCollide = false;
			Projectile.scale = 0.75f;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 3;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 236, 115, 200)), new RoundCap(), new DefaultTrailPosition(), 200f, 800f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1f));

		public override void AI()
		{
			//follow the exact same path as the scarab line
			if (++Projectile.ai[1] > 110) {
				Projectile.alpha += 7;
				if (Projectile.alpha > 255) {
					Projectile.Kill();
				}
			}
			else
				Projectile.alpha = Math.Max(Projectile.alpha - 7, 0);

			if (Projectile.velocity.Length() < 26)
				Projectile.velocity *= 1.02f;
		}
	}
}