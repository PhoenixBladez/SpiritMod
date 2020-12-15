using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class Briarmoth : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarmoth");
			Main.npcFrameCount[npc.type] = 4;
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
			npc.catchItem = (short)ModContent.ItemType<BriarmothItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
			npc.npcSlots = 0;
			npc.noGravity = true;
            npc.chaseable = false;
			aiType = NPCID.Firefly;
			Main.npcFrameCount[npc.type] = 4;
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
            Player player = spawnInfo.player;
            if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
            {
                return spawnInfo.player.GetSpiritPlayer().ZoneReach ? .325f : 0f;
            }
            return 0f;
        }

        public override void AI()
        {
            npc.spriteDirection = -npc.direction;
        }
        public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.20f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
