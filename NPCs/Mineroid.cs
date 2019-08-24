using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Mineroid : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mineroid");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width =40;
			npc.height = 40;
			npc.damage = 28;
			npc.defense = 13;
			npc.lifeMax = 45;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 260f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = .45f;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneMeteor && spawnInfo.spawnTileY < Main.rockLayer ? 0.15f : 0f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OrbiterStaff"));
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int lol = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 24, 0f, 0f, 100, default(Color), 2f);
			
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 61);
				Gore.NewGore(npc.position, npc.velocity, 62);
				Gore.NewGore(npc.position, npc.velocity, 63);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 24, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.35f, 1.34f, 2.24f);

			npc.spriteDirection = npc.direction;
		}
	}
}
