using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;
using SpiritMod.NPCs.Boss.MoonWizardTwo;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticWall : ModProjectile
	{
		public int InitialDistance;
		public NPC Parent;
		private float alphaCounter;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.hostile = true;
			projectile.friendly = false;

		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			alphaCounter += 0.04f;
			InitialDistance-=10;
			if (InitialDistance < 200)
			{
				projectile.active = false;
				Parent.ai[1] += 0.5f;
				if (Parent.modNPC is MoonWizardTwo modNPC)
					modNPC.cooldownCounter = 30;
			}
			Vector2 direction = player.Center - Parent.Center;
			direction.Normalize();
			direction = direction.RotatedBy(projectile.ai[0] * 0.5f);
			projectile.Center = Parent.Center + (direction * InitialDistance);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineStart = projectile.Center;
			int rightValue = (int)projectile.ai[1] - 1;
			float collisionpoint = 0f;
			if (rightValue < (double)Main.projectile.Length && rightValue != -1) {
				Projectile other = Main.projectile[rightValue];
				Vector2 lineEnd = other.Center;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, 30, ref collisionpoint))
					return true;
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .6f), SpriteEffects.None, 0f);
        }

        public override void Kill(int timeLeft)
        {
            DustHelper.DrawDustImage(projectile.Center, 226, 0.15f, "SpiritMod/Effects/DustImages/MoonSigil3", 1f);
        }
    }
}