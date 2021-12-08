using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class WeaverHeadTelegraph : ModProjectile, ITrailProjectile
	{
		public override string Texture => "Terraria/Projectile_1";

		private const int TrailLength = 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Weaver");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = TrailLength;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			Main.projFrames[projectile.type] = 6;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new StandardColorTrail(Color.Red * 0.66f), new RoundCap(), new DefaultTrailPosition(), 100f, 4000f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.2f, 1f, 1f));

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(10, 10);
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 21;
		}

		public override bool CanDamage() => false;

		public override void AI()
		{
			if (!Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Vector2.Normalize(projectile.velocity.RotatedByRandom(MathHelper.PiOver4)) * Main.rand.NextFloat(0.3f), Color.Pink, Color.Red, Main.rand.NextFloat(0.1f, 0.2f) * projectile.scale, 25));
		}
	}
}