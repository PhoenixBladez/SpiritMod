using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;

		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
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
            Player player = Main.player[Projectile.owner];
			if (!released)
			{
                Projectile.scale = MathHelper.Clamp(Projectile.ai[0] / 10, 0, 1);
			}
            if (!smashed)
            {
                if (player.direction == 1)
                {
                    radians += (double)((Projectile.ai[0] + 10) / 300);
                }
                else
                {
                    radians -= (double)((Projectile.ai[0] + 10) / 300);
                }
                Projectile.timeLeft = 60;
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
                Projectile.ai[0]+= 0.5f;
                if (Projectile.ai[0] >= chargeTime)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
                }
            }
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
			Projectile.Center = player.Center - (Vector2.UnitX.RotatedBy(radians) * 65);
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
                    Projectile.damage *= 6;
                }
                released = true;
                if (Projectile.ai[0] < chargeTime || releasedearly)
                {
                    releasedearly = true;
                    Projectile.friendly = false;
                    Projectile.scale -= 0.15f;
                    if (Projectile.scale < 0.15f)
                    {
                        Projectile.active = false;
                    }
                }
                else
                {

                    if ((Math.Abs(radians - throwingAngle) < 0.75f || Main.tile[(int)Projectile.Center.X / 16, (int)((Projectile.Center.Y + 24) / 16)].collisionType == 1) && !smashed)
                    {
                        smashed = true;
                        for (int i = 0; i < 100; i++)
                        {
                            Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, (Projectile.height / 2) + 24), ModContent.DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);
                        }
                        SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
                        SoundEngine.PlaySound(SoundID.NPCHit42, Projectile.Center);
                        player.GetModPlayer<MyPlayer>().Shake += 14;
                        Projectile.friendly = false;
                    }
				}
            }
		}
		public override bool PreDraw(ref Color lightColor)
        {
                Color color = lightColor;
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 4.3f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 0);
                if (Projectile.ai[0] >= chargeTime && Projectile.ai[1] == 0)
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
                        Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), color * alpha, (float)radians + 4.3f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);
                    }
                }
                return false;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
            if (Projectile.ai[1] == 0)
            {
			Player player = Main.player[Projectile.owner];
			if (target.Center.X > player.Center.X)
				hitDirection = 1;
			else
				hitDirection = -1;
            }
		}
	}
}