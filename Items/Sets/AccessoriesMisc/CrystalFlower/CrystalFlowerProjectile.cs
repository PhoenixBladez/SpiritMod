using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Projectiles;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.AccessoriesMisc.CrystalFlower
{
	public class CrystalFlowerProjectile : ModProjectile, IDrawAdditive, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			DisplayName.SetDefault("Crystal Flower");
		}
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 150;
		}
		int numBounce = 3;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			numBounce--;
			if (numBounce <= 0)
				Projectile.Kill();
			else
			{
				Projectile.Bounce(oldVelocity, .75f);
			}
			return false;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity *= .98f;
			Projectile.alpha++;
			if (Projectile.alpha > 150)
            {
				Projectile.Kill();
            }
		}
		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new GradientTrail(Color.Cyan, Color.Cyan * .5f), new RoundCap(), new DefaultTrailPosition(), 6f, 35f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));

		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 3f;
			for (int num257 = 0; num257 < 12; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 2f;
			}
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(255, 255, 255) * 0.45f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				float scale = Projectile.scale * 1.4f;

				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.oldRot[k], TextureAssets.Projectile[Projectile.type].Value.Size() / 2, scale, default, default);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(Color.White * (float)(.6-(Projectile.alpha/255))) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}