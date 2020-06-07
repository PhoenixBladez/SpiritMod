using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			npc.width = 24;
			npc.height = 24;
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
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 1.25f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 7;
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach3"));
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				int Bark = Main.rand.Next(1) + 1;
				for (int J = 0; J <= Bark; J++)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientBark>());
				}
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
			
			{
				if (Main.rand.NextFloat() < 0.131579f)
				{
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
