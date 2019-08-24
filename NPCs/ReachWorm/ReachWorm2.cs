using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ReachWorm

{
	public class ReachWorm2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Burrower");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 10;
			npc.damage = 32;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.defense = 15;
			npc.lifeMax = 1;
			npc.knockBackResist = 0.0f;
			npc.behindTiles = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.dontCountMe = true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 3, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(180) == 1) //Fires desert feathers like a shotgun
			{
				Main.PlaySound(6, (int)npc.position.X, (int)npc.position.Y);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 14f;
				direction.Y *= 14f;

				int amountOfProjectiles = Main.rand.Next(1, 1);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-150, 150) * 0.01f;
					float B = (float)Main.rand.Next(-150, 150) * 0.01f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ProjectileID.PoisonSeedPlantera, 35, 1, Main.myPlayer, 0, 0);
				}
			}
			if (npc.ai[3] > 0)
				npc.realLife = (int)npc.ai[3];
			if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
				npc.TargetClosest(true);
			if (Main.player[npc.target].dead && npc.timeLeft > 300)
				npc.timeLeft = 300;

			if (Main.netMode != 1 && !Main.npc[(int)npc.ai[1]].active)
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
				NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
			}

			if (npc.ai[1] < (double)Main.npc.Length)
			{
				Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
				float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
				npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				float dist = (length - (float)npc.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				npc.velocity = Vector2.Zero;
				npc.position.X = npc.position.X + posX;
				npc.position.Y = npc.position.Y + posY;
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
	}
}