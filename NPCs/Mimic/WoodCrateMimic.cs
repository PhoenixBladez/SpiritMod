using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mimic
{
	public class WoodCrateMimic : ModNPC
	{
		bool jump = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Crate Mimic");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 42;
			npc.damage = 9;
			npc.defense = 4;
			npc.lifeMax = 50;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 360f;
			npc.knockBackResist = 0.34f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.WoodCrateMimicBanner>();
		}

		int frame = 2;
		int timer = 0;
		int mimictimer = 0;

		public override void AI()
		{
			npc.spriteDirection = npc.direction;

			if (npc.wet)
				npc.noGravity = true;
			else
				npc.noGravity = false;
			mimictimer++;
			if (mimictimer <= 80)
			{
				frame = 0;
				mimictimer = 81;
			}

			timer++;
			if (timer == 4)
			{
				frame++;
				timer = 0;
			}

			if (frame == 4)
				frame = 1;

			if (npc.collideY && jump && npc.velocity.Y > 0)
			{
				if (Main.rand.Next(8) == 0)
				{
					jump = false;
					for (int i = 0; i < 20; i++)
					{
						int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.SpookyWood, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
						Main.dust[dust].noGravity = true;
					}
				}
			}

			if (!npc.collideY)
				jump = true;
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity / 6, 220);
				Gore.NewGore(npc.position, npc.velocity / 6, 221);
				Gore.NewGore(npc.position, npc.velocity / 6, 222);
			}

			for (int k = 0; k < 30; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, 7, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
		}

		public override void NPCLoot() => Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WoodenCrate);
	}
}