using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Reach
{
	public class ReachObserver : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wildwood Watcher");
			Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 30;
			NPC.damage = 21;
			NPC.defense = 6;
			NPC.lifeMax = 44;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 460f;
			NPC.knockBackResist = .423f;
			NPC.aiStyle = 44;
			NPC.noGravity = true;
			AIType = NPCID.FlyingFish;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.WildwoodWatcherBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.Player.GetSpiritPlayer().ZoneReach ? 2.25f : 0f;
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 7, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 4; i++)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(NPC.GetSource_FromAI(), new Vector2(NPC.position.X, NPC.position.Y + (Main.rand.Next(-50, 10))), new Vector2(hitDirection * 3f, 0f), 386, goreScale);
					Main.gore[a].timeLeft = 5;
				}
				for (int i = 0; i < 4; i++)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(NPC.GetSource_FromAI(), new Vector2(NPC.position.X, NPC.position.Y + (Main.rand.Next(-50, 10))), new Vector2(hitDirection * 3f, 0f), 387, goreScale);
					Main.gore[a].timeLeft = 5;
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Reach/ReachObserver_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, new Vector2((float)(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2.0 + vector2_3.X * NPC.scale), (float)(NPC.position.Y - Main.screenPosition.Y + NPC.height - TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4.0 + vector2_3.Y * NPC.scale)), new Microsoft.Xna.Framework.Rectangle?(NPC.frame), Microsoft.Xna.Framework.Color.White * .485f, NPC.rotation, vector2_3, NPC.scale, spriteEffects, 0.0f);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<AncientBark>(2);
			npcLoot.AddFood<CaesarSalad>(33);
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
			if (Main.rand.NextFloat() < 0.131579f)
			{
				int d = Dust.NewDust(NPC.position, NPC.width, NPC.height + 10, DustID.SlimeBunny, 0, 1f, 0, new Color(43, 54, 38, 80), 0.7f);
				Main.dust[d].velocity *= .1f;
			}
			NPC.rotation = NPC.velocity.X * 0.1f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}