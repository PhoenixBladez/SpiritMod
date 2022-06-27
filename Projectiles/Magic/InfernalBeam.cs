using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class InfernalBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Beam");
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.alpha = 255;

			Projectile.penetrate = -1;

			Projectile.friendly = true;
			Projectile.tileCollide = false;
			//projectile.updatedNPCImmunity = true;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
				Projectile.velocity = -Vector2.UnitY;

			if (player.active && !player.dead && player.channel && !player.CCed && !player.noItems) {
				Projectile.Center = player.Center;
				Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
				Projectile.timeLeft = 2;

				player.ChangeDir(Projectile.direction);
				player.heldProj = Projectile.whoAmI;
				player.itemTime = 2;
				player.itemAnimation = 2;
				player.itemRotation = (float)Math.Atan2((Projectile.velocity.Y * Projectile.direction), (Projectile.velocity.X * Projectile.direction));

				Projectile.ai[0]++;
				if (Projectile.ai[0] >= 10) {
					if (!player.CheckMana(player.inventory[player.selectedItem].mana, true, false)) {
						Projectile.Kill();
						return false;
					}
					Projectile.ai[0] = 0;
				}
			}
			else {
				Projectile.Kill();
				return false;
			}

			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
				Projectile.velocity = -Vector2.UnitY;

			float rot = Projectile.velocity.ToRotation();
			Projectile.rotation = rot + 1.57F;
			Projectile.velocity = rot.ToRotationVector2();
			int num811 = 2;
			float scaleFactor7 = 0f;
			Vector2 value37 = Projectile.Center;

			float[] array3 = new float[num811];
			int num812 = 0;
			while ((float)num812 < num811) {
				float num813 = (float)num812 / (num811 - 1);
				Vector2 value38 = value37 + Projectile.velocity.RotatedBy(Math.PI / 2, default) * (num813 - 0.5f) * scaleFactor7 * Projectile.scale;
				int num814 = (int)value38.X / 16;
				int num815 = (int)value38.Y / 16;
				Vector2 vector69 = value38 + Projectile.velocity * 16f * 150f;
				int num816 = (int)vector69.X / 16;
				int num817 = (int)vector69.Y / 16;
				Tuple<int, int> tuple;
				float num818;
				if (!Collision.TupleHitLine(num814, num815, num816, num817, 0, 0, new List<Tuple<int, int>>(), out tuple)) {
					num818 = new Vector2((float)Math.Abs(num814 - tuple.Item1), (float)Math.Abs(num815 - tuple.Item2)).Length() * 16f;
				}
				else if (tuple.Item1 == num816 && tuple.Item2 == num817) {
					num818 = 2400f;
				}
				else {
					num818 = new Vector2((float)Math.Abs(num814 - tuple.Item1), (float)Math.Abs(num815 - tuple.Item2)).Length() * 16f;
				}
				array3[num812] = num818;
				num812++;
			}

			float num819 = 0f;
			for (int num820 = 0; num820 < array3.Length; num820++) {
				num819 += array3[num820];
			}
			num819 /= num811;
			float amount = 0.5f;
			Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num819, amount);

			Vector2 vector72 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);
			/*for (int num826 = 0; num826 < 2; num826++)
            {
                float num827 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float num828 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector73 = new Vector2((float)Math.Cos((double)num827) * num828, (float)Math.Sin((double)num827) * num828);
                int num829 = Dust.NewDust(vector72, 0, 0, 229, vector73.X, vector73.Y, 0, default, 1f);
                Main.dust[num829].noGravity = true;
                Main.dust[num829].scale = 1.7f;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 value40 = projectile.velocity.RotatedBy(1.5707963705062866, default) * ((float)Main.rand.NextDouble() - 0.5f) * (float)projectile.width;
                int num830 = Dust.NewDust(vector72 + value40 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default, 1.5f);
                Main.dust[num830].velocity *= 0.5f;
                Main.dust[num830].velocity.Y = -Math.Abs(Main.dust[num830].velocity.Y);
            }*/
			DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
			//Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float tmp = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], 30f * Projectile.scale, ref tmp)) {
				return true;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//projectile.npcImmune[target.whoAmI] = 10;
			target.immune[Projectile.owner] = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.velocity == Vector2.Zero)
				return false;

			Texture2D tex2 = TextureAssets.Projectile[Projectile.type].Value;
			float num210 = Projectile.localAI[1];
			Microsoft.Xna.Framework.Color c_ = new Microsoft.Xna.Framework.Color(255, 255, 255, 127);
			Vector2 value20 = Projectile.Center.Floor();
			num210 -= Projectile.scale * 10.5f;
			Vector2 vector41 = new Vector2(Projectile.scale);
			DelegateMethods.f_1 = 1f;
			DelegateMethods.c_1 = c_;
			DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
			Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + Projectile.velocity * num210 - Main.screenPosition, vector41, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));
			DelegateMethods.c_1 = new Color(255, 255, 255, 127) * 0.75f * Projectile.Opacity;
			Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + Projectile.velocity * num210 - Main.screenPosition, vector41 / 2f, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));

			return false;
		}

	}
}
