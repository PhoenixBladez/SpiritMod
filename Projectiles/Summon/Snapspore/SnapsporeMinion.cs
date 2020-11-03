using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
namespace SpiritMod.Projectiles.Summon.Snapspore
{
	public class SnapsporeMinion : ModProjectile
    {
        double dist = 80;
        Vector2 direction = Vector2.Zero;
        int counter = 0;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapspore");
			Main.projFrames[base.projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 28;
			projectile.height = 28;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
        bool attackCooldown;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.X * .08f;
            bool flag64 = projectile.type == ModContent.ProjectileType<SnapsporeMinion>();
            Player player = Main.player[projectile.owner];
            if (projectile.Distance(player.Center) > 1500)
            {
                projectile.position = player.position + new Vector2(Main.rand.Next(-125, 126), Main.rand.Next(-125, 126));
            }
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (flag64)
            {
                if (player.dead)
                    modPlayer.snapsporeMinion = false;

                if (modPlayer.snapsporeMinion)
                    projectile.timeLeft = 2;
            }

            int range = 26;   //How many tiles away the projectile targets NPCs
            float lowestDist = float.MaxValue;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float dist = projectile.Distance(npc.Center);
                if (dist / 16 < range)
                {
                    projectile.ai[1] = npc.whoAmI;
                }
            }
            else
            {
                foreach (NPC npc in Main.npc)
                {
                    //if npc is a valid target (active, not friendly, and not a critter)
                    if (npc.active && !npc.friendly && npc.catchItem == 0)
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
                            }
                        }
                    }
                }
            }
            NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
            if (target.active && !target.friendly && target.type != NPCID.TargetDummy && target.type != NPCID.DD2LanePortal)
            {
                counter++;
                if (counter < 179)
                {
                    float num535 = 8f;

                    Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num536 = target.Center.X - vector38.X;
                    float num537 = target.Center.Y - vector38.Y;
                    float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
                    if (num538 > 2000f)
                    {
                        projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                        projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
                    }
                    if (num538 > 70f)
                    {
                        num538 = num535 / num538;
                        num536 *= num538;
                        num537 *= num538;
                        projectile.velocity.X = (projectile.velocity.X * 20f + num536) / 21f;
                        projectile.velocity.Y = (projectile.velocity.Y * 20f + num537) / 21f;
                    }
                    else
                    {
                        if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                        {
                            projectile.velocity.X = -0.15f;
                            projectile.velocity.Y = -0.05f;
                        }
                        projectile.velocity *= 1.01f;
                    }
					if (counter % 60 == 0)
                    {
                        projectile.velocity.Y -= 3.95f;
						for (int z = 0; z < 2; z++)
                        {
                            int a = Gore.NewGore(new Vector2(projectile.Center.X + Main.rand.Next(-10, 10), projectile.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), 915, Main.rand.NextFloat(.4f, .95f));
                            Main.gore[a].timeLeft = 10;
                        }
                    }
                }
				else
                {
                    projectile.friendly = true;
                    float num535 = 8f;
					
                    Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num536 = Main.player[projectile.owner].Center.X - vector38.X;
                    float num537 = Main.player[projectile.owner].Center.Y - vector38.Y - 60f;
                    float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
                    if (num538 > 2000f)
                    {
                        projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                        projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
                    }
                    if (num538 > 70f)
                    {
                        num538 = num535 / num538;
                        num536 *= num538;
                        num537 *= num538;
                        projectile.velocity.X = (projectile.velocity.X * 20f + num536) / 21f;
                        projectile.velocity.Y = (projectile.velocity.Y * 20f + num537) / 21f;
                    }
                    else
                    {
                        if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                        {
                            projectile.velocity.X = -0.15f;
                            projectile.velocity.Y = -0.05f;
                        }
                        projectile.velocity *= 1.01f;
                    }
                }
				if (counter == 150)
                {
                    for (int num621 = 0; num621 < 8; num621++)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height,
                            2, 0f, 0f, 100, default(Color), .7f);
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.PoisonGas>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(), 5f)];
                        dust.noGravity = true;
                        dust.velocity.X = dust.velocity.X * 0.3f;
                        dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
                    }
                    int amountOfProjectiles = Main.rand.Next(2, 4);
                    Main.PlaySound(SoundID.Item, projectile.Center, 95);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < amountOfProjectiles; ++i)
                        {
                            int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), ModContent.ProjectileType<Projectiles.Thrown.Artifact.Miasma>(), projectile.damage / 2, 1, Main.myPlayer, 0, 0);
                        }
                    }
                }
				if (counter > Main.rand.Next(300, 320))
                {

                    for (int z = 0; z < 2; z++)
                    {
                        int a = Gore.NewGore(new Vector2(projectile.Center.X + Main.rand.Next(-10, 10), projectile.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), 915, Main.rand.NextFloat(.4f, .95f));
                        Main.gore[a].timeLeft = 10;
                    }
                    projectile.velocity.Y -= 4.95f;
                    counter = 0;
                }
            }
            else
            {
				if (Main.rand.NextBool(320))
                {
                    projectile.velocity.Y -= 4.95f;
                    for (int z = 0; z < 2; z++)
                    {
                        int a = Gore.NewGore(new Vector2(projectile.Center.X + Main.rand.Next(-10, 10), projectile.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), 915, Main.rand.NextFloat(.4f, .95f));
                        Main.gore[a].timeLeft = 10;
                    }
                }
                projectile.friendly = true;
                float num535 = 8f;

                Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num536 = Main.player[projectile.owner].Center.X - vector38.X;
                float num537 = Main.player[projectile.owner].Center.Y - vector38.Y - 60f;
                float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
                if (num538 > 2000f)
                {
                    projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                    projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
                }
                if (num538 > 100f)
                {
                    num538 = num535 / num538;
                    num536 *= num538;
                    num537 *= num538;
                    projectile.velocity.X = (projectile.velocity.X * 20f + num536) / 21f;
                    projectile.velocity.Y = (projectile.velocity.Y * 20f + num537) / 21f;
                }
                else
                {
                    if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                    {
                        projectile.velocity.X = -0.015f;
                    }
                    projectile.velocity *= 1.0001f;
                }
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = projectile.GetAlpha(Color.White) * (((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.35f);
                float scale = projectile.scale * (float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length;

                spriteBatch.Draw(ModContent.GetTexture("SpiritMod/Projectiles/Summon/Snapspore/SnapsporeMinion_Trail"),
                projectile.oldPos[k] + drawOrigin - Main.screenPosition,
                new Rectangle(0, (Main.projectileTexture[projectile.type].Height / 2) * projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 2),
                color,
                projectile.rotation,
                new Vector2(Main.projectileTexture[projectile.type].Width / 2, Main.projectileTexture[projectile.type].Height / 4),
                scale, default, default);
            }
            return true;
        }
        public override bool MinionContactDamage()
		{
			return true;
		}

	}
}