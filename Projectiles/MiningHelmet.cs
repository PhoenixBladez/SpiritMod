using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class MiningHelmet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light");
		}
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			/*Vector2 vector2_1 = new Vector2(projectile.position.X + (float) projectile.width * 0.5f, projectile.position.Y + (float) projectile.height * 0.5f);
            float num2 = (float) Main.mouseX + Main.screenPosition.X - vector2_1.X;
            float num3 = (float) Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
			projectile.rotation = new Vector2(num2, num3).ToRotation();*/
			Player player = Main.player[Projectile.owner];
			Projectile.Center = player.Center;
			if (player.active && player.head == 11)
			{
				Projectile.active = true;
			}
			else
			{
				Projectile.active = false;
			}

			if (player.direction == -1)
			{
				Projectile.position.X = player.position.X - 10;
			}
			else if (player.direction == 1)
			{
				Projectile.position.X = player.position.X + 20;
			}

			Projectile.position.Y = player.MountedCenter.Y - 19;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 position1 = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D1 = TextureAssets.Projectile[Projectile.type].Value;
			Microsoft.Xna.Framework.Rectangle r = texture2D1.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Microsoft.Xna.Framework.Color alpha = Projectile.GetAlpha(lightColor);
			Vector2 origin1 = r.Size() / 2f;
			origin1.X = (float)(r.Width - 6);
			--origin1.Y;
			r.Height -= 2;
			Texture2D texture2D3 = ModContent.Request<Texture2D>("SpiritMod/Effects/Mining_Helmet");
			//Texture2D texture2D3 = Main.glowMaskTexture[251];
			Microsoft.Xna.Framework.Color color4 = new Color(100, 100, 0)
			{
				A = (byte)0
			};
			color4 *= 1.13f;
			origin1 = texture2D3.Size() / 2f;
			if (player.direction == -1)
			{
				Main.spriteBatch.Draw(texture2D3, position1, new Microsoft.Xna.Framework.Rectangle?(), color4, Projectile.rotation - 1.570796f, origin1, Projectile.scale * 0.9f, SpriteEffects.None, 0.0f);
			}
			else if (player.direction == 1)
			{
				Main.spriteBatch.Draw(texture2D3, position1, new Microsoft.Xna.Framework.Rectangle?(), color4, Projectile.rotation - 1.570796f, origin1, Projectile.scale * 0.9f, SpriteEffects.FlipVertically, 0.0f);
			}

			return false;
		}
	}
}