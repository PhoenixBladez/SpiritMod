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
	class StaminaZone : ModProjectile, IDrawAdditive
	{
		private bool[] _npcAliveLast;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stamina Zone");
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
			if (distance < 80)
            {
                player.AddBuff(ModContent.BuffType<StaminaZoneBuff>(), 130);
            }
			
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(44, 168, 67) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/Zones/StaminaZone");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                var effects = projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                Color color1 = Color.White * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                float num341 = 0f;
                float num340 = projectile.height;
                float num108 = 4;
                float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
                float num106 = 0f;

                Vector2 vector15 = new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type] / 2));
                SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 vector33 = new Vector2(projectile.Center.X, projectile.Center.Y - 18) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity;
                Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(27 - projectile.alpha, 112 - projectile.alpha, 43 - projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightBlue);
                for (int num103 = 0; num103 < 4; num103++)
                {
                    Microsoft.Xna.Framework.Color color28 = color29;
                    color28 = projectile.GetAlpha(color28);
                    color28 *= 1f - num107;
                    Vector2 vector29 = new Vector2(projectile.Center.X - 60, projectile.Center.Y - 60) + drawOrigin + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (6f * num107 + 2f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity * (float)num103;
                    Main.spriteBatch.Draw(mod.GetTexture("Projectiles/Summon/Zones/StaminaZone_Glow"), vector29, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color28 * .85f, projectile.rotation, drawOrigin, projectile.scale, spriteEffects3, 0f);
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 131, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
            }

        }
    }
}
