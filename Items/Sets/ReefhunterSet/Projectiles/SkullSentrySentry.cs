using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentrySentry : ModProjectile
	{
		int[] eyeWhoAmIs = null;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Maneater");

		public override void SetDefaults()
		{
			projectile.width = 46;
			projectile.height = 64;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.scale = 0.75f;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (eyeWhoAmIs is null) //Init
			{
				DoDust();
				eyeWhoAmIs = new int[3];
				Vector2[] offsets = new Vector2[] { new Vector2(-10, -8) * projectile.scale, new Vector2(10, -8) * projectile.scale, new Vector2(0, 18) * projectile.scale };

				for (int i = 0; i < 3; ++i)
				{
					Vector2 pos = projectile.Center + offsets[i];
					int p = Projectile.NewProjectile(pos, Vector2.Zero, ModContent.ProjectileType<SkullSentryEye>(), Main.player[projectile.owner].HeldItem.damage, 0f, projectile.owner, -1, i * SkullSentryEye.SHOOT_TIME / 3);

					Projectile eye = Main.projectile[p];
					(eye.modProjectile as SkullSentryEye).anchor = offsets[i];
					(eye.modProjectile as SkullSentryEye).parent = projectile;
					eye.scale = projectile.scale;
					eye.netUpdate = true;

					eyeWhoAmIs[i] = p;
				}
			}

			foreach (Projectile p in GetEyeList()) //Refresh all eye targets at once per tick
				if (p.modProjectile is SkullSentryEye eye)
					eye.Target = -1;

			projectile.velocity.Y = (float)System.Math.Sin(projectile.timeLeft / 30f) / 6; //Subtle hovering
		}

		public override void Kill(int timeLeft)
		{
			DoDust();
			if (eyeWhoAmIs is null)
				return;

			foreach(Projectile p in GetEyeList())
				p.Kill();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);

			//Manually call drawing for eyes after this projectile's drawing is called (eyes have hide = true, so they don't automatically get drawn)
			List<Projectile> eyes = GetEyeList();
			foreach (Projectile p in eyes) //Draw all the chains before all the eyes
				(p.modProjectile as SkullSentryEye).DrawChain(spriteBatch);

			foreach (Projectile p in eyes) 
				(p.modProjectile as SkullSentryEye).Draw(spriteBatch, lightColor);

			return false;
		}

		public List<Projectile> GetEyeList()
		{
			List<Projectile> temp = new List<Projectile>();

			foreach (int i in eyeWhoAmIs)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == ModContent.ProjectileType<SkullSentryEye>() && proj != null)
					if ((proj.modProjectile as SkullSentryEye).parent == projectile)
						temp.Add(proj);
			}

			return temp;
		}

		private void DoDust()
		{
			const int dustAmount = 25;
			for(int i = 0; i < dustAmount; i++)
			{
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Stone, Main.rand.NextFloat(-0.2f, 0.2f), 
					Main.rand.NextFloat(-0.2f, 0.2f), 0, new Color(98, 180, 162), Main.rand.NextFloat(0.8f, 1.1f));
				Main.dust[d].noGravity = true;
			}
		}
	}
}
