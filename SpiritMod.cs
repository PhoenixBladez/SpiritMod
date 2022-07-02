using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Items.Pins;
using SpiritMod.NPCs.Town;
using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.NPCs.Tides.Tide;
using SpiritMod.Skies;
using SpiritMod.Skies.Overlays;
using SpiritMod.Utilities;
using SpiritMod.World;
using SpiritMod.Sounds;
using SpiritMod.Dusts;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.Utilities;
using Terraria.UI.Chat;
using SpiritMod.Prim;
using SpiritMod.Items.Sets.BowsMisc.GemBows.Emerald_Bow;
using SpiritMod.Items.Sets.BowsMisc.GemBows.Ruby_Bow;
using SpiritMod.Items.Sets.BowsMisc.GemBows.Sapphire_Bow;
using SpiritMod.Items.Sets.BowsMisc.GemBows.Topaz_Bow;
using SpiritMod.Items.Consumable;
using SpiritMod.NPCs.AuroraStag;
using SpiritMod.Particles;
using SpiritMod.UI.QuestUI;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.BoonSystem;
using System.Collections.Concurrent;
using SpiritMod.Effects.Stargoop;
using SpiritMod.Mechanics.PortraitSystem;
using SpiritMod.Mechanics.Boids;
using SpiritMod.Mechanics.AutoSell;
using SpiritMod.Buffs.Summon;
using System.Linq;
using SpiritMod.Items.Weapon.Magic.Rhythm;
using SpiritMod.Items.Weapon.Magic.Rhythm.Anthem;
using SpiritMod.Mechanics.EventSystem;
using SpiritMod.Skies.Starjinx;
using SpiritMod.NPCs.StarjinxEvent;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Items.Sets.OlympiumSet;
using SpiritMod.Mechanics.Coverings;
using static Terraria.ModLoader.Core.TmodFile;
using SpiritMod.Items.Sets.DyesMisc.HairDye;
using SpiritMod.GlobalClasses.Projectiles;
using SpiritMod.GlobalClasses.Tiles;

namespace SpiritMod
{
	public class SpiritMod : Mod
	{
		internal UserInterface BookUserInterface;
		public static QuestBookUI QuestBookUIState;
		public static QuestHUD QuestHUD;

		public static CoveringsManager Coverings;

		public static ModKeybind QuestBookHotkey;
		public static ModKeybind QuestHUDHotkey;

		internal UserInterface SlotUserInterface;

		public static SpiritMod Instance;
		public UnifiedRandom spiritRNG;
		public static Effect auroraEffect;
		public static BasicEffect basicEffect;
		public static Effect GSaber;
		public static TrailManager TrailManager;
		public static PrimTrailManager primitives;
		public static StargoopManager Metaballs;
		public static BoidHost Boids;

		public static Effect stardustOverlayEffect;
		public static Effect glitchEffect;
		public static Effect starjinxBorderEffect;
		public static Effect vignetteEffect;
		public static Effect StarjinxNoise;
		public static Effect CircleNoise;
		public static Effect StarfirePrims;
		public static Effect ScreamingSkullTrail;
		public static Effect RipperSlugShader;
		public static Effect RepeatingTextureShader;
		public static Effect PrimitiveTextureMap;
		public static Effect EyeballShader;
		public static Effect ArcLashShader;
		public static Effect ConicalNoise;
		public static Effect JemShaders;
		public static Effect SunOrbShader;
		public static Effect ThyrsusShader;
		public static Effect JetbrickTrailShader;
		public static Effect OutlinePrimShader;
		public static Effect AnthemCircle;
		public static Effect TeslaShader;

		public static IDictionary<string, Effect> ShaderDict = new Dictionary<string, Effect>();

		public static PerlinNoise GlobalNoise;
		public static GlitchScreenShader glitchScreenShader;
		public static StarjinxBorderShader starjinxBorderShader;
		public static Vignette vignetteShader;
		public static Texture2D noise;

		public AutoSellUI AutoSellUI_SHORTCUT;
		public Mechanics.AutoSell.Sell_NoValue.Sell_NoValue SellNoValue_SHORTCUT;
		public Mechanics.AutoSell.Sell_Lock.Sell_Lock SellLock_SHORTCUT;
		public Mechanics.AutoSell.Sell_Weapons.Sell_Weapons SellWeapons_SHORTCUT;

		public UserInterface AutoSellUI_INTERFACE;
		public UserInterface SellNoValue_INTERFACE;
		public UserInterface SellLock_INTERFACE;
		public UserInterface SellWeapons_INTERFACE;

		public static SoundLooper nighttimeAmbience;
		public static SoundLooper underwaterAmbience;
		public static SoundLooper scarabWings;
		public static SoundLooper wavesAmbience;
		public static SoundLooper lightWind;
		public static SoundLooper desertWind;
		public static SoundLooper caveAmbience;
		public static SoundLooper spookyAmbience;

		public static event Action<SpriteViewMatrix> OnModifyTransformMatrix;
		//public static Dictionary<int, Texture2D> Portraits = new Dictionary<int, Texture2D>(); //Portraits dict - Gabe

		//public static Texture2D MoonTexture;
		public const string EMPTY_TEXTURE = "SpiritMod/Empty";
		public static Texture2D EmptyTexture
		{
			get;
			private set;
		}
		//public static int customEvent;
		public static int GlyphCurrencyID;

		internal static float deltaTime;

		private Vector2 _lastScreenSize;
		private Vector2 _lastViewSize;
		private Viewport _lastViewPort;

		public static int OlympiumCurrencyID = 0;

		/// <summary>Automatically returns false for every NPC ID inside of this list in <seealso cref="NPCs.GNPC.AllowTrickOrTreat(NPC)"/>.
		/// Note that this should only be used in edge cases where an NPC is neither homeless nor has homeTileX/Y set.</summary>
		public readonly List<int> NPCCandyBlacklist = new List<int>();

		public bool FinishedContentSetup { get; private set; }

		public SpiritMod()
		{
			Instance = this;
			spiritRNG = new UnifiedRandom();
		}

		public ModPacket GetPacket(MessageType type, int capacity)
		{
			ModPacket packet = GetPacket(capacity + 1);
			packet.Write((byte)type);
			return packet;
		}

		// this is alright, and i'll expand it so it can still be used, but really this shouldn't be used
		public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
		{
			packet.Write(msg);

			for (int m = 0; m < param.Length; m++)
			{
				object obj = param[m];
				if (obj is bool) packet.Write((bool)obj);
				else if (obj is byte) packet.Write((byte)obj);
				else if (obj is int) packet.Write((int)obj);
				else if (obj is float) packet.Write((float)obj);
				else if (obj is double) packet.Write((double)obj);
				else if (obj is short) packet.Write((short)obj);
				else if (obj is ushort) packet.Write((ushort)obj);
				else if (obj is sbyte) packet.Write((sbyte)obj);
				else if (obj is uint) packet.Write((uint)obj);
				else if (obj is decimal) packet.Write((decimal)obj);
				else if (obj is long) packet.Write((long)obj);
				else if (obj is string) packet.Write((string)obj);
			}
			return packet;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI) => SpiritMultiplayer.HandlePacket(reader, whoAmI);

		public override void UpdateMusic(ref int music, ref SceneEffectPriority priority)
		{
			var config = ModContent.GetInstance<SpiritMusicConfig>();

			if (Main.gameMenu)
				return;

			Player player = Main.LocalPlayer;
			if (!player.active)
				return;

			MyPlayer spirit = player.GetModPlayer<MyPlayer>();

			if (player.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Starjinx");
				priority = SceneEffectPriority.BossMedium; //Should take precedence even over most bosses, same as pillars 
			}

			if (TideWorld.TheTide && player.ZoneBeach)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/DepthInvasion");
				priority = SceneEffectPriority.Event;
			}

			if (MyWorld.ashRain && player.ZoneUnderworldHeight && config.AshfallMusic)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/AshStorm");
				priority = SceneEffectPriority.Event;
			}

			if (config.NeonBiomeMusic && spirit.ZoneSynthwave)
			{
				if (Main.dayTime)
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/NeonTech1");
				else
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/NeonTech");

				priority = SceneEffectPriority.BiomeLow;
			}

			if (priority > SceneEffectPriority.Event)
				return;

