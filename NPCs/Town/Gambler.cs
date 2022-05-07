using SpiritMod.Items.Accessory.AceCardsSet;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Sets.GamblerChestLoot;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using static SpiritMod.NPCUtils;
using SpiritMod.Tiles.Furniture.SlotMachine;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Gambler : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/Gambler";

		public override bool Autoload(ref string name)
		{
			name = "Gambler";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			for (int k = 0; k < 255; k++) {
				Player player = Main.player[k];
				if (player.active) {
					for (int j = 0; j < player.inventory.Length; j++) {
						if (player.inventory[j].type == ItemID.GoldCoin && !NPC.AnyNPCs(NPCType<BoundGambler>())) {
							return true;
						}
					}
				}
			}
			return false;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(7)) {
				case 0:
					return "Yumeko";
				case 1:
					return "Vanessa";
				case 2:
					return "Gray";
				case 3:
					return "Alexandra";
				case 4:
					return "Sasha";
				case 5:
					return "Celine";
				default:
					return "Aleksa";
			}
		}

		public override string GetChat()
		{
			var dialogue = new List<string>
			{
				"Gambling is the sport of royals. Why don't you take a chance?",
				"I should warn you, my game isn't for the faint of heart.",
				"Gambling's bad for you. Unless you win.",
				"Win or lose, the thrill of the game is worth the money.",
				"You have the face of a winner. Step up!",
				"Get a sense of pride and accomplishment, for just a few coins!"
			};

			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0)
				dialogue.Add($"Unlike {Main.npc[merchant].GivenName}, I don't hoard my wealth.");

			int goblin = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
			if (goblin >= 0)
				dialogue.Add($"Tell {Main.npc[goblin].GivenName} to stop taking MY customers!");

			return Main.rand.Next(dialogue);
		}

		public override void SetChatButtons(ref string button, ref string button2) => button = Language.GetTextValue("LegacyInterface.28");

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
				shop = true;
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItem(ref shop, ref nextSlot, ItemType<CopperChest>());
			AddItem(ref shop, ref nextSlot, ItemType<SilverChest>());
			AddItem(ref shop, ref nextSlot, ItemType<GoldChest>());
			AddItem(ref shop, ref nextSlot, ItemType<PlatinumChest>());

			nextSlot += 6;

			switch (Main.moonPhase) {
				case 0 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfClubs>());
					break;
				case 1 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfClubs>());
					break;
				case 2 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfHearts>());
					break;
				case 3 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfHearts>());
					break;
				case 4 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfDiamonds>());
					break;
				case 5 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfDiamonds>());
					break;
				case 6 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfSpades>());
					break;
				case 7 when !Main.dayTime:
					AddItem(ref shop, ref nextSlot, ItemType<AceOfSpades>());
					break;
			}

			AddItem(ref shop, ref nextSlot, ItemType<Dartboard>());
			AddItem(ref shop, ref nextSlot, ItemType<SlotMachine>());
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = 160;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 8f;
			randomOffset = 2f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gambler/Gambler1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gambler/Gambler2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gambler/Gambler3"));
				for (int numGore = 0; numGore < 15; numGore++)
                {
					int g = Gore.NewGore(npc.position, npc.velocity * Main.rand.NextFloat(0.5f, 1.3f), mod.GetGoreSlot("Gores/GamblerCash"));
					Main.gore[g].scale = Main.rand.NextFloat(.5f, 1f);
				}
				for (int num621 = 0; num621 < 14; num621++)
				{
					int num = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.CoinDust>(), 0f, -2f, 0, default, 1.2f);
					Main.dust[num].scale = Main.rand.NextFloat(.8f, 1.2f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X = dust.position.X + ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					dust.position.Y = dust.position.Y + ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center)
					{
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 3f;
					}
				}
			}
		}
	}
}