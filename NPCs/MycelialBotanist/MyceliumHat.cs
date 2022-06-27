
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.MycelialBotanist
{
	public class MyceliumHat : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycelial Botanist");
			Main.projFrames[base.Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 900;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
		}
		//  bool comingHome = false;

		public override bool PreAI()
		{

			if (Projectile.soundDelay > 8) {
				Projectile.soundDelay = 0;
				SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}
			NPC npc = Main.npc[(int)Projectile.ai[0]];
			if (!initialized) {
				initialized = true;
				Xint = Projectile.velocity.X;
				Yint = Projectile.velocity.Y;
			}
			// projectile.velocity.Y += .0625f;
			if (Projectile.Hitbox.Intersects(npc.Hitbox) && Projectile.timeLeft < 855) {
				Projectile.active = false;
			}
			if (Projectile.timeLeft < 854) {
				Vector2 direction9 = npc.Center - Projectile.position;
				Projectile.velocity = Projectile.velocity.RotatedBy(direction9.ToRotation() - Projectile.velocity.ToRotation());
				if (Projectile.velocity.Length() < 0.5f) {
					direction9.Normalize();
					Projectile.velocity = direction9 * 0.8f;
				}
				if (Math.Sqrt((Projectile.velocity.X * Projectile.velocity.X) + (Projectile.velocity.Y * Projectile.velocity.Y)) < 18) {
					Projectile.velocity *= 1.075f;
				}
				Projectile.tileCollide = false;
			}
			else {
				Projectile.velocity -= new Vector2(Xint, Yint) / 45f;
			}
			return false;
		}
		//projectile.ai[0]: X speed initial
		//projectile.ai[1]: y speed initial
		float Xint = 0;
		float Yint = 0;
		bool initialized = false;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(target.position, target.width, target.height, DustID.Butterfly, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .85f;
				if (Main.dust[num].position != target.Center)
					Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 5f;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, (Projectile.height / Main.projFrames[Projectile.type]) * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				var effects = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			int amountOfProjectiles = Main.rand.Next(3, 5);
			bool expertMode = Main.expertMode;
			int damage = expertMode ? 8 : 13;
			for (int i = 0; i < amountOfProjectiles; ++i) {
				float A = (float)Main.rand.Next(-50, 50) * 0.02f;
				float B = (float)Main.rand.Next(-60, -40) * 0.1f;
				//int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, A, B, ModContent.ProjectileType<MyceliumSporeHostile>(), damage, 1);
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Harpy, A, B, 0, default, .61f);
				}
				//Main.projectile[p].hostile = true;
			}
			return true;
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
	}
}
