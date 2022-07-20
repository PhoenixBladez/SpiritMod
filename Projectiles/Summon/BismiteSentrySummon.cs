using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class BismiteSentrySummon : ModProjectile
	{
		public const int BurstAlpha = 210;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Bismite Crystal");

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 46;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
            Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.ignoreWater = true;
			Projectile.sentry = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override void AI()
		{
			if (Projectile.alpha > 0 && Projectile.alpha <= BurstAlpha)
                Projectile.alpha--;
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.3f;
            Projectile.scale = num395 + 0.65f;
            //CONFIG INFO
            int range = 18;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 6f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(Projectile) && !npc.friendly) {
					//if npc is within 50 blocks
					float dist = Projectile.Distance(npc.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							Projectile.ai[1] = npc.whoAmI;
							Projectile.netUpdate = true;
						}
					}
				}
			}

			NPC mainTarget = Projectile.OwnerMinionAttackTargetNPC;
            NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC()); //our target
																 //firing
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 60 && Projectile.Distance(target.Center) / 16 < range)
			{
				if (mainTarget != null && mainTarget.CanBeChasedBy(Projectile))
				{
					Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y - 13);
					Vector2 direction = Vector2.Normalize(mainTarget.Center - ShootArea) * shootVelocity;
					if (Projectile.alpha <= 100)
					{
						for (int i = 0; i < 10; i++)
						{
							int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, 0f, -2f, 0, default, 1.5f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							if (Main.dust[num].position != Projectile.Center)
							{
								Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
							}
						}
						if (Main.netMode != NetmodeID.MultiplayerClient) {

							int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y - 13, direction.X, direction.Y, ModContent.ProjectileType<BismiteShot>(), Projectile.damage, 0, Main.myPlayer);
							Main.projectile[proj2].DamageType = DamageClass.Summon;
						}
						
						SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.Center);  //make bow shooty sound
					}
				}
				else if (target != null && target.CanBeChasedBy(Projectile))
				{
					Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y - 13);
					Vector2 direction = Vector2.Normalize(target.Center - ShootArea) * shootVelocity;
					if (Projectile.alpha <= 100)
					{
						for (int i = 0; i < 10; i++)
						{
							int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, 0f, -2f, 0, default, 1.5f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							if (Main.dust[num].position != Projectile.Center)
							{
								Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
							}
						}
						if (Main.netMode != NetmodeID.MultiplayerClient) {

							int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y - 13, direction.X, direction.Y, ModContent.ProjectileType<BismiteShot>(), Projectile.damage, 0, Main.myPlayer);
							Main.projectile[proj2].DamageType = DamageClass.Summon;
						}
						
						SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.Center);  //make bow shooty sound
					}
				}
				Projectile.ai[0] = 0;
				Projectile.netUpdate = true;
			}
		}

		public void SpecialAttack()
        {
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, 0f, -2f, 0, default, 1.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;

                if (Main.dust[num].position != Projectile.Center)
                    Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 3f;
            }

            for (int i = 0; i < 10; i++)
            {
                float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                Vector2 velocity = new Vector2(Main.rand.NextFloat(2, 4)).RotatedBy(rotation);
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<BismiteShard>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].DamageType = DamageClass.Summon;
                Main.projectile[proj].velocity *= 1.5f;
                Main.projectile[proj].timeLeft = 120;
                Main.projectile[proj].hide = true;
            }
        }

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastHurt, Projectile.Center);

			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, 0f, -2f, 0, default, 1.5f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
	}
}