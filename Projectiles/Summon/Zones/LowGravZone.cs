using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Zones;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zones
{
	class LowGravZone : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Low Gravity Zone");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
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
            projectile.aiStyle = -1;
            projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

			if (!Main.player[projectile.owner].HasBuff(ModContent.BuffType<LowGravZoneTimer>()))
				projectile.Kill();

			if (Vector2.DistanceSquared(projectile.Center, player.Center) < 100 * 100)
                player.AddBuff(ModContent.BuffType<LowGravZoneBuff>(), 300);
        }

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, projectile, new Color(178, 163, 191), "SpiritMod/Projectiles/Summon/Zones/LowGravZone");
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => ZoneHelper.ZonePreDraw(projectile, mod.GetTexture("Projectiles/Summon/Zones/LowGravZone_Glow"));

		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, DustID.Firework_Blue, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
                d.shader = GameShaders.Armor.GetSecondaryShader(87, Main.LocalPlayer);
            }
        }
    }
}
