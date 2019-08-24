using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class MoonSpirit : ModNPC
	{
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 100f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Spirit");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 34;
			npc.damage = 55;
			npc.defense = 11;
			npc.lifeMax = 390;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 1000f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = 0.25f;
		}
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			Vector2 direction = Main.player[npc.target].Center - npc.Center;
			npc.rotation = direction.ToRotation();
			if (Main.rand.Next(10) == 1)
			{
				int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 206, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
			}
			if (Main.rand.Next(1200) == 1)
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Sparkle"), 0, 2, 1, 0, npc.whoAmI, npc.target);
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

			Player player = Main.player[npc.target];

			if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
				moveSpeed--;

			if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -20) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 165f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 20)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.1f;
			if (Main.rand.Next(210) == 1)
				HomeY = -25f;
			return false;
		}


		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
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
				npc.width = 30;
				npc.height = 30;
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
			return MyWorld.BlueMoon ? 6f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.40f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MoonStone"));
		}

	}
}
