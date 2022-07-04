using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.TideDrops;

namespace SpiritMod.NPCs.Tides
{
	public class Crocomount : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crocosaur");
			Main.npcFrameCount[NPC.type] = 11;
		}

		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 70;
			NPC.damage = 32;
			NPC.defense = 14;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = 500f;
			NPC.knockBackResist = .1f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.CrocosaurBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.87f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.Green, .54f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Crocomount/CrocomountGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Crocomount/CrocomountGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Crocomount/CrocomountGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Crocomount/CrocomountGore4").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<CrocodrilloMountItem>(78);

		int frame = 0;
		int timer = 0;

		public override void AI()
		{
			if (NPC.wet)
			{
				NPC.noGravity = true;
				if (NPC.velocity.Y > -7)
					NPC.velocity.Y -= .085f;
				return;
			}
			else
				NPC.noGravity = false;

			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			float distance = NPC.DistanceSQ(target.Center);

			if (distance < 50 * 50)
				attack = true;

			if (distance > 80 * 80)
				attack = false;

			if (attack)
			{
				NPC.velocity.X = .008f * NPC.direction;
				timer++;
				if (timer >= 5)
				{
					frame++;
					timer = 0;
				}

				if (frame > 10)
					frame = 7;

				if (frame < 7)
					frame = 7;

				if (target.position.X > NPC.position.X)
					NPC.direction = 1;
				else
					NPC.direction = -1;
			}
			else
			{
				NPC.aiStyle = 26;
				AIType = NPCID.Unicorn;

				timer++;

				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}

				if (frame > 6)
					frame = 0;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (attack)
				target.AddBuff(BuffID.Bleeding, 600);
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;
	}
}