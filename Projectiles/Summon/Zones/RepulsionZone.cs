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
            Projectile.aiStyle = -1;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                int distance = (int)Vector2.Distance(npc.Center, Projectile.Center);

                if (npc.active && distance < 70 && !npc.boss && !npc.friendly && npc.knockBackResist != 0f && !npc.dontTakeDamage)
                {
					float dist = npc.Distance(Projectile.Center);
                    dist = 8f / dist;
					npc.velocity.X = dist * -.8f;
                    npc.velocity.Y = dist * -.8f;

                    Projectile.ai[1]++;
                    for (int k = 0; k < 10; k++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center, 131, Vector2.One.RotatedByRandom(7.28f) * Main.rand.NextFloat(5), 0, default, 0.5f);
                        d.noGravity = true;
                        d.shader = GameShaders.Armor.GetSecondaryShader(38, Main.LocalPlayer);
                    }
                }
            }

            if (Projectile.ai[1] >= 28 || !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<RepulsionZoneTimer>()))
                Projectile.Kill();
        }

		public void AdditiveCall(SpriteBatch spriteBatch) => ZoneHelper.ZoneAdditiveDraw(spriteBatch, Projectile, new Color(97, 46, 163), "SpiritMod/Projectiles/Summon/Zones/RepulsionZone");
        public override bool PreDraw(ref Color lightColor) => ZoneHelper.ZonePreDraw(Projectile, Mod.Assets.Request<Texture2D>("Projectiles/Summon/Zones/RepulsionZone_Glow").Value);

		public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 131, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(6), 0, default, 0.5f);
                d.noGravity = true;
                d.shader = GameShaders.Armor.GetSecondaryShader(38, Main.LocalPlayer);
            }
        }
    }
}
