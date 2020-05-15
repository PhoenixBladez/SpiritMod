using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor;
using SpiritMod.Items.DonatorItems;
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

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bandit/Bandit1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bandit/Bandit2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bandit/Bandit3"));
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.player.Any(x => x.active) && !NPC.AnyNPCs(NPCType<Rogue>()) && !NPC.AnyNPCs(NPCType<BoundRogue>());
		}

		public override string TownNPCName()
		{
			string[] names = { "Zane", "Carlos", "Tycho", "Damien", "Shane", "Daryl", "Shepard", "Sly" };
			return Main.rand.Next(names);
		}

		public override string GetChat()
		{
			List<string> dialogue = new List<string>
			{
				"Here to peruse my wares? They're quite sharp.",
				"Trust me- the remains of those bosses you kill don't go to waste.",
				"The world is filled with opportunity! Now go kill some things.",
				"This mask is getting musky...",
				"Look at that handsome devil! Oh, it's just a mirror.",
				"Here to satisfy all your throwing needs!",
				"Nice day we're having here! Now, who do you want dead?",
			};

			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0)
			{
				dialogue.Add($"Tell {Main.npc[wizard].GivenName} to stop asking me where I got the charms. He doesn't need to know that. He would die of shock.");
			}

			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0)
			{
				dialogue.Add($"Why should I sell regular shurikens? {Main.npc[merchant].GivenName} sells those...");
			}

			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer >= 0)
			{
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
			if (firstButton)
			{
				shop = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItem(ref shop, ref nextSlot, ItemID.Shuriken);
			AddItem(ref shop, ref nextSlot, ItemType<RogueHood>());
			AddItem(ref shop, ref nextSlot, ItemType<RoguePlate>());
			AddItem(ref shop, ref nextSlot, ItemType<RoguePants>());
            AddItem(ref shop, ref nextSlot, ItemType<Eyeball>(), check: NPC.downedBoss1);
			AddItem(ref shop, ref nextSlot, ItemType<EoWDagger>(), check: NPC.downedBoss2);
			AddItem(ref shop, ref nextSlot, ItemType<BoCShuriken>(), check: NPC.downedBoss2);
			AddItem(ref shop, ref nextSlot, ItemType<SkeletronHand>(), check: NPC.downedBoss3);
			AddItem(ref shop, ref nextSlot, ItemType<TwilightBlades>(), check: NPC.downedMechBossAny);
			AddItem(ref shop, ref nextSlot, ItemType<MechKnife>(), check: NPC.downedMechBossAny);
			AddItem(ref shop, ref nextSlot, ItemType<DuskStone1>(), check: MyWorld.downedRaider);
			AddItem(ref shop, ref nextSlot, ItemType<ThornbloomKnife>(), check: NPC.downedPlantBoss);
            AddItem(ref shop, ref nextSlot, ItemType<PlagueVial>(), check: Main.hardMode);
            AddItem(ref shop, ref nextSlot, ItemType<BladeOfNoah>(), check: Main.hardMode);
            AddItem(ref shop, ref nextSlot, ItemType<ShurikenLauncher>());
			AddItem(ref shop, ref nextSlot, ItemType<SwiftRune>());
            AddItem(ref shop, ref nextSlot, ItemType<AssassinMagazine>());
            AddItem(ref shop, ref nextSlot, ItemType<Dartboard>());
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
