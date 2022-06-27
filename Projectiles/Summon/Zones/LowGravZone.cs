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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
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
            Projectile.aiStyle = -1;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

			if (!Main.player[Projectile.owner].HasBuff(ModContent.BuffType<LowGravZoneTimer>()))
				Projectile.Kill();

			if (Vector2.DistanceSquared(Projectile.Center, player.Center) < 100 * 100)
                player.AddBuff(ModContent.BuffType<LowGravZoneBuff>(), 300);
        }

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, Projectile, new Color(178, 163, 191), "SpiritMod/Projectiles/Summon/Zones/LowGravZone");
		public override bool PreDraw(ref Color lightColor) => ZoneHelper.ZonePreDraw(Projectile, Mod.GetTexture("Projectiles/Summon/Zones/LowGravZone_Glow"));

		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Firework_Blue, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
                d.shader = GameShaders.Armor.GetSecondaryShader(87, Main.LocalPlayer);
            }
        }
    }
}
