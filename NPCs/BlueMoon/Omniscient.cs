using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class Omniscient : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;
		bool hat = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Omniscient");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 62;
			npc.height = 84;
			npc.damage = 48;
			npc.defense = 11;
			npc.lifeMax = 500;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 2000f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			aiType = NPCID.Wraith;
			npc.knockBackResist = 0f;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 1.6f);
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
			return MyWorld.BlueMoon ? 2f : 0f;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.20f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

			if (Main.rand.Next(180) == 1)
			{
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 91);
				Vector2 dir = Main.player[npc.target].Center - npc.Center;
				dir.Normalize();
				dir.X *= 12f;
				dir.Y *= 12f;

				int amountOfProjectiles = 1;
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-100, 100) * 0.01f;
					float B = (float)Main.rand.Next(-100, 100) * 0.01f;
					int damage = expertMode ? 19 : 27;
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, mod.ProjectileType("BlueMoonBeam"), damage, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}
			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200);
		}

		public override void NPCLoot()
		{
			if(Main.rand.Next(25) == 0)
			{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlueEyeStaff"));
			}
		}
	}
}
