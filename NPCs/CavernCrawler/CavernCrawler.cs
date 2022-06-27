using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Accessory.Leather;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

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

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe) {
				return 0f;
			}
			if (Main.hardMode)
			{
				return SpawnCondition.Cavern.Chance * 0.02f;				
			}
			return SpawnCondition.Cavern.Chance * 0.15f;
		}
		public override void OnKill()
		{
			if (Main.rand.Next(100) == 4)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, (ModContent.ItemType<CrawlerockStaff>()));

            if (Main.rand.NextBool(60))
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<ClatterboneShield>());

            if (Main.rand.Next(80) == 0)
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.DepthMeter);

            if (Main.rand.Next(80) == 0)
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Compass);

            if (Main.rand.Next(200) == 0)
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Rally);

            string[] lootTable = { "ClatterboneBreastplate", "ClatterboneFaceplate", "ClatterboneLeggings" };
            if (Main.rand.Next(55) == 0)
                NPC.DropItem(Mod.Find<ModItem>(Main.rand.Next(lootTable)).Type);
        }
        int frame = 0;
		int timer = 0;
		bool trailbehind;
		bool playsound;
		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
            timer++;
            if (distance < 320) {
				{
					AIType = NPCID.Unicorn;
					NPC.aiStyle = 26;
					NPC.knockBackResist = 0.15f;
					trailbehind = true;
				}
				if (timer >= 4) {
					frame++;
					timer = 0;
				}
				if (frame < 8) {
					frame = 8;
				}
				if (frame >= 17) {
					frame = 8;
				}
			}
			else {
				trailbehind = false;
				playsound = false;
				AIType = NPCID.Snail;
				NPC.aiStyle = 3;
				NPC.knockBackResist = 0.75f;
				if (timer >= 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 7) {
					frame = 0;
				}
			}
			if (trailbehind && !playsound) {
				SoundEngine.PlaySound(SoundID.Item9.SoundId, NPC.Center, 74);
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
					Color color = NPC.GetAlpha(lightColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return true;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y = frameHeight * frame;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, .61f);
			}
			if (NPC.life <= 0) {
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, NPC.Center);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Crawler1").Type);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Crawler2").Type);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Crawler3").Type);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Crawler4").Type);
			}
		}
	}
}
