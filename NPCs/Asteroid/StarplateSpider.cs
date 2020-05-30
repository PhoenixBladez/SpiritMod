using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class StarplateSpider : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Spider");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 30;
			npc.damage = 34;
			npc.defense = 9;
			npc.lifeMax = 201;
			npc.value = 860f;
			npc.knockBackResist = 0.95f;
			npc.aiStyle = 3;
			aiType = NPCID.AngryBones;
		}

		public override void NPCLoot()
        {
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarEnergy"));
            }
            string[] lootTable = { "AstronautLegs", "AstronautHelm", "AstronautBody" };
            if (Main.rand.Next(40) == 0)
            {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
            }
        }
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			//npc.targetClosest();
			Player target = Main.player[npc.target];
			if (target.position.Y - npc.position.Y < -300)
			{
				Teleport();
			}
		}
		public void Teleport()
		{
			//npc.ai[0] = 1f;
			int num1 = (int)Main.player[npc.target].position.X / 16;
			int num2 = (int)Main.player[npc.target].position.Y / 16;
			int num3 = (int)npc.position.X / 16;
			int num4 = (int)npc.position.Y / 16;
			int num5 = 20;
			int num6 = 0;
			bool flag1 = false;
			if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000.0)
			{
				num6 = 100;
				flag1 = true;
			}
			while (!flag1 && num6 < 100)
			{
				++num6;
				int index1 = Main.rand.Next(num1 - num5, num1 + num5);
				for (int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2)
				{
					if ((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].nactive())
					{
						bool flag2 = true;
						if (Main.tile[index1, index2 - 1].lava())
							flag2 = false;
						if (flag2 && Main.tileSolid[(int)Main.tile[index1, index2].type] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1))
						{
						//	npc.ai[1] = 20f;
							npc.position.X = ((float)index1 * 16 - (npc.width / 2) + 8);
							npc.position.Y = (float)index2 * 16f - npc.height;
							flag1 = true;
							break;
						}
					}
				}
			}
			npc.netUpdate = true;
		}
	}
}
