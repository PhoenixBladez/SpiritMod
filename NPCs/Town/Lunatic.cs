using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Lunatic : ModNPC
	{
		public static int _type;

		public override string Texture
		{
			get
			{
				return "SpiritMod/NPCs/Town/Lunatic";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SpiritMod/NPCs/Town/Lunatic_Alt_1" };
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunatic");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 2500;
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
			npc.knockBackResist = 0.2f;
			animationType = NPCID.Guide;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(8))
			{
				case 0:
					return "Kah-Il";
				case 1:
					return "Shahal";
				case 2:
					return "Devihel";
				case 3:
					return "Hazra";
				case 4:
					return "Herah-theek";
				case 5:
					return "Azaloth";
				case 6:
					return "Thirah";
				default:
					return "Khualrya";
			}
		}

		public override string GetChat()
		{
			int Wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (Wizard >= 0 && Main.rand.Next(8) == 0)
				return "It's " + Main.npc[Wizard].GivenName + "! Long ago, I taught him some of my tricks.";

			int Clothier = NPC.FindFirstNPC(NPCID.Clothier);
			if (Clothier >= 0 && Main.rand.Next(8) == 0)
				return Main.npc[Clothier].GivenName + "served an insignificant master compared to my former lord.";

			if (!Main.dayTime && Main.rand.Next(6) == 0)
				return "I must hide. I am a traitor to the moon.";

			if (Main.dayTime && Main.rand.Next(6) == 0)
				return "The day is bright, I feel safe. Let me share some wisdom with you.";

			switch (Main.rand.Next(8))
			{
				case 0:
					return "All my disciples are dead, but I will impart the magics upon you.";
				case 1:
					return "You have sided with the Spirits, ancient enemies of my old master...";
				case 2:
					return "I sense a battle approaching, though it may not be the one you expect.";
				case 3:
					return "Focus on your soul, and let it guide you forward.";
				default:
					return "Once again, thank you for pardoning me. You are a kind soul.";

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
			shop.item[nextSlot].SetDefaults(mod.ItemType("AncientGuidance"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("CultistScarf"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("HeartofMoon"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("CosmicHourglass"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("Tesseract"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("FateToken"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("MagicBullet"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("CultDagger"));
			nextSlot++;

			if (NPC.downedMoonlord)
			{
				shop.item[nextSlot].SetDefaults(ItemID.CelestialSigil);
				shop.item[nextSlot].value = 270000;
				nextSlot++;
			}

		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 75;
			knockback = 5f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 4;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.NebulaArcanum;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 30f;
			randomOffset = 2f;
		}
	}
}
