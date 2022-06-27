using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class Blubby : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blubby");
			Main.npcFrameCount[NPC.type] = 2;
			Main.npcCatchable[NPC.type] = true;
			Main.npcFrameCount[NPC.type] = 2;
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
			NPC.catchItem = (short)ModContent.ItemType<BlubbyItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 66;
			NPC.npcSlots = 0;
			NPC.noGravity = false; ;
			AIType = NPCID.Grubby;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				for (int k = 0; k < 10; k++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Scarecrow, 2.75f * hitDirection, -2.75f, 0, new Color(), 0.6f);
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			if (NPC.life == NPC.lifeMax)
				NPC.defense = 999;
			else
				NPC.defense = 0;
		}

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