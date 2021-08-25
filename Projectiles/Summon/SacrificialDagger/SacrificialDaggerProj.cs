using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Summon.SacrificialDagger
{
	public class SacrificialDaggerProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrificial Dagger");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

		public override void SetDefaults()
		{
            projectile.width = 16;
			projectile.height = 30;
            projectile.aiStyle = -1;
			projectile.friendly = true;
            projectile.timeLeft = 600;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

        float alphaCounter;
		public override bool PreAI()
        {
            alphaCounter += 0.08f;
            if (projectile.ai[0] == 0)
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			else {
				projectile.ignoreWater = true;
				projectile.tileCollide = false;
				int num996 = 15;
				bool flag52 = false;
				bool flag53 = false;
				projectile.localAI[0] += 1f;
				if (projectile.localAI[0] % 30f == 0f)
					flag53 = true;

				int num997 = (int)projectile.ai[1];
				if (projectile.localAI[0] >= (float)(60 * num996))
					flag52 = true;
				else if (num997 < 0 || num997 >= 200)
					flag52 = true;
				else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
					projectile.Center = Main.npc[num997].Center - projectile.velocity * 2f;
					projectile.gfxOffY = Main.npc[num997].gfxOffY;
					if (flag53) {
						Main.npc[num997].HitEffect(0, 1.0);
					}
				}
				else
					flag52 = true;

				if (flag52)
					projectile.Kill();
			}
			return true;
		}
        
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2f;
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(191, 102, 255) * 0.85f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/SacrificialDagger/SacrificialDagger_Trail");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * sineAdd, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[0] = 1f;
			projectile.ai[1] = (float)target.whoAmI;
			target.AddBuff(mod.BuffType("SacrificialDaggerBuff"), projectile.timeLeft, false);
			projectile.velocity = (target.Center - projectile.Center) * 0.75f;
            projectile.netUpdate = true;
			projectile.damage = 0;

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
            Player player = Main.player[projectile.owner];
            int num = -1;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(player, false) && Main.npc[i] == target)
                {
                    num = i;
                }
            }
            {
                player.MinionAttackTargetNPC = num;
            }
        }

        public override void Kill(int timeLeft)
        {

            Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
            Vector2 vector9 = projectile.position;
            Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 12f;
            for (int num257 = 0; num257 < 18; num257++)
            {
                int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 173, 0f, 0f, 0, Color.White, 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
                vector9 -= value19 * 8f;
            }
        }

    }
}
