using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon.MadHatter
{
	public class MadHat : ModProjectile
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mad Hat");
			Main.projFrames[base.Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 28;
			Projectile.height = 18;
			Projectile.timeLeft = 225;
			Projectile.light = 0.5f;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			Projectile.Kill();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int k = 0; k < 15; k++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}

			Vector3 RGB = new Vector3(0f, 0.5f, 1.5f);
			float multiplier = 1;
			float max = 2.25f;
			float min = 1.0f;
			RGB *= multiplier;
			if (RGB.X > max) {
				multiplier = 0.5f;
			}
			if (RGB.X < min) {
				multiplier = 1.5f;
			}
			Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

			timer++;
			int range = 650;   //How many tiles away the projectile targets NPCs

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (Player player in Main.player) {
				//if npc is a valid target (active, not friendly, and not a critter)
				if (player.active) {
					//if npc is within 50 blocks
					float dist = Projectile.Distance(player.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							Projectile.ai[1] = player.whoAmI;
						}
					}
				}
			}

			if (timer < 125) {
				Projectile.velocity.Y *= 0.98f;
				if (timer > 100) {
					int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
					Main.dust[dust].noGravity = true;
				}
			}
			int index = (int)Projectile.ai[1];
			if (timer == 125 && index >= 0 && index < Main.maxPlayers && Main.player[index].active) {
				Vector2 direction9 = Main.player[(int)Projectile.ai[1]].Center - Projectile.Center;
				direction9.Normalize();
				direction9.X *= 15f;
				direction9.Y *= 15f;
				Projectile.velocity = direction9;
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