using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SpiritMod.NPCs.Critters
{
	public class SawtoothShark : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sawtooth Shark");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 24;
			npc.damage = 40;
			npc.defense = 0;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.2f;
			npc.aiStyle = 16;
			npc.dontCountMe = true;
			npc.noGravity = true;
			npc.npcSlots = 0;
			aiType = NPCID.Shark;
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
			npc.spriteDirection = npc.direction;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SawtoothSharkGore"), 1f);
			}
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.direction, -1f, 1, default, .91f);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.Bleeding, 200);
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SharkFin, 1);

			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SawtoothShark, 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneBeach && spawnInfo.water ? 0.0035f : 0f;
		}

	}
}
