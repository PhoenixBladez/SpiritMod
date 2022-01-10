using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Magic
{
	public class SandWall : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Wall");
		}
		int counter = -180;
		float distance = 5f;
		int rotationalSpeed = 2;
		float initialSpeedMult = 1;
		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 15;
			projectile.height = 15;
			//projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.alpha = 255;
			projectile.timeLeft = 450;
			projectile.extraUpdates = 2;
			projectile.tileCollide = false;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(255, 236, 115, 200)), new RoundCap(), new DefaultTrailPosition(), 100f, 130f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new DefaultTrailPosition(), 12f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new DefaultTrailPosition(), 12f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new StandardColorTrail(Color.Gold * 0.4f), new RoundCap(), new DefaultTrailPosition(), 20f, 250f, new DefaultShader());
		}

		public override void AI()
		{
			if (projectile.timeLeft < 360) {
				projectile.tileCollide = true;
			}
			if (Main.rand.Next(3) == 1) {
				Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.GoldCoin);
				dust.velocity = Vector2.Zero;
				dust.noGravity = true;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.025f;
			initialSpeedMult += 0.01f;
			counter += rotationalSpeed;
			Vector2 initialSpeed = new Vector2(projectile.ai[0], projectile.ai[1]) * initialSpeedMult;
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(counter * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && target.velocity != Vector2.Zero && target.knockBackResist != 0) {
				target.velocity.Y = -4f;
			}
		}
	}
}
