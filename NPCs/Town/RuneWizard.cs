using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Items.Glyphs;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class RuneWizard : ModNPC
	{
		public static int _type;

		public override string Texture
		{
			get
			{
				return "SpiritMod/NPCs/Town/RuneWizard";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SpiritMod/NPCs/Town/RuneWizard_Alt_1" };
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanter");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 1500;
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
			npc.DeathSound = SoundID.NPCDeath6;
			npc.knockBackResist = 0.4f;
			animationType = NPCID.Guide;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active)
				{
					for (int j = 0; j < player.inventory.Length; j++)
					{
						if (player.inventory[j].active && player.inventory[j].type == Glyph._type)
							return true;
					}
				}
			}
			return false;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(8))
			{
				case 0:
					return "Malachai";
				case 1:
					return "Nisarmah";
				case 2:
					return "Moneque";
				case 3:
					return "Tosalah";
				case 4:
					return "Kentremah";
				case 5:
					return "Salqueeh";
				case 6:
					return "Oarno";
				default:
					return "Cosimo";
			}
		}

		public override string GetChat()
		{
			int Wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (Wizard >= 0 && Main.rand.Next(8) == 0)
				return Main.npc[Wizard].GivenName + "and I often scry the runes together";

			if (Main.hardMode)
			{
				if (Main.rand.Next(8)== 0)
					return "I wonder what enchantements have been placed on the moon- It's all blue!";

				if (Main.rand.Next(8) == 0)
					return "The resurgence of Spirits offer a whole level of enchanting possibility!";
			}
			switch (Main.rand.Next(6))
			{
				case 0:
					return "Power up your weapons with my strange Glyphs!";
				case 1:
					return "Got any Blank Glyphs? I'll enchant those for you in a jiffy.";
				case 2:
					return "I only accept Glyphs for my wares; they're hard to come by nowadays.";
				case 3:
					return "I forgot the essence of Hellebore! Don't touch that!";
				case 4:
					return "If you're unsure of how to stumble upon Glyphs, my master once told me powerful bosses hold many!";
				case 5:
					return "Lost on how to find Glyphs? I've been told all foes can drop them rarely.";
				default:
					return "Anything can be enchanted if you possess the skill, wit, and essence!";
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Lang.inter[28].Value;
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
				shop = true;
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			Item item = shop.item[nextSlot++];
			item.SetDefaults(NullGlyph._type);
			item = shop.item[nextSlot++];
			CustomWare(item, FrostGlyph._type);
			item = shop.item[nextSlot++];
			CustomWare(item, EfficiencyGlyph._type);
			if (NPC.downedBoss1)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, RadiantGlyph._type);
				item = shop.item[nextSlot++];
				CustomWare(item, SanguineGlyph._type, 3);
			}
			if (MyWorld.downedReachBoss)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, StormGlyph._type, 2);
			}
			if (NPC.downedBoss2)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, UnholyGlyph._type, 2);
			}
			if (NPC.downedBoss3)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, VeilGlyph._type, 3);
			}
			if (NPC.downedQueenBee)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, BeeGlyph._type, 3);
			}
			if (Main.hardMode)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, BlazeGlyph._type, 3);
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, VoidGlyph._type, 4);
			}
			if (MyWorld.downedDusking)
			{
				item = shop.item[nextSlot++];
				CustomWare(item, PhaseGlyph._type, 4);
			}
		}

		private static void CustomWare(Item item, int type, int price = 1)
		{
			item.SetDefaults(type);
			item.shopCustomPrice = price;
			item.shopSpecialCurrency = SpiritMod.GlyphCurrencyID;
		}


		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 40;
			knockback = 3f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = Projectiles.Blaze._type;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 20f;
			randomOffset = 2f;
		}
	}
}
