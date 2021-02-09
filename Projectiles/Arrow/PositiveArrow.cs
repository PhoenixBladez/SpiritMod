using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;

namespace SpiritMod.Projectiles.Arrow
{
	public class PositiveArrow : ModProjectile
	{
		bool stuck = false;
		public int oppositearrow;
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Positive Arrow");

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			oppositearrow = ModContent.ProjectileType<NegativeArrow>();
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
		}
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(stuck);
		public override void ReceiveExtraAI(BinaryReader reader) => stuck = reader.ReadBoolean();

		public override void Kill(int timeLeft)
		{
			if (timeLeft <= 0)
				Main.PlaySound(SoundID.Dig, projectile.Center);
			for (int i = 0; i < 3; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 226);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!stuck) {
				projectile.rotation = oldVelocity.ToRotation() + 1.57f;
				projectile.position += projectile.velocity * 2;
				stuck = true;
				projectile.timeLeft = 370;
				projectile.velocity = Vector2.Zero;
				projectile.aiStyle = -1;
			}
			return false;
		}
		public override bool CanDamage() => projectile.ai[0] == 0;
		public override bool PreAI()
		{
			if (stuck) {
				CheckColliding(ref projectile.damage);
				projectile.timeLeft = Math.Min(projectile.timeLeft, 360);
				projectile.velocity = Vector2.Zero;
				return false;
			}
			if (projectile.ai[0] != 0) {
				projectile.timeLeft = Math.Min(projectile.timeLeft, 360);
				projectile.ignoreWater = true;
				projectile.tileCollide = false;

				if (Main.npc[(int)projectile.ai[1]].active && !Main.npc[(int)projectile.ai[1]].dontTakeDamage) {
					projectile.Center = Main.npc[(int)projectile.ai[1]].Center - projectile.velocity * 2f;
					projectile.gfxOffY = Main.npc[(int)projectile.ai[1]].gfxOffY;
					if (projectile.timeLeft % 30f == 0f) {
						Main.npc[(int)projectile.ai[1]].HitEffect(0, 1.0);
					}
				}
				else
					projectile.Kill();

				return false;
			}
			return base.PreAI();
		}

		private void CheckColliding(ref int Damage)
		{
			var list = Main.projectile.Where(x => x.active && x.type == oppositearrow && x.active && (x.Hitbox.Intersects(projectile.Hitbox)|| x.ai[1] == projectile.ai[1] && x.ai[0] == 1 && projectile.ai[0] == 1));
			if (list.Any()) {
				foreach (var proj in list) {
					DustHelper.DrawStar(new Vector2(proj.Center.X, proj.Center.Y), 226, pointAmount: 5, mainSize: 4.5f, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);
					proj.Kill();
				}

				Main.PlaySound(SoundID.Item93, projectile.position);
				for (int num621 = 0; num621 < 40; num621++) {
					int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					Main.dust[num622].noGravity = true;
					Main.dust[num622].scale = 0.5f;
				}
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
				projectile.timeLeft = 8;
				Damage = (int)(Damage * 1.5);
				ProjectileExtras.Explode(projectile.whoAmI, 75, 75,
				delegate {
					DustHelper.DrawStar(new Vector2(projectile.Center.X, projectile.Center.Y), 226, pointAmount: 5, mainSize: 4.5f, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);
				});
				projectile.Kill();
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			projectile.ai[0] = 1f;
			projectile.ai[1] = (float)target.whoAmI;
			projectile.velocity = (target.Center - projectile.Center) * 0.75f;
			projectile.netUpdate = true;
			CheckColliding(ref damage);
			int num31 = 1;
			Point[] array2 = new Point[num31];
			int num32 = 0;

			for (int n = 0; n < 1000; n++) {
				if (n != projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI) {
					array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
					if (num32 >= array2.Length)
						break;
				}
			}

			if (num32 >= array2.Length) {
				int num33 = 0;
				for (int num34 = 1; num34 < array2.Length; num34++) {
					if (array2[num34].Y < array2[num33].Y)
						num33 = num34;
				}
				Main.projectile[array2[num33].X].Kill();
			}
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.velocity.Y += 0.25f;
		}
	}
}