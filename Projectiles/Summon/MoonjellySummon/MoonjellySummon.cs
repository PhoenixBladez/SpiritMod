using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.MoonjellySummon
{
	public class MoonjellySummon : Minion
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Preserver");
			Main.projFrames[projectile.type] = 10;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 50;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.timeLeft = 99999;
            projectile.scale = .8f;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.netImportant = true;
		}

		public override void CheckActive() { }

		public override void SelectFrame()
		{
			projectile.frameCounter++;

			if (projectile.frameCounter >= 10f)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;

				if (projectile.frame >= 10)
					projectile.frame = 0;
			}
		}

		int counter;
        float alphaCounter;

		public override void Behavior()
		{
			alphaCounter += .04f;
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (player.dead)
				modPlayer.lunazoa = false;

			if (modPlayer.lunazoa)
				projectile.timeLeft = 2;

			int summonTime = (int)(34 / (.33f * projectile.minionSlots));
			if (summonTime >= 110)
				summonTime = 110;

			counter++;
			if (counter % summonTime == 0)
			{
				Vector2 vel = Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * new Vector2(5f, 3f);

				int p = Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-50, 50), projectile.Center.Y + Main.rand.Next(-50, 50), vel.X, vel.Y, ModContent.ProjectileType<LunazoaOrbiter>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0.0f, (float)projectile.whoAmI);
				Main.projectile[p].scale = Main.rand.NextFloat(.4f, 1f);
				Main.projectile[p].timeLeft = (int)(62 / (.33f * projectile.minionSlots));
				counter = 0;
			}

			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);

			projectile.rotation = projectile.velocity.X * 0.025f;
			float num = projectile.width * 1.1f;
			for (int i = 0; i < 1000; i++)
			{
				Projectile current = Main.projectile[i];
				if (i != projectile.whoAmI && current.active && projectile.owner == current.owner && projectile.type == current.type && Math.Abs(projectile.position.X - current.position.X) + Math.Abs(projectile.position.Y - current.position.Y) < num)
				{
					if (projectile.position.X < Main.projectile[i].position.X)
						projectile.velocity.X -= 0.08f;
					else
						projectile.velocity.X += 0.08f;

					if (projectile.position.Y < Main.projectile[i].position.Y)
						projectile.velocity.Y -= 0.08f;
					else
						projectile.velocity.Y += 0.08f;
				}
			}

			float num21 = 920f;
			bool flag = false;

			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (nPC.CanBeChasedBy(this, false))
				{
					float num3 = Vector2.Distance(nPC.Center, projectile.Center);
					if ((num3 < num21 || !flag) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height))
					{
						num21 = num3;
						flag = true;
					}
				}
			}

			if (Vector2.Distance(player.Center, projectile.Center) > (flag ? 1000f : 500f))
			{
				projectile.ai[0] = 1f;
				projectile.netUpdate = true;
			}

			if (!Collision.CanHitLine(projectile.Center, 1, 1, player.Center, 1, 1))
				projectile.ai[0] = 1f;

			float num4 = 6f;
			if (projectile.ai[0] == 1f)
				num4 = 15f;

			Vector2 center = projectile.Center;
			Vector2 vector = player.Center - center;
			projectile.ai[1] = 3600f;
			projectile.netUpdate = true;
			int num5 = 1;
			for (int k = 0; k < projectile.whoAmI; k++)
			{
				if (Main.projectile[k].active && Main.projectile[k].owner == projectile.owner && Main.projectile[k].type == projectile.type)
					num5++;
			}
			vector.X -= 0;
			vector.Y -= 70f;
			float num6 = vector.Length();

			if (num6 > 200f && num4 < 9f)
				num4 = 9f;

			if (num6 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
			{
				projectile.ai[0] = 0f;
				projectile.netUpdate = true;
			}

			if (num6 > 2000f)
				projectile.Center = player.Center;

			if (num6 > 48f)
			{
				vector.Normalize();
				vector *= num4;
				float num7 = 10f;
				projectile.velocity = (projectile.velocity * num7 + vector) / (num7 + 1f);
			}
			else
			{
				projectile.direction = Main.player[projectile.owner].direction;
				projectile.velocity *= (float)Math.Pow(0.9, 2.0);
			}

			if (projectile.velocity.X > 0f)
				projectile.spriteDirection = projectile.direction = -1;
			else if (projectile.velocity.X < 0f)
				projectile.spriteDirection = projectile.direction = 1;

			if (projectile.ai[1] > 0f)
				projectile.ai[1] += 1f;

			if (projectile.ai[1] > 140f)
			{
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
		}


		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(44, 168, 67) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale;
				Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/Zones/StaminaZone");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                var effects = projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                Color color1 = Color.White * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;

                Vector2 vector15 = new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type] / 2));
                SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 vector33 = new Vector2(projectile.Center.X, projectile.Center.Y - 18) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity;
                Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(127 - projectile.alpha, 127 - projectile.alpha, 127 - projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightBlue);
                spriteBatch.Draw(mod.GetTexture("Projectiles/Summon/MoonjellySummon/MoonjellySummon_Glow"), drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color1, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);

                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);

                float sineAdd = (float)Math.Sin(alphaCounter) + 3;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (projectile.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                int xpos = (int)((projectile.Center.X + 38) - Main.screenPosition.X) - (int)(Main.projectileTexture[projectile.type].Width / 2);
                int ypos = (int)((projectile.Center.Y + 30) - Main.screenPosition.Y) - (int)(Main.projectileTexture[projectile.type].Width / 2);
                Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
                Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), projectile.rotation, ripple.Size() / 2f, projectile.scale * .8f, spriteEffects, 0);
            }
            return false;
        }
    }
}
