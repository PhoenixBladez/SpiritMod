using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class PositiveArrow : ModProjectile, ITrailProjectile
	{
		bool stuck = false;
		public int oppositearrow;
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Positive Arrow");

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			oppositearrow = ModContent.ProjectileType<NegativeArrow>();
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new StandardColorTrail(new Color(122, 233, 255)), new RoundCap(), new ZigZagTrailPosition(3f), 8f, 250f);

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(stuck);
		public override void ReceiveExtraAI(BinaryReader reader) => stuck = reader.ReadBoolean();

		public override void Kill(int timeLeft)
		{
			if (timeLeft <= 0)
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 3; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!stuck) {
				Projectile.rotation = oldVelocity.ToRotation() + 1.57f;
				Projectile.position += Projectile.velocity * 2;
				stuck = true;
				Projectile.timeLeft = 370;
				Projectile.velocity = Vector2.Zero;
				Projectile.aiStyle = -1;
			}
			return false;
		}
		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.ai[0] == 0;
		public override bool PreAI()
		{
			if (stuck) {
				CheckColliding(ref Projectile.damage);
				Projectile.timeLeft = Math.Min(Projectile.timeLeft, 360);
				Projectile.velocity = Vector2.Zero;
				return false;
			}
			if (Projectile.ai[0] != 0) {
				Projectile.timeLeft = Math.Min(Projectile.timeLeft, 360);
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;

				if (Main.npc[(int)Projectile.ai[1]].active && !Main.npc[(int)Projectile.ai[1]].dontTakeDamage) {
					Projectile.Center = Main.npc[(int)Projectile.ai[1]].Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = Main.npc[(int)Projectile.ai[1]].gfxOffY;
					if (Projectile.timeLeft % 30f == 0f) {
						Main.npc[(int)Projectile.ai[1]].HitEffect(0, 1.0);
					}
				}
				else
					Projectile.Kill();

				return false;
			}
			return base.PreAI();
		}

		private void CheckColliding(ref int Damage)
		{
			var list = Main.projectile.Where(x => x.active && x.type == oppositearrow && x.active && (x.Hitbox.Intersects(Projectile.Hitbox)|| x.ai[1] == Projectile.ai[1] && x.ai[0] == 1 && Projectile.ai[0] == 1));
			if (list.Any()) {
				foreach (var proj in list) {
					DustHelper.DrawStar(new Vector2(proj.Center.X, proj.Center.Y), 226, pointAmount: 5, mainSize: 4.5f, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);
					proj.Kill();
				}

				SoundEngine.PlaySound(SoundID.Item93, Projectile.position);
				for (int num621 = 0; num621 < 40; num621++) {
					int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					Main.dust[num622].noGravity = true;
					Main.dust[num622].scale = 0.5f;
				}
				SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
				Projectile.timeLeft = 8;
				Damage = (int)(Damage * 1.5);
				ProjectileExtras.Explode(Projectile.whoAmI, 75, 75,
				delegate {
					DustHelper.DrawStar(new Vector2(Projectile.Center.X, Projectile.Center.Y), 226, pointAmount: 5, mainSize: 4.5f, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);
				});
				Projectile.Kill();
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Projectile.ai[0] = 1f;
			Projectile.ai[1] = (float)target.whoAmI;
			Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
			Projectile.netUpdate = true;
			CheckColliding(ref damage);
			int num31 = 1;
			Point[] array2 = new Point[num31];
			int num32 = 0;

			for (int n = 0; n < 1000; n++) {
				if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == Projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI) {
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
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity.Y += 0.25f;
		}
	}
}