using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs
{
	public class ForgottenOne : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forgotten One");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DesertGhoul];
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 44;
			npc.damage = 300;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 50000f;
			npc.knockBackResist = .60f;
			npc.aiStyle = 3;
			aiType = NPCID.DesertGhoul;
			aiType = NPCID.DesertGhoul;
			animationType = NPCID.DesertGhoul;
			npc.lavaImmune = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneUnderworldHeight && NPC.downedBoss3 ? 0.08f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/One1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/One2"), 1f);

				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
			}
		}

		public override void NPCLoot()
		{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarvedRock"), Main.rand.Next(1) + 2);
		}
	}
}