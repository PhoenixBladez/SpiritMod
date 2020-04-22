using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.NPCs.Reach
{
	public class Reachman : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feral Hunter");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 44;
			npc.damage = 22;
			npc.defense = 8;
			npc.lifeMax = 66;
			npc.HitSound = SoundID.NPCHit2;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 90f;
			drawOffsetY = 4;
			npc.knockBackResist = .34f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.10f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0 && !Main.dayTime))
			{
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 2.7f : 0f;
			}
			return 0f;
		}
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(15) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SanctifiedStabber"));
			}

			if (Main.rand.Next(3) == 1)
			{
				int Bark = Main.rand.Next(2) + 1;
				for (int J = 0; J <= Bark; J++)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientBark"));
				}
			}
            if (!Main.dayTime)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EnchantedLeaf"));
            }

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(10) == 0 && Main.expertMode)
            {
                target.AddBuff(148, 2000);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Reach/Reachman_Glow"));
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
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
			}
		}
	}
}
