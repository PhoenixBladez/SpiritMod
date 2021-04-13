using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.NPCs.Vulture_Matriarch
{
    public class Sharp_Feather : ModProjectile
    {
		public int dustTimer = 0;
 		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crownoak Javelin");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        } 
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 36;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
			projectile.tileCollide = true;
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}
		
		public float maxTicks = 0f;
 
        public override void AI()
        {
			projectile.velocity.Y += 0.03f;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			for (int i = 0; i<1; i++)
			{
				int num7 = 4;
				float num9 = 6f;
				float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
				float amount1 = 0.5f;
				float num10 = 0.0f;
				float addY = 0f;
				float addHeight = -10f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (projectile.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Rectangle rectangle = texture.Frame(1, 1, 0, 0);
				Vector2 vector2_3 = new Vector2((float) (Main.projectileTexture[projectile.type].Width / 2), (float) (Main.projectileTexture[projectile.type].Height / 1 / 2));
				Vector2 position1 = projectile.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * projectile.scale / 2f + vector2_3 * projectile.scale + new Vector2(0.0f, addY + addHeight + projectile.gfxOffY);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Vector2 position2 = new Vector2 (projectile.Center.X * projectile.spriteDirection, projectile.Center.Y) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + projectile.rotation + num10).ToRotationVector2() * (float) (4.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * projectile.scale / 2f + vector2_3 * projectile.scale + new Vector2(0.0f, addY + addHeight + projectile.gfxOffY);
				}
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position1, new Microsoft.Xna.Framework.Rectangle?(rectangle), drawColor, projectile.rotation, vector2_3, projectile.scale*1f, spriteEffects, 0.0f);
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
			Vector2 usePos = projectile.position; 
			Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); 
			usePos += rotVector * 16f;
			
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(usePos, projectile.width, projectile.height, 85, 0f, 0f, 0, default(Color), 1f);
				Dust currentDust = Main.dust[dustIndex]; 
				currentDust.position = (currentDust.position + projectile.Center) / 2f;
				currentDust.velocity += rotVector * 2f;
				currentDust.velocity *= 0.5f;
				currentDust.noGravity = true;
				usePos -= rotVector * 8f;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}