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
			Main.npcFrameCount[NPC.type] = 4;
			Main.npcFrameCount[NPC.type] = 4;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 12;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.dontCountMe = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = (short)ModContent.ItemType<RotSlugItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 66;
			NPC.npcSlots = 0;
            NPC.noGravity = false; ;
			AIType = NPCID.Grubby;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
                for (int k = 0; k < 10; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Scarecrow, 1.75f * hitDirection, -1.75f, 0, new Color(), 0.6f);
            }
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneOverworldHeight ? .07f : 0f;
		public override void AI() => NPC.spriteDirection = NPC.direction;

		public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity != Vector2.Zero)
            {
                NPC.frameCounter += 0.12f;
                NPC.frameCounter %= Main.npcFrameCount[NPC.type];
                int frame = (int)NPC.frameCounter;
                NPC.frame.Y = frame * frameHeight;
            }
        }
    }
}
