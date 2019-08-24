using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class FieryTrident : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Trident");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 54;
			npc.damage = 70;
			npc.defense = 18;
			npc.lifeMax = 220;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 12060f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = .45f;
			npc.aiStyle = 23;
			aiType = NPCID.EnchantedSword;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneUnderworldHeight && Main.hardMode ? 0.08f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 61);
				Gore.NewGore(npc.position, npc.velocity, 62);
				Gore.NewGore(npc.position, npc.velocity, 63);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(30) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryPendant"));
			}
			if (Main.rand.Next(25) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryTrident"));
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 3f, 1f, 0.8f);

			npc.spriteDirection = npc.direction;

			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 180);
		}
	}
}
