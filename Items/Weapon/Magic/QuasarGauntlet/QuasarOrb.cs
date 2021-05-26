using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.QuasarGauntlet
{
	public class QuasarOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Orb");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.tileCollide = true;
			projectile.hide = false;
			//projectile.scale = 0.8f;
			projectile.extraUpdates = 1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			float speedboost = -1 - (0.5f / oldVelocity.Length());
			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = oldVelocity.X * speedboost;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = oldVelocity.Y * speedboost;

			Main.PlaySound(SoundID.Item10, projectile.Center);
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => projectile.Distance(targetHitbox.Center.ToVector2()) < (50 * (projectile.scale + 0.4f)); //circular collision
		public override void AI()
		{
			projectile.ai[1]++;
			if(projectile.ai[0] == 0)
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.0175f);
				if (projectile.ai[1] > 360)
					projectile.ai[0]++;
			}

			else
			{
				Player player = Main.player[projectile.owner];
				projectile.tileCollide = false;
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(player.Center) * 25, 0.05f);
				if (projectile.Hitbox.Intersects(player.Hitbox))
					projectile.Kill();
			}
			if (projectile.localAI[0] == 0)
				projectile.localAI[0] = projectile.scale;

			if (projectile.localAI[0] > 1.5f)
				projectile.localAI[0] = 1.5f;

			projectile.scale = MathHelper.Lerp(projectile.scale, projectile.localAI[0], 0.1f);

			if (Main.rand.Next(40) == 0)
				Gore.NewGore(projectile.Center, Main.rand.NextVector2Circular(3, 3), mod.GetGoreSlot("Gores/StarjinxGore"), 1);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => IncreaseDamage(ref damage);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => IncreaseDamage(ref damage);

        private void IncreaseDamage(ref int damage)
        {
            damage = (int)(damage * 1 + ((1 - projectile.scale) * 0.4f));
            if (projectile.localAI[0] < 1.5f)
                projectile.localAI[0] += 0.15f;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle rectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			Color color = SpiritMod.StarjinxColor(Main.GlobalTime * 2);
			Vector2 scale = new Vector2(1f - projectile.velocity.Length() / 50, 1f + projectile.velocity.Length() / 50) * projectile.scale;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, color, projectile.rotation, rectangle.Size() / 2, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.White, projectile.rotation, rectangle.Size() / 2, scale * 0.5f, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.Lerp(color, Color.Transparent, 0.5f), projectile.rotation, rectangle.Size() / 2,
				scale * ((float)Math.Sin(Main.GlobalTime * 3) / 10 + 1.3f), SpriteEffects.None, 0);

			/*for(int i = 0; i < 2; i++)
			{
				Vector2 offset = projectile.Center + (projectile.scale * new Vector2((50 + 5 * (float)Math.Sin(Main.GlobalTime * 4))/(1 + projectile.velocity.Length()/8), 0).RotatedBy(i * MathHelper.Pi + Main.GlobalTime * 6)) - Main.screenPosition;
				spriteBatch.Draw(tex, offset, new Rectangle?(rectangle), color, projectile.rotation, rectangle.Size() / 2, projectile.scale * 0.33f, SpriteEffects.None, 0);
			}*/
			return false;
		}
	}
}