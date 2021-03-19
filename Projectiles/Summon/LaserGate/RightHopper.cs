using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class RightHopper : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;
		ref float Timer => ref projectile.ai[0];
		bool Foundother => projectile.ai[1] >= 0;
		public int OtherType;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Right Gate");
		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			OtherType = ModContent.ProjectileType<LeftHopper>();
		}
		public override void AI()
		{
			if (!Foundother) {
				float maxdist = 300;
				foreach (Projectile proj in Main.projectile.Where(x => x.active && x.owner == projectile.owner && x.type == OtherType && x.Distance(projectile.Center) < maxdist)) {
					projectile.ai[1] = proj.whoAmI;
					proj.ai[1] = projectile.whoAmI;
					Main.PlaySound(SoundID.Item93, projectile.position);
				}
				return;
			}
			Projectile other = Main.projectile[(int)projectile.ai[1]];
			if (other.active) {
				//rotating
				direction9 = other.Center - projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				other.ai[1] = projectile.whoAmI;
				//shoot to other guy
				Timer++;
				if (Timer % 30 == 0)
					Main.PlaySound(SoundID.Item15, projectile.Center);

				if (Timer > 4) {
					Timer = 0;
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 15, (float)direction9.Y * 15, ModContent.ProjectileType<GateLaser>(), 14, 0, Main.myPlayer);
					Main.projectile[proj].timeLeft = (int)(distance / 15) - 1;
					DustHelper.DrawElectricity(projectile.Center, other.Center, 226, 0.3f);
				}
			}
			else
				projectile.ai[1] = -1;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (Foundother)
			{
				Color color1 = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				int r1 = color1.R;
				drawOrigin.Y += 34f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;
				Vector2 position1 = projectile.Bottom - Main.screenPosition;
				Texture2D texture2D2 = Main.glowMaskTexture[239];
				float num11 = (float)(Main.GlobalTime % 1.0 / 1.0);
				float num12 = num11;
				if (num12 > 0.5)
					num12 = 1f - num11;
				if (num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)((num11 + 0.5) % 1.0);
				float num14 = num13;
				if (num14 > 0.5)
					num14 = 1f - num13;
				if (num14 < 0.0)
					num14 = 0.0f;
				Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(3f, -6f);
				Color color3 = new Color(84, 207, 255) * 1.6f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, projectile.rotation, drawOrigin, projectile.scale * 0.33f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, projectile.rotation, drawOrigin, projectile.scale * 0.33f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, projectile.rotation, drawOrigin, projectile.scale * 0.33f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				Texture2D texture2D3 = Main.extraTexture[89];
				Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
				drawOrigin = r3.Size() / 2f;
		
			}
		}

		public override void Kill(int timeLeft)
		{
			if (!Foundother)
				return;
			Projectile other = Main.projectile[(int)projectile.ai[1]];
			if (other.active) 
				other.ai[1] = -1;
		}
	}
}
