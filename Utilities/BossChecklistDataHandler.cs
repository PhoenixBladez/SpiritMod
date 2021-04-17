using System;
using System.Collections.Generic;
using System.Linq;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Armor.JellynautHelmet;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Equipment.ScarabExpertDrop;
using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Bow.AdornedBow;
using SpiritMod.Items.Weapon.Club;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Magic.RadiantCane;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Summon.ElectricGun;
using SpiritMod.Items.Weapon.Summon.LocustCrook;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Swung.Khopesh;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.NPCs.Boss;
using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.NPCs.Boss.Dusking;
using SpiritMod.NPCs.Boss.Infernon;
using SpiritMod.NPCs.Boss.MoonWizard;
using SpiritMod.NPCs.Boss.ReachBoss;
using SpiritMod.NPCs.Boss.Scarabeus;
using SpiritMod.NPCs.Boss.SteamRaider;
using SpiritMod.NPCs.MoonjellyEvent;
using SpiritMod.NPCs.Tides;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	/// <summary>
	/// A class containing useful methods for registering <c>BossChecklist</c> information,
	/// along with internal methods for initializing <see cref="SpiritMod"/>-specific data.
	/// </summary>
	public static class BossChecklistDataHandler
	{
		public enum EntryType
		{
			Boss,
			Miniboss,
			Event
		}

		public static Mod BossChecklistMod => ModLoader.GetMod("BossChecklist");

		public static bool BossChecklistIsLoaded => BossChecklistMod != null;

		public class BCIDData
		{
			public readonly List<int> npcIDs;
			public readonly List<int> itemSpawnIDs;
			public readonly List<int> itemCollectionIDs;
			public readonly List<int> itemLootIDs;

			public BCIDData(List<int> npcIDs, List<int> itemSpawnIDs, 
				List<int> itemCollectionIDs, List<int> itemLootIDs)
			{
				this.npcIDs = npcIDs;
				this.itemSpawnIDs = itemSpawnIDs;
				this.itemCollectionIDs = itemCollectionIDs;
				this.itemLootIDs = itemLootIDs;
			}
		}

		public static void AddBoss(this Mod mod, float progression, string npcName, Func<bool> downedCondition,
			BCIDData identificationData, string spawnInfo, string despawnMessage, string texture,
			string overrideHeadIconTexture, Func<bool> bossAvailable) =>
			AddBCEntry(EntryType.Boss, mod, progression, npcName, downedCondition, identificationData, spawnInfo,
				despawnMessage, texture, overrideHeadIconTexture, bossAvailable);

		public static void AddMiniBoss(this Mod mod, float progression, string npcName, Func<bool> downedCondition,
			BCIDData identificationData, string spawnInfo, string despawnMessage, string texture,
			string overrideHeadIconTexture, Func<bool> bossAvailable) =>
			AddBCEntry(EntryType.Miniboss, mod, progression, npcName, downedCondition, identificationData, spawnInfo,
				despawnMessage, texture, overrideHeadIconTexture, bossAvailable);

		public static void AddEvent(this Mod mod, float progression, string eventName, Func<bool> downedCondition,
			BCIDData identificationData, string spawnInfo, string despawnMessage, string texture,
			string overrideHeadIconTexture,
			Func<bool> eventAvailable) =>
			AddBCEntry(EntryType.Event, mod, progression, eventName, downedCondition, identificationData, spawnInfo,
				despawnMessage, texture, overrideHeadIconTexture, eventAvailable);

		private static void AddBCEntry(EntryType entryType, Mod mod, float progression, string bcName,
			Func<bool> downedCondition, BCIDData identificationData, string spawnInfo,
			string despawnMessage, string texture, string overrideHeadIconTexture,
			Func<bool> bossAvailable)
		{
			string addType;

			switch (entryType) {
				case EntryType.Boss:
					addType = "AddBoss";
					break;

				case EntryType.Miniboss:
					addType = "AddMiniBoss";
					break;

				case EntryType.Event:
					addType = "AddEvent";
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(entryType), entryType, null);
			}

			BossChecklistMod.Call(
				addType,
				progression,
				identificationData.npcIDs ?? new List<int>(),
				mod,
				bcName,
				downedCondition,
				identificationData.itemSpawnIDs ?? new List<int>(),
				identificationData.itemCollectionIDs ?? new List<int>(),
				identificationData.itemLootIDs ?? new List<int>(),
				spawnInfo.IsNullOrEmptyFallback("Mods.BossChecklist.BossLog.DrawnText.NoInfo"),
				despawnMessage.IsNullOrEmptyFallback(entryType == EntryType.Boss
					? "Mods.BossChecklist.BossVictory.Generic"
					: ""),
				texture.IsNullOrEmptyFallback("BossChecklist/Resources/BossTextures/BossPlaceholder_byCorrina"),
				overrideHeadIconTexture,
				bossAvailable ?? (() => true)
			);
		}
		
		internal static void RegisterSpiritData(Mod spiritMod)
		{
			if (!BossChecklistIsLoaded)
				return;

			RegisterSpiritEvents(spiritMod);
			RegisterInterfaces(spiritMod);
		}

		private static void RegisterSpiritBosses(Mod spiritMod)
		{
			spiritMod.AddBoss(
				1.4f,
				"Scarabeus",
				() => MyWorld.downedScarabeus,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<Scarabeus>()
					},
					new List<int> {
						ModContent.ItemType<ScarabIdol>()
					},
					new List<int> {
						ModContent.ItemType<Trophy1>(),
						ModContent.ItemType<ScarabMask>(),
						ModContent.ItemType<ScarabBox>()
					},
					new List<int> {
						ModContent.ItemType<ScarabPendant>(),
						ModContent.ItemType<Chitin>(),
						ModContent.ItemType<ScarabBow>(),
						ModContent.ItemType<LocustCrook>(),
						ModContent.ItemType<RoyalKhopesh>(),
						ModContent.ItemType<RadiantCane>(),
						ModContent.ItemType<DesertSnowglobe>(),
						ItemID.LesserHealingPotion
					}),
				$"Use a [i:{ModContent.ItemType<ScarabIdol>()}] in the Desert during the daytime. A [i:{ModContent.ItemType<ScarabIdol>()}] can be found upon completing a certain Adventurer quest, or can be crafted, and is non-consumable.",
				"",
				"SpiritMod/Textures/BossChecklist/ScarabeusTexture",
				"SpiritMod/NPCs/Boss/Scarabeus/Scarabeus_Head_Boss",
				null);

			spiritMod.AddBoss(
				2.5f,
				"Moon Jelly Wizard",
				() => MyWorld.downedMoonWizard,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<MoonWizard>()
					},
					new List<int> {
						ModContent.ItemType<DreamlightJellyItem>()
					},
					new List<int> {
						ModContent.ItemType<MJWTrophy>(),
						ModContent.ItemType<MJWMask>(),
						ModContent.ItemType<MJWBox>()
					},
					new List<int> {
						ModContent.ItemType<Cornucopion>(),
						ModContent.ItemType<MoonjellySummonStaff>(),
						ModContent.ItemType<Moonburst>(),
						ModContent.ItemType<JellynautBubble>(),
						ModContent.ItemType<Moonshot>(),
						ModContent.ItemType<MoonJelly>()
					}
				),
				$"Use a [i:{ModContent.ItemType<DreamlightJellyItem>()}] anywhere at nighttime. A [i:{ModContent.ItemType<DreamlightJellyItem>()}] can be caught with a bug net during the Jelly Deluge, and is non-consumable",
				"",
				"SpiritMod/Textures/BossChecklist/MoonWizardTexture",
				"SpiritMod/NPCs/Boss/MoonWizard/MoonWizard_Head_Boss",
				null);

			spiritMod.AddBoss(
				3.5f,
				"Vinewrath Bane",
				() => MyWorld.downedReachBoss,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<ReachBoss>()
					},
					new List<int> {
						ModContent.ItemType<ReachBossSummon>()
					},
					new List<int> {
						ModContent.ItemType<Trophy5>(),
						ModContent.ItemType<ReachMask>(),
						ModContent.ItemType<VinewrathBox>()
					},
					new List<int> {
						ModContent.ItemType<DeathRose>(),
						ModContent.ItemType<SunbeamStaff>(),
						ModContent.ItemType<ThornBow>(),
						ModContent.ItemType<ReachVineStaff>(),
						ModContent.ItemType<ReachBossSword>(),
						// ModContent.ItemType<ReachKnife>(),
						ItemID.LesserHealingPotion
					}),
				$"Right-click the Bloodblossom, a glowing flower found at the bottom of the Briar. The Vinewrath Bane can be fought at any time and any place in progression. If a Bloodblossom is not present, use a [i:{ModContent.ItemType<ReachBossSummon>()}] in the Briar below the surface at any time.",
				"",
				"SpiritMod/Textures/BossChecklist/ReachBossTexture",
				"SpiritMod/NPCs/Boss/ReachBoss/ReachBoss/ReachBoss_Head_Boss",
				null
			);

			spiritMod.AddBoss(
				4.2f,
				"Ancient Avian",
				() => MyWorld.downedAncientFlier,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<AncientFlyer>()
					},
					new List<int> {
						ModContent.ItemType<JewelCrown>()
					},
					new List<int> {
						ModContent.ItemType<Trophy2>(), ModContent.ItemType<FlierMask>(),
						ModContent.ItemType<AvianBox>()
					},
					new List<int> {
						ModContent.ItemType<AvianHook>(),
						ModContent.ItemType<TalonBlade>(),
						ModContent.ItemType<Talonginus>(),
						ModContent.ItemType<SoaringScapula>(),
						ModContent.ItemType<TalonPiercer>(),
						ModContent.ItemType<SkeletalonStaff>(),
						ModContent.ItemType<TalonHeaddress>(),
						ModContent.ItemType<TalonGarb>(),
						ItemID.LesserHealingPotion
					}),
				$"Use a[i:{ModContent.ItemType<JewelCrown>()}] in the sky biome at any time.Alternatively, smash a Giant Avian Egg, which is found on Avian Islands near the sky layer.The Ancient Avian can be fought at any time and any place in progression.",
				"",
				"SpiritMod/Textures/BossChecklist/AvianTexture",
				"SpiritMod/NPCs/Boss/AncientFlyer_Head_Boss",
				null
			);

			spiritMod.AddBoss(
				5.9f,
				"Starplate Voyager",
				() => MyWorld.downedRaider,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<SteamRaiderHead>()
					},
					new List<int> {
						ModContent.ItemType<StarWormSummon>()
					},
					new List<int> {
						ModContent.ItemType<Trophy3>(),
						ModContent.ItemType<StarplateMask>(),
						ModContent.ItemType<StarplateBox>()
					},
					new List<int> {
						ModContent.ItemType<StarMap>(),
						ModContent.ItemType<CosmiliteShard>(),
						ItemID.LesserHealingPotion
					}),
				$"Use a [i:{ModContent.ItemType<StarWormSummon>()}] at an Astralite Beacon, located in the Asteroids, at nighttime.",
				"",
				"SpiritMod/Textures/BossChecklist/StarplateTexture",
				"SpiritMod/NPCs/Boss/SteamRaider/SteamRaiderHead_Head_Boss",
				null
			);

			spiritMod.AddBoss(
				6.8f,
				"Infernon",
				() => MyWorld.downedInfernon,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<Infernon>()
					},
					new List<int> {
						ModContent.ItemType<CursedCloth>()
					},
					new List<int> {
						ModContent.ItemType<Trophy4>(),
						ModContent.ItemType<InfernonMask>(),
						ModContent.ItemType<InfernonBox>()
					},
					new List<int> {
						ModContent.ItemType<HellsGaze>(),
						ModContent.ItemType<InfernalAppendage>(),
						ModContent.ItemType<InfernalJavelin>(),
						ModContent.ItemType<InfernalSword>(),
						ModContent.ItemType<DiabolicHorn>(),
						ModContent.ItemType<SevenSins>(),
						ModContent.ItemType<InfernalStaff>(),
						ModContent.ItemType<EyeOfTheInferno>(),
						ModContent.ItemType<InfernalShield>(),
						ItemID.GreaterHealingPotion
					}),
				$"Use a [i:{ModContent.ItemType<CursedCloth>()}] in the Underworld at any time.",
				"",
				"SpiritMod/Textures/BossChecklist/InfernonTexture",
				"SpiritMod/NPCs/Boss/Infernon/Infernon_Head_Boss",
				null
			);

			spiritMod.AddBoss(
				7.3f,
				"Dusking",
				() => MyWorld.downedDusking,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<Dusking>()
					},
					new List<int> {
						ModContent.ItemType<DuskCrown>()
					},
					new List<int> {
						ModContent.ItemType<Trophy6>(),
						ModContent.ItemType<DuskingMask>()
					},
					new List<int> {
						ModContent.ItemType<DuskPendant>(),
						ModContent.ItemType<ShadowflameSword>(),
						ModContent.ItemType<UmbraStaff>(),
						ModContent.ItemType<ShadowSphere>(),
						ModContent.ItemType<Shadowmoor>(),
						ModContent.ItemType<DuskStone>(),
						ItemID.GreaterHealingPotion
					}),
				$"Use a [i:{ModContent.ItemType<DuskCrown>()}] anywhere at nighttime.",
				"",
				"SpiritMod/Textures/BossChecklist/DuskingTexture",
				"SpiritMod/NPCs/Boss/Dusking/Dusking_Head_Boss",
				null
			);

			spiritMod.AddBoss(
				12.4f,
				"Atlas",
				() => MyWorld.downedAtlas,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<Atlas>()
					},
					new List<int> {
						ModContent.ItemType<StoneSkin>()
					},
					new List<int> {
						ModContent.ItemType<Trophy8>(),
						ModContent.ItemType<AtlasMask>(),
						ModContent.ItemType<AtlasBox>()
					},
					new List<int> {
						ModContent.ItemType<AtlasEye>(),
						ModContent.ItemType<Mountain>(),
						ModContent.ItemType<TitanboundBulwark>(),
						ModContent.ItemType<CragboundStaff>(),
						ModContent.ItemType<QuakeFist>(),
						ModContent.ItemType<Earthshatter>(),
						ModContent.ItemType<ArcaneGeyser>(),
						ItemID.GreaterHealingPotion
					}),
				$"Use a [i:{ModContent.ItemType<StoneSkin>()}] anywhere at any time.",
				"",
				"SpiritMod/Textures/BossChecklist/AtlasTexture",
				"SpiritMod/NPCs/Boss/Atlas/Atlas_Head_Boss",
				null
			);
		}

		private static void RegisterSpiritEvents(Mod spiritMod)
		{
			spiritMod.AddEvent(
				2.4f,
				"Jelly Deluge",
				() => MyWorld.downedJellyDeluge,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<DreamlightJelly>(), ModContent.NPCType<ExplodingMoonjelly>(),
						ModContent.NPCType<MoonjellyGiant>(), ModContent.NPCType<MoonlightPreserver>(),
						ModContent.NPCType<TinyLunazoa>()
					},
					new List<int> {
						ModContent.ItemType<DistressJellyItem>()
					},
					new List<int> {
						ModContent.ItemType<JellyDelugeBox>()
					},
					new List<int> {
						ModContent.ItemType<NautilusClub>(), ModContent.ItemType<ElectricGun>(),
						ModContent.ItemType<DreamlightJellyItem>(), ModContent.ItemType<TinyLunazoaItem>(),
						ModContent.ItemType<MoonJelly>()
					}),
				"Naturally occurs in space after any boss has been defeated. Can also be summoned by using a Distress Jelly, found in Asteroid Biomes and caught using a bug net. Occurs less frequently after the Moon Jelly Wizard has been defeated.",
				"",
				"SpiritMod/Textures/BossChecklist/JellyDeluge",
				"SpiritMod/Textures/BossChecklist/JellyDelugeIcon",
				null
			);

			spiritMod.AddEvent(
				5.6f,
				"The Tide",
				() => MyWorld.downedTide,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<Crocomount>(),
						ModContent.NPCType<KakamoraParachuter>(),
						ModContent.NPCType<KakamoraRunner>(),
						ModContent.NPCType<KakamoraShaman>(),
						ModContent.NPCType<KakamoraShielder>(),
						ModContent.NPCType<KakamoraShielderRare>(),
						ModContent.NPCType<LargeCrustecean>(),
						ModContent.NPCType<MangoJelly>(),
						ModContent.NPCType<Rylheian>(),
						ModContent.NPCType<SpearKakamora>(),
						ModContent.NPCType<SwordKakamora>()
					},
					new List<int> {
						ModContent.ItemType<BlackPearl>()
					},
					new List<int> {
						ModContent.ItemType<Trophy10>(),
						ModContent.ItemType<RlyehMask>(),
						ModContent.ItemType<TideBox>()
					},
					new List<int> {
						ModContent.ItemType<TribalScale>(),
						ModContent.ItemType<PumpBubbleGun>(),
						ModContent.ItemType<MagicConch>(),
						ModContent.ItemType<TikiJavelin>(),
						ModContent.ItemType<MangoJellyStaff>(),
						ModContent.ItemType<TomeOfRylien>(),
						ModContent.ItemType<TentacleChain>(),
						ModContent.ItemType<CoconutGun>()
					}),
				$"Use a [i:{ModContent.ItemType<BlackPearl>()}] at the Ocean at any time.",
				"",
				"SpiritMod/Textures/BossChecklist/TideTexture",
				"SpiritMod/Effects/InvasionIcons/Depths_Icon",
				null
			);

			spiritMod.AddEvent(
				6.5f,
				"Mystic Moon",
				() => MyWorld.downedBlueMoon,
				new BCIDData(
					new List<int> {
						ModContent.NPCType<NPCs.BlueMoon.Bloomshroom.Bloomshroom>(),
						ModContent.NPCType<NPCs.BlueMoon.Glitterfly.Glitterfly>(),
						ModContent.NPCType<NPCs.BlueMoon.GlowToad.GlowToad>(),
						ModContent.NPCType<NPCs.BlueMoon.Lumantis.Lumantis>(),
						ModContent.NPCType<NPCs.BlueMoon.LunarSlime.LunarSlime>(),
						ModContent.NPCType<NPCs.BlueMoon.MadHatter.MadHatter>()
					},
					new List<int> {
						ModContent.ItemType<BlueMoonSpawn>()
					},
					null,
					new List<int> {
						ModContent.ItemType<MoonStone>(),
						ModContent.ItemType<StopWatch>(),
						ModContent.ItemType<MagicConch>(),
						ModContent.ItemType<GloomgusStaff>(),
						ModContent.ItemType<MadHat>()
					}),
				$"Use a [i:{ModContent.ItemType<BlueMoonSpawn>()}] at nighttime.",
				"",
				"SpiritMod/Textures/BossChecklist/MysticMoonTexture",
				"SpiritMod/Textures/BossChecklist/BlueMoonIcon",
				null
			);
		}

		private static void RegisterInterfaces(Mod spiritMod)
		{
			foreach (Type type in spiritMod.Code.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IBCRegistrable)))) {
				BCIDData identificationData = new BCIDData(null, null, null, null);
				string spawnInfo = "";
				string despawnMessage = "";
				string texture = "";
				string headTextureOverride = "";
				Func<bool> isAvailable = null;

				if (!(Activator.CreateInstance(type) is IBCRegistrable registrableType))
					continue;

				registrableType.RegisterToChecklist(out EntryType entryType, out float progression, out string name,
					out Func<bool> downedCondition, ref identificationData, ref spawnInfo, ref despawnMessage,
					ref texture, ref headTextureOverride, ref isAvailable);

				AddBCEntry(entryType, spiritMod, progression, name, downedCondition, identificationData, spawnInfo, despawnMessage, texture, headTextureOverride, isAvailable);
			}
		}
	}
}
