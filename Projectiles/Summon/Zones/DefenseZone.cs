using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Zones;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace SpiritMod.Projectiles.Summon.Zones
{
	class DefenseZone : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Defense Zone");
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

			if (!Main.player[Projectile.owner].HasBuff(ModContent.BuffType<ShieldZoneTimer>()))
				Projectile.Kill();

			if (player.DistanceSQ(Projectile.Center) < 100 * 100)
                player.AddBuff(ModContent.BuffType<DefenseZoneBuff>(), 300);
		}

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, Projectile, new Color(150, 129, 35), "SpiritMod/Projectiles/Summon/Zones/DefenseZone");
		public override bool PreDraw(ref Color lightColor) => ZoneHelper.ZonePreDraw(Projectile, Mod.GetTexture("Projectiles/Summon/Zones/DefenseZone_Glow"));

		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 25; k++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 131, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.5f);
                d.noGravity = true;
                d.shader = GameShaders.Armor.GetSecondaryShader(90, Main.LocalPlayer);
            }
        }
    }
}
