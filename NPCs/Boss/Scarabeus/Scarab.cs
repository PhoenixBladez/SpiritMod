using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class Scarab : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 20;
			NPC.damage = 14;
			NPC.defense = 0;
			NPC.lifeMax = 19;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 3;
			AIType = 508;
		}

		public override bool PreAI()
		{
			NPC.spriteDirection = NPC.direction;
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 3; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0) {
				for (int k = 0; k < 10; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, 1f);
				}
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Scarabeus/LittleScarab4").Type, 1f);
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.75f);
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
	}
}