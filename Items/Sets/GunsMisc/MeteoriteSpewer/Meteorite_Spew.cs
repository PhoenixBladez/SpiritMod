using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.MeteoriteSpewer
{
	public class Meteorite_Spew : ModProjectile
	{
		public bool hasCreatedSound = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteorite Spew");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 1;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 240;
			Projectile.hide = false;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(24, 60*3);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X = 0f;
			if (Main.rand.Next(3) == 0 && !Projectile.wet)
			{
				int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 3f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity.X = 0f;
				Main.dust[index].velocity.Y = -2f;
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 0);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (!Projectile.wet)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				float addHeight = -4f;
				float addWidth = 0f;
				Vector2 vector2_3 = new Vector2((float) (TextureAssets.Projectile[Projectile.type].Value.Width / 2), (float) (TextureAssets.Projectile[Projectile.type].Value.Height / 1 / 2));
				Texture2D texture2D = TextureAssets.Extra[55].Value;
				if (Projectile.velocity.X == 0)
				{
					addHeight = -8f;
					addWidth = -6f;
					texture2D = ModContent.Request<Texture2D>("SpiritMod/Textures/Flames", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value; ;
				}
				Vector2 origin = new Vector2((float) (texture2D.Width / 2), (float) (texture2D.Height / 8 + 14));
				int num1 = (int) Projectile.ai[1] / 2;
				float num2 = -1.570796f * (float) Projectile.rotation;
				float amount = Projectile.ai[1] / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 4;
				for (int index = 5; index >= 0; --index)
				{
					Vector2 oldPo = Projectile.oldPos[index];
					var color2 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Gold, Microsoft.Xna.Framework.Color.OrangeRed, amount);
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
					Microsoft.Xna.Framework.Rectangle rectangle = texture2D.Frame(1, 4, 0, frameY);
					Main.spriteBatch.Draw(texture2D, new Vector2((float) ((double) Projectile.oldPos[index].X - (double) Main.screenPosition.X + (double) (Projectile.width / 2) - (double) TextureAssets.Projectile[Projectile.type].Value.Width * (double) Projectile.scale / 2.0 + (double) vector2_3.X * (double) Projectile.scale) + addWidth, (float) ((double) Projectile.oldPos[index].Y - (double) Main.screenPosition.Y + (double) Projectile.height - (double) TextureAssets.Projectile[Projectile.type].Value.Height * (double) Projectile.scale / (double) 1 + 4.0 + (double) vector2_3.Y * (double) Projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.1f, 1.2f, (float) ((10.0 - (double) index) / 40.0)), spriteEffects, 0.0f);
				}
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 5; ++index)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 2f);
			}
			SoundEngine.PlaySound(SoundID.Trackable, (int)Projectile.position.X, (int)Projectile.position.Y, 6, 1f, 0f);
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.velocity.X < 0f)
				Projectile.spriteDirection = -1;
			else
				Projectile.spriteDirection = 1;
			++Projectile.ai[1];
			bool flag2 = (double) Vector2.Distance(Projectile.Center, player.Center) > (double) 0f && (double) Projectile.Center.Y == (double) player.Center.Y;
			if ((double) Projectile.ai[1] >= (double) 30f && flag2)
			{
				Projectile.ai[1] = 0.0f;
			}
			
			
			if (!hasCreatedSound)
			{
				hasCreatedSound = true;
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 34, 1f, 0f);
			}
			
			if (Projectile.wet)
			{
				Projectile.friendly = false;
			}
		}
	}
}