using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.BloodMoon.SkeletonBrute
{
	public class Skeleton_Brute : ModNPC
	{
		public int attacking = 0;
		public bool frameRes = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeleton Brute");
			Main.npcFrameCount[npc.type] = 10;
		}
		public override void SetDefaults()
		{
			npc.lifeMax = 250;
			npc.defense = 10;
			npc.value = 400f;
			aiType = 0;
			npc.knockBackResist = 0.3f;
			npc.width = 35;
			npc.height = 80;
			npc.damage = 26;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			if ((double)Vector2.Distance(player.Center, npc.Center) <= (double)120f)
			{
				attacking = 1;
				npc.aiStyle = 0;
				npc.velocity.X = 0f;
				npc.velocity.Y = 8f;				
			}
			else
			{
				npc.aiStyle = 3;
				frameRes = true;	
				attacking = 0;
			}
			if (attacking == 1)
			{
				if (!frameRes)
				{
					frameRes = true;
					npc.frameCounter = 0;
				}	
				npc.velocity.X = 0f;
				npc.velocity.Y = 8f;				
			}
		}

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BloodFire>(), Main.rand.Next(3, 6));
        }
        public override void HitEffect(int hitDirection, double damage)
		{		
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 26, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 8, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore5"), 1f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && !NPC.AnyNPCs(ModContent.NPCType<Skeleton_Brute>()) && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedAncientFlier || MyWorld.downedMoonWizard || MyWorld.downedRaider) ? 0.05f : 0f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X + 20*npc.spriteDirection, npc.Center.Y-22) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}
        public override void FindFrame(int frameHeight)
        {
            const int Frame_1 = 0;
            const int Frame_2 = 1;
            const int Frame_3 = 2;
            const int Frame_4 = 3;
            const int Frame_5 = 4;
            const int Frame_6 = 5;
            const int Frame_7 = 6;
            const int Frame_8 = 7;
            const int Frame_9 = 8;
            const int Frame_10 = 9;

            npc.frame.Width = 240;
            Player player = Main.player[npc.target];
            npc.frameCounter++;

            if (npc.velocity.Y != 0f)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (attacking == 0 && npc.velocity.X == 0f)
            {
                npc.frame.Y = 2 * frameHeight;
                npc.frame.X = 0;
            }
            else if (attacking == 1)
            {
                if (npc.frameCounter < 4)
                {
                    npc.frame.Y = 3 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 8)
                {
                    npc.frame.Y = 3 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 12)
                {
                    npc.frame.Y = 4 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 16)
                {
                    npc.frame.Y = 4 * frameHeight;
                    npc.frame.X = 240;
                    if (npc.frameCounter == 13 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) && (double)Vector2.Distance(player.Center, npc.Center) <= (double)150f)
                    {
                        player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage * 1.5f), 0, false, false, false, -1);
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 37, 1f, 0.3f);
                        player.velocity.X = npc.direction * 13f;
                        player.velocity.Y = -9f;
                    }
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = 5 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 24)
                {
                    npc.frame.Y = 5 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 28)
                {
                    npc.frame.Y = 6 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 32)
                {
                    npc.frame.Y = 6 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 36)
                {
                    npc.frame.Y = 7 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 40)
                {
                    npc.frame.Y = 7 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 44)
                {
                    npc.frame.Y = 8 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 48)
                {
                    npc.frame.Y = 8 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 52)
                {
                    npc.frame.Y = 9 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 70)
                {
                    npc.frame.Y = 9 * frameHeight;
                    npc.frame.X = 240;
                    if (npc.frameCounter == 69)
                    {
                        attacking = 0;
                        frameRes = false;
                    }
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            else if (attacking == 0)
            {
                if (npc.frameCounter < 7)
                {
                    npc.frame.Y = 0 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 14)
                {
                    npc.frame.Y = 0 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 21)
                {
                    npc.frame.Y = 1 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 28)
                {
                    npc.frame.Y = 1 * frameHeight;
                    npc.frame.X = 240;
                }
                else if (npc.frameCounter < 35)
                {
                    npc.frame.Y = 2 * frameHeight;
                    npc.frame.X = 0;
                }
                else if (npc.frameCounter < 42)
                {
                    npc.frame.Y = 2 * frameHeight;
                    npc.frame.X = 240;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
        }
	}
}