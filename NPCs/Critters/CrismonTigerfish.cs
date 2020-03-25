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
	public class CrismonTigerfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Tigerfish");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ItemID.CrimsonTigerfish;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.dontCountMe = true;
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
				for (int num621 = 0; num621 < 20; num621++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 5);
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity *= 0.5f;
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CTiger"), 1f);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 1f;
					Main.dust[num622].noGravity = true;
					{
						Main.dust[num622].scale = 0.023f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RawFish"), 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCrimson && spawnInfo.water ? 0.06f : 0f;
		}
	
	}
}
