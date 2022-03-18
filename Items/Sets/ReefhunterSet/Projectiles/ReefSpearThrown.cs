using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class ReefSpearThrown : ModProjectile
	{
		public override string Texture => mod.Name + "/Items/Sets/ReefhunterSet/Projectiles/ReefSpearProjectile";

		private bool hasTarget = false;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Reefe Speare");

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

		public override void AI()
		{
			if (!hasTarget)
			{
				if (projectile.ai[0]++ > 60)
				{
					projectile.velocity.X *= 0.97f;
					projectile.velocity.Y += 0.15f;
				}

				projectile.rotation = projectile.velocity.ToRotation();
				projectile.velocity.Y += 0.1f;
			}
			else
			{

			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[1] = target.whoAmI;
			projectile.netUpdate = true;

			hasTarget = true;
		}
	}
}
