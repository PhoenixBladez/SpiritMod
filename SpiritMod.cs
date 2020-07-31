using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects;

using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pins;

using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.NPCs.Boss.Overseer;
using SpiritMod.NPCs.Town;
using SpiritMod.NPCs.Tides;
using SpiritMod.Projectiles;
using SpiritMod.Skies;
using SpiritMod.Skies.Overlays;
using SpiritMod.Tide;
using SpiritMod.Utilities;
using SpiritMod.World;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Shaders;
using Terraria.GameContent.UI;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;

namespace SpiritMod
{
	class SpiritMod : Mod
	{
		public static SpiritMod Instance;
		public UnifiedRandom spiritRNG;
		public static AdventurerQuestHandler AdventurerQuests;
		public static Effect auroraEffect;
		public static TrailManager TrailManager;
		public static Effect glitchEffect;
		public static PerlinNoise GlobalNoise;
		public static GlitchScreenShader glitchScreenShader;
		public static Texture2D noise;
		//public static Texture2D MoonTexture;
		public const string EMPTY_TEXTURE = "SpiritMod/Empty";
		public static Texture2D EmptyTexture {
			get;
			private set;
		}
		//public static int customEvent;
		public static int GlyphCurrencyID;

		internal static SpiritMod instance;
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

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType id = (MessageType)reader.ReadByte();
			byte player;
			switch (id) {
				case MessageType.AuroraData:
					MyWorld.auroraType = reader.ReadInt32();
					break;
				case MessageType.ProjectileData:
					gProj.ReceiveProjectileData(reader, whoAmI);
					break;
				case MessageType.Dodge:
					player = reader.ReadByte();
					byte type = reader.ReadByte();
					if (Main.netMode == NetmodeID.Server) {
						ModPacket packet = GetPacket(MessageType.Dodge, 2);
						packet.Write(player);
						packet.Write(type);
						packet.Send(-1, whoAmI);
					}
					if (type == 1)
						Items.Glyphs.VeilGlyph.Block(Main.player[player]);
					else
						Logger.Error("Unknown message (2:" + type + ")");
					break;
				case MessageType.Dash:
					player = reader.ReadByte();
					DashType dash = (DashType)reader.ReadByte();
					sbyte dir = reader.ReadSByte();
					if (Main.netMode == NetmodeID.Server) {
						ModPacket packet = GetPacket(MessageType.Dash, 3);
						packet.Write(player);
						packet.Write((byte)dash);
						packet.Write(dir);
						packet.Send(-1, whoAmI);
					}
					Main.player[player].GetModPlayer<MyPlayer>().PerformDash(dash, dir, false);
					break;
				case MessageType.PlayerGlyph:
					player = reader.ReadByte();
					GlyphType glyph = (GlyphType)reader.ReadByte();
					if (Main.netMode == NetmodeID.Server) {
						ModPacket packet = GetPacket(MessageType.PlayerGlyph, 2);
						packet.Write(player);
						packet.Write((byte)glyph);
						packet.Send(-1, whoAmI);
					}
					if (player == Main.myPlayer)
						break;
					Main.player[player].GetModPlayer<MyPlayer>().glyph = glyph;
					break;
				case MessageType.AdventurerNewQuest:
				case MessageType.AdventurerQuestCompleted:
					AdventurerQuests.HandlePacket(id, reader);
					break;
				default:
					Logger.Error("Unknown message (" + id + ")");
					break;
			}
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			var config = ModContent.GetInstance<SpiritClientConfig>();

			if (Main.gameMenu)
				return;
			if (priority > MusicPriority.Event)
				return;
			Player player = Main.LocalPlayer;
			if (!player.active)
				return;
			MyPlayer spirit = player.GetModPlayer<MyPlayer>();
			if (TideWorld.TheTide && player.ZoneBeach) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/DepthInvasion");
				priority = MusicPriority.Event;
			}

