using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HungryMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hungry");
			Main.projFrames[base.projectile.type] = 3;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 22;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.netImportant = true;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.minionSlots = 1;
			projectile.timeLeft = 18000;
		}

		public override bool PreAI()
		{
			bool flag64 = projectile.type == mod.ProjectileType("HungryMinion");
			Player player = Main.player[projectile.owner];
			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			if (mp.player.dead)
				mp.hungryMinion = false;

			if (mp.hungryMinion)
				projectile.timeLeft = 2;


			// Has a target
			if (projectile.ai[0] >= 0)
			{
				NPC target = Main.npc[(int)projectile.ai[0]];

				if (target.active && (target.position - mp.player.position).Length() <= 220)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter >= 6)
					{
						projectile.frame++;
						if (projectile.frame >= Main.projFrames[projectile.type])
							projectile.frame = 1;
						projectile.frameCounter = 0;
					}

					float speed = 16;

					Vector2 projCenter = new Vector2(projectile.position.X + projectile.width * 0.5F, projectile.position.Y + projectile.height * 0.5F);
					float xDir = target.position.X + (target.width * 0.5F) - projCenter.X;
					float yDir = target.position.Y + (target.height * 0.5F) - projCenter.Y;
					float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

					if (length < speed)
					{
						projectile.velocity.X = xDir;
						projectile.velocity.Y = yDir;

						if (length > speed / 2)
							projectile.rotation = (mp.player.Center - projectile.Center).ToRotation();
					}
					else
					{
						length = speed / length;
						xDir *= length;
						yDir *= length;
						projectile.velocity.X = xDir;
						projectile.velocity.Y = yDir;

						projectile.rotation = (mp.player.Center - projectile.Center).ToRotation();
					}
				}
				else
				{
					projectile.ai[0] = -1;
				}
			}
			else // Does not have a target.
			{
				for (int i = 0; i < 200; ++i)
				{
					if (Main.npc[i].active && Main.npc[i].CanBeChasedBy(projectile) && (Main.npc[i].position - mp.player.position).Length() <= 320)
					{
						projectile.ai[0] = i;
						break;
					}
				}

				projectile.frame = 0;

				float acceleration = 0.2f;
				Vector2 direction = mp.player.Center - projectile.Center;
				if (direction.Length() < 200f)
					acceleration = 0.12f;
				if (direction.Length() < 140f)
					acceleration = 0.06f;
				if (direction.Length() > 100f)
				{
					if (Math.Abs(mp.player.Center.X - projectile.Center.X) > 20f)
						projectile.velocity.X = projectile.velocity.X + acceleration * (float)Math.Sign(mp.player.Center.X - projectile.Center.X);

					if (Math.Abs(mp.player.Center.Y - projectile.Center.Y) > 10f)
						projectile.velocity.Y = projectile.velocity.Y + acceleration * (float)Math.Sign(mp.player.Center.Y - projectile.Center.Y);

				}
				else if (projectile.velocity.Length() > 2f)
				{
					projectile.velocity *= 0.96f;
				}

				if (Math.Abs(projectile.velocity.Y) < 1f)
					projectile.velocity.Y -= 0.1f;

				float maxSpeed = 15f;
				if (projectile.velocity.Length() > maxSpeed)
					projectile.velocity = projectile.velocity.SafeNormalize(Vector2.UnitY) * maxSpeed;

				projectile.rotation = (mp.player.Center - projectile.Center).ToRotation();
			}

			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player owner = Main.player[projectile.owner];
			float targetX = owner.position.X + owner.width * 0.5F;
			float targetY = owner.position.Y + owner.height * 0.5F;
			bool flag2 = false;

			Vector2 vector2 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
			float targetDirX = targetX - vector2.X;
			float targetDirY = targetY - vector2.Y;
			float rotation = (float)Math.Atan2(targetDirY, targetDirX) - 1.57f;
			bool flag3 = true;
			while (flag3)
			{
				SpriteEffects effects = SpriteEffects.None;
				if (flag2)
				{
					effects = SpriteEffects.FlipHorizontally;
					flag2 = false;
				}
				else
					flag2 = true;

				int height = 28;
				float num11 = (float)Math.Sqrt(targetDirX * targetDirX + targetDirY * targetDirY);
				if (num11 < 24)
				{
					height = (int)num11 - 24 + 28;
					flag3 = false;
				}
				num11 = 28f / num11;
				targetDirX *= num11;
				targetDirY *= num11;
				vector2.X += targetDirX;
				vector2.Y += targetDirY;
				targetDirX = targetX - vector2.X;
				targetDirY = targetY - vector2.Y;
				Color color2 = Lighting.GetColor((int)vector2.X / 16, (int)(vector2.Y / 16f));
				Main.spriteBatch.Draw(Main.chain12Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain4Texture.Width, height)), color2, rotation, new Vector2((float)Main.chain4Texture.Width * 0.5f, (float)Main.chain4Texture.Height * 0.5f), 1f, effects, 0f);
			}
			return true;
		}

	}
}
