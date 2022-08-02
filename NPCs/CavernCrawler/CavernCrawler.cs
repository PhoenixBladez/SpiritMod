using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Accessory.Leather;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
//using SpiritMod.Items.Armor.ClatterboneArmor;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.CavernCrawler
{
	public class CavernCrawler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Crawler");
			Main.npcFrameCount[NPC.type] = 18;
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 46;
			NPC.height = 34;
			NPC.damage = 17;
			NPC.defense = 9;
			NPC.lifeMax = 45;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath16;
			NPC.value = 160f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.CavernCrawlerBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("Despite a timid outward appearance, these curious cave-dwellers should not be underestimated."),
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
				return 0f;
			if (Main.hardMode)
				return SpawnCondition.Cavern.Chance * 0.02f;				
			return SpawnCondition.Cavern.Chance * 0.15f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrawlerockStaff>(), 100));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClatterboneShield>(), 60));
			npcLoot.Add(ItemDropRule.Common(ItemID.DepthMeter, 80));
			npcLoot.Add(ItemDropRule.Common(ItemID.Compass, 80));
			npcLoot.Add(ItemDropRule.Common(ItemID.Rally, 200));
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClatterboneBreastplate>(), 65));
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClatterboneFaceplate>(), 65));
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClatterboneLeggings>(), 65));
		}

		int frame = 0;
		int timer = 0;
		bool trailbehind;
		bool playsound;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			float distance = NPC.DistanceSQ(target.Center);
			if (distance < 320 * 320)
			{
				AIType = NPCID.Unicorn;
				NPC.aiStyle = 26;
				NPC.knockBackResist = 0.15f;
				trailbehind = true;
			}
			else
			{
				trailbehind = false;
				playsound = false;
				AIType = NPCID.Snail;
				NPC.aiStyle = 3;
				NPC.knockBackResist = 0.75f;
			}

			if (trailbehind && !playsound)
			{
				SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
				playsound = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			if (trailbehind) {
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				for (int k = 0; k < NPC.oldPos.Length; k++) {
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			timer++;
			float distance = NPC.DistanceSQ(Main.player[NPC.target].Center);

			if (distance < 320 * 320 || NPC.IsABestiaryIconDummy)
			{
				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}
				if (frame < 8)
				{
					frame = 8;
				}

				if (frame >= 17)
					frame = 8;
			}
			else
			{
				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}

				if (frame >= 7)
					frame = 0;
			}
			NPC.frame.Y = frameHeight * frame;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, .61f);
			}
			if (NPC.life <= 0) {
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Crawler1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Crawler2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Crawler3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Crawler4").Type);
			}
		}
	}
}
