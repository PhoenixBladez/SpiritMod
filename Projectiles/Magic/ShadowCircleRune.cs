using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ShadowCircleRune : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Circle Rune");
		}

		public override void SetDefaults()
		{
			Projectile.width = 86;
			Projectile.height = 80;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 3600;
            Projectile.minionSlots = 1;
			Projectile.sentry = true;
		}

		public override bool PreAI()
		{
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

			Projectile.rotation += 0.05f * Projectile.direction;
			return true;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 30) {
				Projectile.frameCounter = 0;
				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++) {
					float num3 = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 1640f && Main.npc[i].CanBeChasedBy(Projectile, false)) {
						num2 = i;
						num = num3;
					}
				}

				if (num2 != -1) {
					bool flag = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag) {
						Vector2 value = Main.npc[num2].Center - Projectile.Center;
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
							num5 = num4 / num5;

						value *= num5;
						int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ModContent.ProjectileType<NPCs.Boss.Dusking.CrystalShadow>(), Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

	}
}
