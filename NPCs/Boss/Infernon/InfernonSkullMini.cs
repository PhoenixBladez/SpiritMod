using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	public class InfernonSkullMini : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernon Skull");
		}

		public override void SetDefaults()
		{
			npc.width = npc.height = 27;
			npc.alpha = 255;

			npc.damage = 32;
			npc.lifeMax = 30;
			npc.defense = 5;
			Main.npcFrameCount[npc.type] = 4;
			npc.noGravity = true;
			npc.noTileCollide = true;

		}

		public override bool PreAI()
		{
			float length = (float)Math.Sqrt((double)(npc.velocity.X * npc.velocity.X + npc.velocity.Y * npc.velocity.Y));

			if (npc.localAI[0] == 0)
				npc.localAI[0] = length;

			if (npc.alpha > 0)
				npc.alpha -= 25;
			else
				npc.alpha = 0;

			float dirX = npc.position.X;
			float dirY = npc.position.Y;
			float distance = 1200f;
			bool hasTarget = false;
			int target = 0;

			if (npc.ai[1] == 0f)
			{
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead && (npc.ai[1] == 0f || npc.ai[1] == (float)(i + 1)))
					{
						Vector2 playerPosition = new Vector2(Main.player[i].position.X + (Main.player[i].width / 2), Main.player[i].position.Y + (Main.player[i].height / 2));
						Vector2 npcPosition = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
						float currentDistance = (playerPosition - npcPosition).Length();
						if (currentDistance < distance && Collision.CanHit(npcPosition, 1, 1, playerPosition, 1, 1))
						{
							distance = currentDistance;
							dirX = playerPosition.X;
							dirY = playerPosition.Y;
							hasTarget = true;
							target = i;
						}
					}
				}
				if (hasTarget)
					npc.ai[1] = (target + 1);

				hasTarget = false;
			}

			if (npc.ai[1] > 0f)
			{
				int index = (int)(npc.ai[1] - 1f);
				if (Main.player[index].active && !Main.player[index].dead)
				{
					Vector2 playerPosition = new Vector2(Main.player[index].position.X + (Main.player[index].width / 2), Main.player[index].position.Y + (Main.player[index].height / 2));
					Vector2 npcPosition = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
					float currentDistance = (npcPosition - playerPosition).Length();
					if (currentDistance < 600f)
					{
						hasTarget = true;
						dirX = playerPosition.X;
						dirY = playerPosition.Y;
					}
				}
				else
					npc.ai[1] = 0f;
			}

			if (hasTarget)
			{
				Vector2 npcPosition = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
				dirX = dirX - npcPosition.X;
				dirY = dirY - npcPosition.Y;
				float l = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				l = npc.localAI[0] / l;
				dirX *= l;
				dirY *= l;
				// Higher followSpeed = slower rotation.
				float followSpeed = 15;
				npc.velocity.X = (npc.velocity.X * (followSpeed - 1) + dirX) / followSpeed;
				npc.velocity.Y = (npc.velocity.Y * (followSpeed - 1) + dirY) / followSpeed;
			}

			if (npc.velocity.X > 0)
			{
				npc.spriteDirection = -1;
				npc.rotation = npc.velocity.ToRotation();
			}
			if (npc.velocity.X < 0)
			{
				npc.spriteDirection = 1;
				npc.rotation = npc.velocity.ToRotation() + 3.14f;
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, npc.alpha);
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.ai[1] == 0)
			{
				if (npc.frameCounter >= 10)
				{
					npc.frame.Y += frameHeight;
					if (npc.frame.Y > frameHeight)
						npc.frame.Y = 0;
					npc.frameCounter = 0;
				}
			}
			else
			{
				if (npc.frameCounter >= 10)
				{
					npc.frame.Y += frameHeight;
					if (npc.frame.Y > frameHeight * 3)
						npc.frame.Y = 0;
					if (npc.frame.Y < frameHeight)
						npc.frame.Y = frameHeight;
					npc.frameCounter = 0;
				}
			}
		}

		public override void AI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.OnFire, 300);
		}
	}
}
