using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Projectiles.Returning;
using Terraria.Audio;

namespace SpiritMod.Items.Sets.SlingHammerSubclass
{
	public abstract class SlingHammerProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
		}

		protected virtual int height => 60;
		protected virtual int width => 60;
		protected virtual int chargeTime => 50;
		protected virtual float chargeRate => 0.7f;
		protected virtual int thrownProj => ModContent.ProjectileType<SlagHammerProjReturning>();
		protected virtual float damageMult => 1.5f;
		protected virtual int throwSpeed => 16;


		protected double radians = 0;
		protected int flickerTime = 0;
		protected float alphaCounter = 0;
		protected bool released = false;
		public override void AI()
		{
			alphaCounter += 0.08f;
            Player player = Main.player[Projectile.owner];
			if (!released)
			{
                Projectile.scale = MathHelper.Clamp(Projectile.ai[0] / 10, 0, 1);
			}
            if (player.direction == 1)
            {
                radians += (double)((Projectile.ai[0] + 10) / 200);
            }
            else
            {
                radians -= (double)((Projectile.ai[0] + 10) / 200);
            }
            if (radians > 6.28)
            {
                radians -= 6.28;
            }
            if (radians < -6.28)
            {
                radians += 6.28;
            }
			Projectile.velocity = Vector2.Zero;
			if (Projectile.ai[0] % 20 == 0)
				SoundEngine.PlaySound(new LegacySoundStyle(SoundID.Item, 19).WithPitchVariance(0.1f).WithVolume(0.5f), Projectile.Center);

			if (Projectile.ai[0] < chargeTime)
            {
                Projectile.ai[0]+= chargeRate;
                if (Projectile.ai[0] >= chargeTime)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
                }
            }
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
            double throwingAngle = direction.ToRotation() + 3.14;
			Projectile.position = player.Center - (Vector2.UnitX.RotatedBy(radians) * 40) - (Projectile.Size / 2);
			player.itemTime = 4;
			player.itemAnimation = 4;
			player.itemRotation = MathHelper.WrapAngle((float)radians);
			if (player.whoAmI == Main.myPlayer)
				player.ChangeDir(Math.Sign(direction.X));
			if (player.direction != 1)
            {
                throwingAngle -= 6.28;
            }
			if (!player.channel || released)
            {
                if (Projectile.ai[0] < chargeTime || released)
                {
                    released = true;
                    Projectile.scale -= 0.15f;
                    if (Projectile.scale < 0.15f)
                    {
                        Projectile.active = false;
                    }
                }
                else
                {
                    Projectile.active = false;
                    direction *= throwSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, direction, thrownProj, (int)(Projectile.damage * damageMult), Projectile.knockBack, Projectile.owner);
				}
            }
		}
		public override bool PreDraw(ref Color lightColor)
        {
                Color color = lightColor;
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 0);
                if (Projectile.ai[0] >= chargeTime)
                {
                    Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height * 2, width, height), Color.White * 0.9f, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);

                    if (flickerTime < 16)
                    {
                        flickerTime++;
                        color = Color.White;
                        float flickerTime2 = (float)(flickerTime / 20f);
                        float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
                        if (alpha < 0)
                        {
                            alpha = 0;
                        }
                        Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), color * alpha, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);
                    }
                }
                return false;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			SafeModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
			Player player = Main.player[Projectile.owner];
			if (target.Center.X > player.Center.X)
				hitDirection = 1;
			else
				hitDirection = -1;
		}
		public virtual void SafeModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) { }
	}
}