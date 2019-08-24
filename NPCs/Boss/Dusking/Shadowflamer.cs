using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class Shadowflamer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflamer");
		}

		public override void SetDefaults()
		{
			npc.width = npc.height = 40;
			Main.npcFrameCount[npc.type] = 5;
			npc.lifeMax = 90;
			npc.damage = 32;
			npc.knockBackResist = 0;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			float speed = 8f;
			float acceleration = 0.13f;
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
			return false;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Shadowflame"), 150);
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= 5)
			{
				npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
				npc.frameCounter = 0;
			}
		}
	}
}
