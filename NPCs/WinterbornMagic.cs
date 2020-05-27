using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class WinterbornMagic : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winterborn Mage");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 30;
			npc.height = 44;

			npc.lifeMax = 112;
			npc.defense = 6;
			npc.damage = 34;

			npc.HitSound = SoundID.NPCDeath15;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[mod.BuffType("CryoCrush")] = true;
            npc.value = 289f;
			npc.knockBackResist = 0.15f;
			npc.noGravity = false;
			npc.netAlways = true;
			npc.chaseable = false;
			npc.lavaImmune = true;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CryoliteOre"), 1 + Main.rand.Next(2, 4));
			if(Main.rand.Next(5) == 0)
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WintryCharmMage"));
		}

		public override bool PreAI()
		{
            bool expertMode = Main.expertMode;
			npc.TargetClosest(true);
			npc.velocity.X = npc.velocity.X * 0.93f;
			if (npc.velocity.X > -0.1F && npc.velocity.X < 0.1F)
				npc.velocity.X = 0;
			if (npc.ai[0] == 0)
				npc.ai[0] = 500f;

			if (npc.ai[2] != 0 && npc.ai[3] != 0)
			{
				// Teleport effects: away.
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
				npc.position.X = (npc.ai[2] * 16 - (npc.width / 2) + 8);
				npc.position.Y = npc.ai[3] * 16f - npc.height;
				npc.velocity.X = 0.0f;
				npc.velocity.Y = 0.0f;
				npc.ai[2] = 0.0f;
				npc.ai[3] = 0.0f;
				// Teleport effects: arrived.
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
			}

			++npc.ai[0];

			if (npc.ai[0] == 100|| npc.ai[0] == 300)
			{
				npc.ai[1] = 30f;
				npc.netUpdate = true;
			}

			bool teleport = false;
			// Teleport
			if (npc.ai[0] >= 500 && Main.netMode != 1)
			{
				teleport = true;
			}

			if (teleport)
			{
				Teleport();
				npc.ai[0] = 1;
				npc.ai[1] = 0;
			}
			if (npc.ai[1] > 0)
			{
				--npc.ai[1];
				if (npc.ai[1] == 15)
				{
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                    if (Main.netMode != 1)
					{
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        direction.X *= 4.9f;
                        direction.Y *= 4.9f;		
					    int amountOfProjectiles = 1;
                        for (int i = 0; i < amountOfProjectiles; ++i)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                int somedamage = expertMode ? 15 : 30;
                                int p = Projectile.NewProjectile(Main.player[npc.target].Center.X, Main.player[npc.target].Center.Y - 300, 0, 0  , mod.ProjectileType("IceCloudHostile"), somedamage, 1, Main.myPlayer, 0, 0);
                                Main.projectile[p].hostile = true;
                                Main.projectile[p].friendly = false;
                                Main.projectile[p].tileCollide = false;
                            }
                            else
                            {
                                int somedamage = expertMode ? 17 : 34;
                                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, 118, somedamage, 1, Main.myPlayer, 0, 0);
                                Main.projectile[p].hostile = true;
                                Main.projectile[p].friendly = false;
                                Main.projectile[p].tileCollide = false;
                            }
                        }
					}
				}
			}

			if (Main.rand.Next(3) == 0)
				return false;
			Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 187, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
			dust.noGravity = true;
			dust.velocity.X = dust.velocity.X * 0.3f;
			dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

			return false;
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
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/WinterbornMagic_Glow"));
        }
	
		public void Teleport()
		{
			npc.ai[0] = 1f;
			int num1 = (int)Main.player[npc.target].position.X / 16;
			int num2 = (int)Main.player[npc.target].position.Y / 16;
			int num3 = (int)npc.position.X / 16;
			int num4 = (int)npc.position.Y / 16;
			int num5 = 20;
			int num6 = 0;
			bool flag1 = false;
			if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000.0)
			{
				num6 = 100;
				flag1 = true;
			}
			while (!flag1 && num6 < 100)
			{
				++num6;
				int index1 = Main.rand.Next(num1 - num5, num1 + num5);
				for (int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2)
				{
					if ((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].nactive())
					{
						bool flag2 = true;
						if (Main.tile[index1, index2 - 1].lava())
							flag2 = false;
						if (flag2 && Main.tileSolid[(int)Main.tile[index1, index2].type] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1))
						{
							npc.ai[1] = 20f;
							npc.ai[2] = (float)index1;
							npc.ai[3] = (float)index2;
							flag1 = true;
							break;
						}
					}
				}
			}
			npc.netUpdate = true;
		}

		public override void FindFrame(int frameHeight)
		{
			int currShootFrame = (int)npc.ai[1];
			if (currShootFrame >= 25)
				npc.frame.Y = frameHeight;
			else if (currShootFrame >= 20)
				npc.frame.Y = frameHeight * 2;
			else if (currShootFrame >= 15)
				npc.frame.Y = frameHeight * 3;
			else if (currShootFrame >= 10)
				npc.frame.Y = frameHeight * 2;
			else if (currShootFrame >= 5)
				npc.frame.Y = frameHeight;
			else
				npc.frame.Y = 0;

			npc.spriteDirection = npc.direction;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.downedBoss3 && ((spawnInfo.spawnTileY > Main.rockLayer && spawnInfo.player.ZoneSnow) || (spawnInfo.player.ZoneSnow && Main.raining && spawnInfo.player.ZoneOverworldHeight)) ? 0.035f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			int d = 206;
			int d1 = 187;
			for (int k = 0; k < 5; k++)
			{
				{
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
					Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
				}	
			}		
			if (npc.life <= 0)
			{
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore3"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore4"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore5"), 1f);
                }
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0f, 0f, 100, default(Color), .8f);
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.35f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), .43f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 3f;
				}
			}
		}
	}
}