using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class PhantomArc : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Arc");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.tileCollide = false;

			projectile.penetrate = -1;
			projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			Vector2? vector68 = null;
			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
				projectile.velocity = -Vector2.UnitY;

			if (Main.projectile[(int)projectile.ai[1]].active && Main.projectile[(int)projectile.ai[1]].type == mod.ProjectileType("PhantomArcHandle"))
			{
				projectile.Center = Main.projectile[(int)projectile.ai[1]].Center;
				projectile.velocity = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
			}
			else
			{
				projectile.Kill();
			}

			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
				projectile.velocity = -Vector2.UnitY;

			float num810 = projectile.velocity.ToRotation();
			projectile.rotation = num810 - (float)(Math.PI / 2);
			projectile.velocity = num810.ToRotationVector2();
			float scaleFactor7 = 0f;
			Vector2 value37 = projectile.Center;
			if (vector68.HasValue)
				value37 = vector68.Value;

			int num811 = 2;
			scaleFactor7 = 0f;

			float[] array3 = new float[num811];
			int num812 = 0;
			while (num812 < num811)
			{
				float num813 = num812 / (num811 - 1f);
				Vector2 value38 = value37 + projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * (num813 - 0.5f) * scaleFactor7 * projectile.scale;
				int num814 = (int)value38.X >> 4;
				int num815 = (int)value38.Y >> 4;
				Vector2 vector69 = value38 + projectile.velocity * 16f * 150f;
				int num816 = (int)vector69.X >> 4;
				int num817 = (int)vector69.Y >> 4;
				Tuple<int, int> tuple;
				float num818;
				if (!Collision.TupleHitLine(num814, num815, num816, num817, 0, 0, new List<Tuple<int, int>>(), out tuple))
					num818 = new Vector2((float)Math.Abs(num814 - tuple.Item1), (float)Math.Abs(num815 - tuple.Item2)).Length() * 16f;
				else if (tuple.Item1 == num816 && tuple.Item2 == num817)
					num818 = 2400f;
				else
					num818 = new Vector2((float)Math.Abs(num814 - tuple.Item1), (float)Math.Abs(num815 - tuple.Item2)).Length() * 16f;

				array3[num812] = num818;
				num812++;
			}
			float num819 = 0f;
			for (int num820 = 0; num820 < array3.Length; num820++)
				num819 += array3[num820];

			num819 /= num811;
			float amount = 0.5f;
			projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num819, amount);
			Vector2 vector72 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);

			for (int num826 = 0; num826 < 2; num826++)
			{
				float num827 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
				float num828 = (float)Main.rand.NextDouble() * 2f + 2f;
				Vector2 vector73 = new Vector2((float)Math.Cos((double)num827) * num828, (float)Math.Sin((double)num827) * num828);
				int num829 = Dust.NewDust(vector72, 0, 0, 206, vector73.X, vector73.Y, 0, default(Color), 1f);
				Main.dust[num829].noGravity = true;
				Main.dust[num829].scale = 1.7f;
			}

			if (Main.rand.Next(5) == 0)
			{
				Vector2 value40 = projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
				int num830 = Dust.NewDust(vector72 + value40 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num830].velocity *= 0.5f;
				Main.dust[num830].velocity.Y = -Math.Abs(Main.dust[num830].velocity.Y);
			}
			DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));

			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float n = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 30f * projectile.scale, ref n))
				return true;

			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.velocity == Vector2.Zero)
				return false;

			Texture2D tex2 = Main.projectileTexture[projectile.type];
			float num210 = projectile.localAI[1];
			Color c_ = new Color(255, 255, 255, 127);
			Vector2 value20 = projectile.Center.Floor();
			num210 -= projectile.scale * 10.5f;
			Vector2 vector41 = new Vector2(projectile.scale);
			DelegateMethods.f_1 = 1f;
			DelegateMethods.c_1 = c_;
			DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
			Vector2 vector42 = projectile.oldPos[0] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + projectile.velocity * num210 - Main.screenPosition, vector41, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
			DelegateMethods.c_1 = new Color(255, 255, 255, 127) * 0.75f * projectile.Opacity;
			Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + projectile.velocity * num210 - Main.screenPosition, vector41 / 2f, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
			return false;
		}

	}
}
