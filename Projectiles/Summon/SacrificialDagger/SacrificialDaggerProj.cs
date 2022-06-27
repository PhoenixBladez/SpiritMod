using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

		public override void SetDefaults()
		{
            Projectile.width = 16;
			Projectile.height = 30;
            Projectile.aiStyle = -1;
			Projectile.friendly = true;
            Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

        float alphaCounter;
		public override bool PreAI()
        {
            alphaCounter += 0.08f;
            if (Projectile.ai[0] == 0)
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			else {
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;
				int num996 = 15;
				bool flag52 = false;
				bool flag53 = false;
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] % 30f == 0f)
					flag53 = true;

				int num997 = (int)Projectile.ai[1];
				if (Projectile.localAI[0] >= (float)(60 * num996))
					flag52 = true;
				else if (num997 < 0 || num997 >= 200)
					flag52 = true;
				else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
					Projectile.Center = Main.npc[num997].Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = Main.npc[num997].gfxOffY;
					if (flag53) {
						Main.npc[num997].HitEffect(0, 1.0);
					}
				}
				else
					flag52 = true;

				if (flag52)
					Projectile.Kill();
			}
			return true;
		}
        
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2f;
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Color color = new Color(191, 102, 255) * 0.85f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                    float scale = Projectile.scale;
                    Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Summon/SacrificialDagger/SacrificialDagger_Trail");

                    spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color * sineAdd, Projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 1f;
			Projectile.ai[1] = (float)target.whoAmI;
			target.AddBuff(Mod.Find<ModBuff>("SacrificialDaggerBuff").Type, Projectile.timeLeft, false);
			Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true;
			Projectile.damage = 0;

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
            Player player = Main.player[Projectile.owner];
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

            SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y, 1);
            Vector2 vector9 = Projectile.position;
            Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 12f;
            for (int num257 = 0; num257 < 18; num257++)
            {
                int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, Color.White, 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
                vector9 -= value19 * 8f;
            }
        }

    }
}
