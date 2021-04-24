using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
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
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 2.25f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 7;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}
			if (npc.life <= 0) {
				for (int i = 0; i<4; i++)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (Main.rand.Next(-50,10))), new Vector2(hitDirection*3f, 0f), 386, goreScale);
					Main.gore[a].timeLeft = 5;
				}
				for (int i = 0; i<4; i++)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (Main.rand.Next(-50,10))), new Vector2(hitDirection*3f, 0f), 387, goreScale);
					Main.gore[a].timeLeft = 5;
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Microsoft.Xna.Framework.Color color12 = Lighting.GetColor((int) ((double) npc.position.X + (double) npc.width * 0.5) / 16, (int) (((double) npc.position.Y + (double) npc.height * 0.5) / 16.0));
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Reach/ReachObserver_Glow"), new Vector2((float) ((double) npc.position.X - (double) Main.screenPosition.X + (double) (npc.width / 2) - (double) Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) npc.position.Y - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) ), new Microsoft.Xna.Framework.Rectangle?(npc.frame), Microsoft.Xna.Framework.Color.White * .485f, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				int Bark = Main.rand.Next(1) + 1;
				for (int J = 0; J <= Bark; J++) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientBark>());
				}
            }
            if (Main.rand.NextBool(33))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CaesarSalad>());
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
			{
				if (Main.rand.NextFloat() < 0.131579f) {
					{
						Dust dust;
						Vector2 position = npc.Center;
						int d = Dust.NewDust(npc.position, npc.width, npc.height + 10, 193, 0, 1f, 0, new Color(43, 54, 38, 80), 0.7f);
						Main.dust[d].velocity *= .1f;
					}
				}
				npc.rotation = npc.velocity.X * 0.1f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
