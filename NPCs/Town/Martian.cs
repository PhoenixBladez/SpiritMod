using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Martian : ModNPC
	{
		public static int _type;

		public override string Texture
		{
			get
			{
				return "SpiritMod/NPCs/Town/Martian";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SpiritMod/NPCs/Town/Martian_Alt_1" };
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Martian Scientist");
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
			npc.knockBackResist = 0.2f;
			animationType = NPCID.Guide;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int num = npc.life > 0 ? 1 : 5;
			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 206);
			}
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
						if (player.inventory[j].type == ItemID.GoldCoin && NPC.downedMartians)
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
					return "Marvin";
				case 1:
					return "Grunk";
				case 2:
					return "24-x3888";
				case 3:
					return "Dr. Quagnar";
				case 4:
					return "Dr. Boidzerg";
				case 5:
					return "Zorgnax";
				case 6:
					return "Gazorp";
				default:
					return "Xanthor";
			}
		}

		public override string GetChat()
		{
			int Dealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (Dealer >= 0 && Main.rand.Next(7) == 0)
				return "The illudium Q-36 explosive space modulator! " + Main.npc[Dealer].GivenName + " has stolen the space modulator!";

			int Tinkerer = NPC.FindFirstNPC(107);
			if (Tinkerer >= 0 && Main.rand.Next(7) == 0)
				return "I wonder why " + Main.npc[Tinkerer].GivenName + " was so angry when I asked to 'borrow' his arm for science";

			switch (Main.rand.Next(5))
			{
				case 0:
					return "Oh, I'm going to blow it up; it obstructs my view of Venus.";
				case 1:
					return "EX-TERM-IN-ATE";
				case 2:
					return "So many interesting creatures on this planet!";
				case 3:
					return "Open your mouth and say 'ah'... your other mouth.";
				default:
					return "How many earth-dollars do you want for your earth brain?";
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
			shop.item[nextSlot].SetDefaults(mod.ItemType("MartianTransmitter"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("MartianGrenade"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("MarsBullet"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("MartianArrow"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("BrainslugLauncher"));
			nextSlot++;

		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 65;
			knockback = 6f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 2;
			randExtraCooldown = 2;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = 440;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 11f;
			randomOffset = 2f;
		}
	}
}
