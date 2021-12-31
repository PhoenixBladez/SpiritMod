using SpiritMod.Items.Accessory.AceCardsSet;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Sets.GamblerChestLoot;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Microsoft.Xna.Framework;

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
			};
			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0) {
				dialogue.Add($"Unlike {Main.npc[merchant].GivenName}, I don't hoard my wealth.");
			}

			return Main.rand.Next(dialogue);
		}

		/* 
		// Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
		// The WeightedRandom class needs "using Terraria.Utilities;" to use
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.Next(4) == 0)
			{
				chat.Add("Can you please tell " + Main.npc[partyGirl].GivenName + " to stop decorating my house with colors?");
			}
			chat.Add("Sometimes I feel like I'm different from everyone else here.");
			chat.Add("What's your favorite color? My favorite colors are white and black.");
			chat.Add("What? I don't have any arms or legs? Oh, don't be ridiculous!");
			chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);
			return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
		}
		*/

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