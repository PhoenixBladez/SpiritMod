using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Sets.SeraphSet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.BlueMoon.LunarSlime
{
	public class LunarSlime : ModNPC
	{
		bool jump = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stargazer Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 26;
			NPC.damage = 35;
			NPC.defense = 12;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath22;
			NPC.value = 600f;
			NPC.knockBackResist = .4f;
			NPC.aiStyle = 1;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[ModContent.BuffType<StarFlame>()] = true;

			AIType = NPCID.BlueSlime;
			AnimationType = NPCID.BlueSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.LunarSlimeBanner>();
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Gelatinous nectar from the stars. Stargazer Slimes may be aggressive, but the oil they secrete makes for a delicious spread."),
			});
		}

		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 200);
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => MyWorld.BlueMoon && spawnInfo.Player.ZoneOverworldHeight ? 3.4f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 8; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, hitDirection, -1f, 0, default, 1.4f);
			}
			if (NPC.life <= 0) {
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);
				for (int num621 = 0; num621 < 100; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 1.1f);
					Main.dust[num622].velocity *= 3f;
				}
				float ScaleMult = 2.33f;
				DustHelper.DrawStar(new Vector2(NPC.Center.X, NPC.Center.Y), 206, pointAmount: 5, mainSize: 5.25f * ScaleMult, dustDensity: 4, pointDepthMult: 0.3f, noGravity: true);
			}
		}
		public override bool PreAI()
		{
			if (NPC.collideY && jump && NPC.velocity.Y > 0) {
				NPC.ai[3]++;
				if (NPC.ai[3] >= 2) {
					NPC.ai[3] = 0;
					SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
					float ScaleMult = 2.33f;
					DustHelper.DrawStar(new Vector2(NPC.Center.X, NPC.Center.Y), 206, pointAmount: 5, mainSize: 4.25f * ScaleMult, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);
					for (int i = 0; i < Main.rand.Next(1, 3); i++) {
						Vector2 vector2_1 = new Vector2((float)((double)NPC.position.X + (double)NPC.width * 0.5 + (double)(Main.rand.Next(201) * -NPC.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)NPC.position.X)), (float)((double)NPC.position.Y + (double)NPC.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
						vector2_1.X = (float)(((double)vector2_1.X + (double)NPC.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
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
						int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-60, 60), NPC.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, ModContent.ProjectileType<LunarStar>(), 20, 3, Main.myPlayer, 0.0f, 1);
					}
				}
				jump = false;
			}
			if (!NPC.collideY)
				jump = true;


			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(5))
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200);

		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonJellyDonut>(), 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonStone>(), 5));
		}
	}
}
