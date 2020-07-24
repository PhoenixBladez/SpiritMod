using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class EyeballBlue : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Watchful Eye");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 42;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.sentry = true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter % 20 == 0) {
				bool hasTarget = false;
				float maxRange = 1000f;
				NPC target = projectile.OwnerMinionAttackTargetNPC;
				if (target != null && target.CanBeChasedBy(projectile)) {
					float dist = projectile.Distance(target.Center);
					if (dist < maxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height)) {
						maxRange = dist;
						hasTarget = true;
					}
				}
				if (!hasTarget) {
					for (int i = 0; i < 200; i++) {
						if (Main.npc[i].CanBeChasedBy(this)) {
							float dist = projectile.Distance(Main.npc[i].Center);
							if (dist < maxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)) {
								maxRange = dist;
								target = Main.npc[i];
								hasTarget = true;
							}
						}
					}
				}
				if (hasTarget) {
					for (int i = 0; i < 8; i++)
						Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 91);
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= 2f;
					Main.dust[dust].velocity *= 3f;

					var angle = projectile.DirectionTo(target.Center);
					int p = Projectile.NewProjectile(projectile.Center, angle * 9f, ModContent.ProjectileType<BlueMoonBeam>(), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
					Main.projectile[p].friendly = true;
					Main.projectile[p].hostile = false;
				}
			}
			for (int i = 0; i < 2; i++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
			}
		}
	}
}
