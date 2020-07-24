using SpiritMod.Items.Placeable;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Projectiles.Held;
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
	public class LoneTrapper : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/LoneTrapper";

		public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/LoneTrapper_Alt_1" };

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
			return NPC.downedMechBossAny && Main.player.Any(x => x.active && x.inventory.Any(y => y.type == ItemID.GoldCoin));
		}

		public override string TownNPCName()
		{
			string[] names = { "Jace", "Moral", "Benei", "Castiel" };
			return Main.rand.Next(names);
		}

		public override string GetChat()
		{
			List<string> dialogue = new List<string>
			{
				"I've captured the souls of the deadliest, but it seems that they're returning...",
				"Hell was nice, but I needed a change.",
				"Buy something and leave. Please.",
				"Need to capture souls? You came to the right place.",
				"Sorrow is all I see in this wretched world... No longer, if I have something to say about it.",
				"...",
				"I ask for you to leave my presence.",
				"Leave me alone...",
				"Thank you for rescuing me from the clutches of the demons.",
			};

			return Main.rand.Next(dialogue);
		}


		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton) {
				shop = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItem(ref shop, ref nextSlot, ItemType<EtherealSword>());
			AddItem(ref shop, ref nextSlot, ItemType<EtherealBow>());
			AddItem(ref shop, ref nextSlot, ItemType<EtherealStaff>());
			AddItem(ref shop, ref nextSlot, ItemType<EtherealSpear>());
			AddItem(ref shop, ref nextSlot, ItemType<SoulSeeds>());
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
			projType = ProjectileType<EtherealSpearProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 20f;
			randomOffset = 2f;
		}
	}
}

