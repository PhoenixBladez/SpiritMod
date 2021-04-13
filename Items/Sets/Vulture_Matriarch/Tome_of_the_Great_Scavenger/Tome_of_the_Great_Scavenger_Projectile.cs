using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Items.Sets.Vulture_Matriarch.Tome_of_the_Great_Scavenger
{
    public class Tome_of_the_Great_Scavenger_Projectile : ModProjectile
    {
 		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crownoak Javelin");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        } 
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.friendly = true;
			projectile.magic = true;
            projectile.hostile = false;
			projectile.tileCollide = true;
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}
		
        public override void AI()
        {
			Lighting.AddLight(projectile.position, 0.3f, .15f, 0f);
			Player player = Main.player[projectile.owner];
			if (projectile.ai[0]==0)
				projectile.velocity.Y += 0.18f;
			else if (projectile.ai[0]==1)
				projectile.velocity.Y += 0.15f;
			else if (projectile.ai[0]==2)
				projectile.velocity.Y += 0.03f;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			++projectile.ai[1];
			bool flag2 = (double) Vector2.Distance(projectile.Center, player.Center) > (double) 0f && (double) projectile.Center.Y == (double) player.Center.Y;
			if ((double) projectile.ai[1] >= (double) 30f && flag2)
			{
				projectile.ai[1] = 0.0f;
			}
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (!projectile.wet)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				float addY = 0.0f;
				float addHeight = -4f;
				float addWidth = 0f;
				Vector2 vector2_3 = new Vector2((float) (Main.projectileTexture[projectile.type].Width / 2), (float) (Main.projectileTexture[projectile.type].Height / 1 / 2));
				Texture2D texture2D = ModContent.GetTexture("SpiritMod/Effects/Desert_Shadow");
				if (projectile.velocity.X == 0)
				{
					addHeight = -8f;
					addWidth = -6f;
					texture2D = ModContent.GetTexture("SpiritMod/Items/Sets/Vulture_Matriarch/Tome_of_the_Great_Scavenger/Tome_of_the_Great_Scavenger_Projectile_Glow");
				}
				Vector2 origin = new Vector2((float) (texture2D.Width / 2), (float) (texture2D.Height / 8 + 14));
				int num1 = (int) projectile.ai[1] / 2;
				float num2 = -1.570796f * (float) projectile.rotation;
				float amount = projectile.ai[1] / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 1;
				for (int index = 5; index >= 0; --index)
				{
					Vector2 oldPo = projectile.oldPos[index];
					Microsoft.Xna.Framework.Color color2 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.White, Microsoft.Xna.Framework.Color.Gold, amount);
					color2 = Microsoft.Xna.Framework.Color.Lerp(color2, Microsoft.Xna.Framework.Color.Blue, (float) index / 12f);
					color2.A = (byte) (64.0 * (double) amount);
					color2.R = (byte) ((int) color2.R * (10 - index) / 20);
					color2.G = (byte) ((int) color2.G * (10 - index) / 20);
					color2.B = (byte) ((int) color2.B * (10 - index) / 20);
					color2.A = (byte) ((int) color2.A * (10 - index) / 20);
					color2 *= amount;
					int frameY = (num3 - index) % 4;
					if (frameY < 0)
						frameY += 4;
					Microsoft.Xna.Framework.Rectangle rectangle = texture2D.Frame(1, 1, 0, frameY);
					Main.spriteBatch.Draw(texture2D, new Vector2((float) ((double) projectile.oldPos[index].X - (double) Main.screenPosition.X + (double) (projectile.width / 2) - (double) Main.projectileTexture[projectile.type].Width * (double) projectile.scale / 2.0 + (double) vector2_3.X * (double) projectile.scale) + addWidth, (float) ((double) projectile.oldPos[index].Y - (double) Main.screenPosition.Y + (double) projectile.height - (double) Main.projectileTexture[projectile.type].Height * (double) projectile.scale / (double) 1 + 1.0 + (double) vector2_3.Y * (double) projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.1f, 1.2f, (float) ((10.0 - (double) index) / 9.0)), spriteEffects, 0.0f);
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
			Vector2 usePos = projectile.position; 
			Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); 
			usePos += rotVector * 16f;
			
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(usePos, projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f);
				Dust currentDust = Main.dust[dustIndex]; 
				currentDust.position = (currentDust.position + projectile.Center) / 2f;
				currentDust.velocity += rotVector * 2f;
				currentDust.velocity *= 0.5f;
				currentDust.noGravity = true;
				usePos -= rotVector * 8f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			{
				if (Main.rand.Next(4) == 0 && !target.SpawnedFromStatue) {
					target.AddBuff(BuffID.Midas, 180);
				}
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}