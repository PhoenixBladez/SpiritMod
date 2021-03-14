using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Projectiles.Returning;
using Terraria.Audio;

namespace SpiritMod.Projectiles.Clubs
{
	public class BruteHammerProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brute Hammer");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;

		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.ownerHitCheck = true;
		}

		readonly int height = 80;
		readonly int width = 50;
		double radians = 0;
        int flickerTime = 0;
        float alphaCounter = 0;
		readonly int chargeTime = 50;
        bool released = false;
        bool smashed = false;
        bool releasedearly = false;
		public override void AI()
		{
			alphaCounter += 0.08f;
            Player player = Main.player[projectile.owner];
			if (!released)
			{
                projectile.scale = MathHelper.Clamp(projectile.ai[0] / 10, 0, 1);
			}
            if (!smashed)
            {
                if (player.direction == 1)
                {
                    radians += (double)((projectile.ai[0] + 10) / 300);
                }
                else
                {
                    radians -= (double)((projectile.ai[0] + 10) / 300);
                }
                projectile.timeLeft = 60;
            }
            if (radians > 6.28)
            {
                radians -= 6.28;
            }
            if (radians < -6.28)
            {
                radians += 6.28;
            }
			projectile.velocity = Vector2.Zero;
			if (projectile.ai[0] % 20 == 0)
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 19).WithPitchVariance(0.1f).WithVolume(0.5f), projectile.Center);

			if (projectile.ai[0] < chargeTime)
            {
                projectile.ai[0]+= 0.5f;
                if (projectile.ai[0] >= chargeTime)
                {
                    Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
                }
            }
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
			projectile.Center = player.Center - (Vector2.UnitX.RotatedBy(radians) * 65);
			player.itemTime = 4;
			player.itemAnimation = 4;
			player.itemRotation = 0;
			if (player.whoAmI == Main.myPlayer)
				player.ChangeDir(Math.Sign(direction.X));
			double throwingAngle = 3.14f;
            if (player.direction == -1)
                throwingAngle = -3.14f;
			if ((!player.channel && Math.Abs(radians) < 0.45) || released)
            {
                if (!released)
                {
                    projectile.damage *= 6;
                }
                released = true;
                if (projectile.ai[0] < chargeTime || releasedearly)
                {
                    releasedearly = true;
                    projectile.friendly = false;
                    projectile.scale -= 0.15f;
                    if (projectile.scale < 0.15f)
                    {
                        projectile.active = false;
                    }
                }
                else
                {

                    if ((Math.Abs(radians - throwingAngle) < 0.75f || Main.tile[(int)projectile.Center.X / 16, (int)((projectile.Center.Y + 24) / 16)].collisionType == 1) && !smashed)
                    {
                        smashed = true;
                        for (int i = 0; i < 100; i++)
                        {
                            Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, (projectile.height / 2) + 24), ModContent.DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
                        }
                        Main.PlaySound(SoundID.Item70, projectile.Center);
                        Main.PlaySound(SoundID.NPCHit42, projectile.Center);
                        player.GetModPlayer<MyPlayer>().Shake += 14;
                        projectile.friendly = false;
                    }
				}
            }
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
                Color color = lightColor;
                Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 4.3f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 0);
                if (projectile.ai[0] >= chargeTime && projectile.ai[1] == 0)
                {
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
                        Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), color * alpha, (float)radians + 4.3f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 1);
                    }
                }
                return false;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
            if (projectile.ai[1] == 0)
            {
			Player player = Main.player[projectile.owner];
			if (target.Center.X > player.Center.X)
				hitDirection = 1;
			else
				hitDirection = -1;
            }
		}
	}
}