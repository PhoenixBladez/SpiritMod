using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Critters
{
	public class Prismite : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismite");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 28;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
						Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ItemID.Prismite;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.dontCountMe = true;
			npc.npcSlots = 0;
			aiType = NPCID.Goldfish;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
			
				for (int num621 = 0; num621 < 20; num621++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 242);
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity *= 0.5f;
				}
			}
		}
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .5f, .5f, .5f);
			npc.spriteDirection = -npc.direction;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RawFish"), 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneHoly && spawnInfo.water ? 0.05f : 0f;
		}
	
	}
}
