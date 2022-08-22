using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

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
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.tileCollide = false;

			Projectile.penetrate = -1;
			Projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			Vector2? vector68 = null;
			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
				Projectile.velocity = -Vector2.UnitY;

			if (Main.projectile[(int)Projectile.ai[1]].active && Main.projectile[(int)Projectile.ai[1]].type == ModContent.ProjectileType<PhantomArcHandle>()) {
				Projectile.Center = Main.projectile[(int)Projectile.ai[1]].Center;
				Projectile.velocity = Vector2.Normalize(Main.projectile[(int)Projectile.ai[1]].velocity);
			}
			else {
				Projectile.Kill();
			}

			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
				Projectile.velocity = -Vector2.UnitY;

			float num810 = Projectile.velocity.ToRotation();
			Projectile.rotation = num810 - (float)(Math.PI / 2);
			Projectile.velocity = num810.ToRotationVector2();
			float scaleFactor7 = 0f;
			Vector2 value37 = Projectile.Center;
			if (vector68.HasValue)
				value37 = vector68.Value;

			int num811 = 2;
			scaleFactor7 = 0f;

			float[] array3 = new float[num811];
			int num812 = 0;
			while (num812 < num811) {
				float num813 = num812 / (num811 - 1f);
				Vector2 value38 = value37 + Projectile.velocity.RotatedBy(Math.PI / 2, default) * (num813 - 0.5f) * scaleFactor7 * Projectile.scale;
				int num814 = (int)value38.X >> 4;
				int num815 = (int)value38.Y >> 4;
				Vector2 vector69 = value38 + Projectile.velocity * 16f * 150f;
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
			Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num819, amount);
			Vector2 vector72 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);

			for (int num826 = 0; num826 < 2; num826++) {
				float num827 = Projectile.velocity.ToRotation() + ((Main.rand.NextBool(2)) ? -1f : 1f) * 1.57079637f;
				float num828 = (float)Main.rand.NextDouble() * 2f + 2f;
				Vector2 vector73 = new Vector2((float)Math.Cos((double)num827) * num828, (float)Math.Sin((double)num827) * num828);
				int num829 = Dust.NewDust(vector72, 0, 0, DustID.UnusedWhiteBluePurple, vector73.X, vector73.Y, 0, default, 1f);
				Main.dust[num829].noGravity = true;
				Main.dust[num829].scale = 1.7f;
			}

			if (Main.rand.NextBool(5)) {
				Vector2 value40 = Projectile.velocity.RotatedBy(Math.PI / 2, default) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
				int num830 = Dust.NewDust(vector72 + value40 - Vector2.One * 4f, 8, 8, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				Main.dust[num830].velocity *= 0.5f;
				Main.dust[num830].velocity.Y = -Math.Abs(Main.dust[num830].velocity.Y);
			}
			DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], Projectile.width * Projectile.scale, new Utils.TileActionAttempt(DelegateMethods.CastLight));

			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float n = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], 30f * Projectile.scale, ref n))
				return true;

			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.velocity == Vector2.Zero)
				return false;

			Texture2D tex2 = TextureAssets.Projectile[Projectile.type].Value;
			float num210 = Projectile.localAI[1];
			Color c_ = new Color(255, 255, 255, 127);
			Vector2 value20 = Projectile.Center.Floor();
			num210 -= Projectile.scale * 10.5f;
			Vector2 vector41 = new Vector2(Projectile.scale);
			DelegateMethods.f_1 = 1f;
			DelegateMethods.c_1 = c_;
			DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
			Vector2 vector42 = Projectile.oldPos[0] + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + Projectile.velocity * num210 - Main.screenPosition, vector41, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
			DelegateMethods.c_1 = new Color(255, 255, 255, 127) * 0.75f * Projectile.Opacity;
			Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + Projectile.velocity * num210 - Main.screenPosition, vector41 / 2f, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
			return false;
		}

	}
}
