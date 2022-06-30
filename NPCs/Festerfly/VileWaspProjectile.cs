using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Festerfly
{
	public class VileWaspProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly Hatchling");
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			Projectile.spriteDirection = -Projectile.direction;
			if (Projectile.frameCounter >= 4) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}
			int num1 = ModContent.NPCType<Vilemoth>();
			if (!Main.npc[(int)Projectile.ai[1]].active) {
				Projectile.velocity.X = -4 * Main.npc[(int)Projectile.ai[1]].spriteDirection;
				Projectile.velocity.Y = -3;
			}
			float num2 = 120f;
			float x = 0.85f;
			float y = 0.35f;
			bool flag2 = false;
			if ((double)Projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)Projectile.ai[1];
				if (Main.npc[index1].active && Main.npc[index1].type == num1) {
					if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
						Projectile.position = Projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
				}
				else {
					Projectile.ai[0] = num2;
					flag4 = false;
				}
				if (flag4 && !flag2) {
					Projectile.velocity = Projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, (Projectile.height / Main.projFrames[Projectile.type]) * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				var effects = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{

		}
	}
}
