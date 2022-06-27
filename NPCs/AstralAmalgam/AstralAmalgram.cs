using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.ItemDropRules;
using SpiritMod.Items.Armor.AstronautVanity;

namespace SpiritMod.NPCs.AstralAmalgam
{
	public class AstralAmalgram : ModNPC
	{
		private bool hasSpawnedBoys = false;
		private ref float chargetimer => ref NPC.ai[2];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Amalgam");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 60;
			NPC.damage = 24;
			NPC.defense = 4;
			NPC.lifeMax = 80;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 210f;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.knockBackResist = .40f;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.AstralAmalgamBanner>();
		}


		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.14f;
			NPC.scale = num395 + 0.95f;
			float velMax = 1f;
			float acceleration = 0.011f;
			NPC.TargetClosest(true);
			Vector2 center = NPC.Center;
			float deltaX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - center.X;
			float deltaY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
			NPC.ai[1] += 1f;
			if ((double)NPC.ai[1] > 600.0) {
				acceleration *= 8f;
				velMax = 4f;
				if ((double)NPC.ai[1] > 650.0) {
					NPC.ai[1] = 0f;
				}
			}
			else if ((double)distance < 250.0) {
				NPC.ai[0] += 0.9f;
				if (NPC.ai[0] > 0f) {
					NPC.velocity.Y = NPC.velocity.Y + 0.019f;
				}
				else {
					NPC.velocity.Y = NPC.velocity.Y - 0.019f;
				}
				if (NPC.ai[0] < -100f || NPC.ai[0] > 100f) {
					NPC.velocity.X = NPC.velocity.X + 0.019f;
				}
				else {
					NPC.velocity.X = NPC.velocity.X - 0.019f;
				}
				if (NPC.ai[0] > 200f) {
					NPC.ai[0] = -200f;
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
			if (Main.player[NPC.target].dead) {
				velLimitX = (float)((double)((float)NPC.direction * velMax) / 2.0);
				velLimitY = (float)((double)(-(double)velMax) / 2.0);
			}
			if (NPC.velocity.X < velLimitX) {
				NPC.velocity.X = NPC.velocity.X + acceleration;
			}
			else if (NPC.velocity.X > velLimitX) {
				NPC.velocity.X = NPC.velocity.X - acceleration;
			}
			if (NPC.velocity.Y < velLimitY) {
				NPC.velocity.Y = NPC.velocity.Y + acceleration;
			}
			else if (NPC.velocity.Y > velLimitY) {
				NPC.velocity.Y = NPC.velocity.Y - acceleration;
			}
			if ((double)velLimitX > 0.0) {
				NPC.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
			}
			if ((double)velLimitX < 0.0) {
				NPC.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
			}
			chargetimer++;
			if (chargetimer >= 300) {
				chargetimer = 0;
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X = direction.X * Main.rand.Next(8, 10);
				direction.Y = direction.Y * Main.rand.Next(2, 4);
				NPC.velocity.X = direction.X;
				NPC.velocity.Y = direction.Y;
				NPC.velocity.Y *= 0.98f;
				NPC.velocity.X *= 0.995f;
				for (int i = 0; i < 20; i++) {
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit, 0f, -2f, 0, default, .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != NPC.Center)
						Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			NPC.spriteDirection = NPC.direction;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.05f, 0.09f, 0.4f);

			if (!hasSpawnedBoys) {
				int latestNPC = NPC.whoAmI;
				for (int I = 0; I < 3; I++) {
					//cos = y, sin = x
					latestNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + (int)(Math.Sin(I * 120) * 80), (int)NPC.Center.Y + (int)(Math.Sin(I * 120) * 80), ModContent.NPCType<SpaceShield>(), NPC.whoAmI, 0, latestNPC);
					NPC shield = Main.npc[latestNPC];
					shield.ai[3] = NPC.whoAmI;
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
            NPC.frameCounter += 0.12f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GravityModulator>(), 400));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShieldCore>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PopRocks>(), 16));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AstronautLegs>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AstronautHelm>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AstronautBody>(), 50));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit, 2.5f * hitDirection, -2.5f, 0, default, .74f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam4").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam5").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam6").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam7").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Amalgam/Amalgam8").Type);
				for (int i = 0; i < 20; i++) {
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit, 0f, -2f, 0, default, .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != NPC.Center)
						Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				for (int k = 0; k < NPC.oldPos.Length; k++) {
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/AstralAmalgam/AstralAmalgam_Glow").Value);
			}
		}
	}
}