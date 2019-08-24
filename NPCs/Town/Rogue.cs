using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Rogue : ModNPC
	{
		public static int _type;

		public override string Texture
		{
			get
			{
				return "SpiritMod/NPCs/Town/Rogue";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SpiritMod/NPCs/Town/Rogue_Alt_1" };
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue");
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
			npc.lifeMax = 500;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
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
						if (player.inventory[j].type == mod.ItemType("IronShuriken") || player.inventory[j].type == mod.ItemType("LeadShuriken"))
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
					return "Mark";
				case 1:
					return "Carlos";
				case 2:
					return "Luke";
				case 3:
					return "Damien";
				case 4:
					return "Shane";
				case 5:
					return "Leroy";
				case 6:
					return "Nexus";
				default:
					return "Rufus";
			}
		}

		public override string GetChat()
		{
			int Wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (Wizard >= 0 && Main.rand.Next(8) == 0)
				return "Tell " + Main.npc[Wizard].GivenName + " to stop asking me where I got the charms. He doesn't need to know that. He would die of shock.";

			int Merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (Merchant >= 0 && Main.rand.Next(8) == 0)
				return "Why should I sell regular shurikens? " + Main.npc[Merchant].GivenName + " sells those...";

			int ArmsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (ArmsDealer >= 0 && Main.rand.Next(8) == 0)
				return "You just missed the thrilling battle I had with " + Main.npc[ArmsDealer].GivenName + "! I won, of course";

			switch (Main.rand.Next(8))
			{
				case 0:
					return "Here to peruse my wares? They're quite sharp.";
				case 1:
					return "Trust me- the remains of those bosses you kill don't go to waste.";
				case 2:
					return "The world is filled with opportunity! Now go kill some things.";
				case 3:
					return "This mask is getting musky...";
				case 4:
					return "Look at that handsome devil! Oh, it's just a mirror.";
				case 5:
					return "Here to satisfy all your throwing needs!";
				default:
					return "Nice day we're having here! Now, who do you want dead?";
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
			shop.item[nextSlot].SetDefaults(mod.ItemType("IronShuriken"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("LeadShuriken"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("RogueHood"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("RoguePlate"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("RoguePants"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("AssassinMagazine"));
			nextSlot++;

			if (NPC.downedBoss1 == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("Eyeball"));
				nextSlot++;

			}
			if (NPC.downedBoss2 == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("EoWDagger"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("BoCShuriken"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("GoldShuriken"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("PlatinumShuriken"));
				nextSlot++;
			}
			if (NPC.downedBoss3 == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("SkeletronHand"));
				nextSlot++;
			}
			if (NPC.downedMechBossAny)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("TwilightBlades"));
				nextSlot++;
			}
			if (NPC.downedMechBossAny == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("MechKnife"));
				nextSlot++;
			}
			if (MyWorld.downedRaider == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("DuskStone1"));
				nextSlot++;
			}
			if (NPC.downedPlantBoss == true)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ThornbloomKnife"));
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(mod.ItemType("ShurikenLauncher"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("SwiftRune"));
			nextSlot++;

			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("BladeOfNoah"));
				nextSlot++;
			}
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 10;
			knockback = 3f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = mod.ProjectileType("IronShurikenProjectile");
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 20f;
			randomOffset = 2f;
		}
	}
}
