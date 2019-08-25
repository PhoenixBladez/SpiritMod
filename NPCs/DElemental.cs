using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class DElemental : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demonite Elemental");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width =30;
			npc.height = 32;
			npc.damage = 25;
			npc.defense = 13;
			npc.lifeMax = 85;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = .45f;
			npc.aiStyle = 91;
			aiType = NPCID.GraniteFlyer;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCorrupt && NPC.downedBoss1 && spawnInfo.spawnTileY > Main.rockLayer ? 0.02f : 0f;
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
            SpiritUtility.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/DElemental_Glow"));
        }
		public override void NPCLoot()
		{
			if (Main.rand.Next(3) == 2)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 56);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 14, hitDirection, -1f, 0, default(Color), .61f);
				Dust.NewDust(npc.position, npc.width, npc.height, 75, hitDirection, -1f, 0, default(Color), .41f);
			}
			if (npc.life <= 0)
			{
				for (int k = 0; k < 11; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 14, hitDirection, -1f, 0, default(Color), .61f);
					Dust.NewDust(npc.position, npc.width, npc.height, 75, hitDirection, -1f, 0, default(Color), .41f);
				}				
				Gore.NewGore(npc.position, npc.velocity, 825);
				Gore.NewGore(npc.position, npc.velocity, 826);
				Gore.NewGore(npc.position, npc.velocity, 827);
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
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .1f, 0.05f, .136f);
			
			npc.spriteDirection = npc.direction;
		}
	}
}
