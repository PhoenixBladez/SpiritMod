using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SpiritCore
{
	public class Mirage : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;
		bool rotationspawns = false;
		bool rotationspawns1 = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Mirage");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 50;
			npc.damage = 42;
			npc.lifeMax = 2300;
			npc.knockBackResist = 0;
			npc.alpha = 80;

			npc.noGravity = true;
			npc.noTileCollide = true;

			animationType = NPCID.Pixie;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
		}

		private int Counter;
		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;

			if (npc.Center.X >= player.Center.X && moveSpeed >= -80) // flies to players x position
				moveSpeed--;

			if (npc.Center.X <= player.Center.X && moveSpeed <= 70)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -45) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 185f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 45)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.2f;
			if (Main.rand.Next(210) == 6)
				HomeY = -25f;

			if (!rotationspawns1)
			{
				if (Main.expertMode)
				{
					for (int I = 0; I < 4; I++)
					{
						//cos = y, sin = x
						int GeyserEye = NPC.NewNPC((int)(npc.Center.X + (Math.Sin(I * 90) * 100)), (int)(npc.Center.Y + (Math.Cos(I * 90) * 100)), mod.NPCType("ShadowMirage"), npc.whoAmI, 0, 0, 0, -1);
						NPC Eye = Main.npc[GeyserEye];
						Eye.ai[0] = I * 90;
						Eye.ai[3] = I * 90;
						rotationspawns1 = true;
					}
				}
				else
				{
					for (int I = 0; I < 2; I++)
					{
						//cos = y, sin = x
						int GeyserEye = NPC.NewNPC((int)(npc.Center.X + (Math.Sin(I * 180) * 100)), (int)(npc.Center.Y + (Math.Cos(I * 180) * 100)), mod.NPCType("ShadowMirage"), npc.whoAmI, 0, 0, 0, -1);
						NPC Eye = Main.npc[GeyserEye];
						Eye.ai[0] = I * 180;
						Eye.ai[3] = I * 180;
						rotationspawns1 = true;
					}
				}
			}

			bool spirit = false;
			int npcType = mod.NPCType("ShadowMirage");
			for (int num569 = 0; num569 < 200; num569++)
			{
				if ((Main.npc[num569].active && Main.npc[num569].type == (npcType)))
				{
					spirit = true;
				}
			}

			if (spirit)
				npc.dontTakeDamage = true;
			else
			{
				npc.dontTakeDamage = false;
				if (npc.Center.X >= player.Center.X && moveSpeed >= -80) // flies to players x position
					moveSpeed--;
				else if (npc.Center.X <= player.Center.X && moveSpeed <= 80)
					moveSpeed++;

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -50) //Flies to players Y position
				{
					moveSpeedY--;
					HomeY = 175f;
				}
				else if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 50)
				{
					moveSpeedY++;
				}

				npc.velocity.Y = moveSpeedY * 0.2f;
				if (Main.rand.Next(219) == 6)
					HomeY = -25f;
			}
			if (Main.rand.Next(6) == 1)
			{
				int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 173, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
				int dust1 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 187, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust1].velocity *= 0f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust1].noGravity = true;

			}

			return true;
		}

	}
}
