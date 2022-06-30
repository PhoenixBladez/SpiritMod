using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using SpiritMod;
using SpiritMod.Projectiles;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.StarjinxSet.JinxprobeWand
{
	public class JinxprobeEnergy : ModProjectile, ITrailProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Star");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.ignoreWater = true;
            Projectile.scale = Main.rand.NextFloat(0.6f, 0.9f);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Projectile.rotation += 0.1f;

            Projectile.velocity.Y += 0.1f;

			if (Main.rand.Next(50) == 0)
				Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8), Color.White * 0.66f, SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly), Main.rand.NextFloat(0.2f, 0.3f), 35));
        }

        private float Timer => Main.GlobalTimeWrappedHourly * 2 + Projectile.ai[0];

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            Projectile.Bounce(oldVelocity, 0.5f);
            return false;
        }

        public void DoTrailCreation(TrailManager tM)
        {
            tM.CreateTrail(Projectile, new StarjinxTrail(Timer, 2, 0.15f), new RoundCap(), new ArrowGlowPosition(), 48f * Projectile.scale, 200f * Projectile.scale, new DefaultShader());
            tM.CreateTrail(Projectile, new StarjinxTrail(Timer, 2, 0.8f), new RoundCap(), new DefaultTrailPosition(), 20f * Projectile.scale, 80f * Projectile.scale, new DefaultShader());
            tM.CreateTrail(Projectile, new StarjinxTrail(Timer, 2, 0.8f), new RoundCap(), new DefaultTrailPosition(), 20f * Projectile.scale, 80f * Projectile.scale, new DefaultShader());
            tM.CreateTrail(Projectile, new StarjinxTrail(Timer, 2, 0.3f), new RoundCap(), new ArrowGlowPosition(), 48f * Projectile.scale, 40f * Projectile.scale, new DefaultShader());
        }

        public override bool PreDraw(ref Color lightColor)
		{
			StarPrimitive star = new StarPrimitive
			{
				Color = Color.White,
				TriangleHeight = 12 * Projectile.scale,
				TriangleWidth = 4 * Projectile.scale,
				Position = Projectile.Center - Main.screenPosition,
				Rotation = Projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(star);
			return false;
		}

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.Item12.WithVolume(0.25f).WithPitchVariance(0.3f), Projectile.Center);
        }
    }
}