using SpiritMod.Items.Accessory;
using SpiritMod.Items.Ammo.Arrow;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Adventurer : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/Adventurer";

		public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adventurer");
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

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.player.Any(x => x.active) && !NPC.AnyNPCs(NPCType<BoundAdventurer>()) && !NPC.AnyNPCs(NPCType<Adventurer>());
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer4"));
			}
		}

		public override string TownNPCName()
		{
			string[] names = { "Morgan", "Adam", "Aziz", "Temir", "Evan", "Senzen", "Johanovic", "Adrian", "Christopher" };
			return Main.rand.Next(names);
		}
		public override void NPCLoot()
		{
			npc.DropItem(ModContent.ItemType<AdventurerMap>());
		}
		public override string GetChat()
		{
			List<string> dialogue = new List<string>
			{
				"I've been all around this world, and I've got so many things for you to see.",
				"Lovely house you've got here. It's much better lodging than when those savages from The Briar hung me over a spit.",
				"Every dawn brings with it a new opportunity for a journey! You interested?",
				"We're pretty similar, you and I. I sense our shared thirst for adventure.",
				"Buy my stuff and go out there! See what the world has to offer, like I have.",
				"From the depths of temples and the heights of space, peruse my wares.",
			};

			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0) {
				dialogue.Add($"I swear I've got more goods for sale than {Main.npc[merchant].GivenName}.");
			}

			int travellingMerchant = NPC.FindFirstNPC(NPCID.TravellingMerchant);
			if (travellingMerchant >= 0) {
				dialogue.Add($"Ah! It's {Main.npc[travellingMerchant].GivenName}! We've often met on our journeys. I still haven't found all those exotic jungles he speaks of.");
			}

			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer >= 0) {
				dialogue.Add($"Got some great prices today! {Main.npc[armsDealer].GivenName}'s wares can't compete! They literally can't. I don't sell guns anymore.");
			}

			dialogue.AddWithCondition("Like the moon, my merchandise is inconstant.", !Main.dayTime);
			dialogue.AddWithCondition("Everyone seems to be so aggressive tonight. With the zombies knocking at our door, I think you should buy stuff and head underground as quick as you can. Can you take me with you?", Main.bloodMoon);
			dialogue.AddWithCondition("The goblins are more organized than you'd think- I saw their mages build a huge tower over yonder. You should check it out sometime!", MyWorld.gennedTower && !NPC.AnyNPCs(ModContent.NPCType<Rogue>()) && NPC.AnyNPCs(ModContent.NPCType<BoundRogue>()));
			dialogue.AddWithCondition("My old business partner turned to the bandit life a few years ago. I wonder if he's doing okay. I think his associates have set up a bandit camp somewhere near the seas.", !MyWorld.gennedTower && !NPC.AnyNPCs(ModContent.NPCType<Rogue>()) && NPC.AnyNPCs(ModContent.NPCType<BoundRogue>()));
			dialogue.AddWithCondition("A shimmering blue light's on the horizon. Wonder what that's about, huh?", NPC.downedMechBossAny);

			return Main.rand.Next(dialogue);
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{

			AddItem(ref shop, ref nextSlot, ItemID.TrapsightPotion, 2000);
			AddItem(ref shop, ref nextSlot, ItemID.DartTrap, 5000);
			AddItem(ref shop, ref nextSlot, ItemType<WornSword>());
			AddItem(ref shop, ref nextSlot, ItemID.WhoopieCushion, 15000, NPC.downedBoss2);
			AddItem(ref shop, ref nextSlot, ItemID.Book, 20, NPC.downedBoss3);
            AddItem(ref shop, ref nextSlot, ItemType<WWPainting>());
            AddItem(ref shop, ref nextSlot, ItemType<Items.Placeable.Furniture.SkullStick>(), 1000, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
			AddItem(ref shop, ref nextSlot, ItemType<AncientBark>(), 200, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
			AddItem(ref shop, ref nextSlot, ItemType<PolymorphGun>(), check: NPC.downedMechBossAny);
            AddItem(ref shop, ref nextSlot, ItemType<PinGreen>());
			AddItem(ref shop, ref nextSlot, ItemType<PinYellow>());
          
            if (MyWorld.sepulchreComplete) {
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
			}
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
            if (MyWorld.owlComplete)
            {
                AddItem(ref shop, ref nextSlot, ItemID.MusicBox);
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
				//Main.LocalPlayer.talkNPC = -1;
				// inventory should already be closed but im paranoid
				//if (!Main.playerInventory) Main.LocalPlayer.ToggleInv();
				Mechanics.QuestSystem.QuestManager.SetBookState(true);

				// TODO: clicking this gives quest book if not unlocked already
			}
		}
	}
}