			if (Main.invasionType == 2 && config.FrostLegionMusic && player.ZoneOverworldHeight && Main.invasionProgressNearInvasion)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/FrostLegion");
				priority = SceneEffectPriority.BossLow;
			}

			if (priority > SceneEffectPriority.Environment)
				return;

			if (spirit.ZoneBlueMoon && !Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight))
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/BlueMoon");
				priority = SceneEffectPriority.Environment;
			}
			if (MyWorld.jellySky && !Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight))
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/JellySky");
				priority = SceneEffectPriority.Environment;
			}
			if (MyWorld.rareStarfallEvent && !MyWorld.jellySky && !spirit.ZoneAsteroid && !Main.dayTime && player.ZoneSkyHeight)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Starfall");
				priority = SceneEffectPriority.Environment;
			}

			if (priority > SceneEffectPriority.BiomeHigh)
				return;

			if (spirit.ZoneReach)
			{
				priority = SceneEffectPriority.BiomeHigh;
				if (!player.ZoneRockLayerHeight)
				{
					if (Main.dayTime)
						music = GetSoundSlot(SoundType.Music, "Sounds/Music/Reach");
					else
						music = GetSoundSlot(SoundType.Music, "Sounds/Music/ReachNighttime");
				}
				else
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/ReachUnderground");
			}
			else if (spirit.ZoneReach)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/ReachNighttime");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.AuroraMusic
				&& MyWorld.aurora
				&& player.ZoneSnow && player.ZoneOverworldHeight
				&& !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneMeteor
				&& !Main.bloodMoon && !Main.dayTime)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/AuroraSnow");
				priority = SceneEffectPriority.BiomeHigh;
			}
			if (config.MeteorMusic && player.ZoneMeteor && !Main.bloodMoon)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Meteor");
				priority = SceneEffectPriority.Environment;
			}

			if (config.BlizzardMusic
				&& player.ZoneSnow
				&& player.ZoneOverworldHeight
				&& !player.ZoneCorrupt
				&& !player.ZoneMeteor
				&& !player.ZoneCrimson
				&& Main.raining)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Blizzard");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.UnderwaterMusic && player.ZoneBeach && !MyWorld.luminousOcean && spirit.isFullySubmerged)
			{
				Mod thoriumMod = ModLoader.GetMod("ThoriumMod"); //Thorium compat
				if (thoriumMod == null || (!(thoriumMod.Call("GetZoneAquaticDepths", player) is null) && thoriumMod.Call("GetZoneAquaticDepths", player) is bool inDepths && !inDepths))
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/UnderwaterMusic");
					priority = SceneEffectPriority.BiomeHigh;
				}
			}

			if (config.LuminousMusic
				&& player.ZoneBeach
				&& MyWorld.luminousOcean
				&& !Main.dayTime)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/OceanNighttime");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.HallowNightMusic
				&& player.ZoneHallow && player.ZoneOverworldHeight
				&& !Main.dayTime && !Main.raining && !Main.bloodMoon
				&& !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneJungle && !player.ZoneBeach && !player.ZoneMeteor)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/HallowNight");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.CorruptNightMusic
				&& player.ZoneCorrupt
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneHallow
				&& !player.ZoneMeteor
				&& !Main.bloodMoon)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/CorruptNight");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.CrimsonNightMusic
				&& player.ZoneCrimson
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneHallow
				&& !player.ZoneMeteor
				&& !Main.bloodMoon)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/CrimsonNight");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.CalmNightMusic
				&& MyWorld.calmNight
				&& !player.ZoneSnow
				&& !spirit.ZoneReach
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !player.ZoneJungle
				&& !player.ZoneBeach
				&& !player.ZoneHallow
				&& !player.ZoneMeteor
				&& !player.ZoneDesert
				&& !Main.raining
				&& !Main.bloodMoon)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/CalmNight");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (config.SnowNightMusic
				&& player.ZoneSnow
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneCorrupt
				&& !player.ZoneMeteor
				&& !player.ZoneCrimson
				&& !player.ZoneHallow
				&& !MyWorld.aurora
				&& !Main.raining
				&& !Main.bloodMoon)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/SnowNighttime");
				priority = SceneEffectPriority.BiomeMedium;
			}

			if (config.DesertNightMusic
				&& player.ZoneDesert
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !player.ZoneBeach)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/DesertNighttime");
				priority = SceneEffectPriority.BiomeHigh;
			}

			if (spirit.ZoneAsteroid)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Asteroids");
				priority = SceneEffectPriority.Environment;
			}

			if (priority > SceneEffectPriority.BiomeMedium)
				return;

			if (spirit.ZoneSpirit && NPC.downedMechBossAny && !player.ZoneDungeon)
			{
				priority = SceneEffectPriority.BiomeMedium;
				if (player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f)
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer1");
				if (player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f)
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer2");
				if (player.position.Y / 16 >= Main.maxTilesY - 330)
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer3");
				if (!player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f)
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritOverworld");
			}

			if (config.GraniteMusic
				&& spirit.ZoneGranite && !player.ZoneHallow && !player.ZoneCorrupt && !player.ZoneCrimson
				&& !player.ZoneOverworldHeight && !spirit.ZoneSpirit && spirit.inGranite)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/GraniteBiome");
				priority = SceneEffectPriority.BiomeMedium;
			}

			if (config.MarbleMusic
				&& spirit.ZoneMarble && !player.ZoneHallow && !player.ZoneCorrupt && !player.ZoneCrimson
				&& !player.ZoneOverworldHeight && !spirit.ZoneSpirit && spirit.inMarble)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/MarbleBiome");
				priority = SceneEffectPriority.BiomeMedium;
			}
			if (config.SpiderCaveMusic
				&& spirit.ZoneSpider && !player.ZoneHallow && !player.ZoneCorrupt && !player.ZoneCrimson
				&& !player.ZoneOverworldHeight && !spirit.ZoneSpirit)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiderCave");
				priority = SceneEffectPriority.BiomeMedium;
			}
		}

		public override object Call(params object[] args)
		{
			if (args.Length < 1)
			{
				var stack = new System.Diagnostics.StackTrace(true);
				Logger.Error("Call Error: No arguments given:\n" + stack.ToString());
				return null;
			}

			CallContext context;
			int? contextNum = args[0] as int?;
			if (contextNum.HasValue)
				context = (CallContext)contextNum.Value;
			else
				context = ParseCallName(args[0] as string);

			if (context == CallContext.Invalid && !contextNum.HasValue)
			{ //Check if it has a valid value
				var stack = new System.Diagnostics.StackTrace(true);
				Logger.Error("Call Error: Context invalid or null:\n" + stack.ToString());
				return null;
			}

			if (context <= CallContext.Invalid || context >= CallContext.Limit)
			{ //Check if value is in-bounds
				var stack = new System.Diagnostics.StackTrace(true);
				Logger.Error("Call Error: Context invalid:\n" + stack.ToString());
				return null;
			}

			try
			{
				if (context == CallContext.Downed)
					return BossDowned(args);
				if (context == CallContext.GlyphGet)
					return GetGlyph(args);
				if (context == CallContext.GlyphSet)
				{
					SetGlyph(args);
					return null;
				}
				if (context == CallContext.AddQuest)
					return QuestManager.ModCallAddQuest(args);
				if (context == CallContext.UnlockQuest)
				{
					QuestManager.ModCallUnlockQuest(args);
					return null;
				}
				if (context == CallContext.GetQuestIsUnlocked)
					return QuestManager.ModCallGetQuestValueFromContext(args, 0);
				if (context == CallContext.GetQuestIsCompleted)
					return QuestManager.ModCallGetQuestValueFromContext(args, 2);
				if (context == CallContext.GetQuestIsActive)
					return QuestManager.ModCallGetQuestValueFromContext(args, 1);
				if (context == CallContext.GetQuestRewardsGiven)
					return QuestManager.ModCallGetQuestValueFromContext(args, 3);
				if (context == CallContext.Portrait)
				{
					PortraitManager.ModCallAddPortrait(args);
					return null;
				}
			}
			catch (Exception e)
			{
				Logger.Error("Call Error: " + e.Message + "\n" + e.StackTrace);
			}
			return null;
		}

		private static CallContext ParseCallName(string context)
		{
			if (context == null)
				return CallContext.Invalid;
			switch (context)
			{
				case "downed":
					return CallContext.Downed;
				case "getGlyph":
					return CallContext.GlyphGet;
				case "setGlyph":
					return CallContext.GlyphSet;
				case "AddQuest":
					return CallContext.AddQuest;
				case "UnlockQuest":
					return CallContext.UnlockQuest;
				case "IsQuestUnlocked":
					return CallContext.GetQuestIsUnlocked;
				case "IsQuestActive":
					return CallContext.GetQuestIsActive;
				case "IsQuestCompleted":
					return CallContext.GetQuestIsCompleted;
				case "QuestRewardsGiven":
					return CallContext.GetQuestRewardsGiven;
				case "Portrait":
					return CallContext.Portrait;
			}
			return CallContext.Invalid;
		}

		private static bool BossDowned(object[] args)
		{
			if (args.Length < 2)
				throw new ArgumentException("No boss name specified");
			string name = args[1] as string;
			switch (name)
			{
				case "Scarabeus": return MyWorld.downedScarabeus;
				case "Moon Jelly Wizard": return MyWorld.downedMoonWizard;
				case "Vinewrath Bane": return MyWorld.downedReachBoss;
				case "Ancient Avian": return MyWorld.downedAncientFlier;
				case "Starplate Raider": return MyWorld.downedRaider;
				case "Infernon": return MyWorld.downedInfernon;
				case "Dusking": return MyWorld.downedDusking;
				case "Atlas": return MyWorld.downedAtlas;
			}
			throw new ArgumentException("Invalid boss name:" + name);
		}

		private static void SetGlyph(object[] args)
		{
			if (args.Length < 2)
				throw new ArgumentException("Missing argument: Item");
			else if (args.Length < 3)
				throw new ArgumentException("Missing argument: Glyph");
			if (!(args[1] is Item item))
				throw new ArgumentException("First argument must be of type Item");
			int? glyphID = args[2] as int?;
			if (!glyphID.HasValue)
				throw new ArgumentException("Second argument must be of type int");
			GlyphType glyph = (GlyphType)glyphID;
			if (glyph < GlyphType.None || glyph >= GlyphType.Count)
				throw new ArgumentException("Glyph must be in range [" +
					(int)GlyphType.None + "," + (int)GlyphType.Count + ")");
			item.GetGlobalItem<Items.GItem>().SetGlyph(item, glyph);
		}

		private static int GetGlyph(object[] args)
		{
			if (args.Length < 2)
				throw new ArgumentException("Missing argument: Item");
			if (!(args[1] is Item item))
				throw new ArgumentException("First argument must be of type Item");
			return (int)item.GetGlobalItem<Items.GItem>().Glyph;
		}

		public override void Load()
		{
			//Always keep this call in the first line of Load!
			LoadReferences();
			StructureLoader.Load(this);
			QuestBookHotkey = RegisterHotKey("SpiritMod:QuestBookToggle", "C");
			QuestHUDHotkey = RegisterHotKey("SpiritMod:QuestHUDToggle", "V");

			if (!Main.dedServ)
			{
				ParticleHandler.RegisterParticles();
				BookUserInterface = new UserInterface();

				QuestBookUIState = new QuestBookUI();
				QuestHUD = new QuestHUD();
				Boids = new BoidHost();
				EventManager.Load();
				_lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
				_lastViewSize = Main.ViewSize;
				_lastViewPort = Main.graphics.GraphicsDevice.Viewport;
			}

			QuestManager.Load();
			BoonLoader.Load();
			SpiritMultiplayer.Load();
			SpiritDetours.Initialize();
			//Coverings = new CoveringsManager();
			//Coverings.Load(this);

			GlobalNoise = new PerlinNoise(Main.rand.Next());

			if (Main.rand == null)
				Main.rand = new UnifiedRandom();

			Items.Halloween.CandyBag.Initialize();

			OlympiumCurrencyID = CustomCurrencyManager.RegisterCurrency(new OlympiumCurrency(ModContent.ItemType<OlympiumToken>(), 999));

			#region configLabels
			//Regular Config labels
			ModTranslation config1 = CreateTranslation("Screenshake");
			config1.SetDefault($"[i:" + Find<ModItem>("BlueNeonSign").Type + "]  Visuals: Screenshake");
			AddTranslation(config1);

			ModTranslation config2 = CreateTranslation("Distortion");
			config2.SetDefault($"[i:" + Find<ModItem>("BlueNeonSign").Type + "]  Visuals: Screen Distortion");
			AddTranslation(config2);

			ModTranslation config3 = CreateTranslation("Particles");
			config3.SetDefault($"[i:" + Find<ModItem>("BlueNeonSign").Type + "]  Visuals: Foreground Particles");
			AddTranslation(config3);

			ModTranslation config4 = CreateTranslation("Quicksell");
			config4.SetDefault($"[i:" + Find<ModItem>("SeedBag").Type + "]  QoL: Quick-Sell Feature");
			AddTranslation(config4);

			ModTranslation config5 = CreateTranslation("Autoswing");
			config5.SetDefault($"[i:" + Find<ModItem>("PurpleNeonSign").Type + "]  QoL: Auto-Reuse Tooltip");
			AddTranslation(config5);

			ModTranslation config6 = CreateTranslation("AmbientSounds");
			config6.SetDefault($"[i:" + Find<ModItem>("SurrenderBell").Type + "]  Ambience: Ambient Sounds");
			AddTranslation(config6);

			ModTranslation config7 = CreateTranslation("LeafFallAmbience");
			config7.SetDefault($"[i:" + Find<ModItem>("EnchantedLeaf").Type + "]  Ambience: Falling Leaf Effects");
			AddTranslation(config7);

			ModTranslation config8 = CreateTranslation("QuestButton");
			config8.SetDefault($"[i:" + Find<ModItem>("Book_Slime").Type + "]  Quests: Quest Book Button Location");
			AddTranslation(config8);

			ModTranslation config15 = CreateTranslation("QuestBookSwitching");
			config15.SetDefault($"[i:" + Find<ModItem>("Book_Slime").Type + "]  Quests: Quest Book Tab Switching");
			AddTranslation(config15);

			ModTranslation config9 = CreateTranslation("QuestIcons");
			config9.SetDefault($"[i:" + Find<ModItem>("Brightbulb").Type + "]  Quests: Town NPC Quest Icons");
			AddTranslation(config9);

			ModTranslation config10 = CreateTranslation("ArcaneHideoutGen");
			config10.SetDefault($"[i:" + Find<ModItem>("JellyCandle").Type + "]  Worldgen: Arcane Tower and Bandit Hideout Generation");
			AddTranslation(config10);

			ModTranslation config11 = CreateTranslation("OceanShape");
			config11.SetDefault($"[i:" + Find<ModItem>("BlackPearl").Type + "]  Oceans: Ocean Generation Shape");
			AddTranslation(config11);

			ModTranslation config12 = CreateTranslation("OceanVents");
			config12.SetDefault($"[i:" + Find<ModItem>("BlackPearl").Type + "]  Oceans: Hydothermal Vent Ecosystems");
			AddTranslation(config12);

			ModTranslation config13 = CreateTranslation("OceanWater");
			config13.SetDefault($"[i:" + Find<ModItem>("BlackPearl").Type + "]  Oceans: Ocean Water Transparency");
			AddTranslation(config13);

			ModTranslation config14 = CreateTranslation("WaterEnemies");
			config14.SetDefault($"[i:" + Find<ModItem>("SpiritKoi").Type + "]  Fishing: Fishing Encounters");
			AddTranslation(config14);

			//Music Config
			ModTranslation musicConfig1 = CreateTranslation("BlizzardMusic");
			musicConfig1.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for Blizzards");
			AddTranslation(musicConfig1);

			ModTranslation musicConfig2 = CreateTranslation("SnowNightMusic");
			musicConfig2.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Snow biome at night");
			AddTranslation(musicConfig2);

			ModTranslation musicConfig3 = CreateTranslation("DesertNightMusic");
			musicConfig3.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Desert biome at night");
			AddTranslation(musicConfig3);

			ModTranslation musicConfig4 = CreateTranslation("HallowNightMusic");
			musicConfig4.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for The Hallow at night");
			AddTranslation(musicConfig4);

			ModTranslation musicConfig5 = CreateTranslation("CorruptNightMusic");
			musicConfig5.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for The Corruption at night");
			AddTranslation(musicConfig5);

			ModTranslation musicConfig6 = CreateTranslation("CrimsonNightMusic");
			musicConfig6.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for The Crimson at night");
			AddTranslation(musicConfig6);

			ModTranslation musicConfig7 = CreateTranslation("GraniteMusic");
			musicConfig7.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Granite biome");
			AddTranslation(musicConfig7);


			ModTranslation musicConfig8 = CreateTranslation("MarbleMusic");
			musicConfig8.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Marble biome");
			AddTranslation(musicConfig8);

			ModTranslation musicConfig9 = CreateTranslation("SpiderMusic");
			musicConfig9.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Spider caverns");
			AddTranslation(musicConfig9);

			ModTranslation musicConfig10 = CreateTranslation("MeteorMusic");
			musicConfig10.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Meteorite biome");
			AddTranslation(musicConfig10);

			ModTranslation musicConfig18 = CreateTranslation("FrostLegionMusic");
			musicConfig18.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Frost Legion invasion");
			AddTranslation(musicConfig18);

			ModTranslation musicConfig11 = CreateTranslation("SkeletonPrimeMusic");
			musicConfig11.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for Skeletron Prime");
			AddTranslation(musicConfig11);

			ModTranslation musicConfig16 = CreateTranslation("AuroraMusic");
			musicConfig16.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Aurora");
			AddTranslation(musicConfig16);

			ModTranslation musicConfig12 = CreateTranslation("LuminousMusic");
			musicConfig12.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for Luminous Oceans");
			AddTranslation(musicConfig12);

			ModTranslation musicConfig13 = CreateTranslation("CalmNightMusic");
			musicConfig13.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for Calm Night mini-events");
			AddTranslation(musicConfig13);

			ModTranslation musicConfig14 = CreateTranslation("OceanDepthsMusic");
			musicConfig14.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Ocean depths");
			AddTranslation(musicConfig14);

			ModTranslation musicConfig15 = CreateTranslation("HyperspaceMusic");
			musicConfig15.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds two unique tracks for the artificial Hyperspace biome");
			AddTranslation(musicConfig15);

			ModTranslation musicConfig17 = CreateTranslation("AshfallMusic");
			musicConfig17.SetDefault($"[i:" + Find<ModItem>("NeonMusicBox").Type + "]  Adds a unique track for the Ashfall weather in the Underworld");
			AddTranslation(musicConfig17);
			#endregion

			if (Main.netMode != NetmodeID.Server)
			{
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Effects/ShockwaveEffect", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();

				Filters.Scene["PulsarShockwave"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Effects/PulsarShockwave", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "PulsarShockwave"), EffectPriority.VeryHigh);
				Filters.Scene["PulsarShockwave"].Load();

				SlotUserInterface = new UserInterface();

				Filters.Scene["ShockwaveTwo"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Effects/ShockwaveTwo", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "ShockwaveTwo"), EffectPriority.VeryHigh);
				Filters.Scene["ShockwaveTwo"].Load();

				Filters.Scene["SpiritMod:AshRain"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Effects/AshRain", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "AshRain"), EffectPriority.VeryLow);
				Filters.Scene["SpiritMod:AshRain"].Load();
			}

			Filters.Scene["SpiritMod:ReachSky"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.05f, 0.05f, .05f).UseOpacity(0.4f), EffectPriority.High);

			Filters.Scene["CystalTower"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.149f, 0.142f, 0.207f).UseOpacity(0.5f), EffectPriority.VeryHigh);
			Filters.Scene["CystalBloodMoon"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.149f, 0.142f, 0.207f).UseOpacity(2f), EffectPriority.VeryHigh);

			Filters.Scene["SpiritMod:SpiritUG1"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.2f, 0.2f, .2f).UseOpacity(0.8f), EffectPriority.High);
			Filters.Scene["SpiritMod:SpiritUG2"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.45f, 0.45f, .45f).UseOpacity(0.9f), EffectPriority.High);

			Filters.Scene["SpiritMod:WindEffect"] = new Filter((new BlizzardShaderData("FilterBlizzardForeground")).UseColor(0.4f, 0.4f, 0.4f).UseSecondaryColor(0.2f, 0.2f, 0.2f).UseImage("Images/Misc/noise", 0, null).UseOpacity(0.149f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High);
			Filters.Scene["SpiritMod:WindEffect2"] = new Filter((new BlizzardShaderData("FilterBlizzardForeground")).UseColor(0.4f, 0.4f, 0.4f).UseSecondaryColor(0.2f, 0.2f, 0.2f).UseImage("Images/Misc/noise", 0, null).UseOpacity(0.549f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High);

			GlyphCurrencyID = CustomCurrencyManager.RegisterCurrency(new Currency(ModContent.ItemType<Items.Glyphs.Glyph>(), 999L));

			AutoloadMinionDictionary.AddBuffs(Code);

			if (Main.netMode != NetmodeID.Server)
			{
				TrailManager = new TrailManager(this);
				AddEquipTexture(null, EquipType.Legs, "TalonGarb_Legs", "SpiritMod/Items/Sets/AvianDrops/ApostleArmor/TalonGarb_Legs");
				EmptyTexture = ModContent.Request<Texture2D>("Empty", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value; 
				auroraEffect = ModContent.Request<Effect>("Effects/aurora", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				ShaderDict = new Dictionary<string, Effect>();
				var tmodfile = (TmodFile)typeof(SpiritMod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Instance);
				var files = (IDictionary<string, FileEntry>)typeof(TmodFile).GetField("files", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(tmodfile);
				foreach (KeyValuePair<string, FileEntry> kvp in files.Where(x => x.Key.Contains("Effects/") && x.Key.Contains(".xnb")))
					ShaderDict.Add(kvp.Key.Remove(kvp.Key.Length - ".xnb".Length, ".xnb".Length).Remove(0, "Effects/".Length), ModContent.Request<Effect>(kvp.Key.Remove(kvp.Key.Length - ".xnb".Length, ".xnb".Length)));

				int width = Main.graphics.GraphicsDevice.Viewport.Width;
				int height = Main.graphics.GraphicsDevice.Viewport.Height;
				Vector2 zoom = Main.GameViewMatrix.Zoom;
				Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
				Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

				basicEffect = new BasicEffect(Main.graphics.GraphicsDevice)
				{
					VertexColorEnabled = true,
					View = view,
					Projection = projection
				};

				noise = ModContent.Request<Texture2D>("Textures/noise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				SpiritModAutoSellTextures.Load();

				GameShaders.Hair.BindShader(ModContent.ItemType<SeafoamDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(Color.Cyan, Color.White, MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f)))));
				GameShaders.Hair.BindShader(ModContent.ItemType<BrightbloodDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.IndianRed));
				GameShaders.Hair.BindShader(ModContent.ItemType<MeteorDye>(), new HairShaderData(Main.PixelShaderRef, "ArmorHades")).UseImage("Images/Misc/noise").UseColor(Color.Orange).UseSecondaryColor(Color.DarkOrange).UseSaturation(5.3f);
				GameShaders.Hair.BindShader(ModContent.ItemType<ViciousDye>(), new HairShaderData(Main.PixelShaderRef, "ArmorVortex")).UseImage("Images/Misc/noise").UseColor(Color.Crimson).UseSaturation(3.3f);
				GameShaders.Hair.BindShader(ModContent.ItemType<CystalDye>(), new HairShaderData(Main.PixelShaderRef, "ArmorNebula")).UseImage("Images/Misc/Perlin").UseColor(Color.Plum).UseSaturation(5.3f);
				GameShaders.Hair.BindShader(ModContent.ItemType<SnowMirageDye>(), new HairShaderData(Main.PixelShaderRef, "ArmorMirage")).UseImage("Images/Misc/Perlin").UseColor(Color.PaleTurquoise).UseSaturation(2.3f);

				PortraitManager.Load();

				AutoSellUI_INTERFACE = new UserInterface();
				SellNoValue_INTERFACE = new UserInterface();
				SellLock_INTERFACE = new UserInterface();
				SellWeapons_INTERFACE = new UserInterface();

				AutoSellUI_SHORTCUT = new AutoSellUI();
				SellNoValue_SHORTCUT = new Mechanics.AutoSell.Sell_NoValue.Sell_NoValue();
				SellLock_SHORTCUT = new Mechanics.AutoSell.Sell_Lock.Sell_Lock();
				SellWeapons_SHORTCUT = new Mechanics.AutoSell.Sell_Weapons.Sell_Weapons();

				AutoSellUI_SHORTCUT.Activate();
				SellNoValue_SHORTCUT.Activate();
				SellLock_SHORTCUT.Activate();
				SellWeapons_SHORTCUT.Activate();

				stardustOverlayEffect = ModContent.Request<Effect>("Effects/StardustOverlay", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				glitchEffect = ModContent.Request<Effect>("Effects/glitch", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				glitchScreenShader = new GlitchScreenShader(glitchEffect);
				Filters.Scene["SpiritMod:Glitch"] = new Filter(glitchScreenShader, (EffectPriority)50);

				starjinxBorderEffect = ModContent.Request<Effect>("Effects/StarjinxBorder", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				starjinxBorderShader = new StarjinxBorderShader(starjinxBorderEffect, "MainPS");
				Filters.Scene["SpiritMod:StarjinxBorder"] = new Filter(starjinxBorderShader, (EffectPriority)50);

				Filters.Scene["SpiritMod:StarjinxBorderFade"] = new Filter(new StarjinxBorderShader(starjinxBorderEffect, "FadePS"), (EffectPriority)70);

				vignetteEffect = ModContent.Request<Effect>("Effects/Vignette", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				vignetteShader = new Vignette(vignetteEffect, "MainPS");
				Filters.Scene["SpiritMod:Vignette"] = new Filter(vignetteShader, (EffectPriority)100);

				StarjinxNoise = ModContent.Request<Effect>("Effects/StarjinxNoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				CircleNoise = ModContent.Request<Effect>("Effects/CircleNoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				StarfirePrims = ModContent.Request<Effect>("Effects/StarfirePrims", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ScreamingSkullTrail = ModContent.Request<Effect>("Effects/ScreamingSkullTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				RipperSlugShader = ModContent.Request<Effect>("Effects/RipperSlugShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				RepeatingTextureShader = ModContent.Request<Effect>("Effects/RepeatingTextureShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				PrimitiveTextureMap = ModContent.Request<Effect>("Effects/PrimitiveTextureMap", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				EyeballShader = ModContent.Request<Effect>("Effects/EyeballShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ArcLashShader = ModContent.Request<Effect>("Effects/ArcLashShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ConicalNoise = ModContent.Request<Effect>("Effects/ConicalNoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				JemShaders = ModContent.Request<Effect>("Effects/JemShaders", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				SunOrbShader = ModContent.Request<Effect>("Effects/SunOrbShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ThyrsusShader = ModContent.Request<Effect>("Effects/ThyrsusShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				JetbrickTrailShader = ModContent.Request<Effect>("Effects/JetbrickTrailShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				OutlinePrimShader = ModContent.Request<Effect>("Effects/OutlinePrimShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				GSaber = ModContent.Request<Effect>("Effects/GSaber", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				AnthemCircle = ModContent.Request<Effect>("Effects/AnthemCircle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				TeslaShader = ModContent.Request<Effect>("Effects/TeslaShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				SkyManager.Instance["SpiritMod:AuroraSky"] = new AuroraSky();
				Filters.Scene["SpiritMod:AuroraSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
				Overlays.Scene["SpiritMod:AuroraSky"] = new AuroraOverlay();

				Filters.Scene["SpiritMod:BlueMoonSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.1f, 0.2f, 0.5f).UseOpacity(0.53f), EffectPriority.High);
				SkyManager.Instance["SpiritMod:BlueMoonSky"] = new BlueMoonSky();

				SkyManager.Instance["SpiritMod:StarjinxSky"] = new StarjinxSky();
				Filters.Scene["SpiritMod:StarjinxSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:MeteorSky"] = new MeteorSky();
				SkyManager.Instance["SpiritMod:AsteroidSky2"] = new MeteorBiomeSky2();
				Filters.Scene["SpiritMod:MeteorSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
				Filters.Scene["SpiritMod:AsteroidSky2"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:SpiritBiomeSky"] = new SpiritBiomeSky();
				Filters.Scene["SpiritMod:SpiritBiomeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:JellySky"] = new JellySky();
				Filters.Scene["SpiritMod:JellySky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:PurpleAlgaeSky"] = new PurpleAlgaeSky();
				Filters.Scene["SpiritMod:PurpleAlgaeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:GreenAlgaeSky"] = new GreenAlgaeSky();
				Filters.Scene["SpiritMod:GreenAlgaeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:OceanFloorSky"] = new OceanFloorSky();
				Filters.Scene["SpiritMod:OceanFloorSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:BlueAlgaeSky"] = new BlueAlgaeSky();
				Filters.Scene["SpiritMod:BlueAlgaeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:BloodMoonSky"] = new BloodMoonSky();
				Filters.Scene["SpiritMod:BloodMoonSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				Filters.Scene["SpiritMod:Atlas"] = new Filter(new AtlasScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0.6f), EffectPriority.VeryHigh);
				SkyManager.Instance["SpiritMod:Atlas"] = new AtlasSky();

				Filters.Scene["SpiritMod:SynthwaveSky"] = new Filter(new AtlasScreenShaderData("FilterMiniTower").UseColor(0.158f, 0.083f, 0.212f).UseOpacity(0.43f), EffectPriority.VeryHigh);
				SkyManager.Instance["SpiritMod:SynthwaveSky"] = new VaporwaveSky();

				Filters.Scene["SpiritMod:MeteoriteSky"] = new Filter(new AtlasScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);
				SkyManager.Instance["SpiritMod:MeteoriteSky"] = new MeteoriteSky();

				//Music Boxes
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/CrimsonNight"), Find<ModItem>("CrimsonNightBox").Type, Find<ModTile>("CrimsonNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/UnderwaterMusic"), Find<ModItem>("OceanDepthsBox").Type, Find<ModTile>("OceanDepthsBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/TranquilWinds"), Find<ModItem>("TranquilWindsBox").Type, Find<ModTile>("TranquilWindsBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/NeonTech"), Find<ModItem>("NeonMusicBox").Type, Find<ModTile>("NeonMusicBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/NeonTech1"), Find<ModItem>("HyperspaceDayBox").Type, Find<ModTile>("HyperspaceDayBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritOverworld"), Find<ModItem>("SpiritBox1").Type, Find<ModTile>("SpiritBox1").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer1"), Find<ModItem>("SpiritBox2").Type, Find<ModTile>("SpiritBox2").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer2"), Find<ModItem>("SpiritBox3").Type, Find<ModTile>("SpiritBox3").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer3"), Find<ModItem>("SpiritBox4").Type, Find<ModTile>("SpiritBox4").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Reach"), Find<ModItem>("ReachBox").Type, Find<ModTile>("ReachBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/ReachNighttime"), Find<ModItem>("BriarNightBox").Type, Find<ModTile>("BriarNightBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Asteroids"), Find<ModItem>("AsteroidBox").Type, Find<ModTile>("AsteroidBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Starplate"), Find<ModItem>("StarplateBox").Type, Find<ModTile>("StarplateBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/MoonJelly"), Find<ModItem>("MJWBox").Type, Find<ModTile>("MJWBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Scarabeus"), Find<ModItem>("ScarabBox").Type, Find<ModTile>("ScarabBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Atlas"), Find<ModItem>("AtlasBox").Type, Find<ModTile>("AtlasBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/ReachBoss"), Find<ModItem>("VinewrathBox").Type, Find<ModTile>("VinewrathBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/AncientAvian"), Find<ModItem>("AvianBox").Type, Find<ModTile>("AvianBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon"), Find<ModItem>("InfernonBox").Type, Find<ModTile>("InfernonBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Blizzard"), Find<ModItem>("BlizzardBox").Type, Find<ModTile>("BlizzardBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/AuroraSnow"), Find<ModItem>("AuroraBox").Type, Find<ModTile>("AuroraBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SnowNighttime"), Find<ModItem>("SnowNightBox").Type, Find<ModTile>("SnowNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/DesertNighttime"), Find<ModItem>("DesertNightBox").Type, Find<ModTile>("DesertNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/OceanNighttime"), Find<ModItem>("LuminousNightBox").Type, Find<ModTile>("LuminousNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/HallowNight"), Find<ModItem>("HallowNightBox").Type, Find<ModTile>("HallowNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/CalmNight"), Find<ModItem>("CalmNightBox").Type, Find<ModTile>("CalmNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/CorruptNight"), Find<ModItem>("CorruptNightBox").Type, Find<ModTile>("CorruptNightBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Meteor"), Find<ModItem>("MeteorBox").Type, Find<ModTile>("MeteorBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/MarbleBiome"), Find<ModItem>("MarbleBox").Type, Find<ModTile>("MarbleBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/GraniteBiome"), Find<ModItem>("GraniteBox").Type, Find<ModTile>("GraniteBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiderCave"), Find<ModItem>("SpiderCaveBox").Type, Find<ModTile>("SpiderCaveBox").Type);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/BlueMoon"), Find<ModItem>("BlueMoonBox").Type, Find<ModTile>("BlueMoonBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/DepthInvasion"), Find<ModItem>("TideBox").Type, Find<ModTile>("TideBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/JellySky"), Find<ModItem>("JellyDelugeBox").Type, Find<ModTile>("JellyDelugeBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/FrostLegion"), Find<ModItem>("FrostLegionBox").Type, Find<ModTile>("FrostLegionBox").Type);
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/AshStorm"), Find<ModItem>("AshfallBox").Type, Find<ModTile>("AshfallBox").Type);


				AutoSellUI.visible = false;
				Mechanics.AutoSell.Sell_NoValue.Sell_NoValue.visible = false;
				Mechanics.AutoSell.Sell_Lock.Sell_Lock.visible = false;
				Mechanics.AutoSell.Sell_Weapons.Sell_Weapons.visible = false;

				AutoSellUI_INTERFACE.SetState(AutoSellUI_SHORTCUT);
				SellNoValue_INTERFACE.SetState(SellNoValue_SHORTCUT);
				SellLock_INTERFACE.SetState(SellLock_SHORTCUT);
				SellWeapons_INTERFACE.SetState(SellWeapons_SHORTCUT);

				Main.OnPreDraw += DrawStarGoopTarget;

				primitives = new PrimTrailManager();
				primitives.LoadContent(Main.graphics.GraphicsDevice);

				InitStargoop();
				Boids.LoadContent();
				AdditiveCallManager.Load();

				RhythmMinigame.LoadStatic();
				GuitarMinigame.LoadStatic();
			}
			// LoadDetours();

			// using a mildly specific name to avoid mod clashes
			ChatManager.Register<UI.Chat.QuestTagHandler>(new string[] { "sq", "spiritQuest" });
		}

		public override void PostAddRecipes()
		{
			ExplosivesCache.Initialize(this);
			ModContent.GetInstance<IndestructibleGlobalTile>().Load(this);

			//WikiWriter.WriteWiki();
		}

		public void CheckScreenSize()
		{
			if (!Main.dedServ)
			{
				if (_lastScreenSize != new Vector2(Main.screenWidth, Main.screenHeight) && primitives != null)
					primitives.InitializeTargets(Main.graphics.GraphicsDevice);

				if (_lastViewSize != Main.ViewSize && Metaballs != null)
					Metaballs.Initialize(Main.graphics.GraphicsDevice);

				if ((_lastViewPort.Bounds != Main.graphics.GraphicsDevice.Viewport.Bounds || _lastScreenSize != new Vector2(Main.screenWidth, Main.screenHeight) || _lastViewSize != Main.ViewSize)
					&& basicEffect != null && primitives != null)
				{
					Helpers.SetBasicEffectMatrices(ref basicEffect, Main.GameViewMatrix.Zoom);
					Helpers.SetBasicEffectMatrices(ref primitives.pixelEffect, Main.GameViewMatrix.Zoom);
					Helpers.SetBasicEffectMatrices(ref primitives.galaxyEffect, new Vector2(1));
				}

				_lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
				_lastViewSize = Main.ViewSize;
				_lastViewPort = Main.graphics.GraphicsDevice.Viewport;
			}
		}

		/// <summary>
		/// Finds additional textures attached to things
		/// Puts the textures in _textures array
		/// </summary>
		private void LoadReferences()
		{
			foreach (Type type in Code.GetTypes())
			{
				if (type.IsAbstract)
					continue;

				bool modType = true;

				var types = new[]{ typeof(ModItem), typeof(ModNPC), typeof(ModProjectile), typeof(ModDust), typeof(ModTile), typeof(ModWall), typeof(ModBuff), typeof(ModMount) };
				if (types.Any(x => type.IsSubclassOf(x)))
				{
				}
				else
					modType = false;

				if (Main.dedServ || !modType)
					continue;

				FieldInfo _texField = type.GetField("_textures");
				if (_texField == null || !_texField.IsStatic || _texField.FieldType != typeof(Texture2D[]))
					continue;

				string path = type.FullName.Substring(10).Replace('.', '/'); //Substring(10) removes "SpiritMod."
				int texCount = 0;

				while (TextureExists(path + "_" + (texCount + 1)))
					texCount++;

				Texture2D[] textures = new Texture2D[texCount + 1];

				if (TextureExists(path))
					textures[0] = ModContent.Request<Texture2D>(path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				for (int i = 1; i <= texCount; i++)
					textures[i] = ModContent.Request<Texture2D>(path + "_" + i, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				_texField.SetValue(null, textures);
			}
		}

		public static void InitStargoop()
		{
			Metaballs = new StargoopManager();
			Metaballs.LoadContent();
			Metaballs.Initialize(Main.graphics.GraphicsDevice);

			var friendlyDust = (FriendlyStargoopDust)ModContent.GetModDust(ModContent.DustType<FriendlyStargoopDust>());
			var enemyDust = (EnemyStargoopDust)ModContent.GetModDust(ModContent.DustType<EnemyStargoopDust>());

			friendlyDust.Reset();
			enemyDust.Reset();
		}

		public override void Unload()
		{
			BoonLoader.Unload();
			nighttimeAmbience = null;
			underwaterAmbience = null;
			wavesAmbience = null;
			desertWind = null;
			caveAmbience = null;
			spookyAmbience = null;
			lightWind = null;
			scarabWings = null;
			spiritRNG = null;
			auroraEffect = null;
			StarjinxNoise = null;
			CircleNoise = null;
			StarfirePrims = null;
			ScreamingSkullTrail = null;
			RipperSlugShader = null;
			EyeballShader = null;
			RepeatingTextureShader = null;
			PrimitiveTextureMap = null;
			ArcLashShader = null;
			ConicalNoise = null;
			JemShaders = null;
			ThyrsusShader = null;
			JetbrickTrailShader = null;
			OutlinePrimShader = null;
			SunOrbShader = null;
			noise = null;

			AutoSellUI_INTERFACE = null;
			SellNoValue_INTERFACE = null;
			SellWeapons_INTERFACE = null;
			SellLock_INTERFACE = null;

			SpiritModAutoSellTextures.Unload();

			QuestManager.Unload();
			QuestBookUIState = null;
			QuestHUD = null;
			QuestBookHotkey = null;
			QuestHUDHotkey = null;
			EventManager.Unload();
			//Coverings.Unload();
			//Coverings = null;

			SpiritMultiplayer.Unload();
			AdditiveCallManager.Unload();
			SpiritGlowmask.Unload();
			StructureLoader.Unload();
			ParticleHandler.Unload();
			AutoloadMinionDictionary.Unload();
			Mechanics.BackgroundSystem.BackgroundItemManager.Unload();

			if (Boids != null)
				Boids.UnloadContent();

			glitchEffect = null;
			glitchScreenShader = null;
			TrailManager = null;
			GlobalNoise = null;
			Items.Glyphs.GlyphBase.UninitGlyphLookup();
			primitives = null;
			Metaballs = null;
			ShaderDict = new Dictionary<string, Effect>();
			SpiritDetours.Unload();

			PortraitManager.Unload(); //Idk if this is necessary but it seems like a good move - Gabe
									  //UnloadDetours();

			// remove any custom chat tag handlers
			var handlerDict = (ConcurrentDictionary<string, ITagHandler>)typeof(ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
			handlerDict.TryRemove("spiritQuest", out var ignore);
		}

		private void DrawStarGoopTarget(GameTime obj)
		{
			if (!Main.gameMenu && Metaballs != null && Main.graphics.GraphicsDevice != null && Main.spriteBatch != null)
				Metaballs.DrawToTarget(Main.spriteBatch, Main.graphics.GraphicsDevice);
		}

		public static Color StarjinxColor(float Timer)
		{
			float timer = Timer % 10;
			var yellow = new Color(238, 207, 91);
			var orange = new Color(255, 131, 91);
			var pink = new Color(230, 55, 166);

			if (timer < 3)
				return Color.Lerp(yellow, orange, timer / 3);
			else if (timer < 6)
				return Color.Lerp(orange, pink, (timer - 3) / 3);
			else
				return Color.Lerp(pink, yellow, (timer - 6) / 3);
		}

		internal static string GetWeatherRadioText(string key)
		{
			if (MyWorld.ashRain) return "Ashfall";
			else if (MyWorld.aurora) return "Aurora";
			else if (MyWorld.BlueMoon) return "Mystic Moon";
			else if (MyWorld.jellySky) return "Jelly Deluge";
			else if (MyWorld.luminousOcean) return "Luminous Seas";
			else if (MyWorld.calmNight) return "Calm Might";
			else if (MyWorld.rareStarfallEvent) return "Starfall";

			return LanguageManager.Instance.GetText(key).Value;
		}

		public override void MidUpdateProjectileItem()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				TrailManager.UpdateTrails();
				primitives.UpdateTrails();
			}
		}

		public override void AddRecipeGroups()
		{
			RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
			woodGrp.ValidItems.Add(ModContent.ItemType<AncientBark>());
			woodGrp.ValidItems.Add(ModContent.ItemType<Items.Placeable.Tiles.SpiritWoodItem>());
			woodGrp.ValidItems.Add(ModContent.ItemType<Items.Sets.FloatingItems.Driftwood.DriftwoodTileItem>());

			RecipeGroup butterflyGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Butterflies"]];
			butterflyGrp.ValidItems.Add(ModContent.ItemType<BriarmothItem>());

			RecipeGroup BaseGroup(object GroupName, int[] Items)
			{
				string Name = "";
				switch (GroupName)
				{
					case int i: //modcontent items
						Name += Lang.GetItemNameValue((int)GroupName);
						break;
					case short s: //vanilla item ids
						Name += Lang.GetItemNameValue((short)GroupName);
						break;
					default: //custom group names
						Name += GroupName.ToString();
						break;
				}

				return new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name, Items);
			}

			RecipeGroup.RegisterGroup("SpiritMod:GoldBars", BaseGroup(ItemID.GoldBar, new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			}));

			RecipeGroup.RegisterGroup("SpiritMod:EvilMaterial", BaseGroup(ItemID.CursedFlame, new int[]
			{
				ItemID.CursedFlame,
				ItemID.Ichor
			}));

			RecipeGroup.RegisterGroup("SpiritMod:PHMEvilMaterial", BaseGroup(ItemID.ShadowScale, new int[]
			{
				ItemID.ShadowScale,
				ItemID.TissueSample
			}));

			RecipeGroup.RegisterGroup("SpiritMod:Tier3HMBar", BaseGroup(ItemID.AdamantiteBar, new int[]
			{
				ItemID.AdamantiteBar,
				ItemID.TitaniumBar
			}));

			RecipeGroup.RegisterGroup("SpiritMod:SilverBars", BaseGroup(ItemID.SilverBar, new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			}));

			RecipeGroup.RegisterGroup("SpiritMod:CopperBars", BaseGroup(ItemID.CopperBar, new int[]
			{
				ItemID.CopperBar,
				ItemID.TinBar
			}));

			RecipeGroup.RegisterGroup("SpiritMod:EmeraldBows", BaseGroup("Emerald or Ruby Bow", new int[]
			{
				ModContent.ItemType<Emerald_Bow>(),
				ModContent.ItemType<Ruby_Bow>()
			}));

			RecipeGroup.RegisterGroup("SpiritMod:TopazBows", BaseGroup("Sapphire or Topaz Bow", new int[]
			{
				ModContent.ItemType<Sapphire_Bow>(),
				ModContent.ItemType<Topaz_Bow>()
			}));

			RecipeGroup.RegisterGroup("SpiritMod:AmethystStaffs", BaseGroup("Amethyst or Topaz Staff", new int[]
			{
				ItemID.AmethystStaff,
				ItemID.TopazStaff
			}));

			RecipeGroup.RegisterGroup("SpiritMod:SapphireStaffs", BaseGroup("Sapphire or Emerald Staff", new int[]
			{
				ItemID.SapphireStaff,
				ItemID.EmeraldStaff
			}));

			RecipeGroup.RegisterGroup("SpiritMod:RubyStaffs", BaseGroup("Ruby or Diamond Staff", new int[]
			{
				ItemID.RubyStaff,
				ItemID.DiamondStaff
			}));
		}

		public override void PostUpdateInput()
		{
			nighttimeAmbience?.Update();
			underwaterAmbience?.Update();
			wavesAmbience?.Update();
			lightWind?.Update();
			desertWind?.Update();
			caveAmbience?.Update();
			spookyAmbience?.Update();
			scarabWings?.Update();
		}

		public override void PostUpdateEverything()
		{
			if (!Main.dedServ)
			{
				ParticleHandler.RunRandomSpawnAttempts();
				ParticleHandler.UpdateAllParticles();
			}
		}

		public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				nighttimeAmbience = new SoundLooper(this, "Sounds/NighttimeAmbience");
				underwaterAmbience = new SoundLooper(this, "Sounds/UnderwaterAmbience");
				wavesAmbience = new SoundLooper(this, "Sounds/WavesAmbience");
				lightWind = new SoundLooper(this, "Sounds/LightWind");
				desertWind = new SoundLooper(this, "Sounds/DesertWind");
				caveAmbience = new SoundLooper(this, "Sounds/CaveAmbience");
				spookyAmbience = new SoundLooper(this, "Sounds/SpookyAmbience");
				scarabWings = new SoundLooper(this, "Sounds/BossSFX/Scarab_Wings");
			}

			Items.Glyphs.GlyphBase.InitializeGlyphLookup();

			BossChecklistDataHandler.RegisterSpiritData(this);

			CrossModContent();

			FinishedContentSetup = true;
		}

		private void CrossModContent()
		{
			Mod census = ModLoader.GetMod("Census");
			if (census != null)
			{
				census.Call("TownNPCCondition", ModContent.NPCType<Adventurer>(), "Rescue the Adventurer from the Briar after completing your first quest.");
				census.Call("TownNPCCondition", ModContent.NPCType<Gambler>(), "Rescue the Gambler from a Goblin Tower\nIf your world does not have a Goblin Tower, have at least 1 Gold in your inventory");
				census.Call("TownNPCCondition", ModContent.NPCType<Rogue>(), "Rescue the Bandit from the Bandit Hideout\nIf your world does not have a Goblin Tower, have at least 1 Gold in your inventory");
				census.Call("TownNPCCondition", ModContent.NPCType<RuneWizard>(), "Have a Blank Glyph in your inventory");
			}

			Mod fargos = ModLoader.GetMod("Fargowiltas");
			if (fargos != null)
			{
				// AddSummon, order or value in terms of vanilla bosses, your mod internal name, summon   
				//item internal name, inline method for retrieving downed value, price to sell for in copper
				fargos.Call("AddSummon", 1.4f, "SpiritMod", "ScarabIdol", (Func<bool>)(() => MyWorld.downedScarabeus), 100 * 200);
				fargos.Call("AddSummon", 4.2f, "SpiritMod", "JewelCrown", (Func<bool>)(() => MyWorld.downedAncientFlier), 100 * 200);
				fargos.Call("AddSummon", 5.9f, "SpiritMod", "StarWormSummon", (Func<bool>)(() => MyWorld.downedRaider), 100 * 400);
				fargos.Call("AddSummon", 6.5f, "SpiritMod", "CursedCloth", (Func<bool>)(() => MyWorld.downedInfernon), 100 * 500);
				fargos.Call("AddSummon", 7.3f, "SpiritMod", "DuskCrown", (Func<bool>)(() => MyWorld.downedDusking), 100 * 500);
				fargos.Call("AddSummon", 12.4f, "SpiritMod", "StoneSkin", (Func<bool>)(() => MyWorld.downedAtlas), 100 * 800);
			}
		}

		private bool _questBookHover;
		private bool _questBookToggle = false;

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1)
			{
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"SpiritMod: BookUI",
					delegate
					{
						QuestHUD.Draw(Main.spriteBatch);

						if (Main.playerInventory && QuestManager.QuestBookUnlocked)
						{
							Texture2D bookTexture = ModContent.Request<Texture2D>("UI/QuestUI/Textures/QuestBookInventoryButton", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
							Vector2 bookSize = new Vector2(50, 52);
							QuestUtils.QuestInvLocation loc = ModContent.GetInstance<SpiritClientConfig>().QuestBookLocation;
							Vector2 position = Vector2.Zero;
							switch (loc)
							{
								case QuestUtils.QuestInvLocation.Minimap:
									position = new Vector2(Main.screenWidth - Main.miniMapWidth - bookSize.X * 2.3f, Main.miniMapY + 4);

									if (Main.screenWidth < 900)
										position.Y -= 60;
									break;
								case QuestUtils.QuestInvLocation.Trashcan:
									position = new Vector2(388, 258);
									break;
								case QuestUtils.QuestInvLocation.FarLeft:
									position = new Vector2(20, 258);
									break;
							}

							Rectangle frame = new Rectangle(0, 0, 49, 52);
							bool hover = false;

							if (Main.MouseScreen.Between(position, position + bookSize))
							{
								hover = true;
								frame.X = 50;
								Main.LocalPlayer.mouseInterface = true;
								if (Main.mouseLeft && Main.mouseLeftRelease)
								{
									Main.mouseLeftRelease = false;
									QuestManager.SetBookState(_questBookToggle = !_questBookToggle);
								}
							}

							if (hover != _questBookHover)
							{
								_questBookHover = hover;
								SoundEngine.PlaySound(SoundID.MenuTick);
							}

							Main.spriteBatch.Draw(bookTexture, position, frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}

						BookUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);

				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"SpiritMod: SlotUI",
					delegate
					{
						SlotUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);

				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"SpiritMod: SellUI",
					delegate
					{
						DrawUpdateToggles();
						if (AutoSellUI.visible)
						{
							AutoSellUI_INTERFACE.Update(Main._drawInterfaceGameTime);
							AutoSellUI_SHORTCUT.Draw(Main.spriteBatch);
						}
						if (Mechanics.AutoSell.Sell_NoValue.Sell_NoValue.visible)
						{
							SellNoValue_INTERFACE.Update(Main._drawInterfaceGameTime);
							SellNoValue_SHORTCUT.Draw(Main.spriteBatch);
						}
						if (Mechanics.AutoSell.Sell_Lock.Sell_Lock.visible)
						{
							SellLock_INTERFACE.Update(Main._drawInterfaceGameTime);
							SellLock_SHORTCUT.Draw(Main.spriteBatch);
						}
						if (Mechanics.AutoSell.Sell_Weapons.Sell_Weapons.visible)
						{
							SellWeapons_INTERFACE.Update(Main._drawInterfaceGameTime);
							SellWeapons_SHORTCUT.Draw(Main.spriteBatch);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);

				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer("SpiritMod: Starjinx UI", delegate 
				{
					StarjinxUI.DrawStarjinxEventUI(Main.spriteBatch);

					return true; 
				}, InterfaceScaleType.UI));
			}

			if (TideWorld.TheTide)
			{
				int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
				LegacyGameInterfaceLayer NewLayer = new LegacyGameInterfaceLayer("SpiritMod: Tide UI",
					delegate
					{
						DrawEventUI(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI);
				layers.Insert(index, NewLayer);
			}

			int mouseIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));
			if (mouseIndex != -1)
			{
				layers.Insert(mouseIndex, new LegacyGameInterfaceLayer(
					"Spirit: Stag Hover",
					delegate
					{
						Item item = Main.mouseItem.IsAir ? Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] : Main.mouseItem;
						AuroraStag auroraStag = Main.LocalPlayer.GetModPlayer<MyPlayer>().hoveredStag;

						if (item.type == ModContent.ItemType<Items.Consumable.Food.IceBerries>() && auroraStag != null && !auroraStag.NPC.immortal && auroraStag.TameAnimationTimer == 0)
						{
							Texture2D itemTexture = TextureAssets.Item[item.type].Value;
							Vector2 itemPos = Main.MouseScreen + Vector2.UnitX * -(itemTexture.Width / 2 + 4);
							Vector2 origin = new Vector2(itemTexture.Width / 2, 0);
							Main.spriteBatch.Draw(itemTexture, itemPos, null, Color.White, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 0.2f, origin, 1f, SpriteEffects.None, 0f);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		public override void HotKeyPressed(string name)
		{
			if (name == "Concentration_Hotkey")
			{
				MyPlayer mp = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
				if (mp.leatherSet && !mp.concentrated && mp.concentratedCooldown <= 0)
					mp.concentrated = true;
			}
		}

		public void InvokeModifyTransform(SpriteViewMatrix matrix) => OnModifyTransformMatrix.Invoke(matrix);

		public static float tremorTime;
		public int screenshakeTimer = 0;

		internal void DrawUpdateToggles()
		{
			Point mousePoint = new Point(Main.mouseX, Main.mouseY);

			Rectangle AutoSellUI_TOGGLERECTANGLE = new Rectangle(494, 312, 39, 39);
			bool AutoSellUI_TOGGLE = AutoSellUI_TOGGLERECTANGLE.Contains(mousePoint);

			if (AutoSellUI_TOGGLE && Main.playerInventory && Main.npcShop > 0)
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.HoverItem = new Item();
				Main.hoverItemName = "Click to quick-sell your items";
			}

			Rectangle Sell_NoValue_TOGGLERECTANGLE = new Rectangle(502, 432, 32, 32);
			bool Sell_NoValue_TOGGLE = Sell_NoValue_TOGGLERECTANGLE.Contains(mousePoint);

			if (Sell_NoValue_TOGGLE && Main.playerInventory && Main.npcShop > 0)
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.HoverItem = new Item();
				Main.hoverItemName = "Toggle this to sell 'no value' items with quick-sell";
			}

			Rectangle Sell_Lock_TOGGLERECTANGLE = new Rectangle(502, 356, 32, 32);
			bool Sell_Lock_TOGGLE = Sell_Lock_TOGGLERECTANGLE.Contains(mousePoint);

			if (Sell_Lock_TOGGLE && Main.playerInventory && Main.npcShop > 0)
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.HoverItem = new Item();
				Main.hoverItemName = "Toggle this to lock quick-sell mechanic\nYou won't be able to use quick-sell while this is toggled";
			}

			Rectangle Sell_Weapons_TOGGLERECTANGLE = new Rectangle(502, 394, 32, 32);
			bool Sell_Weapons_TOGGLE = Sell_Weapons_TOGGLERECTANGLE.Contains(mousePoint);

			if (Sell_Weapons_TOGGLE && Main.playerInventory && Main.npcShop > 0)
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.HoverItem = new Item();
				Main.hoverItemName = "Toggle this to disable the selling of weapons";
			}
		}

		public void DrawEventUI(SpriteBatch spriteBatch)
		{
			if (TideWorld.TheTide && Main.LocalPlayer.ZoneBeach)
			{
				const float Scale = 0.875f;
				const float Alpha = 0.5f;
				const int InternalOffset = 6;
				const int OffsetX = 20;
				const int OffsetY = 20;

				Texture2D EventIcon = ModContent.Request<Texture2D>("Textures/InvasionIcons/Depths_Icon", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				Color descColor = new Color(77, 39, 135);
				Color waveColor = new Color(255, 241, 51);

				int width = (int)(200f * Scale);
				int height = (int)(46f * Scale);

				Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 23f), new Vector2(width, height));
				Utils.DrawInvBG(spriteBatch, waveBackground, new Color(63, 65, 151, 255) * 0.785f);

				string waveText = "Wave " + TideWorld.TideWave + " : " + TideWorld.TidePoints + "%";
				Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.Center.X, waveBackground.Y + 5), Color.White, Scale, 0.5f, -0.1f);
				Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.Center.X, waveBackground.Y + waveBackground.Height * 0.75f), TextureAssets.ColorBar.Size());

				var waveProgressAmount = new Rectangle(0, 0, (int)(TextureAssets.ColorBar.Width() * 0.01f * MathHelper.Clamp(TideWorld.TidePoints, 0f, 100f)), TextureAssets.ColorBar.Height());
				var offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * Scale)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * Scale)) * 0.5f);
				spriteBatch.Draw(TextureAssets.ColorBar.Value, waveProgressBar.Location.ToVector2() + offset, null, Color.White * Alpha, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureAssets.ColorBar.Value, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);

				Vector2 descSize = new Vector2(154, 40) * Scale;
				Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 19f), new Vector2(width, height));
				Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), descSize * 0.9f);
				Utils.DrawInvBG(spriteBatch, descBackground, descColor * Alpha);

				int descOffset = (descBackground.Height - (int)(32f * Scale)) / 2;
				var icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * Scale), (int)(32 * Scale));
				spriteBatch.Draw(EventIcon, icon, Color.White); 
				Utils.DrawBorderString(spriteBatch, "The Tide", new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), Color.White, 0.8f, 0.3f, 0.4f);
			}
		}


		#region pin stuff
		public override void PostDrawFullscreenMap(ref string mouseText)
		{
			var pins = ModContent.GetInstance<PinWorld>().pins;
			foreach (var pair in pins)
			{
				var pos = pins.Get<Vector2>(pair.Key);
				// No, I don't know why it draws one tile to the right, but that's how it is
				DrawMirrorOnFullscreenMap((int)pos.X - 1, (int)pos.Y, true, ModContent.Request<Texture2D>($"Items/Pins/Textures/Pin{pair.Key}Map"));
			}
		}

		public void DrawMirrorOnFullscreenMap(int tileX, int tileY, bool isTarget, Texture2D tex)
		{
			float myScale = isTarget ? 0.25f : 0.125f;
			float uiScale = 5f;//Main.mapFullscreenScale;
			float scale = uiScale * myScale;

			int wldBaseX = ((tileX + 1) << 4) + 8;
			int wldBaseY = ((tileY + 1) << 4) + 8;
			var wldPos = new Vector2(wldBaseX, wldBaseY);

			var (ScreenPosition, IsOnScreen) = GetFullMapPositionAsScreenPosition(wldPos);

			if (IsOnScreen && tileX > 0 && tileY > 0)
			{
				Vector2 scrPos = ScreenPosition;
				Main.spriteBatch.Draw(
					texture: tex,
					position: scrPos,
					sourceRectangle: null,
					color: Color.White,
					rotation: 0f,
					origin: new Vector2(tex.Width / 2, tex.Height / 2),
					scale: scale,
					effects: SpriteEffects.None,
					layerDepth: 1f
				);
			}
		}

		public static (Vector2 ScreenPosition, bool IsOnScreen) GetFullMapPositionAsScreenPosition(Vector2 worldPosition) => GetFullMapPositionAsScreenPosition(new Rectangle((int)worldPosition.X, (int)worldPosition.Y, 0, 0));

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the fullscreen map.
		/// </summary>
		/// <param name="worldArea"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static Tuple<int, int> GetScreenSize()
		{
			int screenWid = (int)(Main.screenWidth / Main.GameZoomTarget);
			int screenHei = (int)(Main.screenHeight / Main.GameZoomTarget);

			return Tuple.Create(screenWid, screenHei);
		}

		public static (Vector2 ScreenPosition, bool IsOnScreen) GetFullMapPositionAsScreenPosition(Rectangle worldArea)
		{    //Main.mapFullscreen
			float mapScale = GetFullMapScale();
			var scrSize = GetScreenSize();

			//float offscrLitX = 10f * mapScale;
			//float offscrLitY = 10f * mapScale;

			float mapFullscrX = Main.mapFullscreenPos.X * mapScale;
			float mapFullscrY = Main.mapFullscreenPos.Y * mapScale;
			float mapX = -mapFullscrX + (Main.screenWidth / 2f);
			float mapY = -mapFullscrY + (Main.screenHeight / 2f);

			float originMidX = (worldArea.X / 16f) * mapScale;
			float originMidY = (worldArea.Y / 16f) * mapScale;

			originMidX += mapX;
			originMidY += mapY;

			var scrPos = new Vector2(originMidX, originMidY);
			bool isOnscreen = originMidX >= 0 &&
				originMidY >= 0 &&
				originMidX < scrSize.Item1 &&
				originMidY < scrSize.Item2;

			return (scrPos, isOnscreen);
		}

		public static float GetFullMapScale() => Main.mapFullscreenScale / Main.UIScale;
		#endregion
	}

	internal enum CallContext
	{
		Invalid = -1,
		Downed,
		GlyphGet,
		GlyphSet,
		AddQuest,
		UnlockQuest,
		GetQuestIsUnlocked,
		GetQuestIsActive,
		GetQuestIsCompleted,
		GetQuestRewardsGiven,
		Portrait,
		Limit
	}
}