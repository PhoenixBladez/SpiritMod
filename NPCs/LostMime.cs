using Microsoft.Xna.Framework;
using SpiritMod.Items.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Weapon.Thrown;

namespace SpiritMod.NPCs
{
	public class LostMime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Mime");
			Main.npcFrameCount[npc.type] = 14;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 42;
			npc.damage = 30;
			npc.defense = 10;
			npc.lifeMax = 200;
			npc.value = 80f;
			npc.knockBackResist = .25f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.LostMimeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.015f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Confused, 60);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.rand.Next(2) == 0) {
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
				npc.Transform(ModContent.NPCType<CaptiveMask>());
			}
			int d = 5;
			for (int k = 0; k < 10; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.87f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(3)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MimeMask>(), 1);
            }
            if (Main.rand.Next(30) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Food.Baguette>());
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MimeBomb>(), Main.rand.Next(12, 23));
		}

	}
}
