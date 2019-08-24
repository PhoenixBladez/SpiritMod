using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class NecroWalker : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necro Walker");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.AngryBones];
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 52;
			npc.damage = 28;
			npc.defense = 13;
			npc.lifeMax = 190;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 860f;
			npc.knockBackResist = .37f;
			npc.aiStyle = 3;
			aiType = NPCID.AngryBones;
			animationType = NPCID.AngryBones;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneDungeon ? 0.04f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Skeleton_head"), 1f);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone, 12);

			if (Main.rand.Next(12) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BoneTotem"));
		}
	}
}