			if (priority > MusicPriority.Environment)
				return;
			if (spirit.ZoneBlueMoon && !Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight)) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/BlueMoon");
				priority = MusicPriority.Environment;
			}

			if (priority > MusicPriority.BiomeHigh)
				return;
			if (spirit.ZoneReach && Main.dayTime && !player.ZoneRockLayerHeight) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Reach");
				priority = MusicPriority.BiomeHigh;
			}
			else if (spirit.ZoneReach) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/ReachNighttime");
				priority = MusicPriority.BiomeHigh;
			}
			if (config.AuroraMusic
				&& MyWorld.aurora
				&& player.ZoneSnow
				&& player.ZoneOverworldHeight
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !Main.dayTime) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/AuroraSnow");
				priority = MusicPriority.BiomeHigh;
			}
			if (config.BlizzardMusic
				&& player.ZoneSnow
				&& player.ZoneOverworldHeight
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& Main.raining) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Blizzard");
				priority = MusicPriority.BiomeHigh;
			}
			if (config.LuminousMusic
				&& player.ZoneBeach
				&& MyWorld.luminousOcean
				&& !Main.dayTime) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/OceanNighttime");
				priority = MusicPriority.BiomeHigh;
			}
			if (config.SnowNightMusic
				&& player.ZoneSnow
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !MyWorld.aurora
				&& !Main.raining
				&& !Main.bloodMoon) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/SnowNighttime");
				priority = MusicPriority.BiomeMedium;
			}
			if (config.DesertNightMusic
				&& player.ZoneDesert
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !player.ZoneBeach
				&& !Main.bloodMoon) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/DesertNighttime");
				priority = MusicPriority.BiomeHigh;
			}
			if (spirit.ZoneAsteroid) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Asteroids");
				priority = MusicPriority.Environment;
			}
			if (priority > MusicPriority.BiomeMedium)
				return;
			if (spirit.ZoneSpirit) {
				priority = MusicPriority.BiomeMedium;
				if (player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f) {
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer1");
				}
				if (player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f) {
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer2");
				}
				if (player.position.Y / 16 >= Main.maxTilesY - 330) {
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer3");
				}
				if (!player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f) {
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritOverworld");
				}
			}
			if (config.GraniteMusic
				&& spirit.ZoneGranite
				&& !player.ZoneOverworldHeight) {
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/GraniteBiome");
				priority = MusicPriority.BiomeMedium;
			}
            if (config.MarbleMusic
                && spirit.ZoneMarble
                && !player.ZoneOverworldHeight)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/MarbleBiome");
                priority = MusicPriority.BiomeMedium;
            }
        }

		public override object Call(params object[] args)
		{
			if (args.Length < 1) {
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
			if (context == CallContext.Invalid && !contextNum.HasValue) {
				var stack = new System.Diagnostics.StackTrace(true);
				Logger.Error("Call Error: Context invalid or null:\n" + stack.ToString());
				return null;
			}
			if (context <= CallContext.Invalid || context >= CallContext.Limit) {
				var stack = new System.Diagnostics.StackTrace(true);
				Logger.Error("Call Error: Context invalid:\n" + stack.ToString());
				return null;
			}
			try {
				if (context == CallContext.Downed)
					return BossDowned(args);
				if (context == CallContext.GlyphGet)
					return GetGlyph(args);
				if (context == CallContext.GlyphSet) {
					SetGlyph(args);
					return null;
				}
			}
			catch (Exception e) {
				Logger.Error("Call Error: " + e.Message + "\n" + e.StackTrace);
			}
			return null;
		}

		private static CallContext ParseCallName(string context)
		{
			if (context == null)
				return CallContext.Invalid;
			switch (context) {
				case "downed":
					return CallContext.Downed;
				case "getGlyph":
					return CallContext.GlyphGet;
				case "setGlyph":
					return CallContext.GlyphSet;
			}
			return CallContext.Invalid;
		}

		private static bool BossDowned(object[] args)
		{
			if (args.Length < 2)
				throw new ArgumentException("No boss name specified");
			string name = args[1] as string;
			switch (name) {
				case "Scarabeus": return MyWorld.downedScarabeus;
				case "Vinewrath Bane": return MyWorld.downedReachBoss;
				case "Ancient Avian": return MyWorld.downedAncientFlier;
				case "Starplate Raider": return MyWorld.downedRaider;
				case "Infernon": return MyWorld.downedInfernon;
				case "Dusking": return MyWorld.downedDusking;
				case "Atlas": return MyWorld.downedAtlas;
				case "Overseer": return MyWorld.downedOverseer;
			}
			throw new ArgumentException("Invalid boss name:" + name);
		}

		public override void ModifyLightingBrightness(ref float scale)
		{
			Player player = Main.LocalPlayer;
			MyPlayer spirit = player.GetModPlayer<MyPlayer>();
			if (spirit.ZoneReach && !Main.dayTime) {
				scale *= .89f;
			}
		}

		private static void SetGlyph(object[] args)
		{
			if (args.Length < 2)
				throw new ArgumentException("Missing argument: Item");
			else if (args.Length < 3)
				throw new ArgumentException("Missing argument: Glyph");
			Item item = args[1] as Item;
			if (item == null)
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
			Item item = args[1] as Item;
			if (item == null)
				throw new ArgumentException("First argument must be of type Item");
			return (int)item.GetGlobalItem<Items.GItem>().Glyph;
		}

		public override void Load()
		{
			//Always keep this call in the first line of Load!
			LoadReferences();
			AdventurerQuests = new AdventurerQuestHandler(this);
			StructureLoader.Load(this);

			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile;
			On.Terraria.Player.KeyDoubleTap += Player_KeyDoubleTap;
			//Additive drawing
			On.Terraria.Main.DrawDust += DrawAdditive;

			GlobalNoise = new PerlinNoise(Main.rand.Next());
			instance = this;
			if (Main.rand == null)
				Main.rand = new UnifiedRandom();
			//Don't add any code before this point,
			// unless you know what you're doing.
			Items.Halloween.CandyBag.Initialize();

			if (Main.netMode != NetmodeID.Server) {
				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect")); // The path to the compiled shader file.
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();
			}

			Filters.Scene["SpiritMod:ReachSky"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.05f, 0.05f, .05f).UseOpacity(0.5f), EffectPriority.High);

			Filters.Scene["SpiritMod:SpiritUG1"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.2f, 0.2f, .2f).UseOpacity(0.8f), EffectPriority.High);
			Filters.Scene["SpiritMod:SpiritUG2"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.45f, 0.45f, .45f).UseOpacity(0.9f), EffectPriority.High);


			Filters.Scene["SpiritMod:WindEffect"] = new Filter((new BlizzardShaderData("FilterBlizzardForeground")).UseColor(0.4f, 0.4f, 0.4f).UseSecondaryColor(0.2f, 0.2f, 0.2f).UseImage("Images/Misc/noise", 0, null).UseOpacity(0.149f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High);
			Filters.Scene["SpiritMod:WindEffect2"] = new Filter((new BlizzardShaderData("FilterBlizzardForeground")).UseColor(0.4f, 0.4f, 0.4f).UseSecondaryColor(0.2f, 0.2f, 0.2f).UseImage("Images/Misc/noise", 0, null).UseOpacity(0.549f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High);

			GlyphCurrencyID = CustomCurrencyManager.RegisterCurrency(new Currency(ModContent.ItemType<Items.Glyphs.Glyph>(), 999L));

			if (Main.netMode != NetmodeID.Server) {
				TrailManager = new TrailManager(this);
				AddEquipTexture(null, EquipType.Legs, "TalonGarb_Legs", "SpiritMod/Items/Armor/TalonGarb_Legs");
				EmptyTexture = GetTexture("Empty");
				auroraEffect = GetEffect("Effects/aurora");
				noise = GetTexture("Textures/noise");
				noise = GetTexture("Textures/BlueMoonTexture");

				glitchEffect = GetEffect("Effects/glitch");
				glitchScreenShader = new GlitchScreenShader(glitchEffect);
				Filters.Scene["SpiritMod:Glitch"] = new Filter(glitchScreenShader, EffectPriority.High);


				SkyManager.Instance["SpiritMod:AuroraSky"] = new AuroraSky();
				Filters.Scene["SpiritMod:AuroraSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
				Overlays.Scene["SpiritMod:AuroraSky"] = new AuroraOverlay();

				Filters.Scene["SpiritMod:BlueMoonSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.1f, 0.2f, 0.5f).UseOpacity(0.53f), EffectPriority.High);
				SkyManager.Instance["SpiritMod:BlueMoonSky"] = new BlueMoonSky();

				SkyManager.Instance["SpiritMod:MeteorSky"] = new MeteorSky();
				SkyManager.Instance["SpiritMod:AsteroidSky2"] = new MeteorBiomeSky2();
				Filters.Scene["SpiritMod:MeteorSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
				Filters.Scene["SpiritMod:AsteroidSky2"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:SpiritBiomeSky"] = new SpiritBiomeSky();
				Filters.Scene["SpiritMod:SpiritBiomeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:PurpleAlgaeSky"] = new PurpleAlgaeSky();
				Filters.Scene["SpiritMod:PurpleAlgaeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:GreenAlgaeSky"] = new GreenAlgaeSky();
				Filters.Scene["SpiritMod:GreenAlgaeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:BlueAlgaeSky"] = new BlueAlgaeSky();
				Filters.Scene["SpiritMod:BlueAlgaeSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

				SkyManager.Instance["SpiritMod:AshstormParticles"] = new AshstormSky();
				Filters.Scene["SpiritMod:AshstormParticles"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
				Overlays.Scene["SpiritMod:AshstormParticles"] = new AshstormOverlay(EffectPriority.VeryHigh);

				Filters.Scene["SpiritMod:Overseer"] = new Filter(new SeerScreenShaderData("FilterMiniTower").UseColor(0f, 0.3f, 1f).UseOpacity(0.75f), EffectPriority.VeryHigh);
				SkyManager.Instance["SpiritMod:Overseer"] = new SeerSky();

				Filters.Scene["SpiritMod:Atlas"] = new Filter(new AtlasScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0.6f), EffectPriority.VeryHigh);
				SkyManager.Instance["SpiritMod:Atlas"] = new AtlasSky();

                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Asteroids"), ItemType("AsteroidBox"), TileType("AsteroidBox"));
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Starplate"), ItemType("StarplateBox"), TileType("StarplateBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Scarabeus"), ItemType("ScarabBox"), TileType("ScarabBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/ReachBoss"), ItemType("VinewrathBox"), TileType("VinewrathBox"));
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/MarbleBiome"), ItemType("MarbleBox"), TileType("MarbleBox"));
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/GraniteBiome"), ItemType("GraniteBox"), TileType("GraniteBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Blizzard"), ItemType("BlizzardBox"), TileType("BlizzardBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/AuroraSnow"), ItemType("AuroraBox"), TileType("AuroraBox"));
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/BlueMoon"), ItemType("BlueMoonBox"), TileType("BlueMoonBox"));
            }
        }

		//Additive drawing stuff. Optimize this later?
		private void DrawAdditive(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);
			Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

			for (int k = 0; k < Main.maxProjectiles; k++) //projectiles
				if (Main.projectile[k].active && Main.projectile[k].modProjectile is IDrawAdditive) (Main.projectile[k].modProjectile as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			for (int k = 0; k < Main.maxNPCs; k++) //NPCs
				if (Main.npc[k].active && Main.npc[k].modNPC is IDrawAdditive) (Main.npc[k].modNPC as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			Main.spriteBatch.End();
		}

		/// <summary>
		/// Finds additional textures attached to things
		/// Puts the textures in _textures array
		/// </summary>
		private void LoadReferences()
		{
			foreach (Type type in Code.GetTypes()) {
				if (type.IsAbstract) {
					continue;
				}

				bool modType = true;

				if (type.IsSubclassOf(typeof(ModItem))) {
				}
				else if (type.IsSubclassOf(typeof(ModNPC))) {
				}
				else if (type.IsSubclassOf(typeof(ModProjectile))) {
				}
				else if (type.IsSubclassOf(typeof(ModDust))) {
				}
				else if (type.IsSubclassOf(typeof(ModTile))) {
				}
				else if (type.IsSubclassOf(typeof(ModWall))) {
				}
				else if (type.IsSubclassOf(typeof(ModBuff))) {
				}
				else if (type.IsSubclassOf(typeof(ModMountData))) {
				}
				else
					modType = false;

				if (Main.dedServ || !modType)
					continue;

				System.Reflection.FieldInfo _texField = type.GetField("_textures");
				if (_texField == null || !_texField.IsStatic || _texField.FieldType != typeof(Texture2D[]))
					continue;

				string path = type.FullName.Substring(10).Replace('.', '/'); //Substring(10) removes "SpiritMod."
				int texCount = 0;
				while (TextureExists(path + "_" + (texCount + 1))) {
					texCount++;
				}
				Texture2D[] textures = new Texture2D[texCount + 1];
				if (TextureExists(path))
					textures[0] = GetTexture(path);
				for (int i = 1; i <= texCount; i++) {
					textures[i] = GetTexture(path + "_" + i);
				}
				_texField.SetValue(null, textures);
			}
		}

		private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			TrailManager.DrawTrails(Main.spriteBatch);
			orig(self);
		}

		//ugh this name
		private int Projectile_NewProjectile(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
		{
			int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
			Projectile projectile = Main.projectile[index];

			if (Main.netMode != NetmodeID.Server) TrailManager.DoTrailCreation(projectile);

			return index;
		}

		private void Player_KeyDoubleTap(On.Terraria.Player.orig_KeyDoubleTap orig, Player self, int keyDir)
		{
			orig(self, keyDir);
			self.GetSpiritPlayer().DoubleTapEffects(keyDir);
		}

		public override void Unload()
		{
			spiritRNG = null;
			auroraEffect = null;
			noise = null;
			instance = null;
			SpiritGlowmask.Unload();
			StructureLoader.Unload();
			glitchEffect = null;
			glitchScreenShader = null;
			TrailManager = null;
			GlobalNoise = null;
		}

		public override void MidUpdateProjectileItem()
		{
			if (Main.netMode != NetmodeID.Server) {
				TrailManager.UpdateTrails();
			}
		}

		public override void AddRecipeGroups()
		{
			RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
			woodGrp.ValidItems.Add(ModContent.ItemType<AncientBark>());
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ItemID.GoldBar), new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup("SpiritMod:GoldBars", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ItemID.CursedFlame), new int[]
			{
				ItemID.CursedFlame,
				ItemID.Ichor
			});
			RecipeGroup.RegisterGroup("SpiritMod:EvilMaterial", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ModContent.ItemType<CursedPendant>()), new int[]
			{
				ModContent.ItemType<CursedPendant>(),
				ModContent.ItemType<IchorPendant>()
			});
			RecipeGroup.RegisterGroup("SpiritMod:EvilNecklace", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ItemID.ShadowScale), new int[]
			{
				ItemID.ShadowScale,
				ItemID.TissueSample
			});
			RecipeGroup.RegisterGroup("SpiritMod:EvilMaterial1", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ModContent.ItemType<CursedFire>()), new int[]
			{
				ModContent.ItemType<CursedFire>(),
				ModContent.ItemType<NightmareFuel>()
			});
			RecipeGroup.RegisterGroup("SpiritMod:ModEvil", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ItemID.SilverBar), new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup("SpiritMod:SilverBars", group);
		}
		public override void PostUpdateEverything()
		{
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I) && Terraria.GameInput.PlayerInput.MouseInfo.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) {
				Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
				if (!tile.active()) {
					tile.active(true);
					tile.type = (ushort)ModContent.TileType<Tiles.Ambient.Briar.BriarFoliage>();
					tile.frameX = (short)(Main.rand.NextBool() ? 108 : 126);
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
				}
			}

			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I) && Terraria.GameInput.PlayerInput.MouseInfo.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) {
				Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
				if (!tile.active()) {
					tile.active(true);
					tile.type = (ushort)ModContent.TileType<Tiles.Block.BriarGrass>();
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
				}
			}
			base.PostUpdateEverything();
		}
        public override void PostSetupContent()
        {
            Items.Glyphs.GlyphBase.InitializeGlyphLookup();
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call(
                   "AddMiniBoss",
                   .8f,
                   ModContent.NPCType<NPCs.Reach.ForestWraith>(),
                   this, // Mod
                   "Glade Wraith",
                   (Func<bool>)(() => MyWorld.downedGladeWraith),
                   null,
                   null,
                   new List<int> { ModContent.ItemType<HuskstalkStaff>(), ModContent.ItemType<AncientBark>(), ModContent.ItemType<SacredVine>() },
                   "Destroy a Bone Altar in the Underground Briar. The Glade Wraith also spawns naturally at nighttime after defeating the Eye of Cthulhu.",
                   null,
                   "SpiritMod/Textures/BossChecklist/GladeWraithTexture",
                   "SpiritMod/NPCs/Reach/ForestWraith_Head_Boss",
                   null);

                bossChecklist.Call(
                    "AddBoss",
                    1.4f,
                    ModContent.NPCType<NPCs.Boss.Scarabeus.Scarabeus>(),
                    this, // Mod
                    "Scarabeus",
                    (Func<bool>)(() => MyWorld.downedScarabeus),
                    ModContent.ItemType<ScarabIdol>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy1>(), ModContent.ItemType<Items.Armor.Masks.ScarabMask>(), ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>() },
                    new List<int> { ModContent.ItemType<Items.BossBags.BagOScarabs>(), ModContent.ItemType<Items.Material.Chitin>(), ModContent.ItemType<Items.Weapon.Bow.ScarabBow>(), ModContent.ItemType<Items.Weapon.Summon.OrnateStaff>(), ModContent.ItemType<Items.Weapon.Swung.ScarabSword>() },
                    "Use a [i: " + ModContent.ItemType<ScarabIdol>() + "] in the Desert during daytime",
                    null,
                    "SpiritMod/Textures/BossChecklist/ScarabeusTexture",
                    "SpiritMod/NPCs/Boss/Scarabeus/Scarabeus_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddMiniBoss",
                    2.3f,
                    ModContent.NPCType<NPCs.BloodMoon.Occultist>(),
                    this, // Mod
                    "Occultist",
                    (Func<bool>)(() => MyWorld.downedOccultist),
                    null,
                    null,
                    new List<int> { ModContent.ItemType<Handball>(), ModContent.ItemType<OccultistStaff>(), ModContent.ItemType<BloodFire>() },
                    "The Occultist spawns rarely during a Blood Moon after the Eye of Cthulhu has been defeated.",
                    null,
                    "SpiritMod/Textures/BossChecklist/OccultistTexture",
                    "SpiritMod/NPCs/BloodMoon/Occultist_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddMiniBoss",
                    2.8f,
                    ModContent.NPCType<NPCs.Mecromancer>(),
                    this, // Mod
                    "Mechromancer",
                    (Func<bool>)(() => MyWorld.downedMechromancer),
                    null,
                    new List<int> { ModContent.ItemType<CoiledMask>(), ModContent.ItemType<CoiledChestplate>(), ModContent.ItemType<CoiledLeggings>() },
                    new List<int> { ModContent.ItemType<KnocbackGun>(), ModContent.ItemType<TechDrive>(), ItemID.RocketBoots },
                    "The Mechromancer spawns rarely during a Goblin Army after the Eye of Cthulhu has been defeated.",
                    null,
                    "SpiritMod/Textures/BossChecklist/MechromancerTexture",
                    "SpiritMod/NPCs/Mecromancer_Head_Boss",

                    null);

                bossChecklist.Call(
                    "AddMiniBoss",
                    3.2f,
                    ModContent.NPCType<NPCs.Beholder>(),
                    this, // Mod
                    "Beholder",
                    (Func<bool>)(() => MyWorld.downedBeholder),
                    null,
                    null,
                    new List<int> { ModContent.ItemType<BeholderYoyo>(), ModContent.ItemType<MarbleChunk>() },
                    "The Beholder spawns rarely in Marble Caverns after the Eater of Worlds or Brain of Cthulhu has been defeated.",
                    null,
                    "SpiritMod/Textures/BossChecklist/BeholderTexture",
                    "SpiritMod/NPCs/Beholder_Head_Boss",
                    null);


                bossChecklist.Call(
                    "AddBoss",
                    3.5f,
                    ModContent.NPCType<NPCs.Boss.ReachBoss.ReachBoss>(),
                    this, // Mod
                    "Vinewrath Bane",
                    (Func<bool>)(() => MyWorld.downedReachBoss),
                    null,
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy5>(), ModContent.ItemType<Items.Armor.Masks.ReachMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.ReachBossBag>(), ModContent.ItemType<Items.Weapon.Magic.SunbeamStaff>(), ModContent.ItemType<Items.Weapon.Bow.ThornBow>(), ModContent.ItemType<Items.Weapon.Magic.ReachVineStaff>(), ModContent.ItemType<Items.Weapon.Swung.ReachBossSword>(), ModContent.ItemType<Items.Weapon.Thrown.ReachKnife>() },
                    "Right-click the Bloodblossom, a glowing flower found at the bottom of the Briar. The Vinewrath Bane can be fought at any time and any place in progression.",
                    null,
                    "SpiritMod/Textures/BossChecklist/ReachBossTexture",
                    "SpiritMod/NPCs/Boss/ReachBoss/ReachBoss/ReachBoss_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddBoss",
                    4.2f,
                    ModContent.NPCType<NPCs.Boss.AncientFlyer>(),
                    this, // Mod
                    "Ancient Avian",
                    (Func<bool>)(() => MyWorld.downedAncientFlier),
                    ModContent.ItemType<JewelCrown>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy2>(), ModContent.ItemType<Items.Armor.Masks.FlierMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.FlyerBag>(), ModContent.ItemType<TalonBlade>(), ModContent.ItemType<Talonginus>(), ModContent.ItemType<SoaringScapula>(), ModContent.ItemType<TalonPiercer>(), ModContent.ItemType<SkeletalonStaff>(), ModContent.ItemType<TalonHeaddress>(), ModContent.ItemType<TalonGarb>() },
                     "Use a [i: " + ModContent.ItemType<JewelCrown>() + "] in the sky biome at any time. Alternatively, smash a Giant Avian Egg, which is found on Avian Islands near the sky layer. The Ancient Avian can be fought at any time and any place in progression.",
                    null,
                    "SpiritMod/Textures/BossChecklist/AvianTexture",
                    "SpiritMod/NPCs/Boss/AncientFlyer_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddEvent",
                    5.6f,
                    new List<int> { ModContent.NPCType<Crocomount>(), ModContent.NPCType<KakamoraParachuter>(), ModContent.NPCType<KakamoraRunner>(), ModContent.NPCType<KakamoraShaman>(), ModContent.NPCType<KakamoraShielder>(), ModContent.NPCType<KakamoraShielderRare>(), ModContent.NPCType<LargeCrustecean>(), ModContent.NPCType<MangoJelly>(), ModContent.NPCType<Rylheian>(), ModContent.NPCType<SpearKakamora>(), ModContent.NPCType<SwordKakamora>() },
                    this, // Mod
                    "The Tide",
                    (Func<bool>)(() => MyWorld.downedTide),
                    ModContent.ItemType<BlackPearl>(),
                    null,
                    new List<int> { ModContent.ItemType<TribalScale>(), ModContent.ItemType<PumpBubbleGun>(), ModContent.ItemType<MagicConch>(), ModContent.ItemType<TikiJavelin>(), ModContent.ItemType<MangoJellyStaff>(), ModContent.ItemType<TomeOfRylien>(), ModContent.ItemType<TentacleChain>(), ModContent.ItemType<CoconutGun>() },
                     "Use a [i: " + ModContent.ItemType<BlackPearl>() + "] at the Ocean at any time.",
                    null,
                    "SpiritMod/Textures/BossChecklist/TideTexture",
                    "SpiritMod/Effects/InvasionIcons/Depths_Icon",
                    null);


                bossChecklist.Call(
                    "AddBoss",
                    5.9f,
                    ModContent.NPCType<NPCs.Boss.SteamRaider.SteamRaiderHead>(),
                    this, // Mod
                    "Starplate Voyager",
                    (Func<bool>)(() => MyWorld.downedRaider),
                    ModContent.ItemType<StarWormSummon>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy3>(), ModContent.ItemType<Items.Armor.Masks.StarplateMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.SteamRaiderBag>(), ModContent.ItemType<Items.Material.CosmiliteShard>() },
                     "Use a [i: " + ModContent.ItemType<StarWormSummon>() + "] at an Astralite Beacon, located in the Asteroids.",
                    null,
                    "SpiritMod/Textures/BossChecklist/StarplateTexture",
                    "SpiritMod/NPCs/Boss/SteamRaider/SteamRaiderHead_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddBoss",
                    6.5f,
                    ModContent.NPCType<NPCs.Boss.Infernon.Infernon>(),
                    this, // Mod
                    "Infernon",
                    (Func<bool>)(() => MyWorld.downedInfernon),
                    ModContent.ItemType<CursedCloth>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy4>(), ModContent.ItemType<Items.Armor.Masks.InfernonMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.InfernonBag>(), ModContent.ItemType<InfernalAppendage>(), ModContent.ItemType<Items.Weapon.Thrown.InfernalJavelin>(), ModContent.ItemType<InfernalSword>(), ModContent.ItemType<DiabolicHorn>(), ModContent.ItemType<SevenSins>(), ModContent.ItemType<InfernalStaff>(), ModContent.ItemType<EyeOfTheInferno>(), ModContent.ItemType<InfernalShield>() },
                     "Use a [i: " + ModContent.ItemType<StarWormSummon>() + "] in the Underworld at any time.",
                    null,
                    "SpiritMod/Textures/BossChecklist/InfernonTexture",
                    "SpiritMod/NPCs/Boss/Infernon/Infernon_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddBoss",
                    7.3f,
                    ModContent.NPCType<NPCs.Boss.Dusking.Dusking>(),
                    this, // Mod
                    "Dusking",
                    (Func<bool>)(() => MyWorld.downedDusking),
                    ModContent.ItemType<DuskCrown>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy6>(), ModContent.ItemType<Items.Armor.Masks.DuskingMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.DuskingBag>(), ModContent.ItemType<Items.Weapon.Thrown.CrystalShadow>(), ModContent.ItemType<ShadowflameSword>(), ModContent.ItemType<UmbraStaff>(), ModContent.ItemType<ShadowSphere>(), ModContent.ItemType<DuskCarbine>(), ModContent.ItemType<DuskStone>() },
                     "Use a [i: " + ModContent.ItemType<DuskCrown>() + "] anywhere at nighttime.",
                    null,
                    "SpiritMod/Textures/BossChecklist/DuskingTexture",
                    "SpiritMod/NPCs/Boss/Dusking/Dusking_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddBoss",
                    12.4f,
                    ModContent.NPCType<NPCs.Boss.Atlas.Atlas>(),
                    this, // Mod
                    "Atlas",
                    (Func<bool>)(() => MyWorld.downedAtlas),
                    ModContent.ItemType<StoneSkin>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy8>(), ModContent.ItemType<Items.Armor.Masks.AtlasMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.AtlasBag>(), ModContent.ItemType<KingRock>(), ModContent.ItemType<Mountain>(), ModContent.ItemType<TitanboundBulwark>(), ModContent.ItemType<CragboundStaff>(), ModContent.ItemType<QuakeFist>(), ModContent.ItemType<Earthshatter>(), ModContent.ItemType<ArcaneGeyser>() },
                     "Use a [i: " + ModContent.ItemType<StoneSkin>() + "] anywhere at any time.",
                    null,
                    "SpiritMod/Textures/BossChecklist/AtlasTexture",
                    "SpiritMod/NPCs/Boss/Atlas/Atlas_Head_Boss",
                    null);

                bossChecklist.Call(
                    "AddBoss",
                    14.2f,
                    ModContent.NPCType<NPCs.Boss.Overseer.Overseer>(),
                    this, // Mod
                    "Overseer",
                    (Func<bool>)(() => MyWorld.downedOverseer),
                    ModContent.ItemType<SpiritIdol>(),
                    new List<int> { ModContent.ItemType<Items.Boss.Trophy9>(), ModContent.ItemType<Items.Armor.Masks.OverseerMask>() /*, ModContent.ItemType<Items.Placeable.MusicBox.ScarabBox>()*/ },
                    new List<int> { ModContent.ItemType<Items.BossBags.OverseerBag>(), ModContent.ItemType<EternityEssence>(), ModContent.ItemType<Eternity>(), ModContent.ItemType<SoulExpulsor>(), ModContent.ItemType<EssenseTearer>(), ModContent.ItemType<AeonRipper>() },
                     "Use a [i: " + ModContent.ItemType<SpiritIdol>() + "] in the Spirit Biome during nighttime",
                    null,
                    "SpiritMod/Textures/BossChecklist/OverseerTexture",
                    "SpiritMod/NPCs/Boss/Overseer/Overseer_Head_Boss",
                    null);
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
                fargos.Call("AddSummon", 7.3f, "SpiritMod", "SpiritIdol", (Func<bool>)(() => MyWorld.downedOverseer), 100 * 1000);
            }
        }


		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			TidePlayer modPlayer1 = Main.player[Main.myPlayer].GetModPlayer<TidePlayer>();
			if (TideWorld.TheTide) {
				int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
				LegacyGameInterfaceLayer NewLayer = new LegacyGameInterfaceLayer("InstallFix2: NewLayer",
					delegate {
						DrawEventUi(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI);
				layers.Insert(index, NewLayer);
			}
		}

		public override void HotKeyPressed(string name)
		{
			if (name == "Concentration_Hotkey") {
				MyPlayer mp = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
				if (mp.leatherSet && !mp.concentrated && mp.concentratedCooldown <= 0) {
					mp.concentrated = true;
				}
			}
		}

		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		{
			Color white = Color.White;
			Color white2 = Color.White;
			if (MyWorld.SpiritTiles > 0) {
				float num255 = MyWorld.SpiritTiles / 160f;
				if (num255 > MyWorld.spiritLight) {
					MyWorld.spiritLight += 0.01f;
				}
				if (num255 < MyWorld.spiritLight) {
					MyWorld.spiritLight -= 0.01f;
				}
			}
			else {
				MyWorld.spiritLight -= 0.02f;
			}
			if (MyWorld.spiritLight < 0f) {
				MyWorld.spiritLight = 0f;
			}
			if (MyWorld.spiritLight > .9f) {
				MyWorld.spiritLight = .9f;
			}
			if (MyWorld.spiritLight > 0f) {
				float num161 = MyWorld.spiritLight;
				int num160 = Main.bgColor.R;
				int num159 = Main.bgColor.G;
				int num158 = Main.bgColor.B;
				num159 -= (int)(250f / 1.14f * num161 * (Main.bgColor.G / 255f));
				num160 -= (int)(250f / 1.14f * num161 * (Main.bgColor.R / 255f));
				num158 -= (int)(250f / 1.14f * num161 * (Main.bgColor.B / 255f));
				if (num159 < 15) {
					num159 = 15;
				}
				if (num160 < 15) {
					num160 = 15;
				}
				if (num158 < 15) {
					num158 = 15;
				}
				Main.bgColor.R = (byte)num160;
				Main.bgColor.G = (byte)num159;
				Main.bgColor.B = (byte)num158;
				num160 = white.R;
				num159 = white.G;
				num158 = white.B;
				num159 -= (int)(10f / 1.14f * num161 * (white.G / 255f));
				num160 -= (int)(30f / 1.14f * num161 * (white.R / 255f));
				num158 -= (int)(10f / 1.14f * num161 * (white.B / 255f));
				if (num160 < 15) {
					num160 = 15;
				}
				if (num159 < 15) {
					num159 = 15;
				}
				if (num158 < 15) {
					num158 = 15;
				}
				white.R = (byte)num160;
				white.G = (byte)num159;
				white.B = (byte)num158;
				num160 = white2.R;
				num159 = white2.G;
				num158 = white2.B;
				num159 -= (int)(140f / 1.14f * num161 * (white2.R / 255f));
				num160 -= (int)(170f / 1.14f * num161 * (white2.G / 255f));
				num158 -= (int)(190f / 1.14f * num161 * (white2.B / 255f));
				if (num160 < 15) {
					num160 = 15;
				}
				if (num159 < 15) {
					num159 = 15;
				}
				if (num158 < 15) {
					num158 = 15;
				}
				white2.R = (byte)num160;
				white2.G = (byte)num159;
				white2.B = (byte)num158;
			}

			if (MyWorld.AsteroidTiles > 0) {
				float num255 = MyWorld.AsteroidTiles / 160f;
				if (num255 > MyWorld.asteroidLight) {
					MyWorld.asteroidLight += 0.01f;
				}
				if (num255 < MyWorld.asteroidLight) {
					MyWorld.asteroidLight -= 0.01f;
				}
			}
			else {
				MyWorld.asteroidLight -= 0.02f;
			}
			if (MyWorld.asteroidLight < 0f) {
				MyWorld.asteroidLight = 0f;
			}
			if (MyWorld.asteroidLight > 1f) {
				MyWorld.asteroidLight = 1f;
			}
			if (MyWorld.asteroidLight > 0f) {
				float num161 = MyWorld.asteroidLight;
				int num160 = Main.bgColor.R;
				int num159 = Main.bgColor.G;
				int num158 = Main.bgColor.B;
				num159 -= (int)(250f * num161 * (Main.bgColor.G / 255f));
				num160 -= (int)(250f * num161 * (Main.bgColor.R / 255f));
				num158 -= (int)(250f * num161 * (Main.bgColor.B / 255f));
				if (num159 < 15) {
					num159 = 15;
				}
				if (num160 < 15) {
					num160 = 15;
				}
				if (num158 < 15) {
					num158 = 15;
				}
				Main.bgColor.R = (byte)num160;
				Main.bgColor.G = (byte)num159;
				Main.bgColor.B = (byte)num158;
				num160 = white.R;
				num159 = white.G;
				num158 = white.B;
				num159 -= (int)(10f * num161 * (white.G / 255f));
				num160 -= (int)(30f * num161 * (white.R / 255f));
				num158 -= (int)(10f * num161 * (white.B / 255f));
				if (num160 < 15) {
					num160 = 15;
				}
				if (num159 < 15) {
					num159 = 15;
				}
				if (num158 < 15) {
					num158 = 15;
				}
				white.R = (byte)num160;
				white.G = (byte)num159;
				white.B = (byte)num158;
				num160 = white2.R;
				num159 = white2.G;
				num158 = white2.B;
				num159 -= (int)(140f * num161 * (white2.R / 255f));
				num160 -= (int)(170f * num161 * (white2.G / 255f));
				num158 -= (int)(190f * num161 * (white2.B / 255f));
				if (num160 < 15) {
					num160 = 15;
				}
				if (num159 < 15) {
					num159 = 15;
				}
				if (num158 < 15) {
					num158 = 15;
				}
				white2.R = (byte)num160;
				white2.G = (byte)num159;
				white2.B = (byte)num158;
			}

			/*int num226 = 15;
			int num227 = 0;
			int num228 = 80;
			int num229 = 80;
			int num230 = 32;*/
		}


		/*const int ShakeLength = 5;
		int ShakeCount = 0;
		float previousRotation = 0;
		float targetRotation = 0;
		float previousOffsetX = 0;
		float previousOffsetY = 0;
		float targetOffsetX = 0;
		float targetOffsetY = 0;

		public static float shittyModTime;*/
		public static float tremorTime;
		public int screenshakeTimer = 0;
		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			if (!Main.gameMenu) {
				screenshakeTimer++;
				if (tremorTime >= 0 && screenshakeTimer >= 20) // so it doesnt immediately decrease
				{
					tremorTime -= 0.5f;
				}
				if (tremorTime < 0) {
					tremorTime = 0;
				}
				Main.screenPosition += new Vector2(tremorTime * Main.rand.NextFloat(), tremorTime * Main.rand.NextFloat()); //NextFloat creates a random value between 0 and 1, multiply screenshake amount for a bit of variety
			}
			else // dont shake on the menu
			{
				tremorTime = 0;
				screenshakeTimer = 0;
			}
		}

		public void DrawEventUi(SpriteBatch spriteBatch)
		{
			{
				//TidePlayer modPlayer1 = Main.player[Main.myPlayer].GetModPlayer<TidePlayer>();
				if (TideWorld.TheTide && Main.LocalPlayer.ZoneBeach) {

					float alpha = 0.5f;
					Texture2D backGround1 = Main.colorBarTexture;
					Texture2D progressColor = Main.colorBarTexture;
					Texture2D EventIcon = SpiritMod.instance.GetTexture("Effects/InvasionIcons/Depths_Icon");
					float scmp = 0.5f + 0.75f * 0.5f;
					Color descColor = new Color(77, 39, 135);
					Color waveColor = new Color(255, 241, 51);
					Color barrierColor = new Color(255, 241, 51);
					const int offsetX = 20;
					const int offsetY = 20;
					int width = (int)(200f * scmp);
					int height = (int)(46f * scmp);
					Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 23f), new Vector2(width, height));
					Utils.DrawInvBG(spriteBatch, waveBackground, new Color(63, 65, 151, 255) * 0.785f);
					string waveText = "Cleared " + TideWorld.TidePoints + "%";
					Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.X + waveBackground.Width / 2, waveBackground.Y + 5), Color.White, scmp * 0.8f, 0.5f, -0.1f);
					Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.X + waveBackground.Width * 0.5f, waveBackground.Y + waveBackground.Height * 0.75f), new Vector2(progressColor.Width, progressColor.Height));
					Rectangle waveProgressAmount = new Rectangle(0, 0, (int)(progressColor.Width * 0.01f * MathHelper.Clamp(TideWorld.TidePoints, 0f, 100f)), progressColor.Height);
					Vector2 offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * scmp)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * scmp)) * 0.5f);
					spriteBatch.Draw(backGround1, waveProgressBar.Location.ToVector2() + offset, null, Color.White * alpha, 0f, new Vector2(0f), scmp, SpriteEffects.None, 0f);
					spriteBatch.Draw(backGround1, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), scmp, SpriteEffects.None, 0f);
					const int internalOffset = 6;
					Vector2 descSize = new Vector2(154, 40) * scmp;
					Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 19f), new Vector2(width, height));
					Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), descSize * 0.9f);
					Utils.DrawInvBG(spriteBatch, descBackground, descColor * alpha);
					int descOffset = (descBackground.Height - (int)(32f * scmp)) / 2;
					Rectangle icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * scmp), (int)(32 * scmp));
					spriteBatch.Draw(EventIcon, icon, Color.White);
					Utils.DrawBorderString(spriteBatch, "The Tide", new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), Color.White, 0.8f, 0.3f, 0.4f);
				}
			}
		}
		#region pin stuff
		public override void PostDrawFullscreenMap(ref string mouseText)
		{
			var pins = ModContent.GetInstance<PinWorld>().pins;
			foreach (var pair in pins) {
				var pos = pins.Get<Vector2>(pair.Key);
				// No, I don't know why it draws one tile to the right, but that's how it is
				DrawMirrorOnFullscreenMap((int)pos.X - 1, (int)pos.Y, true, GetTexture($"Items/Pins/Textures/Pin{pair.Key}Map"));
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

			//var overMapData = HUDMapHelpers.GetFullMapPositionAsScreenPosition( wldPos );
			//
			//if( overMapData.IsOnScreen ) {
			//	Vector2 scrPos = overMapData.ScreenPosition;
			var overMapData = GetFullMapPositionAsScreenPosition(wldPos);

			if (overMapData.IsOnScreen && tileX > 0 && tileY > 0) {
				Vector2 scrPos = overMapData.ScreenPosition;
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
		public static (Vector2 ScreenPosition, bool IsOnScreen) GetFullMapPositionAsScreenPosition(Vector2 worldPosition)
		{    //Main.mapFullscreen
			return GetFullMapPositionAsScreenPosition(new Rectangle((int)worldPosition.X, (int)worldPosition.Y, 0, 0));
		}

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the fullscreen map.
		/// </summary>
		/// <param name="worldArea"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static Tuple<int, int> GetScreenSize()
		{
			int screenWid = (int)((float)Main.screenWidth / Main.GameZoomTarget);
			int screenHei = (int)((float)Main.screenHeight / Main.GameZoomTarget);

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
			float mapX = -mapFullscrX + (float)(Main.screenWidth / 2);
			float mapY = -mapFullscrY + (float)(Main.screenHeight / 2);

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
		public static float GetFullMapScale()
		{
			return Main.mapFullscreenScale / Main.UIScale;
		}
		#endregion
	}
	internal enum CallContext
	{
		Invalid = -1,
		Downed,
		GlyphGet,
		GlyphSet,
		Limit
	}
}