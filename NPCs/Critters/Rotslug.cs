using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class Rotslug : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rotslug");
			Main.npcFrameCount[npc.type] = 4;
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
			npc.catchItem = (short)ModContent.ItemType<RotSlugItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 66;
			npc.npcSlots = 0;
            npc.noGravity = false; ;
			aiType = NPCID.Grubby;
			Main.npcFrameCount[npc.type] = 4;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
                int d = 194;
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 1.75f * hitDirection, -1.75f, 0, new Color(), 0.6f);
                }
            }
			
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.player.ZoneCorrupt && spawnInfo.player.ZoneOverworldHeight ? .07f : 0f;
        }
		public override void AI()
        {
            npc.spriteDirection = npc.direction;
        }
        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity != Vector2.Zero)
            {
                npc.frameCounter += 0.12f;
                npc.frameCounter %= Main.npcFrameCount[npc.type];
                int frame = (int)npc.frameCounter;
                npc.frame.Y = frame * frameHeight;
            }
        }
    }
}
