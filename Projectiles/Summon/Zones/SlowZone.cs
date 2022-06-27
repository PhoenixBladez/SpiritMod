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
			if (!Main.player[Projectile.owner].HasBuff(ModContent.BuffType<CryoZoneTimer>()))
				Projectile.Kill();

			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < 100 * 100)
					npc.AddBuff(ModContent.BuffType<MageFreeze>(), 300);
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, Projectile, new Color(47, 190, 222), "SpiritMod/Projectiles/Summon/Zones/SlowZone");
		public override bool PreDraw(ref Color lightColor) => ZoneHelper.ZonePreDraw(Projectile, Mod.GetTexture("Projectiles/Summon/Zones/SlowZone_Glow"));

        public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
				d.noGravity = true;
			}
		}
	}
}
