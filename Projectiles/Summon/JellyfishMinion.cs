using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.Linq;

namespace SpiritMod.Projectiles.Summon
{
	public class JellyfishMinion : ModProjectile
    {
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Little Jellyfish");
			Main.projFrames[base.projectile.type] = 6;
			Main.projPet[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 28;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			aiType = ProjectileID.Raven;
		}
		
        int colorType;
        Color colorVer;
        bool showOutline;
        bool chosenColor;
		
		public override bool? CanCutTiles() {
		return false;
		}
		
		public override void AI()
		{
            showOutline = false;
			if (!chosenColor)
            {
                colorType = Main.rand.Next(0, 2);
                chosenColor = true;
            }
			if (colorType == 0)
            {
                colorVer = new Color(133 + Main.rand.Next(-10, 20), 177 + Main.rand.Next(-10, 20), 255 + Main.rand.Next(0, 10));
            }
			if (colorType == 1)
            {
                colorVer = new Color(248 + Main.rand.Next(-13, 6), 148 + Main.rand.Next(-10, 20), 255 + Main.rand.Next(-20, 0));
            }

			bool flag64 = projectile.type == ModContent.ProjectileType<Projectiles.Summon.JellyfishMinion>();
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
					modPlayer.jellyfishMinion = false;

				if (modPlayer.jellyfishMinion)
					projectile.timeLeft = 2;

			}

			foreach (Projectile p in Main.projectile.Where(x => x.active && x != null && x.type == projectile.type && x.owner == projectile.owner && x != projectile))
			{
				if (p.Hitbox.Intersects(projectile.Hitbox))
					projectile.velocity += projectile.DirectionFrom(p.Center) / 5;
			}

			float num527 = projectile.position.X;
			float num528 = projectile.position.Y;
			float num529 = 900f;
			bool flag19 = false;

			if (projectile.ai[0] == 0f) {
				for (int num531 = 0; num531 < 200; num531++) {
					if (Main.npc[num531].CanBeChasedBy(projectile, false)) {
						float num532 = Main.npc[num531].position.X + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 150 + (float)(Main.npc[num531].height / 2);
						float num534 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num532) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num533);
						if (num534 < num529 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num531].position, Main.npc[num531].width, Main.npc[num531].height)) {
							num529 = num534;
							num527 = num532;
							num528 = num533;
							flag19 = true;
						}
					}
				}
			}
			else {
				projectile.tileCollide = false;
			}

			if (!flag19) {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 10f)
                {
                    projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                    projectile.frameCounter = 0;
					if (projectile.frame >= 2)
                    {
                        projectile.frame = 0;
                    }
                }
                projectile.friendly = true;
				float num535 = 8f;
				if (projectile.ai[0] == 1f)
					num535 = 12f;

				Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num536 = Main.player[projectile.owner].Center.X - vector38.X;
				float num537 = Main.player[projectile.owner].Center.Y - vector38.Y - 60f;
				float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
				if (num538 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height)) {
					projectile.ai[0] = 0f;
				}
				if (num538 > 2000f) {
					projectile.position.X = Main.player[projectile.owner].Center.X - (projectile.width * .5f);
					projectile.position.Y = Main.player[projectile.owner].Center.Y - (projectile.width * .5f);
				}

				if (num538 > 70f) {
					num538 = num535 / num538;
					num536 *= num538;
					num537 *= num538;
					projectile.velocity.X = (projectile.velocity.X * 20f + num536) * (1f / 21f);
					projectile.velocity.Y = (projectile.velocity.Y * 20f + num537) * (1f / 21f);
				}
				else {
					if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f) {
						projectile.velocity.X = -0.05f;
						projectile.velocity.Y = -0.025f;
					}
					projectile.velocity *= 1.005f;
				}
				projectile.friendly = false;
				projectile.rotation = projectile.velocity.X * 0.05f;

				if (Math.Abs(projectile.velocity.X) > 0.1) {
					projectile.spriteDirection = -projectile.direction;
					return;
				}
			}

			else {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 6f)
                {
                    projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                    projectile.frameCounter = 0;
                    if (projectile.frame > 5 || projectile.frame < 4) 
                    {
                        projectile.frame = 3;
                    }
                }
                timer++;

                if (timer >= Main.rand.Next(50, 90))
                {
                    int range = 100;   //How many tiles away the projectile targets NPCs
                    float shootVelocity = 6.5f; //magnitude of the shoot vector (speed of arrows shot)
                    int shootSpeed = 20;

                    //TARGET NEAREST NPC WITHIN RANGE
                    float lowestDist = float.MaxValue;
                    for (int i = 0; i < 200; ++i)
                    {
                        NPC npc = Main.npc[i];
                        //if npc is a valid target (active, not friendly, and not a critter)
                        if (npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly)
                        {
                            //if npc is within 50 blocks
                            float dist = projectile.Distance(npc.Center);
                            if (dist / 16 < range)
                            {
                                //if npc is closer than closest found npc
                                if (dist < lowestDist)
                                {
                                    lowestDist = dist;

                                    //target this npc
                                    projectile.ai[1] = npc.whoAmI;
                                    projectile.netUpdate = true;
                                }
                            }
                        }
                    }
                    NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC());
                    Main.PlaySound(SoundID.Item, projectile.Center, 12);
                    int dustType;
                    timer = 0;
                    Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 13);
                    Vector2 direction = target.Center - ShootArea;
                    direction.Normalize();
                    direction.X *= shootVelocity;
                    direction.Y *= shootVelocity;
                    for (int i = 0; i < 10; i++)
                    {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), .5f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        if (Main.dust[num].position != projectile.Center)
                        {
                            Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 1.1f;
                        }
                        if (colorType == 0)
                        {
                            Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(96, Main.LocalPlayer);
                        }
						else
                        {
                            Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
                        }
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (colorType == 0)
                        {
                            int proj1 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, mod.ProjectileType("BlueJellyfishBolt"), projectile.damage, 0, Main.myPlayer);
                        }
                        else
                        {
                            int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, mod.ProjectileType("PinkJellyfishBolt"), projectile.damage, 0, Main.myPlayer);
                        }
                    }
                }
                if (projectile.ai[1] == -1f)
					projectile.ai[1] = 17f;

				if (projectile.ai[1] > 0f)
					projectile.ai[1] -= 1f;

				if (projectile.ai[1] == 0f) {
					projectile.friendly = true;
					float num539 = 8f;
					Vector2 vector39 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num540 = num527 - vector39.X;
					float num541 = num528 - vector39.Y;
					float num542 = (float)Math.Sqrt((double)(num540 * num540 + num541 * num541));
					if (num542 < 100f)
						num539 = 10f;

					num542 = num539 / num542;
					num540 *= num542;
					num541 *= num542;
					projectile.velocity.X = (projectile.velocity.X * 12.5f + num540) / 15f;
					projectile.velocity.Y = (projectile.velocity.Y * 12.5f + num541) / 15f;
				}
				else {
					projectile.friendly = false;
					if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 10f)
						projectile.velocity *= 1.05f;
				}
				projectile.rotation = projectile.velocity.X * 0.05f;

				if (Math.Abs(projectile.velocity.X) > 0.2) {
					projectile.spriteDirection = -projectile.direction;
					return;
				}
			}
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return colorVer;
        }
        public override bool MinionContactDamage()
		{
			return true;
		}

	}
}