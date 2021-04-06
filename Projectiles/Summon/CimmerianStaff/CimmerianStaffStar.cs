
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.CimmerianStaff
{
	class CimmerianStaffStar : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Star");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
			projectile.height = 18;
			projectile.width = 18;
			projectile.alpha = 0;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}
		int timer;
        public override void AI()
        {
            projectile.rotation += .3f;
            for (int i = 0; i < 5; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, 173, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }
			if (Main.rand.NextBool(3))
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, 173, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity *= .6f;
            }
        }
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(173, 102, 255) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/CimmerianStaff/CimmerianStaffStar");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                    //spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
        public override void Kill(int timeLeft)
		{
            DustHelper.DrawStar(projectile.Center, 272, pointAmount: 5, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
            Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
		}
	}
}
