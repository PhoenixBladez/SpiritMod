using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	public class SnowMongerBeam : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Icy Beam");

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
            Projectile.extraUpdates = 5;
			Projectile.tileCollide = false;
            Projectile.hide = true;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255), new Color(179, 222, 230)), new RoundCap(), new DefaultTrailPosition(), 10f, 900f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(79, 227, 255) * .46f, new Color(44, 140, 219) * .46f), new RoundCap(), new DefaultTrailPosition(), 30f, 500f, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.6f), new RoundCap(), new DefaultTrailPosition(), 12f, 80f, new DefaultShader());
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
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 226);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.scale = .5f;
            }
        }
	}
}