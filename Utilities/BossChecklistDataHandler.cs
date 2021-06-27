using System;
using System.Collections.Generic;
using System.Linq;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Sets.RlyehianDrops;
using SpiritMod.Items.Sets.SeraphSet;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Summon.ElectricGun;
using SpiritMod.Items.Sets.ClubSubclass;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
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
