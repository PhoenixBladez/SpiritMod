using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Zones;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zones
{
	class HealingZone : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Healing Zone");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
            projectile.tileCollide = false;
			projectile.penetrate = 4;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.height = 110;
            projectile.sentry = true;
            projectile.width = 110;
            projectile.scale = 1.2f;
        }

		public override void AI()
		{
			Player player = Main.LocalPlayer;

            int distance = (int)Vector2.Distance(projectile.Center, player.Center);
			if (distance < 100)
            {
                player.AddBuff(ModContent.BuffType<HealingZoneBuff>(), 300);
                if (Main.rand.NextBool(30))
                {
					var pos = new Vector2(projectile.Center.X + Main.rand.Next(-50, 50), projectile.Center.Y + Main.rand.Next(-50, 50));
					Gore.NewGore(pos, new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f), 331, Main.rand.Next(80, 120) * 0.01f);
                }
            }
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(194, 21, 85) * 0.75f * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale;
				Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/Zones/HealingZone");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                var effects = projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                float num108 = 4;
                float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
                float num106 = 0f;

                SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Color color29 = new Color(107 - projectile.alpha, 14 - projectile.alpha, 48 - projectile.alpha, 0).MultiplyRGBA(Color.LightBlue);
                for (int num103 = 0; num103 < 4; num103++)
                {
					Color color28 = color29;
                    color28 = projectile.GetAlpha(color28);
                    color28 *= 1f - num107;
                    Vector2 vector29 = new Vector2(projectile.Center.X - 60, projectile.Center.Y - 60) + drawOrigin + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity * (float)num103;
                    Main.spriteBatch.Draw(mod.GetTexture("Projectiles/Summon/Zones/HealingZone_Glow"), vector29, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color28, projectile.rotation, drawOrigin, projectile.scale, spriteEffects3, 0f);
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 130, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
            }
        }
    }
}
