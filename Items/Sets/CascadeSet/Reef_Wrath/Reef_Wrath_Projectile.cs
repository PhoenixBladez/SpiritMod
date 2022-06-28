using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Reef_Wrath
{
	public class Reef_Wrath_Projectile_1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Reef");
		}

		readonly int maxtime = 50;
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 250;
			Projectile.tileCollide = false;
			Projectile.timeLeft = maxtime;
		}

		int Number => int.Parse(Name.Remove(0, Name.Length - 1));
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int num2 = Main.rand.Next(20, 40);
			for (int index1 = 0; index1 < num2; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Blood, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Main.dust[index2].velocity *= 1.2f;
				--Main.dust[index2].velocity.Y;
				Main.dust[index2].velocity += Projectile.velocity;
				Main.dust[index2].noGravity = true;
			}
		}
		public override bool PreAI()
		{
			Projectile.position -= Projectile.velocity;
			return base.PreAI();
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.alpha > 0 && Projectile.timeLeft > 20)
				Projectile.alpha -= 25;
			
			if (Projectile.timeLeft <= 10)
				Projectile.alpha += 25;
		}

		public override bool PreDraw(ref Color lightColor) //dumb way of doing this but i didnt feel like making them all one projectile
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			float sinewave = 1.3f * (float)Math.Sin(Math.PI * (maxtime - Projectile.timeLeft) / maxtime);
			sinewave = Math.Min(sinewave, 1);
			Rectangle drawRect = new Rectangle(0, 0, (int)(sinewave * tex.Width), tex.Height);
			Vector2 position = Projectile.Center - Main.screenPosition;
			position += new Vector2(18 * (Number - 1), 0).RotatedBy(Projectile.rotation) * (1 - sinewave);
			position -= new Vector2(18, 0).RotatedBy(Projectile.rotation) * sinewave;
			position += new Vector2(18, 0).RotatedBy(Projectile.rotation);
			Main.spriteBatch.Draw(tex, position, drawRect, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}

	public class Reef_Wrath_Projectile_2 : Reef_Wrath_Projectile_1
	{

	}

	public class Reef_Wrath_Projectile_3 : Reef_Wrath_Projectile_1
	{

	}
}
