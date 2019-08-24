
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon.Jabberwocky
{
	public class JabberwockyBodyOne : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jabberwocky");
		}

		public override void SetDefaults()
		{
			npc.noTileCollide = true;
			npc.width = 34;
			npc.height = 56;
			npc.aiStyle = 6;
			npc.netAlways = true;
			npc.damage = 20;
			npc.defense = 10;

			npc.npcSlots = 0;
			npc.lifeMax = 20000;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.noGravity = true;
			npc.knockBackResist = 0f;
			//  npc.value = 10000f;
			npc.scale = 1f;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[39] = true;
			npc.dontCountMe = true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 20;
				npc.height = 40;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}

		public override bool PreAI()
		{
			if (npc.ai[3] > 0)
				npc.realLife = (int)npc.ai[3];
			if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
				npc.TargetClosest(true);
			if (Main.player[npc.target].dead && npc.timeLeft > 300)
				npc.timeLeft = 300;

			if (Main.netMode != 1)
			{
				if (!Main.npc[(int)npc.ai[1]].active)
				{
					npc.life = 0;
					npc.HitEffect(0, 10.0);
					npc.active = false;
					// NetMessage.SendData(28, -1, -1, "", npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
				}
			}

			if (npc.ai[1] < (double)Main.npc.Length)
			{
				// We're getting the center of this NPC.
				Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				// Then using that center, we calculate the direction towards the 'parent NPC' of this NPC.
				float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
				float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
				// We then use Atan2 to get a correct rotation towards that parent NPC.
				npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				// We also get the length of the direction vector.
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				// We calculate a new, correct distance.
				float dist = (length - (float)npc.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				// Reset the velocity of this NPC, because we don't want it to move on its own
				npc.velocity = Vector2.Zero;
				// And set this NPCs position accordingly to that of this NPCs parent NPC.
				npc.position.X = npc.position.X + posX;
				npc.position.Y = npc.position.Y + posY;
			}
			return false;
		}
	}
}
