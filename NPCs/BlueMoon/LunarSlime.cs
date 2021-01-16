using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.BlueMoon
{
	public class LunarSlime : ModNPC
	{
		bool jump = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stargazer Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 26;
			npc.damage = 35;
			npc.defense = 12;
			npc.lifeMax = 300;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.value = 600f;
			npc.knockBackResist = .4f;
			npc.aiStyle = 1;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, 200);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && spawnInfo.player.ZoneOverworldHeight ? 3.4f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 8; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 206, hitDirection, -1f, 0, default(Color), 1.4f);
			}
			if (npc.life <= 0) {
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 100; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 1.1f);
					Main.dust[num622].velocity *= 3f;
				}
				float ScaleMult = 2.33f;
				DustHelper.DrawStar(new Vector2(npc.Center.X, npc.Center.Y), 206, pointAmount: 5, mainSize: 5.25f * ScaleMult, dustDensity: 4, pointDepthMult: 0.3f, noGravity: true);

			}
		}
		public override bool PreAI()
		{
			if (npc.collideY && jump && npc.velocity.Y > 0) {
				npc.ai[3]++;
				if (npc.ai[3] >= 2) {
					npc.ai[3] = 0;
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 9);
					float ScaleMult = 2.33f;
					DustHelper.DrawStar(new Vector2(npc.Center.X, npc.Center.Y), 206, pointAmount: 5, mainSize: 4.25f * ScaleMult, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);
					for (int i = 0; i < Main.rand.Next(1, 3); i++) {
						Vector2 vector2_1 = new Vector2((float)((double)npc.position.X + (double)npc.width * 0.5 + (double)(Main.rand.Next(201) * -npc.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)npc.position.X)), (float)((double)npc.position.Y + (double)npc.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
						vector2_1.X = (float)(((double)vector2_1.X + (double)npc.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
						vector2_1.Y -= (float)(100);
						float num12 = Main.rand.Next(-30, 30);
						float num13 = 120;
						if ((double)num13 < 0.0) num13 *= -1f;
						if ((double)num13 < 20.0) num13 = 20f;
						float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
						float num15 = 10 / num14;
						float num16 = num12 * num15;
						float num17 = num13 * num15;
						float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile X position speed and randomnes
						float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
						int proj = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-60, 60), npc.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, ModContent.ProjectileType<LunarStar>(), 20, 3, Main.myPlayer, 0.0f, 1);
					}
				}
				jump = false;
			}
			if (!npc.collideY)
				jump = true;


			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200);

		}

		public override void NPCLoot()
		{
			//if (Main.rand.Next(4) == 1)
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonJelly>());

			if (Main.rand.Next(5) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonStone>());
		}

	}
}
