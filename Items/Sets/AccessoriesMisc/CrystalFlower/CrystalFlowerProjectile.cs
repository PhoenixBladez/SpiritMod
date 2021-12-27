using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Utilities;
using SpiritMod.Projectiles;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.AccessoriesMisc.CrystalFlower
{
	public class CrystalFlowerProjectile : ModProjectile, IDrawAdditive, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			DisplayName.SetDefault("Crystal Flower");
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.tileCollide = true;
			projectile.timeLeft = 150;
		}
		int numBounce = 3;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			numBounce--;
			if (numBounce <= 0)
				projectile.Kill();
			else
			{
				projectile.Bounce(oldVelocity, .75f);
			}
			return false;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.velocity *= .98f;
			projectile.alpha++;
			if (projectile.alpha > 150)
            {
				projectile.Kill();
            }
		}
		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new GradientTrail(Color.Cyan, Color.Cyan * .5f), new RoundCap(), new DefaultTrailPosition(), 6f, 35f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));

		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 3f;
			for (int num257 = 0; num257 < 12; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 2f;
			}
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(255, 255, 255) * 0.45f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale * 1.4f;

				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.oldRot[k], Main.projectileTexture[projectile.type].Size() / 2, scale, default, default);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(Color.White * (float)(.6-(projectile.alpha/255))) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}