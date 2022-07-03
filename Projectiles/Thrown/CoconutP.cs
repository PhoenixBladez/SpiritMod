using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class CoconutP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		private bool cracky = false;
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			//projectile.aiStyle = 8;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.light = 0;
			//		aiType = ProjectileID.ThrowingKnife;
		}
		public override void AI()
		{
			Projectile.rotation += .2f;
			Projectile.velocity.X *= .98f;
			if (Projectile.velocity.Y < 0) {
				Projectile.velocity.Y *= 0.95f;
			}
			if (!cracky)
				Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 7f && !cracky) {
				Projectile.damage = (int)(Projectile.damage * 2.85f);
				cracky = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit7, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item2, Projectile.Center);
			SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);
			if (cracky) {
				Vector2 GoreVel = Projectile.velocity;
				GoreVel.X = 2f;
				GoreVel.Y *= -0.2f;
				SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, GoreVel, Mod.Find<ModGore>("Gores/Coconut/CoconutGore1").Type, 1f);
				GoreVel.X = -2f;
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, GoreVel, Mod.Find<ModGore>("Gores/Coconut/CoconutGore2").Type, 1f);
			}
			else {
				Vector2 GoreVel = Projectile.velocity;
				if (Projectile.velocity.X > 0) {
					GoreVel.X = 2f;
				}
				else {
					GoreVel.X = 0f;
				}
				GoreVel.Y *= -0.2f;
				int g = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, GoreVel, Mod.Find<ModGore>("Gores/Coconut/CoconutGore").Type, 1f);
				Main.gore[g].timeLeft = 40;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (cracky) {
				Projectile.position.Y -= 20;
				crit = true;
				Projectile.damage *= 3;
				target.AddBuff(BuffID.Confused, 200);
			}
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