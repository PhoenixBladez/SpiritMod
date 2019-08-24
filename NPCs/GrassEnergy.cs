using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class GrassEnergy : ModNPC
	{
		int timer = 0;
		bool start = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Disturbed Soul");
		}

		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 20;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.damage = 18;
			npc.knockBackResist = 0;
			npc.lifeMax = 80;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			if (Main.rand.Next(180) == 1)
			{
				Main.PlaySound(6, (int)npc.position.X, (int)npc.position.Y, 0);
				Vector2 dir = Main.player[npc.target].Center - npc.Center;
				dir.Normalize();
				dir.X *= 12f;
				dir.Y *= 12f;

				int amountOfProjectiles = 1;
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-200, 200) * 0.01f;
					float B = (float)Main.rand.Next(-200, 200) * 0.01f;
					int damage = expertMode ? 9 : 12;
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, mod.ProjectileType("OvergrowthLeaf"), damage, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].hostile = true;
					Main.projectile[p].friendly = false;

				}
			}

			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.01f, 0.01f, 0.01f);

			npc.ai[1] = npc.ai[0];
			start = false;

			npc.TargetClosest(true);
			Vector2 direction = Main.player[npc.target].Center - npc.Center;
			direction.Normalize();
			direction *= 9f;
			npc.rotation = direction.ToRotation();

			Player player = Main.player[npc.target];
			NPC parent = Main.npc[NPC.FindFirstNPC(mod.NPCType("ForestWraith"))];
			//Factors for calculations
			double deg = (double)npc.ai[1]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
			double rad = deg * (Math.PI / 180); //Convert degrees to radians
			double dist = 50; //Distance away from the player

			/*Position the npc based on where the player is, the Sin/Cos of the angle times the /
    		/distance for the desired distance away from the player minus the npc's width   /
    		/and height divided by two so the center of the npc is at the right place.     */
			npc.position.X = parent.Center.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
			npc.position.Y = parent.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;

			//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
			npc.ai[1] += 2f;
			return true;
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

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.55f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.velocity != Vector2.Zero)
			{
				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				for (int i = 1; i < npc.oldPos.Length; ++i)
				{
					Vector2 vector2_2 = npc.oldPos[i];
					Microsoft.Xna.Framework.Color color2 = Color.White * npc.Opacity;
					color2.R = (byte)(0.5 * (double)color2.R * (double)(10 - i) / 20.0);
					color2.G = (byte)(0.5 * (double)color2.G * (double)(10 - i) / 20.0);
					color2.B = (byte)(0.5 * (double)color2.B * (double)(10 - i) / 20.0);
					color2.A = (byte)(0.5 * (double)color2.A * (double)(10 - i) / 20.0);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.oldPos[i].X - Main.screenPosition.X + (npc.width / 2),
						npc.oldPos[i].Y - Main.screenPosition.Y + npc.height / 2), new Rectangle?(npc.frame), color2, npc.oldRot[i], origin, npc.scale, SpriteEffects.None, 0.0f);
				}
			}
			return true;
		}

	}
}