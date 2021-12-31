using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;

namespace SpiritMod.NPCs.Bloatfish
{
	public class SwollenFish : ModNPC
	{
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
		int frame = 1;
		int timer = 0;
		int dashtimer = 0;
		public override void AI()
		{
			Player target = Main.player[npc.target];
			if (target.wet) {
				npc.noGravity = false;
				npc.spriteDirection = -npc.direction;

				timer++;
				dashtimer++;
				if (timer == 3) {
					frame++;
					timer = 0;
				}
				if (frame >= 5) {
					frame = 1;
				}
				if (dashtimer >= 60 && Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16 - 2)].liquid == 255) {
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					npc.spriteDirection = (int)(1 * npc.velocity.X);

					npc.velocity.Y = direction.Y * Main.rand.Next(10, 20);
					npc.velocity.X = direction.X * Main.rand.Next(10, 20);
					npc.rotation = npc.velocity.X * 0.2f;
					dashtimer = 0;
				}
			}
			else {
				npc.spriteDirection = -npc.direction;
				npc.aiStyle = 16;
				npc.noGravity = true;
				aiType = NPCID.Goldfish;
				timer++;
				dashtimer++;
				if (timer == 3) {
					frame++;
					timer = 0;
				}
				if (frame >= 5) {
					frame = 1;
				}
			}

		}
		public override void NPCLoot()
		{
			string[] lootTable = { "DiverLegs", "DiverHead", "DiverBody" };
			if (Main.rand.Next(40) == 0) {
				int loot = Main.rand.Next(lootTable.Length);
				{
					npc.DropItem(mod.ItemType(lootTable[loot]));
				}
			}
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Sushi>());
            }
        }
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 || npc.life >= 0) {
				int d = 138;
				for (int k = 0; k < 10; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .67f);
				}
                if (npc.life <= 0)
                {
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bloatfish/Bloatfish1"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bloatfish/Bloatfish2"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bloatfish/Bloatfish3"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bloatfish/Bloatfish4"), 1f);
                }
			}
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(frame);
			writer.Write(timer);
			writer.Write(dashtimer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			frame = reader.ReadInt32();
			timer = reader.ReadInt32();
			dashtimer = reader.ReadInt32();
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.06f;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Bleeding, 1800);
		}
	}
}
