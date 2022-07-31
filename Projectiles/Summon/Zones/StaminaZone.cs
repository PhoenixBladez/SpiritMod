using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Zones;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zones
{
	class StaminaZone : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stamina Zone");
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

			if (!Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StaminaZoneTimer>()))
				Projectile.Kill();

			if (player.DistanceSQ(Projectile.Center) < 100 * 100)
                player.AddBuff(ModContent.BuffType<StaminaZoneBuff>(), 300);
		}

		public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, Projectile, new Color(44, 168, 67), "SpiritMod/Projectiles/Summon/Zones/StaminaZone");
		public override bool PreDraw(ref Color lightColor) => ZoneHelper.ZonePreDraw(Projectile, Mod.Assets.Request<Texture2D>("Projectiles/Summon/Zones/StaminaZone_Glow").Value);

		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 131, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
            }
        }
    }
}
