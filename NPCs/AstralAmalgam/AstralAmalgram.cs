using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Buffs;

namespace SpiritMod.NPCs.AstralAmalgam
{
	public class AstralAmalgram : ModNPC
	{
		private bool hasSpawnedBoys = false;
		private static int[] SpawnTiles = { };
		private ref float chargetimer => ref npc.ai[2];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Amalgam");
			Main.npcFrameCount[npc.type] = 4;
		}
		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 60;
			npc.damage = 24;
			npc.defense = 4;
			npc.lifeMax = 80;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 210f;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = true;
			npc.knockBackResist = .40f;
			npc.noTileCollide = true;
			npc.noGravity = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.AstralAmalgamBanner>();

		}


		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.14f;
			npc.scale = num395 + 0.95f;
			float velMax = 1f;
			float acceleration = 0.011f;
			npc.TargetClosest(true);
			Vector2 center = npc.Center;
			float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
			float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
			npc.ai[1] += 1f;
			if ((double)npc.ai[1] > 600.0) {
				acceleration *= 8f;
				velMax = 4f;
				if ((double)npc.ai[1] > 650.0) {
					npc.ai[1] = 0f;
				}
			}
			else if ((double)distance < 250.0) {
				npc.ai[0] += 0.9f;
				if (npc.ai[0] > 0f) {
					npc.velocity.Y = npc.velocity.Y + 0.019f;
				}
				else {
					npc.velocity.Y = npc.velocity.Y - 0.019f;
				}
				if (npc.ai[0] < -100f || npc.ai[0] > 100f) {
					npc.velocity.X = npc.velocity.X + 0.019f;
				}
				else {
					npc.velocity.X = npc.velocity.X - 0.019f;
				}
				if (npc.ai[0] > 200f) {
					npc.ai[0] = -200f;
				}
			}
			if ((double)distance > 350.0) {
				velMax = 5f;
				acceleration = 0.3f;
			}
			else if ((double)distance > 300.0) {
				velMax = 3f;
				acceleration = 0.2f;
			}
			else if ((double)distance > 250.0) {
				velMax = 1.5f;
				acceleration = 0.1f;
			}
			float stepRatio = velMax / distance;
			float velLimitX = deltaX * stepRatio;
			float velLimitY = deltaY * stepRatio;
			if (Main.player[npc.target].dead) {
				velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
				velLimitY = (float)((double)(-(double)velMax) / 2.0);
			}
			if (npc.velocity.X < velLimitX) {
				npc.velocity.X = npc.velocity.X + acceleration;
			}
			else if (npc.velocity.X > velLimitX) {
				npc.velocity.X = npc.velocity.X - acceleration;
			}
			if (npc.velocity.Y < velLimitY) {
				npc.velocity.Y = npc.velocity.Y + acceleration;
			}
			else if (npc.velocity.Y > velLimitY) {
				npc.velocity.Y = npc.velocity.Y - acceleration;
			}
			if ((double)velLimitX > 0.0) {
				npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
			}
			if ((double)velLimitX < 0.0) {
				npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
			}
			chargetimer++;
			if (chargetimer >= 300) {
				chargetimer = 0;
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X = direction.X * Main.rand.Next(8, 10);
				direction.Y = direction.Y * Main.rand.Next(2, 4);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
				npc.velocity.Y *= 0.98f;
				npc.velocity.X *= 0.995f;
				for (int i = 0; i < 20; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.DungeonSpirit, 0f, -2f, 0, default, .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			npc.spriteDirection = npc.direction;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.05f, 0.09f, 0.4f);

			if (!hasSpawnedBoys) {
				int latestNPC = npc.whoAmI;
				for (int I = 0; I < 3; I++) {
					//cos = y, sin = x
					latestNPC = NPC.NewNPC((int)npc.Center.X + (int)(Math.Sin(I * 120) * 80), (int)npc.Center.Y + (int)(Math.Sin(I * 120) * 80), ModContent.NPCType<SpaceShield>(), npc.whoAmI, 0, latestNPC);
					NPC shield = Main.npc[latestNPC];
					shield.ai[3] = npc.whoAmI;
					shield.ai[1] = I * 120;
					shield.netUpdate = true;
				}
				hasSpawnedBoys = true;
			}
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(hasSpawnedBoys);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			hasSpawnedBoys = reader.ReadBoolean();
		}
		public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.12f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
        public override void NPCLoot()
		{
			if (Main.rand.Next(1) == 400) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityModulator>());
			}
			if (Main.rand.Next(1) == 50) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ShieldCore>());
			}
			string[] lootTable = { "AstronautLegs", "AstronautHelm", "AstronautBody" };
			if (Main.rand.Next(40) == 0) {
				int loot = Main.rand.Next(lootTable.Length);
				{
					npc.DropItem(mod.ItemType(lootTable[loot]));
				}
            }
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PopRocks>());
            }
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.DungeonSpirit, 2.5f * hitDirection, -2.5f, 0, default, .74f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam4"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam5"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam6"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam7"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Amalgam/Amalgam8"));
				for (int i = 0; i < 20; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.DungeonSpirit, 0f, -2f, 0, default, .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			{
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/AstralAmalgam/AstralAmalgam_Glow"));
			}
		}
	}
}