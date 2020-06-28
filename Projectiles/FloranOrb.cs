using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class FloranOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Orb");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 4;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 300;
			projectile.light = 0;
			projectile.extraUpdates = 1;
		}

		Vector2 offset = new Vector2(60, 60);
		public float counter = -1440;
		float j = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach(var proj in list) {
				counter++;
				if(counter == 0) {
					counter = -1440;
				}
				projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			}
			j = projectile.ai[1] - 60;
			for(int i = 0; i < 100; i += 20) {
				float xdist = (int)(Math.Sin((i + j) * (Math.PI / 180)) * 15);
				float ydist = (int)(Math.Cos((i + j) * (Math.PI / 180)) * 15);
				Vector2 offset = new Vector2(xdist, ydist);
				Dust.NewDustPerfect(projectile.Center + offset, ModContent.DustType<FloranDust>(), Vector2.Zero);

			}
			//Making player variable "p" set as the projectile's owner

			//Factors for calculations
			double deg = (double)projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
			double rad = deg * (Math.PI / 180); //Convert degrees to radians
			double dist = 80; //Distance away from the player

			/*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
    		/distance for the desired distance away from the player minus the projectile's width   /
    		/and height divided by two so the center of the projectile is at the right place.     */
			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;

			//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
			projectile.ai[1] += 1f;
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<FloranDust>());
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
		}


		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}

	}
}