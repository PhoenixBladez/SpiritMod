using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Critters
{
	public class Gulper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gulper");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 16;
			npc.damage = 0;
			npc.defense = 0;
			npc.dontCountMe = true;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)mod.ItemType("GulperItem");
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.npcSlots = 0;
			aiType = NPCID.Goldfish;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 1f;
					Main.dust[num622].noGravity = true;
					{
						Main.dust[num622].scale = 0.23f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
		private int Counter;
		public override void AI()
		{
			npc.spriteDirection = -npc.direction;
			Counter++;
			if (Counter == 100)
			{
				npc.velocity.Y *= 10.0f;
				npc.velocity.X *= 4.0f;
			}
			if (Counter >= 200)
			{
				Counter = 0;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.31f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RawFish"), 1);
			}
		}
	}
}
