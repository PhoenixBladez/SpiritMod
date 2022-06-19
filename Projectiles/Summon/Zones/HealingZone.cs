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

			if (!Main.player[projectile.owner].HasBuff(ModContent.BuffType<HealthZoneTimer>()))
				projectile.Kill();

			if (player.DistanceSQ(projectile.Center) < 100 * 100)
            {
                player.AddBuff(ModContent.BuffType<HealingZoneBuff>(), 300);

                if (Main.rand.NextBool(30))
                {
					var pos = new Vector2(projectile.Center.X + Main.rand.Next(-50, 50), projectile.Center.Y + Main.rand.Next(-50, 50));
					Gore.NewGore(pos, new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f), 331, Main.rand.Next(80, 120) * 0.01f);
                }
            }
		}

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, projectile, new Color(194, 21, 85), "SpiritMod/Projectiles/Summon/Zones/HealingZone");
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => ZoneHelper.ZonePreDraw(projectile, mod.GetTexture("Projectiles/Summon/Zones/HealingZone_Glow"));

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
