using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	public class SnowMongerBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icy Beam");
		}

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