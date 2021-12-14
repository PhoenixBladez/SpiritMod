using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Critters
{
	public class Hemoglorb : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hemoglorb");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 16;
			npc.height = 12;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.dontCountMe = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<HemoglorbItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 67;
			npc.npcSlots = 0;
            npc.noGravity = false; ;
			aiType = NPCID.Snail;
			Main.npcFrameCount[npc.type] = 7;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 1.75f * hitDirection, -1.75f, 0, new Color(), 0.86f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneCrimson && spawnInfo.player.ZoneOverworldHeight ? .03f : 0f;
		public override void AI() => npc.spriteDirection = npc.direction;

		public override void FindFrame(int frameHeight)
		{
			if (npc.velocity != Vector2.Zero)
			{
				npc.frameCounter += 0.15f;
				npc.frameCounter %= Main.npcFrameCount[npc.type];
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
		}
	}
}
