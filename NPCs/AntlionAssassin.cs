using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class AntlionAssassin : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Assassin");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 22;
			npc.height = 32;
			npc.damage = 21;
			npc.defense = 8;
			npc.lifeMax = 74;
			npc.HitSound = SoundID.NPCHit32;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 329f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.AntlionAssassinBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{

			if (Main.tileSand[spawnInfo.spawnTileType])
				return SpawnCondition.OverworldDayDesert.Chance * 0.1f;
			return 0;
		}
		int timer;
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 85, hitDirection, -1f, 1, default(Color), .61f);
			}
			if (npc.life <= 0) {
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin4"), 1f);
				}
				if (Main.LocalPlayer.GetSpiritPlayer().emptyAntlionScroll) {
					MyWorld.numAntlionsKilled++;
				}
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 85, hitDirection, -1f, 1, default(Color), .61f);
				}
				int ing = Gore.NewGore(npc.position, npc.velocity, 825);
				Main.gore[ing].timeLeft = 30;
				int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
				Main.gore[ing1].timeLeft = 30;
				int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
				Main.gore[ing2].timeLeft = 30;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			npc.alpha++;
			timer++;
			if (timer >= 500) {
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
				}
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
				int ing = Gore.NewGore(npc.position, npc.velocity, 825);
				Main.gore[ing].timeLeft = 130;
				int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
				Main.gore[ing1].timeLeft = 130;
				int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
				Main.gore[ing2].timeLeft = 130;
				npc.alpha = 0;
				timer = 0;
			}
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
			}
			Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
			int ing = Gore.NewGore(npc.position, npc.velocity, 825);
			Main.gore[ing].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
			Main.gore[ing1].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
			Main.gore[ing2].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			npc.alpha = 0;
			timer = 0;
			npc.alpha = 0;
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
			}
			Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
			int ing = Gore.NewGore(npc.position, npc.velocity, 825);
			Main.gore[ing].timeLeft = 130;
			int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
			Main.gore[ing1].timeLeft = 130;
			int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
			Main.gore[ing2].timeLeft = 130;
			npc.alpha = 0;
			timer = 0;
			npc.alpha = 0;
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(25)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 857);
			}
		}
	}
}
