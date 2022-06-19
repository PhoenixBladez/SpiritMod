using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Zones;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zones
{
	class RepulsionZone : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Repulsion Zone");
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
            projectile.aiStyle = -1;
            projectile.scale = 1.2f;
        }

        public override void AI()
        {
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                int distance = (int)Vector2.Distance(npc.Center, projectile.Center);

                if (npc.active && distance < 70 && !npc.boss && !npc.friendly && npc.knockBackResist != 0f && !npc.dontTakeDamage)
                {
					float dist = npc.Distance(projectile.Center);
                    dist = 8f / dist;
					npc.velocity.X = dist * -.8f;
                    npc.velocity.Y = dist * -.8f;

                    projectile.ai[1]++;
                    for (int k = 0; k < 10; k++)
                    {
                        Dust d = Dust.NewDustPerfect(projectile.Center, 131, Vector2.One.RotatedByRandom(7.28f) * Main.rand.NextFloat(5), 0, default, 0.5f);
                        d.noGravity = true;
                        d.shader = GameShaders.Armor.GetSecondaryShader(38, Main.LocalPlayer);
                    }
                }
            }

            if (projectile.ai[1] >= 28 || !Main.player[projectile.owner].HasBuff(ModContent.BuffType<RepulsionZoneTimer>()))
                projectile.Kill();
        }

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, projectile, new Color(97, 46, 163), "SpiritMod/Projectiles/Summon/Zones/RepulsionZone");
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => ZoneHelper.ZonePreDraw(projectile, mod.GetTexture("Projectiles/Summon/Zones/RepulsionZone_Glow"));

		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 131, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
                d.shader = GameShaders.Armor.GetSecondaryShader(38, Main.LocalPlayer);
            }
        }
    }
}
