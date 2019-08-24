using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class EtherealArrow : ModProjectile
	{
		public const float GRAVITY = .05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Arrow");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 6;
			projectile.height = 16;
		}

		public override bool PreAI()
		{
			projectile.velocity.Y += GRAVITY;
			ProjectileExtras.LookAlongVelocity(this);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("EssenceTrap"), 540, true);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 229);
				Main.dust[dust].noGravity = true;
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}