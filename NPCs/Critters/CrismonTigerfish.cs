using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			aiType = NPCID.CorruptGoldfish;
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
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tigerfish1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tigerfish2"), 1f);
			}
		}
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
		public override void NPCLoot()
		{
			if(Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCrimson && spawnInfo.water ? 0.06f : 0f;
		}

	}
}
