using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class Scapula : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soaring Scapula");
		}
		float ai0 = 0;
		float ai1 = 0;
		float localai0 = 0;
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = true;
		}

		public override bool PreAI()
		{
            int num = 5;
            if (ai0 == 0)
				return true;
			else
			{
				projectile.ignoreWater = true;
				projectile.tileCollide = false;
				int num996 = 15;
				bool flag52 = false;
				bool flag53 = false;
				localai0 += 1f;
				if (localai0 % 30f == 0f)
					flag53 = true;

				int num997 = (int)ai1;
				if (localai0 >= (float)(60 * num996))
					flag52 = true;
				else if (num997 < 0 || num997 >= 200)
					flag52 = true;
				else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage)
				{
					projectile.Center = Main.npc[num997].Center - projectile.velocity * 2f;
					projectile.gfxOffY = Main.npc[num997].gfxOffY;
					if (flag53)
					{
						Main.npc[num997].HitEffect(0, 1.0);
					}
				}
				else
					flag52 = true;

				if (flag52)
					projectile.Kill();
				return false;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && target.velocity != Vector2.Zero && target.knockBackResist != 0)
			{
				target.velocity.Y = 6f;
			}
			projectile.timeLeft = 30;
			ai0 = 1f;
			ai1 = (float)target.whoAmI;
			projectile.velocity = (target.Center - projectile.Center) * 0.75f;
			projectile.netUpdate = true;
			projectile.damage = 0;

			int num31 = 1;
			Point[] array2 = new Point[num31];
			int num32 = 0;
			if (num32 >= array2.Length)
			{
				int num33 = 0;
				for (int num34 = 1; num34 < array2.Length; num34++)
				{
					if (array2[num34].Y < array2[num33].Y)
						num33 = num34;
				}
				Main.projectile[array2[num33].X].Kill();
			}
            if (target.life <= 0)
            {
                projectile.timeLeft = 2;
            }
        }
	}
}
