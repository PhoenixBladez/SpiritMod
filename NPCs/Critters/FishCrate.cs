using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class FishCrate : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Packing Crate");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 16;
			npc.damage = 0;
			npc.defense = 1000;
			npc.lifeMax = 1;
			npc.aiStyle = 1;
			npc.npcSlots = 0;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<Items.Placeable.FishCrate>();
			npc.noGravity = false;
			aiType = NPCID.Grasshopper;
			npc.alpha = 40;
			npc.dontCountMe = true;
			npc.dontTakeDamage = true;
		}
		public override void AI()
		{
			npc.spriteDirection = -npc.direction;
			if(npc.wet) {
				npc.aiStyle = 1;
				npc.npcSlots = 0;
				npc.noGravity = false;
				aiType = NPCID.Grasshopper;
				npc.velocity.X *= 0f;
				npc.velocity.Y *= .9f;
			} else {
				npc.aiStyle = 0;
				npc.npcSlots = 0;
				npc.noGravity = false;
				aiType = NPCID.BoundGoblin;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OceanMonster.Chance * 0.0181f;
		}
	}
}
