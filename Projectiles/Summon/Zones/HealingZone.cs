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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.tileCollide = false;
			Projectile.penetrate = 4;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.height = 110;
            Projectile.sentry = true;
            Projectile.width = 110;
            Projectile.scale = 1.2f;
        }

		public override void AI()
		{
			Player player = Main.LocalPlayer;

			if (!Main.player[Projectile.owner].HasBuff(ModContent.BuffType<HealthZoneTimer>()))
				Projectile.Kill();

			if (player.DistanceSQ(Projectile.Center) < 100 * 100)
            {
                player.AddBuff(ModContent.BuffType<HealingZoneBuff>(), 300);

                if (Main.rand.NextBool(30))
                {
					var pos = new Vector2(Projectile.Center.X + Main.rand.Next(-50, 50), Projectile.Center.Y + Main.rand.Next(-50, 50));
					Gore.NewGore(pos, new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f), 331, Main.rand.Next(80, 120) * 0.01f);
                }
            }
		}

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, Projectile, new Color(194, 21, 85), "SpiritMod/Projectiles/Summon/Zones/HealingZone");
		public override bool PreDraw(ref Color lightColor) => ZoneHelper.ZonePreDraw(Projectile, Mod.Assets.Request<Texture2D>("Projectiles/Summon/Zones/HealingZone_Glow").Value);

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 130, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
            }
        }
    }
}
