using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Sets.ReefhunterSet;

namespace SpiritMod.NPCs.Bloatfish
{
	public class SwollenFish : ModNPC
	{
		public ref float DashTimer => ref npc.ai[1];

		public int frame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloatfish");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 50;
			npc.damage = 20;
			npc.defense = 8;
			npc.lifeMax = 130;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 90f;
			npc.buffImmune[BuffID.Confused] = true;
			npc.knockBackResist = 0.3f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			aiType = NPCID.Goldfish;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.BloatfishBanner>();
		}

		public override void AI()
		{
			Player target = Main.player[npc.target];

			if (target.wet)
			{
				npc.noGravity = false;
				npc.spriteDirection = -npc.direction;

				if (DashTimer++ >= 60 && Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16 - 2)].liquid == 255)
				{
					var direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center);
					npc.velocity = direction * Main.rand.Next(10, 20);
					npc.rotation = npc.velocity.X * 0.2f;
					DashTimer = 0;
				}
			}
			else
			{
				npc.spriteDirection = -npc.direction;
				npc.aiStyle = 16;
				npc.noGravity = true;
				aiType = NPCID.Goldfish;
			}

			if (npc.frameCounter++ == 3)
			{
				frame++;
				npc.frameCounter = 0;
			}

			if (frame >= 4)
				frame = 1;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(40) == 0)
				npc.DropItem(mod.ItemType(Main.rand.Next(new string[] { "DiverLegs", "DiverHead", "DiverBody" })));

			if (Main.rand.NextBool(16))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Sushi>());

			if (Main.rand.NextBool(45))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BalloonPufferfish);

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<IridescentScale>(), Main.rand.Next(3, 7));
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 || npc.life >= 0)
			{
				for (int k = 0; k < 10; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Amber, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Amber, 2.5f * hitDirection, -2.5f, 0, Color.White, .67f);
				}

				if (npc.life <= 0)
					for (int i = 1; i < 5; ++i)
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bloatfish/Bloatfish" + i), 1f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
				return 0f;
			return SpawnCondition.OceanMonster.Chance * 0.06f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Bleeding, 1800);
	}
}