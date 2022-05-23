using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reef Trident");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

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
				projectile.velocity.Y += 0.3f;

				projectile.rotation = projectile.velocity.ToRotation();
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

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D projTex = Main.projectileTexture[projectile.type];
			const int halfTipWidth = 15;
			Vector2 drawOrigin = new Vector2(projectile.spriteDirection > 0 ? projTex.Width - halfTipWidth : halfTipWidth, projTex.Height / 2);
			if(!hasTarget)
				projectile.QuickDrawTrail(spriteBatch, 0.25f, drawOrigin: drawOrigin);

			projectile.QuickDraw(spriteBatch, drawOrigin: drawOrigin);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, projectile.Center);
			Vector2 goreVel = hasTarget ? Vector2.Zero : projectile.oldVelocity / 3;
			Vector2 pos = projectile.Center;
			for (int i = 1; i <= 6; i++)
			{
				if (i >= 4)
					pos -= Vector2.Normalize(projectile.oldVelocity) * (15 + (i - 3) * 3);

				Gore g = Gore.NewGorePerfect(pos, goreVel, mod.GetGoreSlot("Gores/Projectiles/ReefTrident/Trident" + i));
				g.timeLeft = 0;
				g.rotation = projectile.rotation;
			}
		}
	}
}
