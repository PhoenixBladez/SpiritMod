using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Sets.HuskstalkSet;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class WanderingAdventurer : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/WanderingAdventurer";

		public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/WanderingAdventurer_Alt_1" };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wandering Adventurer");
			Main.npcFrameCount[NPC.type] = 26;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 500;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 16;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Guide);
			NPC.townNPC = false;
			NPC.friendly = true;
			NPC.immortal = true;
			NPC.aiStyle = 7;
			NPC.damage = 30;
			NPC.defense = 30;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.4f;
			AnimationType = NPCID.Guide;
		}

		public override void AI()
		{
			if (Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.FirstAdventure>().IsActive)
			{
				if (NPC.active)
				{
					SoundEngine.PlaySound(SoundID.DoubleJump, NPC.Center);
					Rectangle textPos = new Rectangle((int)NPC.position.X, (int)NPC.position.Y - 60, NPC.width, NPC.height);
					CombatText.NewText(textPos, new Color(255, 240, 0, 100), "Gotta go adventurin', see you later!");
					for (int i = 0; i < 2; i++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 11);
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 13);
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 12);
					}
					NPC.life = -1;
					NPC.active = false;
					NPC.netUpdate = true;
				}
			}
		}

		public override bool CanChat() => true;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Adventurer/Adventurer1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Adventurer/Adventurer2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Adventurer/Adventurer3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Adventurer/Adventurer4").Type);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(NPCType<WanderingAdventurer>()) || NPC.AnyNPCs(NPCType<Adventurer>()) || Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.FirstAdventure>().IsUnlocked)
                return 0f;
			return SpawnCondition.OverworldDay.Chance * 0.2f;
		}

		public override List<string> SetNPCNameList() => new List<string>() { "Indie", "Guy", "Nathan" };
		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<AdventurerMap>();

		public override string GetChat()
		{
			var dialogue = new List<string>
			{
				"Hey there! I see you're explorin' the world, too. Care to become an adventurer like me? I've got a lot of quests for you to get started with! Happy adventuring!",
				"I'm actually headin' on an expedition to the dangerous Briar later. Would you be able to do these quests for me in the meantime?",
				"What's that? No, I'm not lookin' for lodging right now. But I've got an offer for you, if you're interested. Want to explore the world and take on some excitin' quests?",
			};
			return Main.rand.Next(dialogue);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) 
		{
			if (!Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.FirstAdventure>().IsUnlocked)
			{
				Texture2D tex = Mod.Assets.Request<Texture2D>("UI/QuestUI/Textures/ExclamationMark").Value;
				float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(NPC.Center.X - 2, NPC.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItem(ref shop, ref nextSlot, ItemID.TrapsightPotion, 2000);
			AddItem(ref shop, ref nextSlot, ItemID.DartTrap, 5000);
			AddItem(ref shop, ref nextSlot, ItemID.WhoopieCushion, 15000, NPC.downedBoss2);
			AddItem(ref shop, ref nextSlot, ItemID.Book, 20, NPC.downedBoss3);
            AddItem(ref shop, ref nextSlot, ItemType<WWPainting>());
            AddItem(ref shop, ref nextSlot, ItemType<SkullStick>(), 1000, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
			AddItem(ref shop, ref nextSlot, ItemType<AncientBark>(), 200, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
			AddItem(ref shop, ref nextSlot, ItemType<Items.Sets.GunsMisc.PolymorphGun.PolymorphGun>(), check: NPC.downedMechBossAny);
			AddItem(ref shop, ref nextSlot, ItemType<PinGreen>());
			AddItem(ref shop, ref nextSlot, ItemType<PinYellow>());
          
            AddItem(ref shop, ref nextSlot, ItemType<Items.Accessory.VitalityStone>(), check: Main.bloodMoon);
            int glowStick = Main.moonPhase == 4 && !Main.dayTime ? ItemID.SpelunkerGlowstick : ItemID.StickyGlowstick;
            AddItem(ref shop, ref nextSlot, glowStick);

            switch (Main.moonPhase)
            {
                case 4 when !Main.dayTime:
                    AddItem(ref shop, ref nextSlot, ItemID.CursedTorch);
                    break;

                case 7 when !Main.dayTime:
                    AddItem(ref shop, ref nextSlot, ItemID.UltrabrightTorch);
                    break;
            }
        }

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 13;
			knockback = 3f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = 507;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 11f;
			randomOffset = 2f;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");

			if (!Mechanics.QuestSystem.QuestManager.QuestBookUnlocked)
				button2 = "Quest Book";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton) 
				shop = true;
			else
				Mechanics.QuestSystem.QuestManager.UnlockQuestBook();
		}
	}
}
