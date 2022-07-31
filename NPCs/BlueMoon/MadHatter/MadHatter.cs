using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.BlueMoon.MadHatter
{
	public class MadHatter : ModNPC
	{
		int timer = 0;
		bool hat = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mad Hatter");
			Main.npcFrameCount[NPC.type] = 15;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 48;
			NPC.damage = 44;
			NPC.defense = 20;
			NPC.lifeMax = 670;
			NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath8;
			NPC.value = 1000f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 3;
			AIType = 104;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("They’ve been driven mad, completely bonkers! But all the best people are."),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0) {

				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 40;
				NPC.height = 48;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 200; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2)) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && spawnInfo.Player.ZoneOverworldHeight ? 1f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.IsABestiaryIconDummy)
			{
				NPC.frameCounter += 0.40f;
				NPC.frameCounter %= 13;
				int frame = (int)NPC.frameCounter + 2;
				NPC.frame.Y = frame * 80;
				return;
			}

			if (timer % 300 >= 80)
			{
				NPC.frameCounter += 0.40f;
				NPC.frameCounter %= 13;
				int frame = (int)NPC.frameCounter + 2;
				NPC.frame.Y = frame * 80;
			}
			else if (timer % 300 < 40)
				NPC.frame.Y = 80;
			else
				NPC.frame.Y = 0;
		}

		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			timer++;
			if (timer % 300 == 40 && hat == false) {
				SoundEngine.PlaySound(SoundID.Item43, NPC.Center);

				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 15, 0, -7, ModContent.ProjectileType<MadHat>(), 40, 1, Main.myPlayer, 0, 0);
				hat = true;
			}

			if (timer % 300 < 80) {
				if (player.position.X > NPC.position.X) {
					NPC.spriteDirection = 1;
					NPC.netUpdate = true;
				}
				else {
					NPC.spriteDirection = 0;
					NPC.netUpdate = true;
				}
				NPC.velocity.X = 0;
			}
			else {
				NPC.spriteDirection = NPC.direction;
				if (hat)
					hat = false;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(5))
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(239, 12));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.MadHat>(), 20));
		}
	}
}

