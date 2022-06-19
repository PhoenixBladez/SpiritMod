using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.BloodcourtSet;
using System;
using System.Collections.Generic;
using SpiritMod.Items.Weapon.Summon.SacrificialDagger;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory.SanguineWardTree;
using SpiritMod.NPCs.Critters;

namespace SpiritMod.NPCs.Boss.Occultist
{
	[AutoloadBossHead]
	public partial class OccultistBoss : SpiritNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Occultist");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 1;
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 42;
			npc.height = 56;
			npc.lifeMax = 1000;
			npc.defense = 14;
			npc.damage = 30;
			npc.HitSound = SoundID.DD2_SkeletonHurt;
			npc.DeathSound = SoundID.NPCDeath59;
			npc.aiStyle = -1;
			npc.value = 300f;
			npc.knockBackResist = 0.45f;
			npc.netAlways = true;
			npc.lavaImmune = true;
			//npc.boss = true;

			banner = npc.type;
			music = MusicID.Eerie;
			bannerItem = ModContent.ItemType<Items.Banners.OccultistBanner>();
		}

		private ref float AIState => ref npc.ai[0];

		private const float AISTATE_SPAWN = 0;
		private const float AISTATE_DESPAWN = 1;
		private const float AISTATE_PHASE1 = 2;
		private const float AISTATE_PHASETRANSITION = 3;
		private const float AISTATE_PHASE2 = 4;
		private const float AISTATE_DEATH = 5;

		private ref float AttackType => ref npc.ai[1];

		private ref float AiTimer => ref npc.ai[2];

		private ref float SecondaryCounter => ref npc.ai[3];

		private void UpdateAIState(float State)
		{
			AIState = State;
			AiTimer = 0;
			frame.Y = 0;
			SecondaryCounter = 0;
			npc.netUpdate = true;

			if (!Main.dedServ)
				_rotMan.KillAllObjects();
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void AI()
		{
			Lighting.AddLight((int)(npc.Center.Y / 16f), (int)(npc.Center.Y / 16f), 0.46f, 0.12f, .64f);
			Player target = Main.player[npc.target];

			if (AIState == AISTATE_PHASE1 || AIState == AISTATE_PHASE2)
			{
				if(Main.dayTime)
					UpdateAIState(AISTATE_DESPAWN);

				if (target.dead || !target.active)
				{
					npc.TargetClosest(true); //look for another player
					if (target.dead || !target.active)
						UpdateAIState(AISTATE_DESPAWN); //despawn if still none alive
				}
			}

			//if (AIState == AISTATE_PHASE1 && npc.life < (npc.lifeMax / 2)) //Updates the boss to the second phase
			//	UpdateAIState(AISTATE_PHASETRANSITION);

			switch (AIState)
			{
				case AISTATE_SPAWN:
					npc.TargetClosest(true);
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = true;
					SpawnAnimation(target);
					break;

				case AISTATE_DESPAWN:
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = true;
					Despawn();
					break;

				case AISTATE_PHASE1:
					npc.noGravity = false;
					npc.noTileCollide = false;
					npc.dontTakeDamage = false;
					npc.velocity.X *= 0.9f;
					Phase1(target);
					break;

				case AISTATE_PHASETRANSITION:
					npc.TargetClosest(true);
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = true;
					PhaseTransition();
					break;

				case AISTATE_PHASE2:
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = false;
					Phase2(target);
					break;

				case AISTATE_DEATH:
					npc.dontTakeDamage = true;
					npc.noTileCollide = true;
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
					npc.TargetClosest(true);
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
				npc.TargetClosest(true);
				float RestingTime = 150;
				float halfRestTime = RestingTime / 2;
				frame.X = 0;
				UpdateYFrame(4, 0, 2);
				_pulseGlowmask = (float)Math.Max(Math.Pow(Math.Abs(halfRestTime - AiTimer) / halfRestTime, 3) - 0.2f, 0);
				npc.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / halfRestTime) * 0.8f;
				npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.1f);
				if(AiTimer > RestingTime)
				{
					ResetAttackP2();
					SecondaryCounter = 0;
				}
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override bool CheckActive() => false; //uses custom despawn so not needed

		public override bool CheckDead()
		{
			if (AIState != AISTATE_DEATH)
			{
				UpdateAIState(AISTATE_DEATH);
				npc.life = 1;
				npc.dontTakeDamage = true;

				MyWorld.downedOccultist = true;
				return false;
			}
			return true;
		}

		public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            MyWorld.downedOccultist = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
            return true;
        }

		public override void SafeFindFrame(int frameHeight) => npc.frame.Width = 72;
		
		public override void NPCLoot()
		{
			string[] lootTable = { "Handball", "SacrificialDagger", "BloodWard" };
			int loot = Main.rand.Next(lootTable.Length);
			{
				npc.DropItem(mod.ItemType(lootTable[loot]));
			}
			for(int i = 0; i < 4; i++)
			{
				int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<DreamstrideWisp>());
				if(Main.npc[n].type == ModContent.NPCType<DreamstrideWisp>() && Main.npc[n].active)
				{
					Main.npc[n].velocity = Main.rand.NextVector2Circular(3, 3);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => Main.bloodMoon && spawnInfo.player.Center.Y / 16f < Main.worldSurface ? 0.02f : 0f;

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