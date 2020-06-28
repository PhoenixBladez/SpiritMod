using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TeethTop : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Fangs");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 14;
			projectile.friendly = false;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		int counter = 0;
		int counter2 = 0;
		static int closeSpeed = 9;
		public override void AI()
		{
			counter++;
			if(counter >= 30) {
				projectile.friendly = true;
				if(counter2 < projectile.ai[0]) {
					projectile.velocity.Y = closeSpeed * 2;
					counter2 += closeSpeed;
				} else {
					projectile.velocity.Y = 0;
				}
				if(counter == 33) {
					Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 2);
				}
			}
		}


		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<NightmareDust>());
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<SurgingAnguish>(), 180);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for(int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(160, 160, 160, 100);
		}
	}
}