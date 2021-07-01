using SpiritMod.Items.Accessory;
using SpiritMod.Items.Ammo.Arrow;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;

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
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 500;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 16;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.Guide);
			npc.townNPC = true;
			npc.friendly = true;
			npc.aiStyle = 7;
			npc.damage = 30;
			npc.defense = 30;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.4f;
			animationType = NPCID.Guide;
		}
		public override void AI()
		{
			if (Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.FirstAdventure>().IsActive)
			{
				Main.PlaySound(SoundID.DoubleJump, npc.Center, 0);
				Rectangle textPos = new Rectangle((int)npc.position.X, (int)npc.position.Y - 60, npc.width, npc.height);
				CombatText.NewText(textPos, new Color(255, 240, 0, 100), "Gotta go adventurin', see you later!");
				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(npc.position, npc.velocity, 11);
					Gore.NewGore(npc.position, npc.velocity, 13);
					Gore.NewGore(npc.position, npc.velocity, 12);
				}
				npc.life = 0;
				npc.active = false;

				if (Main.netMode != NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
			}
		}
		public override bool CanTownNPCSpawn(int numTownNPCs, int money) => false;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer4"));
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<WanderingAdventurer>()) || NPC.AnyNPCs(ModContent.NPCType<Adventurer>()) || Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.FirstAdventure>().IsUnlocked)
                return 0f;
            
			return SpawnCondition.OverworldDay.Chance * 0.2f;
		}
		public override string TownNPCName()
		{
			string[] names = { "Wandering Adventurer" };
			return Main.rand.Next(names);
		}
		public override void NPCLoot()
		{
			npc.DropItem(ItemType<AdventurerMap>());
		}
		public override string GetChat()
		{
			List<string> dialogue = new List<string>
			{
				"Hey there! I see you're explorin' the world, too. Care to become an adventurer like me? I've got a lot of quests for you to get started with! Happy adventuring!",
				"I'm actually headin' on an expedition to the dangerous Briar later. Would you be able to do these quests for me in the meantime?",
				"What's that? No, I'm not lookin' for lodging right now. But I've got an offer for you, if you're interested. Want to explore the world and take on some excitin' quests?",
			};

			return Main.rand.Next(dialogue);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) 
		{
			bool valid = ModContent.GetInstance<SpiritClientConfig>().ShowNPCQuestNotice && npc.CanTalk; 
			if (!Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.FirstAdventure>().IsUnlocked)
			{
				Texture2D tex = mod.GetTexture("UI/QuestUI/Textures/ExclamationMark");
				float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItem(ref shop, ref nextSlot, ItemID.TrapsightPotion, 2000);
			AddItem(ref shop, ref nextSlot, ItemID.DartTrap, 5000);
			AddItem(ref shop, ref nextSlot, ItemID.WhoopieCushion, 15000, NPC.downedBoss2);
			AddItem(ref shop, ref nextSlot, ItemID.Book, 20, NPC.downedBoss3);
            AddItem(ref shop, ref nextSlot, ItemType<WWPainting>());
            AddItem(ref shop, ref nextSlot, ItemType<Items.Placeable.Furniture.SkullStick>(), 1000, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
			AddItem(ref shop, ref nextSlot, ItemType<AncientBark>(), 200, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
			AddItem(ref shop, ref nextSlot, ItemType<Items.Sets.GunsMisc.PolymorphGun.PolymorphGun>(), check: NPC.downedMechBossAny);
			AddItem(ref shop, ref nextSlot, ItemType<PinGreen>());
			AddItem(ref shop, ref nextSlot, ItemType<PinYellow>());
          
            /*if (MyWorld.sepulchreComplete) {
				AddItem(ref shop, ref nextSlot, ItemType<SepulchreArrow>());
				AddItem(ref shop, ref nextSlot, ItemType<SepulchreBannerItem>());
				AddItem(ref shop, ref nextSlot, ItemType<SepulchreChest>());
			}
			if (MyWorld.jadeStaffComplete) {
				AddItem(ref shop, ref nextSlot, ItemType<PottedSakura>());
				AddItem(ref shop, ref nextSlot, ItemType<PottedWillow>());
			}
			if (MyWorld.vibeShroomComplete) {
				AddItem(ref shop, ref nextSlot, ItemType<Tiles.Furniture.Critters.VibeshroomJarItem>());
			}
			if (MyWorld.drBonesComplete) {
				AddItem(ref shop, ref nextSlot, ItemType<Items.Consumable.SeedBag>());
			}
			if (MyWorld.winterbornComplete) {
				AddItem(ref shop, ref nextSlot, ItemType<Items.Weapon.Thrown.CryoKnife>());
			}*/
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
            /*if (MyWorld.owlComplete)
            {
                AddItem(ref shop, ref nextSlot, ItemID.MusicBox);
            }*/
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

			// TODO: only show this if the player hasn't gotten their quest book yet?
			button2 = "Quest Book";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton) 
			{
				shop = true;
			}
			else
			{
				Mechanics.QuestSystem.QuestManager.QuestBookUnlocked = true;
				Mechanics.QuestSystem.QuestManager.UnlockQuest<Mechanics.QuestSystem.Quests.FirstAdventure>(true);
				Mechanics.QuestSystem.QuestManager.SayInChat("Press 'C' to open the Quest Journal!", Color.White);
				Mechanics.QuestSystem.QuestManager.SayInChat("Press 'V' to keep track of your progress wih the HUD!", Color.White);
				Mechanics.QuestSystem.QuestManager.SetBookState(true);
			}
		}
	}
}
