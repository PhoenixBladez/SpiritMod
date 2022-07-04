using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Glyphs;
using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class RuneWizard : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/RuneWizard";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanter");
			Main.npcFrameCount[NPC.type] = 26;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 1500;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 25;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Guide);
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.aiStyle = 7;
			NPC.damage = 14;
			DrawOffsetY = -2;
			NPC.defense = 30;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.4f;
			AnimationType = NPCID.Guide;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.player.Any(x => x.active && x.inventory.Any(y => y.type == ItemType<Glyph>()));
		}

		public override List<string> SetNPCNameList() => new List<string>() { "Malachai", "Nisarmah", "Moneque", "Tosalah", "Kentremah", "Salqueeh", "Oarno", "Cosimo" };

		public override string GetChat()
		{
			List<string> dialogue = new List<string>
			{
				"Power up your weapons with my strange Glyphs!",
				"Got any Blank Glyphs? I'll enchant those for you in a jiffy.",
				"I only accept Glyphs for my wares; they're hard to come by nowadays.",
				"I forgot the essence of Hellebore! Don't touch that!",
				"If you're unsure of how to stumble upon Glyphs, my master once told me powerful bosses hold many!",
				"Lost on how to find Glyphs? I've been told all foes can drop them rarely.",
				"Anything can be enchanted if you possess the skill, wit, and essence!",
			};

			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0)
			{
				dialogue.Add($"{Main.npc[wizard].GivenName} and I often scry the runes together");
			}

			dialogue.AddWithCondition("I wonder what enchantements have been placed on the moon- It's all blue!", Main.hardMode);
			dialogue.AddWithCondition("The resurgence of Spirits offer a whole level of enchanting possibility!", Main.hardMode);

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
			AddItem(ref shop, ref nextSlot, ItemType<NullGlyph>());

			Item item = shop.item[nextSlot++];
			CustomWare(item, ItemType<FrostGlyph>());

			item = shop.item[nextSlot++];
			CustomWare(item, ItemType<EfficiencyGlyph>());

			if (NPC.downedBoss1)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<RadiantGlyph>());
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<SanguineGlyph>(), 3);
			}

			if (MyWorld.downedReachBoss)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<StormGlyph>(), 2);
			}

			if (NPC.downedBoss2)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<UnholyGlyph>(), 2);
			}

			if (NPC.downedBoss3)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<VeilGlyph>(), 3);
			}

			if (NPC.downedQueenBee)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<BeeGlyph>(), 3);
			}

			if (Main.hardMode)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<BlazeGlyph>(), 3);
			}

			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<VoidGlyph>(), 4);
			}

			if (MyWorld.downedDusking)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, ItemType<PhaseGlyph>(), 4);
			}
			AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.WitchSet.WitchHead>(), 12000, !Main.dayTime);
			AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.WitchSet.WitchBody>(), 15000, !Main.dayTime);
			AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.WitchSet.WitchLegs>(), 10000, !Main.dayTime);
		}

		private static void CustomWare(Item item, int type, int price = 1)
		{
			item.SetDefaults(type);
			item.shopCustomPrice = price;
			item.shopSpecialCurrency = SpiritMod.GlyphCurrencyID;
		}


		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 18;
			knockback = 3f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.RubyBolt;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 14f;
			randomOffset = 2f;
		}

		public override ITownNPCProfile TownNPCProfile() => new RuneWizardProfile();
	}

	public class RuneWizardProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public ReLogic.Content.Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.altTexture == 1 && !(npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn))
				return Request<Texture2D>("SpiritMod/NPCs/Town/RuneWizard_Alt_1");

			return Request<Texture2D>("SpiritMod/NPCs/Town/RuneWizard");
		}

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("SpiritMod/NPCs/Town/RuneWizard_Head");
	}
}