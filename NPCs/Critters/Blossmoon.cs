using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Critters
{
	public class Blossmoon : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blossmoon");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 18;
			npc.height = 20;
			npc.damage = 0;
			npc.dontCountMe = true;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<BlossmoonItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 0;
			npc.npcSlots = 0;
			npc.noGravity = false;
			Main.npcFrameCount[npc.type] = 45;
		}

        public override void HitEffect(int hitDirection, double damage)
        {
            int d1 = 173;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime && !spawnInfo.playerSafe && !spawnInfo.invasion && !spawnInfo.sky && !Main.eclipse ? 0.0509f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .92f/2, .632f/2, 1.71f/2);
			{
				Player target = Main.player[npc.target];
				int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
				if (distance < 480)
				{
					player.AddBuff(BuffID.Calm, 300); 				
				}
			}
			return true;
		}
	}
}
