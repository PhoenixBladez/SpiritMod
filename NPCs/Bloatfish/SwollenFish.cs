using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Sets.ReefhunterSet;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using SpiritMod.Items.Armor.DiverSet;

namespace SpiritMod.NPCs.Bloatfish
{
	public class SwollenFish : ModNPC
	{
		public ref float DashTimer => ref NPC.ai[1];

		public int frame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloatfish");
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 50;
			NPC.damage = 20;
			NPC.defense = 8;
			NPC.lifeMax = 130;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 90f;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.knockBackResist = 0.3f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			AIType = NPCID.Goldfish;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BloatfishBanner>();
		}

		public override void AI()
		{
			Player target = Main.player[NPC.target];

			if (target.wet)
			{
				NPC.noGravity = false;
				NPC.spriteDirection = -NPC.direction;

				if (DashTimer++ >= 60 && Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16 - 2)].LiquidAmount == 255)
				{
					var direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);
					NPC.velocity = direction * Main.rand.Next(10, 20);
					NPC.rotation = NPC.velocity.X * 0.2f;
					DashTimer = 0;
				}
			}
			else
			{
				NPC.spriteDirection = -NPC.direction;
				NPC.aiStyle = 16;
				NPC.noGravity = true;
				AIType = NPCID.Goldfish;
			}

			if (NPC.frameCounter++ == 3)
			{
				frame++;
				NPC.frameCounter = 0;
			}

			if (frame >= 4)
				frame = 1;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.BalloonPufferfish, 45));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IridescentScale>(), 1, 3, 6));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sushi>(), 16));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DiverLegs>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DiverHead>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DiverBody>(), 50));
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 || NPC.life >= 0)
			{
				for (int k = 0; k < 10; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemAmber, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemAmber, 2.5f * hitDirection, -2.5f, 0, Color.White, .67f);
				}

				if (NPC.life <= 0)
					for (int i = 1; i < 5; ++i)
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Bloatfish/Bloatfish" + i).Type, 1f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
				return 0f;
			return SpawnCondition.OceanMonster.Chance * 0.06f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Bleeding, 1800);
	}
}