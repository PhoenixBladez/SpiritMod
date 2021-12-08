using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	public class SnowMongerBeam : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Icy Beam");

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
            projectile.extraUpdates = 5;
			projectile.tileCollide = false;
            projectile.hide = true;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255), new Color(179, 222, 230)), new RoundCap(), new DefaultTrailPosition(), 10f, 900f, new DefaultShader());
			tManager.CreateTrail(projectile, new GradientTrail(new Color(79, 227, 255) * .46f, new Color(44, 140, 219) * .46f), new RoundCap(), new DefaultTrailPosition(), 30f, 500f, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.6f), new RoundCap(), new DefaultTrailPosition(), 12f, 80f, new DefaultShader());
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Frostburn, 180, true);
		}
		public override void AI()
        {
            if (Main.rand.Next(3) == 1)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 226);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.scale = .5f;
            }
        }
	}
}