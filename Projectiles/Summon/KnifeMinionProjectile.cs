using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class KnifeMinionProjectile : Minion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unbound Soul");
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 0f;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.netImportant = true;

			projectile.usesLocalNPCImmunity = true;
			//5a: projectile.localNPCHitCooldown = -1; // 1 hit per npc max
			projectile.localNPCHitCooldown = 20; // o
		}

		public override void CheckActive()
		{
			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
			if (mp.player.dead || !mp.rogueCrest)
				projectile.active = false;

			if (mp.rogueCrest)
				projectile.timeLeft = 2;

		}
        bool trailing = false;
        public override void Behavior()
        {
            projectile.minionSlots = 0f;
            projectile.rotation = projectile.velocity.X * 0.25f;
            trailing = false;
            projectile.tileCollide = false;
			Player player = Main.player[projectile.owner];
			float num = projectile.width * 1.1f;
			for (int i = 0; i < 1000; i++) {
				Projectile current = Main.projectile[i];
				if (i != projectile.whoAmI && current.active && projectile.owner == current.owner && projectile.type == current.type && Math.Abs(projectile.position.X - current.position.X) + Math.Abs(projectile.position.Y - current.position.Y) < num) {
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

			Vector2 value = projectile.position;
			float num21 = 500f;
			bool flag = false;
			projectile.tileCollide = false;
			for (int j = 0; j < 200; j++) {
				NPC nPC = Main.npc[j];
				if (nPC.CanBeChasedBy(this, false)) {
					float num3 = Vector2.Distance(nPC.Center, projectile.Center);
					if ((num3 < num21 || !flag) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height)) {
						num21 = num3;
						value = nPC.Center;
						flag = true;
					}
				}
			}

			if (Vector2.Distance(player.Center, projectile.Center) > (flag ? 1000f : 500f)) {
				projectile.ai[0] = 1f;
				projectile.netUpdate = true;
			}

			if (projectile.ai[0] == 1f)
				projectile.tileCollide = false;

			if (flag && projectile.ai[0] == 0f) {
				Vector2 value2 = value - projectile.Center;
				if (value2.Length() > 200f) {
					value2.Normalize();
					projectile.velocity = (projectile.velocity * 20f + value2 * 6f) / 21f;
				}
				else {
					projectile.velocity *= (float)Math.Pow(0.97, 2.0);
				}
                trailing = true;
                projectile.tileCollide = false;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
                bool flag25 = false;
                int jim = 1;
                for (int index1 = 0; index1 < 200; index1++)
                {
                    if (Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1))
                    {
                        float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
                        float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
                        float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
                        if (num25 < 500f)
                        {
                            flag25 = true;
                            jim = index1;
                        }

                    }
                }
                if (flag25)
                {
                    float num1 = 68.5f;
                    Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num2 = Main.npc[jim].Center.X - vector2.X;
                    float num3 = Main.npc[jim].Center.Y - vector2.Y;
                    Vector2 direction5 = Main.npc[jim].Center - projectile.Center;
                    direction5.Normalize();
                    projectile.rotation = projectile.DirectionTo(Main.npc[jim].Center).ToRotation() + 1.57f;
                    float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
                    float num5 = num1 / num4;
                    float num6 = num2 * num5;
                    float num7 = num3 * num5;
                    int num8 = 10;
                    if (Main.rand.Next(20) == 0)
                    {
                        projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
                        projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
                        projectile.netUpdate = true;
                    }
                }
            }
			else {
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
				for (int k = 0; k < projectile.whoAmI; k++) {
					if (Main.projectile[k].active && Main.projectile[k].owner == projectile.owner && Main.projectile[k].type == projectile.type)
						num5++;
				}
				vector.X -= (10 + num5 * 40) * player.direction;
				vector.Y -= 70f;
				float num6 = vector.Length();
				if (num6 > 200f && num4 < 9f)
					num4 = 9f;

				if (num6 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height)) {
					projectile.ai[0] = 0f;
					projectile.netUpdate = true;
				}
				if (num6 > 2000f)
					projectile.Center = player.Center;

				if (num6 > 48f) {
					vector.Normalize();
					vector *= num4;
					float num7 = 10f;
					projectile.velocity = (projectile.velocity * num7 + vector) / (num7 + 1f);
				}
				else {
					projectile.direction = Main.player[projectile.owner].direction;
					projectile.velocity *= (float)Math.Pow(0.9, 2.0);
				}
			}
			if (projectile.velocity.X > 0f)
				projectile.spriteDirection = (projectile.direction = -1);
			else if (projectile.velocity.X < 0f)
				projectile.spriteDirection = (projectile.direction = 1);

			if (projectile.ai[1] > 0f)
				projectile.ai[1] += 1f;

			if (projectile.ai[1] > 140f) {
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
		}

		public override void SelectFrame()
		{
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            if (trailing)
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color * .6f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f); ;
                }
            }
			if (!trailing)
            {
				return true;
            }
            return false;
        }

	}
}
