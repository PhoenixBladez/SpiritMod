using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class ReefSpearThrown : ModProjectile
	{
		public override string Texture => mod.Name + "/Items/Sets/ReefhunterSet/Projectiles/ReefSpearProjectile";

		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Reefe Tridente");

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.aiStyle = 0;
		}

		public override bool CanDamage() => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			if (!hasTarget)
			{
				if (projectile.ai[0]++ > 60)
				{
					projectile.velocity.X *= 0.97f;
					projectile.velocity.Y += 0.3f;
				}

				projectile.rotation = projectile.velocity.ToRotation();
				projectile.velocity.Y += 0.05f;
			}
			else
			{
				NPC npc = Main.npc[(int)projectile.ai[1]];

				if (!npc.active)
				{
					projectile.netUpdate = true;
					projectile.tileCollide = true;
					projectile.timeLeft *= 2;
					projectile.velocity *= 0;

					hasTarget = false;
					return;
				}

				projectile.ai[0]++;
				float factor = 1 - (projectile.ai[0] / 10f);
				if (projectile.ai[0] >= 10f)
					factor = 0;

				relativePoint += projectile.velocity * factor * 0.1f;

				projectile.Center = npc.Center + relativePoint;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[0] = 0;
			projectile.ai[1] = target.whoAmI;
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			projectile.timeLeft = 240;

			target.AddBuff(BuffID.Poisoned, 300);

			hasTarget = true;
			relativePoint = projectile.Center - target.Center;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Vector2 pos = projectile.Center - new Vector2(-10, 10);
			pos += Vector2.Normalize(projectile.velocity) * 26;

			hitbox.X = (int)pos.X;
			hitbox.Y = (int)pos.Y;
		}
	}
}
