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
		protected virtual int OtherType => ModContent.ProjectileType<LeftHopper>();
		ref float Timer => ref projectile.ai[0];
		bool FoundOther => projectile.ai[1] >= 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Right Gate");

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8000;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (!FoundOther)
			{
				float maxdist = 300;
				foreach (Projectile proj in Main.projectile.Where(x => x.active && x.owner == projectile.owner && x.type == OtherType && x.Distance(projectile.Center) < maxdist))
				{
					projectile.ai[1] = proj.whoAmI;
					proj.ai[1] = projectile.whoAmI;
					Main.PlaySound(SoundID.Item93, projectile.position);
				}
				return;
			}

			Projectile other = Main.projectile[(int)projectile.ai[1]];

			if (other.active)
			{
				//rotating
				Vector2 direction9 = other.Center - projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				other.ai[1] = projectile.whoAmI;
				//shoot to other guy
				Timer++;
				if (Timer % 30 == 0)
					Main.PlaySound(SoundID.Item15, projectile.Center);

				if (Timer > 4)
				{
					Timer = 0;
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction9.X * 15, direction9.Y * 15, ModContent.ProjectileType<GateLaser>(), 14, 0, Main.myPlayer);
					Main.projectile[proj].timeLeft = (distance / 15) - 1;
					DustHelper.DrawElectricity(projectile.Center, other.Center, 226, 0.3f);
				}
			}
			else
				projectile.ai[1] = -1;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (FoundOther)
			{
				Texture2D glowMask = Main.glowMaskTexture[239];
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
				Rectangle r2 = glowMask.Frame(1, 1, 0, 0);
				Vector2 drawOrigin = r2.Size() / 2f;
				Vector2 position3 = (projectile.Bottom - Main.screenPosition) + new Vector2(3f, -6f);
				Color color3 = new Color(84, 207, 255) * 1.6f;
				Main.spriteBatch.Draw(glowMask, position3, r2, color3, projectile.rotation, drawOrigin, projectile.scale * 0.33f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(glowMask, position3, r2, color3 * num12, projectile.rotation, drawOrigin, projectile.scale * 0.33f * num15, SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(glowMask, position3, r2, color3 * num14, projectile.rotation, drawOrigin, projectile.scale * 0.33f * num16, SpriteEffects.FlipHorizontally, 0.0f);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (!FoundOther)
				return;
			Projectile other = Main.projectile[(int)projectile.ai[1]];
			if (other.active)
				other.ai[1] = -1;
		}
	}
}
