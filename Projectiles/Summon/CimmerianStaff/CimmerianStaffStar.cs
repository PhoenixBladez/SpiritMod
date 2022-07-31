
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.CimmerianStaff
{
	class CimmerianStaffStar : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Star");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.alpha = 0;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}
        public override void AI()
        {
            Projectile.rotation += .3f;
            for (int i = 0; i < 5; i++)
            {
                Vector2 position = Projectile.Center;
                Dust dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }
			if (Main.rand.NextBool(3))
            {
                Vector2 position = Projectile.Center;
                Dust dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity *= .6f;
            }
        }

		public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(173, 102, 255) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				float scale = Projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Summon/CimmerianStaff/CimmerianStaffStar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
				//spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}

        public override void Kill(int timeLeft)
		{
            DustHelper.DrawStar(Projectile.Center, 272, pointAmount: 5, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
            SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
		}
	}
}
