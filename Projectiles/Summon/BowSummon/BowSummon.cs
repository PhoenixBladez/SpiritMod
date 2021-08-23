using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
namespace SpiritMod.Projectiles.Summon.BowSummon
{
	public class BowSummon : ModProjectile
    {
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jinxbow");
			Main.projFrames[base.projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 20;
			projectile.height = 40;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI()
        {
            bool flag64 = projectile.type == mod.ProjectileType("BowSummon");
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (flag64)
            {
                if (player.dead)
                    modPlayer.bowSummon = false;

                if (modPlayer.bowSummon)
                    projectile.timeLeft = 2;

            }

            for (int num526 = 0; num526 < 1000; num526++) {
				if (num526 != projectile.whoAmI && Main.projectile[num526].active && Main.projectile[num526].owner == projectile.owner && Main.projectile[num526].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[num526].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num526].position.Y) < (float)projectile.width) {
					if (projectile.position.X < Main.projectile[num526].position.X)
						projectile.velocity.X = projectile.velocity.X - 0.05f;
					else
						projectile.velocity.X = projectile.velocity.X + 0.05f;

					if (projectile.position.Y < Main.projectile[num526].position.Y)
						projectile.velocity.Y = projectile.velocity.Y - 0.05f;
					else
						projectile.velocity.Y = projectile.velocity.Y + 0.05f;

				}
			}

			float num527 = projectile.position.X;
			float num528 = projectile.position.Y;
			float num529 = 900f;
			bool flag19 = false;

			if (projectile.ai[0] == 0f) {
				for (int num531 = 0; num531 < 200; num531++) {
					if (Main.npc[num531].CanBeChasedBy(projectile, false)) {
						float num532 = Main.npc[num531].position.X + 40 + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 90 + (float)(Main.npc[num531].height / 2);
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

            if (!flag19)
            {
                projectile.friendly = true;
                projectile.position.X = Main.player[projectile.owner].Center.X - (projectile.width * .5f);
                projectile.position.Y = Main.player[projectile.owner].Center.Y - (projectile.width * .5f) - 40;
                projectile.rotation = Main.player[projectile.owner].velocity.X * 0.085f;
                projectile.spriteDirection = Main.player[projectile.owner].direction;
                projectile.frame = 0;
            }

            else
            {
                projectile.spriteDirection = 1;
                timer++;
                if (timer >= 70)
                {
                    int range = 30;   //How many tiles away the projectile targets NPCs
                    float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

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
                    if (target.CanBeChasedBy(projectile, false))
                        projectile.rotation = projectile.DirectionTo(target.Center).ToRotation();
                    projectile.frameCounter++;
                    if (projectile.frameCounter >= 5)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame++;
                    }
                    if (projectile.frame > 3)
                    {
                        Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 13);
                        Vector2 direction = target.Center - ShootArea;
                        direction.Normalize();
                        direction.X *= shootVelocity;
                        direction.Y *= shootVelocity;
                        int shootType = 1;
                        if (player.HasItem(ItemID.WoodenArrow))
                            shootType = 1;
						if (player.HasItem(ItemID.EndlessQuiver))
                            shootType = 1;
						if (player.HasItem(ItemID.BoneArrow))
                            shootType = 117;
						if (player.HasItem(ModContent.ItemType<Items.Ammo.Arrow.PoisonArrow>()))
                            shootType = ModContent.ProjectileType<Projectiles.Arrow.PoisonArrowProj>();
                        if (player.HasItem(ItemID.FlamingArrow))
                            shootType = 2;
						if (player.HasItem(ItemID.FrostburnArrow))
                            shootType = 172;
                        if (player.HasItem(ModContent.ItemType<Items.Ammo.Arrow.SepulchreArrow>()))
                            shootType = ModContent.ProjectileType<Projectiles.Arrow.AccursedArrow>();
                        if (player.HasItem(ItemID.UnholyArrow))
                            shootType = 4;
						if (player.HasItem(ItemID.JestersArrow))
                            shootType = 5;
                        if (player.HasItem(ItemID.HellfireArrow))
                            shootType = 41;
						if (player.HasItem(ItemID.CursedArrow))
                            shootType = 103;
						if (player.HasItem(ItemID.IchorArrow))
                            shootType = 278;
						if (player.HasItem(ItemID.HolyArrow))
                            shootType = 91;
						if (player.HasItem(ItemID.ChlorophyteArrow))
                            shootType = 225;
						if (player.HasItem(ItemID.VenomArrow))
                            shootType = 282;
						if (player.HasItem(ModContent.ItemType<Items.Ammo.Arrow.ShroomiteArrow>()))
                            shootType = ModContent.ProjectileType<Projectiles.Arrow.ShroomiteArrow>();
						if (player.HasItem(ModContent.ItemType<Items.Ammo.Arrow.SpectreArrow>()))
                            shootType = ModContent.ProjectileType<Projectiles.Arrow.SpectreArrow>();
						if (player.HasItem(ModContent.ItemType<Items.Ammo.Arrow.BeetleArrow>()))
                            shootType = ModContent.ProjectileType<Projectiles.Arrow.BeetleArrow>();
						if (player.HasItem(ModContent.ItemType<Items.Ammo.Arrow.MartianArrow>()))
                            shootType = ModContent.ProjectileType<Projectiles.Arrow.ElectricArrow>();
						if (player.HasItem(ItemID.MoonlordArrow))
                            shootType = 639;

                        for (int index = 0; index < 58; ++index)
                        {
                            if (player.inventory[index].ammo == AmmoID.Arrow && player.HasItem(ItemID.EndlessQuiver) == false && player.inventory[index].stack >= 1 && player.inventory[index].consumable)
                            {
                                player.inventory[index].stack -= 1;
                                int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, shootType, projectile.damage, projectile.knockBack, Main.myPlayer);
                                Main.projectile[proj2].minion = true;
                                Main.projectile[proj2].ranged = false;
                                Main.projectile[proj2].netUpdate = true;
                                break;
                            }
							else if (player.inventory[index].ammo != AmmoID.Arrow && player.HasItem(ItemID.EndlessQuiver))
                            {
                                int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, shootType, projectile.damage, projectile.knockBack, Main.myPlayer);
                                Main.projectile[proj2].minion = true;
                                Main.projectile[proj2].ranged = false;
                                Main.projectile[proj2].netUpdate = true;
                                break;
                            }
                        }
                        projectile.frame = 0;
                        timer = 0;
                    }

                }
                projectile.position.X = Main.player[projectile.owner].Center.X - (projectile.width * .5f);
                projectile.position.Y = Main.player[projectile.owner].Center.Y - (projectile.width * .5f) - 40;
            }
		}
        public override bool MinionContactDamage()
		{
			return false;
		}
	}
}