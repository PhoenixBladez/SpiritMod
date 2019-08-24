using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class MoonFly : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;
		bool hat = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flutterfly");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 30;
			npc.damage = 41;
			npc.lifeMax = 320;
			npc.defense = 7;
			npc.knockBackResist = 0.1f;

			npc.noGravity = true;

			animationType = NPCID.Pixie;
			npc.HitSound = SoundID.NPCHit44;
			npc.DeathSound = SoundID.NPCDeath46;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 28;
				npc.height = 28;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon ? 5f : 0f;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

			Player player = Main.player[npc.target];

			if (npc.Center.X >= player.Center.X && moveSpeed >= -40) // flies to players x position
				moveSpeed--;

			if (npc.Center.X <= player.Center.X && moveSpeed <= 40)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 185f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.1f;
			if (Main.rand.Next(210) == 3)
				HomeY = -25f;


			if (Main.rand.Next(6) == 1)
			{
				int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 206, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
				int dust1 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 206, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust1].velocity *= 0f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust1].noGravity = true;
			}

			if (Main.rand.Next(200) == 1)
			{
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 43);
				Vector2 dir = Main.player[npc.target].Center - npc.Center;
				dir.Normalize();
				dir.X *= 12f;
				dir.Y *= 12f;

				int amountOfProjectiles = 1;
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-200, 200) * 0.01f;
					float B = (float)Main.rand.Next(-200, 200) * 0.01f;
					int damage = expertMode ? 19 : 27;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, mod.ProjectileType("StarSting"), damage, 1, Main.myPlayer, 0, 0);
				}
			}
			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200);
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MoonStone"));
		}
	}
}
