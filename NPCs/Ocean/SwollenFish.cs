using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Ocean
{
	public class SwollenFish : ModNPC
	{		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloatfish");
			Main.npcFrameCount[npc.type] = 5;

		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 50;
			npc.damage = 20;
			npc.defense = 8;
			npc.lifeMax = 130;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 90f;
			npc.knockBackResist = 0.3f;
            npc.aiStyle = 16;
			npc.noGravity = true;
			aiType = NPCID.Goldfish;		
		}
		int frame = 1;
        int timer = 0;
        int dashtimer = 0;
		public override void AI()
		{
            Player target = Main.player[npc.target];
            if (target.wet)
            {
                npc.noGravity = false;
                npc.spriteDirection = -npc.direction;

                timer++;
                dashtimer++;
                if (timer == 3)
                {
                    frame++;
                    timer = 0;
                }
                if (frame >= 5)
                {
                    frame = 1;
                }
                if (dashtimer >= 60 && Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16 - 2)].liquid == 255)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    npc.spriteDirection = (int)(1 * npc.velocity.X);

                    npc.velocity.Y = direction.Y * Main.rand.Next(10, 20);
                    npc.velocity.X = direction.X * Main.rand.Next(10, 20);
                    npc.rotation = npc.velocity.X * 0.2f;
                    dashtimer = 0;
                }
            }
            else
            {
                npc.spriteDirection = -npc.direction;
                npc.aiStyle = 16;
                npc.noGravity = true;
                aiType = NPCID.Goldfish;
                timer++;
                dashtimer++;
                if (timer == 3)
                {
                    frame++;
                    timer = 0;
                }
                if (frame >= 5)
                {
                    frame = 1;
                }
            }
				
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0 || npc.life >= 0)
			{
				int d = 138;
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .67f);
				}

				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .77f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .47f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
			}
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe)
            {
                return 0f;
            }
            return SpawnCondition.OceanMonster.Chance * 0.1f;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Bleeding, 1800);
		}
	}
}
