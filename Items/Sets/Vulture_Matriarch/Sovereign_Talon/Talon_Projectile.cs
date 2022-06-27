using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
    public class Talon_Projectile : ModProjectile, IDrawAdditive
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gilded Wave");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        } 

        public override void SetDefaults()
        {
			Projectile.Size = new Vector2(20, 20);
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
			Projectile.scale = 3f;
			Projectile.timeLeft = 600;
        }

		public override void AI()
		{
			if(Main.rand.NextBool(3))
				MakeDusts();

			if (Projectile.ai[0] == 0)
			{
				if (Projectile.velocity.Length() < 12)
					Projectile.velocity *= 1.05f;

				Projectile.alpha = Math.Max(Projectile.alpha - 9, 0);
				if (Projectile.scale > 0.75f)
					Projectile.scale -= 0.05f;
				else
					Projectile.ai[0]++;
			}
			else
			{
				Projectile.alpha += 9;
				Projectile.velocity *= 0.98f;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}
			if (Projectile.timeLeft >= 560)
				Projectile.tileCollide = false;
			else
				Projectile.tileCollide = true;

		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(damage * Projectile.scale * 1.5f);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = (int)(damage * Projectile.scale * 1.5f);

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.ai[0] == 0;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = 1;
			Projectile.velocity = oldVelocity;
			Projectile.tileCollide = false;
			return false;
		}

		private void MakeDusts()
		{		
			Dust dust = Dust.NewDustPerfect(Projectile.Center + Vector2.Normalize(Projectile.velocity.RotatedBy(MathHelper.PiOver2)) * Main.rand.NextFloat(-40, 40) * Projectile.scale, DustID.Sandnado, Projectile.velocity * Main.rand.NextFloat() * Projectile.scale/3, 100, new Color(), 1f);
			dust.noGravity = true;
			dust.fadeIn = 1f;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
				Projectile.Center + Vector2.Normalize(Projectile.velocity.RotatedBy(MathHelper.PiOver2)) * 40 * Projectile.scale * 1.25f,
				Projectile.Center - Vector2.Normalize(Projectile.velocity.RotatedBy(MathHelper.PiOver2)) * 40 * Projectile.scale * 1.25f))
				return true;

			return base.Colliding(projHitbox, targetHitbox);
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(255, 236, 115, 200)) * 0.75f, 
				Projectile.velocity.ToRotation() + MathHelper.PiOver2, bloom.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

			for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				float opacity = 0.7f * ((ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type]);
				spriteBatch.Draw(tex, Projectile.oldPos[i] + Projectile.Size/2 - Main.screenPosition, null, Projectile.GetAlpha(new Color(255, 236, 115, 200)) * opacity, 
					Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width/2, tex.Height/4), Projectile.scale * opacity, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(255, 236, 115, 200)), 
				Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), Projectile.scale, SpriteEffects.None, 0);
		}
    }
}