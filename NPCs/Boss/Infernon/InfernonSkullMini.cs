using Microsoft.Xna.Framework;
using System;
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
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = NPC.height = 27;
			NPC.alpha = 255;
			NPC.damage = 45;
			NPC.lifeMax = 65;
			NPC.defense = 5;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		bool hasTarget = false;

		public override bool PreAI()
		{
			if (NPC.localAI[0] == 0)
				NPC.localAI[0] = (float)Math.Sqrt(NPC.velocity.X * NPC.velocity.X + NPC.velocity.Y * NPC.velocity.Y);

			if (NPC.alpha > 0)
				NPC.alpha -= 25;
			else
				NPC.alpha = 0;

			float dirX = NPC.position.X;
			float dirY = NPC.position.Y;
			float distance = 1200f;
			int target = 0;

			if (NPC.ai[1] == 0f && !hasTarget)
			{
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead && (NPC.ai[1] == 0f || NPC.ai[1] == (float)(i + 1)))
					{
						Vector2 playerPosition = new Vector2(Main.player[i].position.X + (Main.player[i].width / 2), Main.player[i].position.Y + (Main.player[i].height / 2));
						Vector2 npcPosition = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
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
					NPC.ai[1] = (target + 1);
			}

			if (NPC.ai[1] > 0f)
			{
				int index = (int)(NPC.ai[1] - 1f);
				if (Main.player[index].active && !Main.player[index].dead)
				{
					Vector2 playerPosition = new Vector2(Main.player[index].position.X + (Main.player[index].width / 2), Main.player[index].position.Y + (Main.player[index].height / 2));
					Vector2 npcPosition = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
					float currentDistance = (npcPosition - playerPosition).Length();
					if (currentDistance < 600f)
					{
						hasTarget = true;
						dirX = playerPosition.X;
						dirY = playerPosition.Y;
					}
				}
				else
				{
					NPC.ai[1] = 0f;
					hasTarget = false;
				}
			}

			if (hasTarget)
			{
				dirX -= NPC.Center.X;
				dirY -= NPC.Center.Y;
				float l = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				l = NPC.localAI[0] / l;
				dirX *= l;
				dirY *= l;
				// Higher followSpeed = slower rotation.
				float followSpeed = 25;
				NPC.velocity.X = (NPC.velocity.X * (followSpeed - 1) + dirX) / followSpeed;
				NPC.velocity.Y = (NPC.velocity.Y * (followSpeed - 1) + dirY) / followSpeed;
			}

			NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
			NPC.rotation = NPC.velocity.ToRotation() + (NPC.velocity.X > 0 ? 0 : MathHelper.Pi);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, NPC.alpha);

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.ai[1] == 0)
			{
				if (NPC.frameCounter >= 10)
				{
					NPC.frame.Y += frameHeight;
					if (NPC.frame.Y > frameHeight)
						NPC.frame.Y = 0;
					NPC.frameCounter = 0;
				}
			}
			else
			{
				if (NPC.frameCounter >= 10)
				{
					NPC.frame.Y += frameHeight;
					if (NPC.frame.Y > frameHeight * 3)
						NPC.frame.Y = 0;
					if (NPC.frame.Y < frameHeight)
						NPC.frame.Y = frameHeight;
					NPC.frameCounter = 0;
				}
			}
		}

		public override void AI() => Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(BuffID.OnFire, 300);
		}
	}
}