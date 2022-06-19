using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Zones;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zones
{
	class SlowZone : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slow Zone");
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
			if (!Main.player[projectile.owner].HasBuff(ModContent.BuffType<CryoZoneTimer>()))
				projectile.Kill();

			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy() && npc.DistanceSQ(projectile.Center) < 100 * 100)
					npc.AddBuff(ModContent.BuffType<MageFreeze>(), 300);
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, projectile, new Color(47, 190, 222), "SpiritMod/Projectiles/Summon/Zones/SlowZone");
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => ZoneHelper.ZonePreDraw(projectile, mod.GetTexture("Projectiles/Summon/Zones/SlowZone_Glow"));

        public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
				d.noGravity = true;
			}
		}
	}
}
