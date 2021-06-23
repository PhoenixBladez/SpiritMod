using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
    public class Talon_Projectile : ModProjectile, IDrawAdditive
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gilded Wave");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        } 

        public override void SetDefaults()
        {
			projectile.Size = new Vector2(20, 20);
			projectile.penetrate = -1;
			projectile.extraUpdates = 2;
			projectile.scale = 1f;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.alpha = 255;
			projectile.ignoreWater = true;
			projectile.hide = true;
			projectile.scale = 3f;
        }

		public override void AI()
		{
			if(Main.rand.NextBool(3))
				MakeDusts();

			if (projectile.ai[0] == 0)
			{
				if (projectile.velocity.Length() < 12)
					projectile.velocity *= 1.05f;

				projectile.alpha = Math.Max(projectile.alpha - 9, 0);
				if (projectile.scale > 0.75f)
					projectile.scale -= 0.05f;
				else
					projectile.ai[0]++;
			}
			else
			{
				projectile.alpha += 9;
				projectile.velocity *= 0.98f;
				if (projectile.alpha >= 255)
					projectile.Kill();
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(damage * projectile.scale * 1.5f);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = (int)(damage * projectile.scale * 1.5f);

		public override bool CanDamage() => projectile.ai[0] == 0;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.ai[0] = 1;
			projectile.velocity = oldVelocity;
			projectile.tileCollide = false;
			return false;
		}

		private void MakeDusts()
		{		
			Dust dust = Dust.NewDustPerfect(projectile.Center + Vector2.Normalize(projectile.velocity.RotatedBy(MathHelper.PiOver2)) * Main.rand.NextFloat(-40, 40) * projectile.scale, DustID.Sandnado, projectile.velocity * Main.rand.NextFloat() * projectile.scale/3, 100, new Color(), 1f);
			dust.noGravity = true;
			dust.fadeIn = 1f;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(new Color(255, 236, 115, 200)) * 0.75f, 
				projectile.velocity.ToRotation() + MathHelper.PiOver2, bloom.Size() / 2, projectile.scale, SpriteEffects.None, 0);

			for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float opacity = 0.7f * ((ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type]);
				spriteBatch.Draw(tex, projectile.oldPos[i] + projectile.Size/2 - Main.screenPosition, null, projectile.GetAlpha(new Color(255, 236, 115, 200)) * opacity, 
					projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width/2, tex.Height/4), projectile.scale * opacity, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(new Color(255, 236, 115, 200)), 
				projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), projectile.scale, SpriteEffects.None, 0);
		}
    }
}