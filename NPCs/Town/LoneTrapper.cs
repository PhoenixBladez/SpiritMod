using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class LoneTrapper : ModNPC
	{
		public static int _type;

		public override string Texture
		{
			get
			{
				return "SpiritMod/NPCs/Town/LoneTrapper";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SpiritMod/NPCs/Town/LoneTrapper_Alt_1" };
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lone Trapper");
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
						if (player.inventory[j].type == ItemID.GoldCoin && NPC.downedMechBossAny)
							return true;
					}
				}
			}
			return false;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(4))
			{
				case 0:
					return "Jace";
				case 1:
					return "Moral";
				case 2:
					return "Benei";
				default:
					return "Castiel";


			}
		}

		public override string GetChat()
		{
			switch (Main.rand.Next(8))
			{
				case 0:
					return "I've captured the souls of the deadliest, but it seems that they're returning...";
				case 1:
					return "Hell was nice, but I needed a change.";
				case 2:
					return "Buy something and leave. Please.";
				case 3:
					return "Need to capture souls? You came to the right place.";
				case 4:
					return "Sorrow is all I see in this wretched world... No longer, if I have something to say about it.";
				case 5:
					return "...";
				case 6:
					return "I ask for you to leave my presence.";
				case 7:
					return "Leave me alone...";
				default:
					return "Thank you for rescuing me from the clutches of the demons.";
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
			shop.item[nextSlot].SetDefaults(mod.ItemType("EtherealSword"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("EtherealBow"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("EtherealStaff"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("EtherealSpear"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("SpiritStaff"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("SoulSeeds"));
			nextSlot++;
			if (NPC.downedPlantBoss)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("SpiritChestKey"));
				nextSlot++;
			}
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 30;
			knockback = 6f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = mod.ProjectileType("EtherealSpearProjectile");
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 20f;
			randomOffset = 2f;
		}
	}
}

