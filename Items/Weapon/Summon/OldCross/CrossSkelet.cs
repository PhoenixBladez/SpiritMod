using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.OldCross
{
	public class CrossSkelet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AngrySkeleton");
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 42;
			projectile.scale = Main.rand.NextFloat(.7f, 1.1f);
			ProjectileID.Sets.SentryShot[projectile.type] = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.minionSlots = 0;
			projectile.netImportant = true;
			projectile.alpha = 0;
			projectile.timeLeft = 360;
			projectile.penetrate = 1;
		}

		Vector2 hometarget;
		bool onground;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(hometarget);
			writer.Write(onground);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			hometarget = reader.ReadVector2();
			onground = reader.ReadBoolean();
		}
		public override void AI()
		{
			projectile.spriteDirection = -projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frame++;
				projectile.frameCounter = 0;

			}
			if (projectile.frame >= 4)
				projectile.frame = 0;

			Projectile coffin = Main.projectile[(int)projectile.ai[0]];

			if (!coffin.active || coffin.type != mod.ProjectileType("CrossCoffin") || coffin.owner != projectile.owner)
				projectile.Kill();

			for (int index = 0; index < 1000; ++index)
			{
				if (index != projectile.whoAmI && Main.projectile[index].active && (Main.projectile[index].owner == projectile.owner && Main.projectile[index].type == projectile.type) && (double)Math.Abs((float)(projectile.position.X - Main.projectile[index].position.X)) + (double)Math.Abs((float)(projectile.position.Y - Main.projectile[index].position.Y)) < (double)projectile.width)
				{
					if (projectile.position.X < Main.projectile[index].position.X)
					{
						projectile.velocity.X -= 0.05f;
					}
					else
					{
						projectile.velocity.X += 0.05f;
					}
					if (projectile.position.Y < Main.projectile[index].position.Y)
					{
						projectile.velocity.Y -= 0.05f;
					}
					else
					{
						projectile.velocity.Y += 0.05f;
					}
				}
			}

			if (Main.npc[(int)projectile.ai[1]].CanBeChasedBy(projectile))
			{
				hometarget = Main.npc[(int)projectile.ai[1]].Center;
			}
			else
			{
				hometarget = coffin.Center;

				NPC miniontarget = projectile.OwnerMinionAttackTargetNPC;
				float maxdist = 900f;
				if (miniontarget != null && projectile.Distance(miniontarget.Center) < maxdist && miniontarget.CanBeChasedBy(projectile))
				{
					projectile.ai[1] = miniontarget.whoAmI;
				}
				else for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC potentialtarget = Main.npc[i];
					if (potentialtarget != null && projectile.Distance(potentialtarget.Center) < maxdist && potentialtarget.CanBeChasedBy(projectile))
					{
						maxdist = projectile.Distance(potentialtarget.Center);
						projectile.ai[1] = potentialtarget.whoAmI;
					}
				}
			}

			if (onground)
			{
				projectile.velocity.X = MathHelper.Lerp(projectile.velocity.X, Math.Sign(projectile.DirectionTo(hometarget).X) * 8, 0.05f);

				if(projectile.Center.Y > hometarget.Y + 20 && Math.Abs(projectile.Center.X - hometarget.X) < 100) {

					projectile.velocity.Y = MathHelper.Clamp((hometarget.Y - projectile.Center.Y) / 10, -16, -6); 
					projectile.velocity.X = MathHelper.Clamp((hometarget.X - projectile.Center.X) / 12, -8, 8);
				}
			}
			if (projectile.velocity.Y < 16)
				projectile.velocity.Y += 0.4f;

			Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
			onground = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 46);
			}
			for(int i = 1; i <= Main.rand.Next(2, 5); i++)
			{
				Gore gore = Gore.NewGoreDirect(projectile.position, projectile.velocity / 2, mod.GetGoreSlot("Gores/Skelet/skeler" + i));
				gore.timeLeft = 40;
			}
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 2, 0.75f, 0.25f);
		}
		public override bool MinionContactDamage() => true;
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = (projectile.Center.Y < hometarget.Y - 20);
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			onground = (projectile.velocity.Y == 0);
			return false;
		}
	}
}