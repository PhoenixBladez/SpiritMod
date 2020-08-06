using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class Maggotfly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maggotfly");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 8;
			npc.height = 8;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.dontCountMe = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<MaggotflyItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
			npc.npcSlots = 0;
			npc.noGravity = true;
			aiType = NPCID.Firefly;
			Main.npcFrameCount[npc.type] = 2;
		}
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                int d = 184;
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.75f * hitDirection, -2.75f, 0, new Color(), 0.6f);
                }
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.player.ZoneCorrupt && spawnInfo.player.ZoneOverworldHeight ? .1f : 0f;
        }
        public override void AI()
        {
            npc.spriteDirection = npc.direction;
        }
        public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
