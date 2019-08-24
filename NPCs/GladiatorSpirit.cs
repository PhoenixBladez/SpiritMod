using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs
{
	public class GladiatorSpirit : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gladiator Spirit");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Wraith];
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 56;
			npc.damage = 51;
			npc.defense = 20;
			npc.lifeMax = 280;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 10041f;
			npc.knockBackResist = .60f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			aiType = NPCID.Wraith;
			animationType = NPCID.Wraith;
			npc.stepSpeed = .5f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && Main.hardMode && spawnInfo.spawnTileY > Main.rockLayer ? 0.09f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 220);
				Gore.NewGore(npc.position, npc.velocity, 221);
				Gore.NewGore(npc.position, npc.velocity, 222);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Ancient Rune"), 3 + Main.rand.Next(3));
		}

	}
}
