using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Summon;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class CavernCrawler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Crawler");
			Main.npcFrameCount[npc.type] = 16;
			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 28;
			npc.damage = 22;
			npc.defense = 9;
			npc.lifeMax = 55;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath16;
			npc.value = 160f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.CavernCrawlerBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !NPC.downedBoss1) {
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.15f;
		}
		public override void NPCLoot()
		{
			if (Main.LocalPlayer.GetSpiritPlayer().emptyWheezerScroll) {
				MyWorld.numWheezersKilled++;
			}
			if (Main.rand.Next(100) == 4) {

				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ModContent.ItemType<CrawlerockStaff>()));
			}
			if (Main.rand.Next(10000) == 125) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
			}
			if (Main.rand.Next(10000) == 125) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Compass);
			}
			if (Main.rand.Next(1000) == 39) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Rally);
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ModContent.ItemType<Carapace>()), Main.rand.Next(2) + 1);
		}
		int frame = 0;
		int timer = 0;
		bool trailbehind;
		bool playsound;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 320) {
				{
					aiType = NPCID.Unicorn;
					npc.aiStyle = 26;
					npc.knockBackResist = 0.15f;
					trailbehind = true;
				}
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame < 14) {
					frame = 14;
				}
				if (frame >= 16) {
					frame = 14;
				}
			}
			else {
				trailbehind = false;
				playsound = false;
				aiType = NPCID.Snail;
				npc.aiStyle = 3;
				npc.knockBackResist = 0.75f;
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 13) {
					frame = 1;
				}
			}
			if (trailbehind && !playsound) {
				Main.PlaySound(29, npc.Center, 74);
				playsound = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			if (trailbehind) {
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return true;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .61f);
			}
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.DD2_WitherBeastDeath, npc.Center);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler4"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler5"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler6"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler7"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler8"));
			}
		}
	}
}
