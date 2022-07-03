using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        } 
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 36;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
			Projectile.tileCollide = true;
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}
		
		public float maxTicks = 0f;
 
        public override void AI()
        {
			Projectile.velocity.Y += 0.03f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
		public override bool PreDraw(ref Color lightColor)
		{
			for (int i = 0; i<1; i++)
			{
				int num7 = 4;
				float num8 = (float) (Math.Cos((double) Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
				float num10 = 0.0f;
				float addY = 0f;
				float addHeight = -10f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (Projectile.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
				Microsoft.Xna.Framework.Rectangle rectangle = texture.Frame(1, 1, 0, 0);
				Vector2 vector2_3 = new Vector2((float) (TextureAssets.Projectile[Projectile.type].Value.Width / 2), (float) (TextureAssets.Projectile[Projectile.type].Value.Height / 1 / 2));
				Vector2 position1 = Projectile.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * Projectile.scale / 2f + vector2_3 * Projectile.scale + new Vector2(0.0f, addY + addHeight + Projectile.gfxOffY);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Vector2 position2 = new Vector2 (Projectile.Center.X * Projectile.spriteDirection, Projectile.Center.Y) + ((float) (index2 / num7 * 6.28318548202515f) + Projectile.rotation + num10).ToRotationVector2() * (float) (4.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * Projectile.scale / 2f + vector2_3 * Projectile.scale + new Vector2(0.0f, addY + addHeight + Projectile.gfxOffY);
				}
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, position1, rectangle, lightColor, Projectile.rotation, vector2_3, Projectile.scale*1f, spriteEffects, 0.0f);
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
			Vector2 usePos = Projectile.position; 
			Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); 
			usePos += rotVector * 16f;
			
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(usePos, Projectile.width, Projectile.height, DustID.UnusedBrown, 0f, 0f, 0, default, 1f);
				Dust currentDust = Main.dust[dustIndex]; 
				currentDust.position = (currentDust.position + Projectile.Center) / 2f;
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