using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class PinkSlimer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Slimer");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 66;
			npc.height = 36;
			npc.damage = 40;
			npc.defense = 22;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = .28f;
			npc.aiStyle = 14;
			npc.noGravity = true;
			aiType = NPCID.Slimer;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneHoly && !spawnInfo.playerSafe && !spawnInfo.invasion && !spawnInfo.sky && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer ? 0.05f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Slime_WIng_1"), 1f);

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
			npc.spriteDirection = npc.direction;
		}

		public override bool CheckDead()
		{
			npc.Transform(NPCID.IlluminantSlime);
			npc.life = npc.lifeMax;
			return false;
		}
	}
}
