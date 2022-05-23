using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;

namespace SpiritMod.NPCs.Reach
{
	public class ReachObserver : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wildwood Watcher");
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 30;
			npc.damage = 21;
			npc.defense = 6;
			npc.lifeMax = 44;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 460f;
			npc.knockBackResist = .423f;
			npc.aiStyle = 44;
			npc.noGravity = true;
			aiType = NPCID.FlyingFish;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.WildwoodWatcherBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 2.25f : 0f;
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.GrassBlades, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, 7, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0)
			{
				for (int i = 0; i < 4; i++)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (Main.rand.Next(-50, 10))), new Vector2(hitDirection * 3f, 0f), 386, goreScale);
					Main.gore[a].timeLeft = 5;
				}
				for (int i = 0; i < 4; i++)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (Main.rand.Next(-50, 10))), new Vector2(hitDirection * 3f, 0f), 387, goreScale);
					Main.gore[a].timeLeft = 5;
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Reach/ReachObserver_Glow"), new Vector2((float)(npc.position.X - Main.screenPosition.X + (npc.width / 2) - Main.npcTexture[npc.type].Width * npc.scale / 2.0 + vector2_3.X * npc.scale), (float)(npc.position.Y - Main.screenPosition.Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4.0 + vector2_3.Y * npc.scale)), new Microsoft.Xna.Framework.Rectangle?(npc.frame), Microsoft.Xna.Framework.Color.White * .485f, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				int Bark = Main.rand.Next(1) + 1;
				for (int J = 0; J <= Bark; J++)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientBark>());
			}

			if (Main.rand.NextBool(33))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CaesarSalad>());
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
			if (Main.rand.NextFloat() < 0.131579f)
			{
				int d = Dust.NewDust(npc.position, npc.width, npc.height + 10, DustID.SlimeBunny, 0, 1f, 0, new Color(43, 54, 38, 80), 0.7f);
				Main.dust[d].velocity *= .1f;
			}
			npc.rotation = npc.velocity.X * 0.1f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++)
			{
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}