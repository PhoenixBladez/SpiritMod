using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class CaptiveSpirit : ModNPC
	{
		bool start = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captive Spirit");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 70;

			npc.damage = 120;
			npc.defense = 74;
			npc.lifeMax = 900;
			npc.knockBackResist = 0;

			npc.noGravity = true;
			npc.noTileCollide = true;

			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		public override bool PreAI()
		{
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
			if (start)
			{
				for (int num621 = 0; num621 < 15; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
				}
				npc.velocity.X = npc.ai[0];
				npc.velocity.Y = npc.ai[1];
				start = false;
			}

			if (Main.rand.Next(3) == 0)
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);

			npc.TargetClosest(true);
			Player targetPlayer = Main.player[npc.target];
			float currentRot = npc.velocity.ToRotation();
			Vector2 direction = targetPlayer.Center - npc.Center;
			float targetAngle = direction.ToRotation();
			if (direction == Vector2.Zero)
				targetAngle = currentRot;

			float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
			npc.velocity = new Vector2(npc.velocity.Length(), 0f).RotatedBy(desiredRot, default(Vector2));
			return false;
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
