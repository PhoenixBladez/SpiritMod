using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class Blossmoon : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blossmoon");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 20;
			NPC.damage = 0;
			NPC.dontCountMe = true;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ModContent.ItemType<BlossmoonItem>();
			NPC.dontTakeDamageFromHostiles = false;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 0;
			NPC.npcSlots = 0;
			NPC.noGravity = false;
			Main.npcFrameCount[NPC.type] = 45;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.SpawnTileY < Main.rockLayer && !Main.dayTime && MyWorld.calmNight && !spawnInfo.Invasion && !spawnInfo.Sky && !Main.eclipse;
			if (!valid)
				return 0f;
			if (QuestManager.GetQuest<CritterCaptureBlossmoon>().IsActive && !NPC.AnyNPCs(NPC.type))
				return 0.25f;
			return 0.076f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), .92f / 2, .632f / 2, 1.71f / 2);
			{
				Player target = Main.player[NPC.target];
				int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
				if (distance < 480) {
					player.AddBuff(BuffID.Calm, 300);
				}
			}
			return true;
		}
	}
}
