using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.BloodcourtSet;
using System;
using System.Collections.Generic;
using SpiritMod.Items.Weapon.Summon.SacrificialDagger;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory.SanguineWardTree;
using SpiritMod.NPCs.Critters;
using SpiritMod.Items.Placeable.Relics;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Boss.Occultist
{
	[AutoloadBossHead]
	public partial class OccultistBoss : SpiritNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Occultist");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			Main.npcFrameCount[NPC.type] = 9;
		}

		public override void SetDefaults()
		{
			NPC.width = 42;
			NPC.height = 56;
			NPC.lifeMax = 1000;
			NPC.defense = 14;
			NPC.damage = 30;
			NPC.HitSound = SoundID.DD2_SkeletonHurt;
			NPC.DeathSound = SoundID.NPCDeath59;
			NPC.aiStyle = -1;
			NPC.value = 300f;
			NPC.knockBackResist = 0.45f;
			NPC.netAlways = true;
			NPC.lavaImmune = true;

			Banner = NPC.type;
			Music = MusicID.Eerie;
			BannerItem = ModContent.ItemType<Items.Banners.OccultistBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,
				new FlavorTextBestiaryInfoElement("An undead dream demon obsessed with the practice and perfection of so-called ‘blood corruption.’ For this practice, he only reveals himself when the moon shimmers sanguine."),
			});
		}

		private ref float AIState => ref NPC.ai[0];

		private const float AISTATE_SPAWN = 0;
		private const float AISTATE_DESPAWN = 1;
		private const float AISTATE_PHASE1 = 2;
		private const float AISTATE_PHASETRANSITION = 3;
		private const float AISTATE_PHASE2 = 4;
		private const float AISTATE_DEATH = 5;

		private ref float AttackType => ref NPC.ai[1];

		private ref float AiTimer => ref NPC.ai[2];

		private ref float SecondaryCounter => ref NPC.ai[3];

		private void UpdateAIState(float State)
		{
			AIState = State;
			AiTimer = 0;
			frame.Y = 0;
			SecondaryCounter = 0;
			NPC.netUpdate = true;

			if (!Main.dedServ)
				_rotMan.KillAllObjects();
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void AI()
		{
			Lighting.AddLight((int)(NPC.Center.Y / 16f), (int)(NPC.Center.Y / 16f), 0.46f, 0.12f, .64f);
			Player target = Main.player[NPC.target];

			if (AIState == AISTATE_PHASE1 || AIState == AISTATE_PHASE2)
			{
				if(Main.dayTime)
					UpdateAIState(AISTATE_DESPAWN);

				if (target.dead || !target.active)
				{
					NPC.TargetClosest(true); //look for another player
					if (target.dead || !target.active)
						UpdateAIState(AISTATE_DESPAWN); //despawn if still none alive
				}
			}

			//if (AIState == AISTATE_PHASE1 && npc.life < (npc.lifeMax / 2)) //Updates the boss to the second phase
			//	UpdateAIState(AISTATE_PHASETRANSITION);

			switch (AIState)
			{
				case AISTATE_SPAWN:
					NPC.TargetClosest(true);
					NPC.noGravity = true;
					NPC.noTileCollide = true;
					NPC.dontTakeDamage = true;
					SpawnAnimation(target);
					break;

				case AISTATE_DESPAWN:
					NPC.noGravity = true;
					NPC.noTileCollide = true;
					NPC.dontTakeDamage = true;
					Despawn();
					break;

				case AISTATE_PHASE1:
					NPC.noGravity = false;
					NPC.noTileCollide = false;
					NPC.dontTakeDamage = false;
					NPC.velocity.X *= 0.9f;
					Phase1(target);
					break;

				case AISTATE_PHASETRANSITION:
					NPC.TargetClosest(true);
					NPC.noGravity = true;
					NPC.noTileCollide = true;
					NPC.dontTakeDamage = true;
					PhaseTransition();
					break;

				case AISTATE_PHASE2:
					NPC.noGravity = true;
					NPC.noTileCollide = true;
					NPC.dontTakeDamage = false;
					Phase2(target);
					break;

				case AISTATE_DEATH:
					NPC.dontTakeDamage = true;
					NPC.noTileCollide = true;
					DeathAnim();
					break;
			}
			++AiTimer;
		}

		private void Phase1(Player target)
		{
			switch (SecondaryCounter)
			{
				case 0:
					NPC.TargetClosest(true);
					AiTimer = 0;
					frame.X = 3;
					UpdateYFrame(10, 0, 6, delegate (int frameY)
					{
						if (frameY == 6)
						{
							frame.Y = 0;
							SecondaryCounter++;
						}
					});
					break;

				case 1:
					frame.X = 1;
					switch (AttackType)
					{
						case WAVEHANDS:
							WaveHandsP1(target);
							break;
						case SACDAGGERS:
							DaggersP1(target);
							break;
						case HOMINGSOULS:
							SoulsP1(target);
							break;
						case SUMMONBRUTE:
							BruteP1(target);
							break;
					}

					break;

				case 2:
					frame.X = 2;
					UpdateYFrame(12, 0, 7, delegate (int frameY)
					{
						if (frameY == 7)
						{
							SwapAttack();
							Teleport(target);
							SecondaryCounter = 0;
						}
					});
					break;
			}
		}

		private void Phase2(Player target)
		{
			int numAttacksPerCooldown = 3;
			if(SecondaryCounter < numAttacksPerCooldown)
			{
				switch (AttackType)
				{
					case WAVEHANDS:
						WaveHandsP2(target);
						break;
					case SACDAGGERS:
						DaggersP2(target);
						break;
					case HOMINGSOULS:
						SoulsP2(target);
						break;
					case SUMMONBRUTE:
						WaveHandsP2(target);
						//	BruteP2(target);
						break;
				}
			}
			else
			{
				NPC.TargetClosest(true);
				float RestingTime = 150;
				float halfRestTime = RestingTime / 2;
				frame.X = 0;
				UpdateYFrame(4, 0, 2);
				_pulseGlowmask = (float)Math.Max(Math.Pow(Math.Abs(halfRestTime - AiTimer) / halfRestTime, 3) - 0.2f, 0);
				NPC.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / halfRestTime) * 0.8f;
				NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, 0, 0.1f);
				if(AiTimer > RestingTime)
				{
					ResetAttackP2();
					SecondaryCounter = 0;
				}
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.75f);
		}

		public override bool CheckActive() => false; //uses custom despawn so not needed

		public override bool CheckDead()
		{
			if (AIState != AISTATE_DEATH)
			{
				UpdateAIState(AISTATE_DEATH);
				NPC.life = 1;
				NPC.dontTakeDamage = true;

				MyWorld.downedOccultist = true;

				if (QuestManager.GetQuest<SlayerQuestOccultist>().IsActive)
					QuestManager.ForceCompleteQuest<SlayerQuestOccultist>();
				return false;
			}
			return true;
		}

		public override bool PreKill()
        {
            SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
            MyWorld.downedOccultist = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
            return true;
        }

		public override void SafeFindFrame(int frameHeight) => NPC.frame.Width = 72;
		
		public override void OnKill()
		{
			for(int i = 0; i < 4; i++)
			{
				int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DreamstrideWisp>());
				if(Main.npc[n].type == ModContent.NPCType<DreamstrideWisp>() && Main.npc[n].active)
				{
					Main.npc[n].velocity = Main.rand.NextVector2Circular(3, 3);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddMasterModeCommonDrop<OccultistRelicItem>();
			npcLoot.AddOneFromOptions(1, ModContent.ItemType<Handball>(), ModContent.ItemType<SacrificialDagger>(), ModContent.ItemType<BloodWard>());
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => Main.bloodMoon && spawnInfo.Player.Center.Y / 16f < Main.worldSurface ? 0.02f : 0f;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 1.5f;
			name = "Occultist";
			downedCondition = () => MyWorld.downedOccultist;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<OccultistBoss>()
				},
				null,
				null,
				new List<int> {
					ModContent.ItemType<Handball>(),
					ModContent.ItemType<SacrificialDagger>(),
					ModContent.ItemType<BloodWard>(),
					ModContent.ItemType<DreamstrideEssence>()
				});
			spawnInfo =
				"The Occultist spawns rarely during a Blood Moon after any prehardmode boss has been defeated.";
			texture = "SpiritMod/Textures/BossChecklist/OccultistTexture";
			headTextureOverride = "SpiritMod/NPCs/BloodMoon/Occultist_Head_Boss";
		}
	}
}