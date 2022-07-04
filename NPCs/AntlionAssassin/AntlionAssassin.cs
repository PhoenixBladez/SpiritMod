using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.DataStructures;
using System.IO;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.AntlionAssassin
{
	public class AntlionAssassin : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Assassin");
			Main.npcFrameCount[NPC.type] = 6;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 22;
			NPC.height = 32;
			NPC.damage = 21;
			NPC.defense = 8;
			NPC.lifeMax = 74;
			NPC.HitSound = SoundID.NPCHit32;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 329f;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 3;
			AIType = NPCID.SnowFlinx;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.AntlionAssassinBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("These desert scavengers are lone wolves. They hunt antlions not to survive – but for sport, to establish dominance among their tribe."),
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.tileSand[spawnInfo.SpawnTileType])
				return SpawnCondition.OverworldDayDesert.Chance * 1.145f;
			return 0;
		}

		int invisibilityTimer;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedBrown, hitDirection, -1f, 1, default, .61f);
			}
			if (NPC.life <= 0)
			{
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Assassin1").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Assassin2").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Assassin3").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Assassin4").Type, 1f);
				}
				int ing = Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 825);
				Main.gore[ing].timeLeft = 30;
				int ing1 = Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 826);
				Main.gore[ing1].timeLeft = 30;
				int ing2 = Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 827);
				Main.gore[ing2].timeLeft = 30;
				for (int k = 0; k < 11; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedBrown, hitDirection, -1f, 1, default, .61f);
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			NPC.alpha++;
			invisibilityTimer++;
			if (invisibilityTimer >= 500)
			{
				for (int k = 0; k < 11; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedBrown, NPC.direction, -1f, 1, default, .61f);
				SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.Center);
				int ing = Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 825);
				Main.gore[ing].timeLeft = 130;
				int ing1 = Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 826);
				Main.gore[ing1].timeLeft = 130;
				int ing2 = Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 827);
				Main.gore[ing2].timeLeft = 130;
				NPC.alpha = 0;
				invisibilityTimer = 0;
			}
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < 11; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedBrown, NPC.direction, -1f, 1, default, .61f);

			if (NPC.alpha >= 220)
				SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.Center);

			int ing = Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, 825);
			Main.gore[ing].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			int ing1 = Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, 826);
			Main.gore[ing1].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			int ing2 = Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, 827);
			Main.gore[ing2].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			NPC.alpha = 0;
			invisibilityTimer = 0;
			NPC.alpha = 0;
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < 11; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedBrown, NPC.direction, -1f, 1, default, .61f);
			if (NPC.alpha >= 220)
				SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.Center);
			int ing = Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, 825);
			Main.gore[ing].timeLeft = 130;
			int ing1 = Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, 826);
			Main.gore[ing1].timeLeft = 130;
			int ing2 = Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, 827);
			Main.gore[ing2].timeLeft = 130;
			NPC.alpha = 0;
			invisibilityTimer = 0;
			NPC.alpha = 0;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(invisibilityTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			invisibilityTimer = reader.ReadInt32();
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(857, 53));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Hummus>(), 16));
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Microsoft.Xna.Framework.Color color12 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/AntlionAssassin/AntlionAssassin_Glow").Value, new Vector2((float)((double)NPC.position.X - (double)Main.screenPosition.X + (double)(NPC.width / 2) - (double)TextureAssets.Npc[NPC.type].Value.Width * (double)NPC.scale / 2.0 + (double)vector2_3.X * (double)NPC.scale), (float)((double)NPC.position.Y - (double)Main.screenPosition.Y + (double)NPC.height - (double)TextureAssets.Npc[NPC.type].Value.Height * (double)NPC.scale / (double)Main.npcFrameCount[NPC.type] + 4.0 + (double)vector2_3.Y * (double)NPC.scale)), new Microsoft.Xna.Framework.Rectangle?(NPC.frame), Microsoft.Xna.Framework.Color.White * .85f, NPC.rotation, vector2_3, NPC.scale, spriteEffects, 0.0f);
			if (NPC.velocity.Y != 0f)
			{
				for (int index = 1; index < 10; ++index)
				{
					Microsoft.Xna.Framework.Color color2 = new Color(255 - index * 10, 110 - index * 10, 110 - index * 10, 110 - index * 10);
					Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/AntlionAssassin/AntlionAssassin_Glow").Value, new Vector2((float)((double)NPC.position.X - (double)Main.screenPosition.X + (double)(NPC.width / 2) - (double)TextureAssets.Npc[NPC.type].Value.Width * (double)NPC.scale / 2.0 + (double)vector2_3.X * (double)NPC.scale), (float)((double)NPC.position.Y - (double)Main.screenPosition.Y + (double)NPC.height - (double)TextureAssets.Npc[NPC.type].Value.Height * (double)NPC.scale / (double)Main.npcFrameCount[NPC.type] + 4.0 + (double)vector2_3.Y * (double)NPC.scale)) - NPC.velocity * (float)index * 0.5f, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color2, NPC.rotation, vector2_3, NPC.scale, spriteEffects, 0.0f);
				}
			}
		}
	}
}
