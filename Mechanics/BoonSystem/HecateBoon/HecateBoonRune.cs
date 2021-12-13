using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoonRune : ModProjectile
	{
		const float ACCELERATION = 0.0005f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Hecate");
			Main.projFrames[projectile.type] = 8;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		bool initialized = false;

		float rotation;

		float speed = 0.03f;

		float radius = 100;

		NPC parent => Main.npc[(int)projectile.ai[0]];


		public override void SetDefaults()
		{
			projectile.width = projectile.height = 18;
			projectile.penetrate = -1;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.frame = Main.rand.Next(6);
		}
		public override void AI()
		{
			if (!parent.active || parent.life <= 0)
				projectile.active = false;
			if (!initialized)
				rotation = projectile.ai[1] * 2.09f;
			initialized = true;
			rotation += speed;
			if (speed > 0.15f)
			{
				radius -= 1;
				if (radius < 20)
				{
					if (Main.player[parent.target] == null)
					{
						projectile.active = false;
						return;
					}
					if (projectile.ai[1] == 0)
						Projectile.NewProjectile(parent.Center, parent.DirectionTo(Main.player[parent.target].Center) * 15, ModContent.ProjectileType<HecateBoonProj>(), Main.expertMode ? (int)(parent.damage / 4) : parent.damage, 4, parent.target);
					projectile.active = false;
				}
			}
			speed += ACCELERATION;
			Vector2 offset = Vector2.UnitX.RotatedBy(rotation) * radius;
			projectile.Center = parent.Center + offset;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];

			Vector2 origin = new Vector2(tex.Width / 2, tex.Height / (frameHeight * 2));

			for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
			{
				float mult = (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length));
				Vector2 drawPos = projectile.oldPos[k] + (new Vector2(projectile.width, projectile.height) / 2);
				Color color = Color.White * mult;
				float num108 = 4;
				float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				float num106 = 0f;
				Color color29 = new Color(110 - projectile.alpha, 31 - projectile.alpha, 255 - projectile.alpha, 0).MultiplyRGBA(color);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = projectile.GetAlpha(color28);
					color28 *= 1.5f - num107;
					color28 *= (float)Math.Pow((((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2), 1.5f);
					Vector2 vector29 = drawPos + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (1.5f * num107 + 3f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity * (float)num103;
					spriteBatch.Draw(tex, vector29, new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight), color28 * .8f, projectile.rotation, origin, projectile.scale * (float)Math.Sqrt(mult), SpriteEffects.None, 0f);
				}
			}

			return true;
		}


		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
