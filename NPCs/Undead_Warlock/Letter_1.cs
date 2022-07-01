using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Undead_Warlock
{
	public class Letter_1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forbidden Letter");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 22;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 360;
			Projectile.alpha = 100;
			Projectile.scale = 0.8f;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			if (Main.npc[(int)Projectile.ai[1]].life < Main.npc[(int)Projectile.ai[1]].lifeMax - 10)
			{
				Main.npc[(int)Projectile.ai[1]].life += 10;
				Main.npc[(int)Projectile.ai[1]].HealEffect(10, true);
			}
			Projectile.Kill();
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft < 260)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				float addHeight = -4f;
				float addWidth = 0f;
				Vector2 vector2_3 = new Vector2((float) (TextureAssets.Projectile[Projectile.type].Value.Width / 2), (float) (TextureAssets.Projectile[Projectile.type].Value.Height / 1 / 2));
				Texture2D texture2D = ModContent.Request<Texture2D>("SpiritMod/Effects/Circles", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				if (Projectile.velocity.X == 0)
				{
					addHeight = -8f;
					addWidth = -6f;
					texture2D = ModContent.Request<Texture2D>("SpiritMod/Effects/Circles");
				}
				Vector2 origin = new Vector2((float) (texture2D.Width / 2), (float) (texture2D.Height / 8 + 14));
				int num1 = (int) Projectile.ai[1] / 2;
				float num2 = -1.570796f * (float) Projectile.rotation;
				float amount = Projectile.ai[1] / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 4;
				for (int index = 10; index >= 0; --index)
				{
					Vector2 oldPo = Projectile.oldPos[index];
					Microsoft.Xna.Framework.Color color2 = Microsoft.Xna.Framework.Color.Lerp(Color.LightGreen, Color.Lime, amount);
					color2 = Microsoft.Xna.Framework.Color.Lerp(color2, Color.PaleGreen, (float) index / 4f);
					color2.A = (byte) (5.0 * (double) amount);
					color2.R = (byte) ((int) color2.R * (10 - index) / 20);
					color2.G = (byte) ((int) color2.G * (10 - index) / 20);
					color2.B = (byte) ((int) color2.B * (10 - index) / 20);
					color2.A = (byte) ((int) color2.A * (10 - index) / 20);
					color2 *= amount;
					int frameY = (num3 - index) % 4;
					if (frameY < 0)
						frameY += 4;
					Microsoft.Xna.Framework.Rectangle rectangle = texture2D.Frame(1, 4, 0, frameY);
					Main.spriteBatch.Draw(texture2D, new Vector2((float) ((double) Projectile.oldPos[index].X - (double) Main.screenPosition.X + (double) (Projectile.width / 2) - (double) TextureAssets.Projectile[Projectile.type].Value.Width * (double) Projectile.scale / 2.0 + (double) vector2_3.X * (double) Projectile.scale) + addWidth, (float) ((double) Projectile.oldPos[index].Y - (double) Main.screenPosition.Y + (double) Projectile.height - (double) TextureAssets.Projectile[Projectile.type].Value.Height * (double) Projectile.scale / (double) 1 + 4.0 + (double) vector2_3.Y * (double) Projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.05f, 1.2f, (float) ((10.0 - (double) index) / 30.0)), spriteEffects, 0.0f);
				}
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 100);
		}
		public override void AI()
		{
			Player player = Main.LocalPlayer;
			Projectile.alpha += 10;
			++Projectile.ai[1];
			bool flag2 = (double) Vector2.Distance(Projectile.Center, player.Center) > (double) 0f && (double) Projectile.Center.Y == (double) player.Center.Y;
			if ((double) Projectile.ai[1] >= (double) 10f && flag2)
			{
				Projectile.ai[1] = 0.0f;
			}
			if (Projectile.timeLeft == 260)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 9, 1f, 0f);
				Vector2 vector2_1 = Projectile.velocity * 0.98f;
				Projectile.velocity = vector2_1;
				float num3 = 9f;
				Vector2 vector2_3 = new Vector2(Projectile.position.X + (float) Projectile.width * 0.5f, Projectile.position.Y + (float) Projectile.height * 0.5f);
				float num4 = player.position.X + (float) (player.width / 2) - vector2_3.X;
				float num5 = player.position.Y + (float) (player.height / 2) - vector2_3.Y;
				float num6 = (float) Math.Sqrt((double) num4 * (double) num4 + (double) num5 * (double) num5);
				float num7 = num3 / num6;
				float num8 = num4 * num7;
				float num9 = num5 * num7;
				Projectile.velocity.X = num8;
				Projectile.velocity.Y = num9;
			}
		}
		public override void Kill(int timeLeft)
        {
			for (int i = 0; i<4; i++)
			{
				int index4 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
				Main.gore[index4].velocity *= 0.4f;
				Main.gore[index4].velocity.X += (float) Main.rand.Next(-10, 11) * 0.1f;
				Main.gore[index4].velocity.Y += (float) Main.rand.Next(-10, 11) * 0.1f;		
			}
			SoundEngine.PlaySound(SoundID.NPCDeath, (int)Projectile.position.X, (int)Projectile.position.Y, 3, 1f, 0f);
	
			Vector2 spinningpoint = new Vector2(0f, -3f).RotatedByRandom(3.14159274101257);
			float num1 = (float) 14*Projectile.scale;
			float num5 = (float) 10*Projectile.scale;
			Vector2 vector2 = new Vector2(1.1f, 1f);
			for (float num2 = 0.0f; (double) num2 < (double) num1; ++num2)
			{
				int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustID.ShadowbeamStaff, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[dustIndex].position = Projectile.Center;
				Main.dust[dustIndex].velocity = spinningpoint.RotatedBy(6.28318548202515 * (double) num2 / (double) num1, new Vector2()) * vector2 * (float) (0.800000011920929 + (double) Main.rand.NextFloat() * 0.400000005960464) * 2f;
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = 2f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				Dust dust = Dust.CloneDust(dustIndex);
				dust.scale /= 2f;
				dust.fadeIn /= 2f;		  
			}
			for (float ag = 0.0f; (double) ag < (double) num5; ++ag)
			{
				int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustID.ChlorophyteWeapon, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[dustIndex].position = Projectile.Center;
				Main.dust[dustIndex].velocity = spinningpoint.RotatedBy(6.28318548202515 * (double) ag / (double) num5, new Vector2()) * vector2 * (float) (0.800000011920929 + (double) Main.rand.NextFloat() * 0.400000005960464);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = 2f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				Dust dust = Dust.CloneDust(dustIndex);
				dust.scale /= 2f;
				dust.fadeIn /= 2f;		  
			}
		}
	}
}
