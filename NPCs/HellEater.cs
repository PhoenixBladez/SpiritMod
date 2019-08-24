using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class HellEater : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gluttonous Devourer");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
		}

		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 34;
			npc.damage = 28;
			npc.defense = 16;
			npc.lifeMax = 82;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 3060f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 85;
			npc.noGravity = true;
			aiType = NPCID.StardustCellBig;
			animationType = NPCID.Pixie;
			npc.lavaImmune = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneUnderworldHeight && NPC.downedBoss3 ? 0.18f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore2"), 1f);
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.1f, 0.04f, 0.02f);

			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 180);
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarvedRock"), Main.rand.Next(2) + 2);

			if (Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HellEater"));
			}
		}
	}
}
