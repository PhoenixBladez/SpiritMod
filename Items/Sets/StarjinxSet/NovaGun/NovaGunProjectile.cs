using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using SpiritMod.Projectiles;
using SpiritMod;

namespace SpiritMod.Items.Sets.StarjinxSet.NovaGun
{
	public class NovaGunProjectile : ModProjectile, ITrailProjectile, IBasicPrimDraw
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Erratic Star");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 720;
            projectile.extraUpdates = 2;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = 4;
            projectile.ignoreWater = true;
        }
        Vector2 originalvel = Vector2.Zero;
        public override void AI()
        {
            projectile.rotation += 0.1f;

            if(projectile.ai[0] == 0)
            {
                originalvel = projectile.velocity;
                projectile.ai[0] += Main.rand.NextFloat(10f); //random offset for color and sin wave
                projectile.ai[1] += Main.rand.NextFloat(0.25f, 1.25f);
            }
            projectile.ai[0] += 0.1f;
            //search for a stellanova that is active, in a charging state, owned by the same player, and is close enough. If it exists, home in on it and die, then charge it up
            var validprojs = Main.projectile.Where(x => x.type == ModContent.ProjectileType<NovaGunStar>() && x.active && x.ai[0] < 90 && x.ai[0] >= 30 && x.owner == projectile.owner && x.Distance(projectile.Center) < 500);
            if (validprojs.Any())
            {
                Projectile proj = validprojs.First();
                projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(proj.Center) * 9, 0.05f);
                originalvel = projectile.velocity; //dont switch back to original velocity when stellanova is gone
                if (projectile.Hitbox.Intersects(proj.Hitbox))
                {
                    projectile.Kill();
                    proj.ai[0] = 30;
                    proj.timeLeft = 360;
                    Main.PlaySound(SoundID.Item15, proj.Center);
                    if (proj.ai[1] < 1.5f)
                        proj.ai[1] += 0.15f;
                    proj.netUpdate = true;
                }
            }
            else
                projectile.velocity = originalvel.RotatedBy(Math.Sin(projectile.ai[0]) * Math.PI / 8);

            if (Main.rand.Next(50) == 0)
                Gore.NewGore(projectile.Center, projectile.velocity / 4, mod.GetGoreSlot("Gores/StarjinxGore"), 0.75f);
        }

        private float Timer => Main.GlobalTime * 2 + projectile.ai[0];

        public void DoTrailCreation(TrailManager tM)
        {
            tM.CreateTrail(projectile, new StarjinxTrail(Timer, 2, 0.15f), new RoundCap(), new ArrowGlowPosition(), 42f, 200f, new DefaultShader());
            tM.CreateTrail(projectile, new StarjinxTrail(Timer, 2, 0.8f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
            tM.CreateTrail(projectile, new StarjinxTrail(Timer, 2, 0.8f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
            tM.CreateTrail(projectile, new StarjinxTrail(Timer, 2, 0.3f), new RoundCap(), new ArrowGlowPosition(), 42f, 40f, new DefaultShader());
        }

        public void DrawPrimShape(BasicEffect effect) => StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, projectile.scale * 15, Color.White);

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starHit").WithVolume(0.35f).WithPitchVariance(0.3f), projectile.Center);
        }
    }
}