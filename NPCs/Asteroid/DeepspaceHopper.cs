using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Asteroid
{
	public class DeepspaceHopper : ModNPC
	{
		Vector2 direction9 = Vector2.Zero;
		private bool shooting;
		private int timer = 300;
		private int distance = 150;
		private bool inblock = true;
		Vector2 target = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deepspace Hopper");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 15;
			npc.defense = 6;
			npc.lifeMax = 100;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .45f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.noTileCollide = false;
		}

		//public override void HitEffect(int hitDirection, double damage)
		//{
		//	if (npc.life <= 0)
		//	{
		//		Gore.NewGore(npc.position, npc.velocity, 13);
		//		Gore.NewGore(npc.position, npc.velocity, 12);
		//		Gore.NewGore(npc.position, npc.velocity, 11);
		//	}
		//}
	/*	public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}*/
       /* public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/GhastlyBeing_Glow"));
        }*/
	/*	public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}*/

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				return spawnInfo.player.GetSpiritPlayer().ZoneAsteroid ? 0.1f : 0f;
			}
			return 0f;
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			Vector2 center = npc.Center;
			Player player = Main.player[npc.target];
			
			timer++;
			if (timer >= 300)
			{
				int angle = Main.rand.Next(360);
				double anglex = Math.Sin(angle * (Math.PI / 180));
				double angley = Math.Cos(angle * (Math.PI / 180));
				npc.position.X = player.Center.X + (int)(distance * anglex);
				npc.position.Y = player.Center.Y + (int)(distance * angley);
				direction9 = player.Center - npc.Center;
				direction9.Normalize();
				npc.rotation = direction9.ToRotation();
				if (Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active())
				{
					npc.alpha = 255;
				}
				else
				{
					timer = 0;
					npc.alpha = 0;
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
					for (int i = 0; i < 15; i++)
					{
						int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Dust expr_62_cp_0 = Main.dust[num];
						expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						Main.dust[num].scale = 0.4f;
						if (Main.dust[num].position != npc.Center)
						{
							Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 3f;
						}
					}
				}
			}
			else
			{
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
			}
			if (timer == 100) //change to frame related later
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, mod.ProjectileType("HopperLaser"), 35, 1, Main.myPlayer);
			}
			return false;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(5) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulShred"), Main.rand.Next(1) + 1);
		}
	}
}
