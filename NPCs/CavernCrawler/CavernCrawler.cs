using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Accessory.Leather;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.CavernCrawler
{
	public class CavernCrawler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Crawler");
			Main.npcFrameCount[npc.type] = 18;
			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 34;
			npc.damage = 17;
			npc.defense = 9;
			npc.lifeMax = 45;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath16;
			npc.value = 160f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.CavernCrawlerBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			if (Main.hardMode)
			{
				return SpawnCondition.Cavern.Chance * 0.02f;				
			}
			return SpawnCondition.Cavern.Chance * 0.15f;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(100) == 4) {

				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ModContent.ItemType<CrawlerockStaff>()));
            }
            if (Main.rand.NextBool(60))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ClatterboneShield>());
            }
            if (Main.rand.Next(80) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
            }
            if (Main.rand.Next(80) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Compass);
            }
            if (Main.rand.Next(200) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Rally);
            }
            string[] lootTable = { "ClatterboneBreastplate", "ClatterboneFaceplate", "ClatterboneLeggings" };
            if (Main.rand.Next(55) == 0)
            {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
            }
            /*int Techs = Main.rand.Next(1, 4);
			for (int J = 0; J <= Techs; J++) {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Carapace>());
			}*/
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
            timer++;
            if (distance < 320) {
				{
					aiType = NPCID.Unicorn;
					npc.aiStyle = 26;
					npc.knockBackResist = 0.15f;
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
				aiType = NPCID.Snail;
				npc.aiStyle = 3;
				npc.knockBackResist = 0.75f;
				if (timer >= 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 7) {
					frame = 0;
				}
			}
			if (trailbehind && !playsound) {
				Main.PlaySound(SoundID.Item9.SoundId, npc.Center, 74);
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
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection, -1f, 0, default(Color), .61f);
			}
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.DD2_WitherBeastDeath, npc.Center);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler4"));
			}
		}
	}
}
