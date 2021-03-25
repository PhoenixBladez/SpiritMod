using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SpiritMod.NPCs.Critters
{
	public class Bladetongue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bladetongue");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 24;
			npc.damage = 50;
			npc.defense = 8;
			npc.lifeMax = 380;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
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
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BladetongueGore"), 1f);
			}
			for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.direction, -1f, 1, default(Color), .91f);
				}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.Bleeding, 1200);
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bladetongue, 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCrimson && spawnInfo.water && Main.hardMode ? 0.0075f : 0f;
		}

	}
}
