using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Dragon
{
	public class DragonTailTwo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jade Dragon");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = 600;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 95;
			Projectile.DamageType = DamageClass.Magic;

			Projectile.width = Projectile.height = 32;

		}
		int num;
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(66 - (int)(num / 3 * 2), 245 - (int)(num / 3 * 2), 120 - (int)(num / 3 * 2), 255 - num);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreAI()
		{
			num += 4;
			Projectile.alpha += 6;
			Projectile.spriteDirection = 1;
			if (Projectile.ai[0] > 0) {
				Projectile.spriteDirection = 0;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (!Main.projectile[(int)Projectile.ai[1]].active) {
					Projectile.timeLeft = 0;
					Projectile.active = false;
					// NetMessage.SendData(28, -1, -1, "", projectile.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
				}
			}

			if (Projectile.ai[1] < (double)Main.projectile.Length) {
				// We're getting the center of this projectile.
				Vector2 projectileCenter = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				// Then using that center, we calculate the direction towards the 'parent projectile' of this projectile.
				float dirX = Main.projectile[(int)Projectile.ai[1]].position.X + (float)(Main.projectile[(int)Projectile.ai[1]].width / 2) - projectileCenter.X;
				float dirY = Main.projectile[(int)Projectile.ai[1]].position.Y + (float)(Main.projectile[(int)Projectile.ai[1]].height / 2) - projectileCenter.Y;
				// We then use Atan2 to get a correct rotation towards that parent projectile.
				Projectile.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				// We also get the length of the direction vector.
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				// We calculate a new, correct distance.
				float dist = (length - (float)Projectile.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				// Reset the velocity of this projectile, because we don't want it to move on its own
				Projectile.velocity = Vector2.Zero;
				// And set this projectiles position accordingly to that of this projectiles parent projectile.
				Projectile.position.X = Projectile.position.X + posX;
				Projectile.position.Y = Projectile.position.Y + posY;
			}
			return false;
		}
	}
}