using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SpiritCore
{
	public class SpiritMinionHostile : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Exploding Soul");
		}

		public override void SetDefaults()
		{
			npc.width = npc.height = 40;
			Main.npcFrameCount[npc.type] = 4;
			npc.lifeMax = 50;
			npc.damage = 40;
			npc.knockBackResist = 0;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

			npc.TargetClosest(true);
			float speed = 9f;
			float acceleration = 0.4f;
			Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5F, npc.position.Y + npc.height * 0.5F);
			float xDir = Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5F) - vector2.X;
			float yDir = Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5F) - vector2.Y;
			float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

			float num10 = speed / length;
			xDir = xDir * num10;
			yDir = yDir * num10;
			if (npc.velocity.X < xDir)
			{
				npc.velocity.X = npc.velocity.X + acceleration;
				if (npc.velocity.X < 0 && xDir > 0)
					npc.velocity.X = npc.velocity.X + acceleration;
			}
			else if (npc.velocity.X > xDir)
			{
				npc.velocity.X = npc.velocity.X - acceleration;
				if (npc.velocity.X > 0 && xDir < 0)
					npc.velocity.X = npc.velocity.X - acceleration;
			}
			if (npc.velocity.Y < yDir)
			{
				npc.velocity.Y = npc.velocity.Y + acceleration;
				if (npc.velocity.Y < 0 && yDir > 0)
					npc.velocity.Y = npc.velocity.Y + acceleration;
			}
			else if (npc.velocity.Y > yDir)
			{
				npc.velocity.Y = npc.velocity.Y - acceleration;
				if (npc.velocity.Y > 0 && yDir < 0)
					npc.velocity.Y = npc.velocity.Y - acceleration;
			}
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].Hitbox.Intersects(npc.Hitbox))
				{
					npc.life = 0;
					npc.HitEffect(0, 10.0);
					npc.checkDead();
					npc.active = false;
					return false;
				}
			}
			return false;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool CheckDead()
		{
			Vector2 center = npc.Center;
			Terraria.Projectile.NewProjectile(center.X, center.Y, 0f, 0f, mod.ProjectileType("UnstableWisp_Explosion"), 40, 0f, Main.myPlayer);
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= 4)
			{
				npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
				npc.frameCounter = 0;
			}
		}
	}
}
