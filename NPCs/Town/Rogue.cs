using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Weapon.Thrown;
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
	public class Rogue : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/Rogue";

		public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Rogue_Alt_1" };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bandit");
			Main.npcFrameCount[NPC.type] = 26;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 1500;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 16;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Guide);
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.aiStyle = 7;
			NPC.damage = 30;
			NPC.defense = 30;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Bandit/Bandit1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Bandit/Bandit2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Bandit/Bandit3").Type);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.player.Any(x => x.active) && !NPC.AnyNPCs(NPCType<Rogue>()) && !NPC.AnyNPCs(NPCType<BoundRogue>());
		}

		public override List<string> SetNPCNameList() => new List<string>() { "Zane", "Carlos", "Tycho", "Damien", "Shane", "Daryl", "Shepard", "Sly" };

		public override string GetChat()
		{
			List<string> dialogue = new List<string>
			{
				"Here to peruse my wares? They're quite sharp.",
				"Trust me- the remains of those bosses you kill don't go to waste.",
				"The world is filled with opportunity! Now go kill some things.",
				"This mask is getting musky...",
				"Look at that handsome devil! Oh, it's just a mirror.",
				"Here to satisfy all your murdering needs!",
				"Nice day we're having here! Now, who do you want dead?",
			};

			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0) {
				dialogue.Add($"Tell {Main.npc[wizard].GivenName} to stop asking me where I got the charms. He doesn't need to know that. He would die of shock.");
			}

			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0) {
				dialogue.Add($"Why is {Main.npc[merchant].GivenName} so intent on selling shurikens? That's totally my thing.");
			}

			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer >= 0) {
				dialogue.Add($"You just missed the thrilling battle I had with {Main.npc[armsDealer].GivenName}! I won, of course");
			}

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
			AddItem(ref shop, ref nextSlot, ItemID.Shuriken);
			AddItem(ref shop, ref nextSlot, ItemType<RogueHood>());
			AddItem(ref shop, ref nextSlot, ItemType<RoguePlate>());
			AddItem(ref shop, ref nextSlot, ItemType<RoguePants>());
            AddItem(ref shop, ref nextSlot, ItemType<RogueCrest>());
			if (!WorldGen.crimson)
			{
            	AddItem(ref shop, ref nextSlot, ItemType<EoWDagger>(), check: NPC.downedBoss2);
			}
			else
			{
				AddItem(ref shop, ref nextSlot, ItemType<BoCShuriken>(), check: NPC.downedBoss2);
			}
			AddItem(ref shop, ref nextSlot, ItemType<SkeletronHand>(), check: NPC.downedBoss3);
			AddItem(ref shop, ref nextSlot, ItemType<PlagueVial>(), check: Main.hardMode);
			AddItem(ref shop, ref nextSlot, ItemType<ShurikenLauncher>());
			AddItem(ref shop, ref nextSlot, ItemType<SwiftRune>());
			AddItem(ref shop, ref nextSlot, ItemType<AssassinMagazine>());
			AddItem(ref shop, ref nextSlot, ItemType<Items.Weapon.Thrown.TargetCan>());
			AddItem(ref shop, ref nextSlot, ItemType<Items.Weapon.Thrown.TargetBottle>());
			AddItem(ref shop, ref nextSlot, ItemType<Items.Placeable.Furniture.TreasureChest>());
            AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.Masks.PsychoMask>());
			AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.OperativeSet.OperativeHead>(), check: Main.hardMode);
            AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.OperativeSet.OperativeBody>(), check: Main.hardMode);
            AddItem(ref shop, ref nextSlot, ItemType<Items.Armor.OperativeSet.OperativeLegs>(), check: Main.hardMode);
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
			projType = ProjectileType<Projectiles.Thrown.Kunai_Throwing>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 13f;
			randomOffset = 2f;
		}
	}
}
