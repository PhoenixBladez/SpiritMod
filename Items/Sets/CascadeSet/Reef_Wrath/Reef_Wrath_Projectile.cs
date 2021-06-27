using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
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
			projectile.width = 18;
			projectile.height = 24;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.scale = 1f;
			projectile.penetrate = -1;
			projectile.alpha = 250;
			projectile.tileCollide = false;
			projectile.timeLeft = maxtime;
		}

		int Number => int.Parse(Name.Remove(0, Name.Length - 1));
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int num2 = Main.rand.Next(20, 40);
			for (int index1 = 0; index1 < num2; ++index1)
			{
				int index2 = Dust.NewDust(projectile.Center, 0, 0, 5, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Main.dust[index2].velocity *= 1.2f;
				--Main.dust[index2].velocity.Y;
				Main.dust[index2].velocity += projectile.velocity;
				Main.dust[index2].noGravity = true;
			}
		}
		public override bool PreAI()
		{
			projectile.position -= projectile.velocity;
			return base.PreAI();
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (projectile.alpha > 0 && projectile.timeLeft > 20)
				projectile.alpha -= 25;
			
			if (projectile.timeLeft <= 10)
				projectile.alpha += 25;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) //dumb way of doing this but i didnt feel like making them all one projectile
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			float sinewave = 1.3f * (float)Math.Sin(Math.PI * (maxtime - projectile.timeLeft) / maxtime);
			sinewave = Math.Min(sinewave, 1);
			Rectangle drawRect = new Rectangle(0, 0, (int)(sinewave * tex.Width), tex.Height);
			Vector2 position = projectile.Center - Main.screenPosition;
			position += new Vector2(18 * (Number - 1), 0).RotatedBy(projectile.rotation) * (1 - sinewave);
			position -= new Vector2(18, 0).RotatedBy(projectile.rotation) * sinewave;
			position += new Vector2(18, 0).RotatedBy(projectile.rotation);
			spriteBatch.Draw(tex, position, drawRect, projectile.GetAlpha(lightColor), projectile.rotation, tex.Size() / 2, projectile.scale, SpriteEffects.None, 0);
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
