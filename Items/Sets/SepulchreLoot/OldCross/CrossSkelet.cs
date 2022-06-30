using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.OldCross
{
	public class CrossSkelet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AngrySkeleton");
			Main.projFrames[Projectile.type] = 5;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.SentryShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.scale = Main.rand.NextFloat(.7f, 1.1f);
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.minionSlots = 0;
			Projectile.netImportant = true;
			Projectile.alpha = 0;
			Projectile.timeLeft = 360;
			Projectile.penetrate = 1;
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
			Projectile.spriteDirection = -Projectile.direction;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;

			}
			if (Projectile.frame >= 4)
				Projectile.frame = 0;

			Projectile coffin = Main.projectile[(int)Projectile.ai[0]];

			if (!coffin.active || coffin.type != ModContent.ProjectileType<CrossCoffin>() || coffin.owner != Projectile.owner)
				Projectile.Kill();

			for (int index = 0; index < 1000; ++index)
			{
				if (index != Projectile.whoAmI && Main.projectile[index].active && (Main.projectile[index].owner == Projectile.owner && Main.projectile[index].type == Projectile.type) && (double)Math.Abs((float)(Projectile.position.X - Main.projectile[index].position.X)) + (double)Math.Abs((float)(Projectile.position.Y - Main.projectile[index].position.Y)) < (double)Projectile.width)
				{
					if (Projectile.position.X < Main.projectile[index].position.X)
						Projectile.velocity.X -= 0.05f;
					else
						Projectile.velocity.X += 0.05f;

					if (Projectile.position.Y < Main.projectile[index].position.Y)
						Projectile.velocity.Y -= 0.05f;
					else
						Projectile.velocity.Y += 0.05f;
				}
			}

			if (Main.npc[(int)Projectile.ai[1]].CanBeChasedBy(Projectile))
				hometarget = Main.npc[(int)Projectile.ai[1]].Center;
			else
			{
				hometarget = coffin.Center;

				NPC miniontarget = Projectile.OwnerMinionAttackTargetNPC;
				float maxdist = 900f;
				if (miniontarget != null && Projectile.Distance(miniontarget.Center) < maxdist && miniontarget.CanBeChasedBy(Projectile))
				{
					Projectile.ai[1] = miniontarget.whoAmI;
				}
				else for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC potentialtarget = Main.npc[i];
					if (potentialtarget != null && Projectile.Distance(potentialtarget.Center) < maxdist && potentialtarget.CanBeChasedBy(Projectile))
					{
						maxdist = Projectile.Distance(potentialtarget.Center);
						Projectile.ai[1] = potentialtarget.whoAmI;
					}
				}
			}

			if (onground)
			{
				Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, Math.Sign(Projectile.DirectionTo(hometarget).X) * 8, 0.05f);

				if(Projectile.Center.Y > hometarget.Y + 20 && Math.Abs(Projectile.Center.X - hometarget.X) < 100) {

					Projectile.velocity.Y = MathHelper.Clamp((hometarget.Y - Projectile.Center.Y) / 10, -16, -6); 
					Projectile.velocity.X = MathHelper.Clamp((hometarget.X - Projectile.Center.X) / 12, -8, 8);
				}
			}
			if (Projectile.velocity.Y < 16)
				Projectile.velocity.Y += 0.4f;

			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
			onground = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Poisoned);
			for(int i = 1; i <= Main.rand.Next(2, 5); i++)
			{
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity / 2, Mod.Find<ModGore>("Gores/Skelet/skeler" + i).Type);
				gore.timeLeft = 40;
			}
			SoundEngine.PlaySound(SoundID.NPCDeath2, Projectile.Center);
		}
		public override bool MinionContactDamage() => true;
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = (Projectile.Center.Y < hometarget.Y - 20);
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			onground = (Projectile.velocity.Y == 0);
			return false;
		}
	}
}