using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Adventurer : ModNPC
	{
		public static int _type;

		public override string Texture
		{
			get
			{
				return "SpiritMod/NPCs/Town/Adventurer";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };
			}
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adventurer");
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
			npc.DeathSound = SoundID.NPCDeath1;
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
						if (player.inventory[j].type == ItemID.GoldCoin && NPC.downedBoss1)
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
					return "Drew";
				case 1:
					return "Adam";
				case 2:
					return "Aziz";
				case 3:
					return "Blade";
				case 4:
					return "Evan";
				case 5:
					return "Jenosis";
				case 6:
					return "Khaelis";
				default:
					return "Vladimier";
			}
		}

		public override string GetChat()
		{
			int TravellingMerchant = NPC.FindFirstNPC(NPCID.TravellingMerchant);
			if (TravellingMerchant >= 0 && Main.rand.Next(8) == 0)
				return "Ah! It's " + Main.npc[TravellingMerchant].GivenName + "! We've often met on our journeys.";

			int ArmsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (ArmsDealer >= 0 && Main.rand.Next(8) == 0)
				return "Got some great prices today!" + Main.npc[ArmsDealer].GivenName + "'s wares can't compete!";

			int Merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (Merchant >= 0 && Main.rand.Next(8) == 0)
				return "I swear I've got more goods today than " + Main.npc[Merchant].GivenName + ".";

			if (NPC.downedMechBossAny && Main.rand.Next(8) == 0)
				return "A shimmering blue light's on the horizon. Wonder what that's about, huh?";

			if (!Main.dayTime && Main.rand.Next(6) == 0)
				return "Like the moon, my merchandise is inconstant.";

			if (Main.dayTime && Main.rand.Next(6) == 0)
				return "A bright new day, and a new shipment of goods await.";

			if (Main.bloodMoon && Main.rand.Next(4) == 0)
				return "Everyone seems to be aggressive tonight. Well, all I've got are new products, so I'm happy.";

			switch (Main.rand.Next(8))
			{
				case 0:
					return "I've been all around this world, and I've got so many things for you to see.";
				case 1:
					return "The wonders I've seen! I have some to share.";
				case 2:
					return "Lovely house! I need to tell you about the time I was trapped in that Reach nearby...";
				case 3:
					return "The wind... A new dawn is approaching soon, I can feel it!";
				case 4:
					return "We're pretty similar, you and I. I sense our similar thirst for adventure.";
				case 5:
					return "Buy my stuff and go out there! See what the world has to offer.";
				default:
					return "From the depths of temples and the heights of space, peruse my wares.";
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
			if (NPC.downedBoss1 == true)
			{
				shop.item[nextSlot].SetDefaults(ItemID.Compass);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DepthMeter);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WarmthPotion);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Hook);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Rope);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PurpleEmperorButterfly);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("GatorPistol"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("Chakram"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("GoldSword"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("PlatinumSword"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("ManaFlame"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("BeserkerShard"));
				nextSlot++;

			}
			if (!Main.dayTime)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("PoisonArrow"));
				nextSlot++;
			}
			if (Main.dayTime)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("SlimeArrow"));
				nextSlot++;
			}
			if (NPC.downedBoss2 == true && !Main.dayTime)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("KnocbackGun"));
				nextSlot++;
			}
			if (Main.bloodMoon)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("BloodWard"));
				nextSlot++;
			}
			if (NPC.downedBoss3 == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("Skyblade"));
				nextSlot++;
			}
			if (NPC.downedBoss3 == true && !Main.dayTime)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("DemonArrow"));
				nextSlot++;

			}
			if (MyWorld.downedRaider == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("DawnStone"));
				nextSlot++;
			}
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ScorpionGun"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("ViashinoStaff"));
				nextSlot++;
			}
			if (NPC.downedMechBossAny == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("PolymorphGun"));
				nextSlot++;
			}
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
			projType = ProjectileID.BoneJavelin;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 20f;
			randomOffset = 2f;
		}
	}
}
