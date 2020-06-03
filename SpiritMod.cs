using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Shaders;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.GameContent;
using Terraria.Utilities;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Terraria.GameContent.UI;
using SpiritMod.NPCs.Boss.Overseer;
using SpiritMod.NPCs.Town;
using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.Tide;
using SpiritMod.Skies.Overlays;
using SpiritMod.Effects;
using SpiritMod.Skies;
using SpiritMod.Projectiles;
using SpiritMod.Utilities;
using Terraria.Graphics;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod
{
    class SpiritMod : Mod
    {
        public static SpiritMod Instance;
        public UnifiedRandom spiritRNG;
        public static AdventurerQuestHandler AdventurerQuests;
        public static Effect auroraEffect;
        public static TrailManager TrailManager;
        public static Texture2D noise;
        public static Texture2D MoonTexture;
        public const string EMPTY_TEXTURE = "SpiritMod/Empty";
        public const string customEventName = "The Tide";
        public static Texture2D EmptyTexture
        {
            get;
            private set;
        }
        public static int customEvent;
        public static ModHotKey SpecialKey;
        public static int GlyphCurrencyID;

        internal static SpiritMod instance;
        public SpiritMod()
        {
            Instance = this;
            spiritRNG = new UnifiedRandom();
        }

        public ModPacket GetPacket(MessageType type, int capacity)
        {
            ModPacket packet = base.GetPacket(capacity + 1);
            packet.Write((byte)type);
            return packet;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType id = (MessageType)reader.ReadByte();
            byte player;
            switch (id)
            {
                case MessageType.AuroraData:
                    MyWorld.auroraType = reader.ReadInt32();
                    break;
                case MessageType.ProjectileData:
                    gProj.ReceiveProjectileData(reader, whoAmI);
                    break;
                case MessageType.Dodge:
                    player = reader.ReadByte();
                    byte type = reader.ReadByte();
                    if (Main.netMode == 2)
                    {
                        ModPacket packet = GetPacket(MessageType.Dodge, 2);
                        packet.Write(player);
                        packet.Write(type);
                        packet.Send(-1, whoAmI);
                    }
                    if (type == 1)
                        Items.Glyphs.VeilGlyph.Block(Main.player[player]);
                    else
                        ErrorLogger.Log("SpiritMod: Unknown message (2:" + type + ")");
                    break;
                case MessageType.Dash:
                    player = reader.ReadByte();
                    DashType dash = (DashType)reader.ReadByte();
                    sbyte dir = reader.ReadSByte();
                    if (Main.netMode == 2)
                    {
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
                    if (Main.netMode == 2)
                    {
                        ModPacket packet = GetPacket(MessageType.PlayerGlyph, 2);
                        packet.Write(player);
                        packet.Write((byte)glyph);
                        packet.Send(-1, whoAmI);
                    }
                    if (player == Main.myPlayer)
                        break;
                    Main.player[player].GetModPlayer<MyPlayer>().glyph = glyph;
                    break;
                default:
                    ErrorLogger.Log("SpiritMod: Unknown message (" + id + ")");
                    break;
            }
        }


        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.gameMenu)
                return;
            if (priority > MusicPriority.Event)
                return;
            Player player = Main.LocalPlayer;
            if (!player.active)
                return;
            MyPlayer spirit = player.GetModPlayer<MyPlayer>();
            if (TideWorld.TheTide && TideWorld.InBeach)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/DepthInvasion");
                priority = MusicPriority.Event;
            }

            if (priority > MusicPriority.Environment)
                return;
            if (spirit.ZoneBlueMoon && !Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight))
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/BlueMoon");
                priority = MusicPriority.Environment;
            }

            if (priority > MusicPriority.BiomeHigh)
                return;
            if (spirit.ZoneReach && Main.dayTime)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Reach");
                priority = MusicPriority.BiomeHigh;
            }
            else if (spirit.ZoneReach && !Main.dayTime)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/ReachNighttime");
                priority = MusicPriority.BiomeHigh;
            }
            if (MyWorld.aurora && player.ZoneSnow && player.ZoneOverworldHeight && !Main.dayTime)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/AuroraSnow");
                priority = MusicPriority.BiomeHigh;
            }
            if (player.ZoneBeach && MyWorld.luminousOcean && !Main.dayTime)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/OceanNighttime");
                priority = MusicPriority.BiomeHigh;
            }
            if (player.ZoneSnow && player.ZoneOverworldHeight && !Main.dayTime && !player.ZoneCorrupt && !player.ZoneCrimson && !MyWorld.aurora)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/SnowNighttime");
                priority = MusicPriority.BiomeMedium;
            }
            if (player.ZoneDesert && player.ZoneOverworldHeight && !Main.dayTime && !player.ZoneCorrupt && !player.ZoneCrimson)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/DesertNighttime");
                priority = MusicPriority.BiomeHigh;
            }
            if (spirit.ZoneAsteroid)
            {
                music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/Asteroids");
                priority = MusicPriority.Environment;
            }
            if (priority > MusicPriority.BiomeMedium)
                return;
            if (spirit.ZoneSpirit)
            {
                priority = MusicPriority.BiomeMedium;
                if (player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer1");
                }
                if (player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer2");
                }
                if (player.position.Y / 16 >= Main.maxTilesY - 300)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritLayer3");
                }
                if (!player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/SpiritOverworld");
                }
            }
        }


        public override object Call(params object[] args)
        {
            if (args.Length < 1)
            {
                var stack = new System.Diagnostics.StackTrace(true);
                ErrorLogger.Log("Spirit Mod Call Error: No arguments given:\n" + stack.ToString());
                return null;
            }
            CallContext context;
            int? contextNum = args[0] as int?;
            if (contextNum.HasValue)
                context = (CallContext)contextNum.Value;
            else
                context = ParseCallName(args[0] as string);
            if (context == CallContext.Invalid && !contextNum.HasValue)
            {
                var stack = new System.Diagnostics.StackTrace(true);
                ErrorLogger.Log("Spirit Mod Call Error: Context invalid or null:\n" + stack.ToString());
                return null;
            }
            if (context <= CallContext.Invalid || context >= CallContext.Limit)
            {
                var stack = new System.Diagnostics.StackTrace(true);
                ErrorLogger.Log("Spirit Mod Call Error: Context invalid:\n" + stack.ToString());
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
            }
            catch (Exception e)
            {
                ErrorLogger.Log("Spirit Mod Call Error: " + e.Message + "\n" + e.StackTrace);
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
                case "Vinewrath Bane": return MyWorld.downedReachBoss;
                case "Ancient Avian": return MyWorld.downedAncientFlier;
                case "Starplate Raider": return MyWorld.downedRaider;
                case "Infernon": return MyWorld.downedInfernon;
                case "Dusking": return MyWorld.downedDusking;
                case "Ethereal Umbra": return MyWorld.downedSpiritCore;
                case "Atlas": return MyWorld.downedAtlas;
                case "Overseer": return MyWorld.downedOverseer;
            }
            throw new ArgumentException("Invalid boss name:" + name);
        }
        public override void ModifyLightingBrightness(ref float scale)
        {
            Player player = Main.LocalPlayer;
            MyPlayer spirit = player.GetModPlayer<MyPlayer>();
            if (spirit.ZoneReach && !Main.dayTime)
            {
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
            TrailManager = new TrailManager(this);

            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile;

            instance = this;
            if (Main.rand == null)
                Main.rand = new Terraria.Utilities.UnifiedRandom();
            //Don't add any code before this point,
            // unless you know what you're doing.
            Items.Halloween.CandyBag.Initialize();

			if (Main.netMode != NetmodeID.Server)
			{
				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect")); // The path to the compiled shader file.
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();
			}
           
            Filters.Scene["SpiritMod:ReachSky"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.05f, 0.05f, .05f).UseOpacity(0.7f), EffectPriority.High);

            Filters.Scene["SpiritMod:SpiritUG1"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.2f, 0.2f, .2f).UseOpacity(0.8f), EffectPriority.High);
            Filters.Scene["SpiritMod:SpiritUG2"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(0.45f, 0.45f, .45f).UseOpacity(0.9f), EffectPriority.High);


            Filters.Scene["SpiritMod:WindEffect"] = new Filter((new BlizzardShaderData("FilterBlizzardForeground")).UseColor(0.4f, 0.4f, 0.4f).UseSecondaryColor(0.2f, 0.2f, 0.2f).UseImage("Images/Misc/noise", 0, null).UseOpacity(0.249f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High);
            Filters.Scene["SpiritMod:WindEffect2"] = new Filter((new BlizzardShaderData("FilterBlizzardForeground")).UseColor(0.4f, 0.4f, 0.4f).UseSecondaryColor(0.2f, 0.2f, 0.2f).UseImage("Images/Misc/noise", 0, null).UseOpacity(0.549f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High);

            SpecialKey = RegisterHotKey("Armor Bonus", "Q");

            GlyphCurrencyID = CustomCurrencyManager.RegisterCurrency(new Currency(ModContent.ItemType<Items.Glyphs.Glyph>(), 999L));

            if (!Main.dedServ)
            {
                AddEquipTexture(null, EquipType.Legs, "TalonGarb_Legs", "SpiritMod/Items/Armor/TalonGarb_Legs");
                EmptyTexture = GetTexture("Empty");
                auroraEffect = GetEffect("Effects/aurora");
                noise = GetTexture("Textures/noise");
                noise = GetTexture("Textures/BlueMoonTexture");
                SkyManager.Instance["SpiritMod:AuroraSky"] = new AuroraSky();
                Filters.Scene["SpiritMod:AuroraSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
                Terraria.Graphics.Effects.Overlays.Scene["SpiritMod:AuroraSky"] = new AuroraOverlay();

                Filters.Scene["SpiritMod:BlueMoonSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.High);
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
                Terraria.Graphics.Effects.Overlays.Scene["SpiritMod:AshstormParticles"] = new AshstormOverlay(EffectPriority.VeryHigh);

                Filters.Scene["SpiritMod:Overseer"] = new Filter(new SeerScreenShaderData("FilterMiniTower").UseColor(0f, 0.3f, 1f).UseOpacity(0.75f), EffectPriority.VeryHigh);
                SkyManager.Instance["SpiritMod:Overseer"] = new SeerSky();

                Filters.Scene["SpiritMod:Atlas"] = new Filter(new AtlasScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0.6f), EffectPriority.VeryHigh);
                SkyManager.Instance["SpiritMod:Atlas"] = new AtlasSky();
            }
        }

        /// <summary>
        /// Scans all classes deriving from any Mod type
        /// for a field called _ref
        /// and populates it, if it exists.
        /// </summary>
        private void LoadReferences()
        {
            foreach (Type type in Code.GetTypes())
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                System.Reflection.FieldInfo _refField = type.GetField("_ref");
                System.Reflection.FieldInfo _typeField = type.GetField("_type");
                bool isDefR = _refField != null && _refField.IsStatic;
                bool isDefT = _typeField != null && _typeField.IsStatic && _typeField.FieldType == typeof(int);
                bool modType = true;

                if (type.IsSubclassOf(typeof(ModItem)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModItem))
                        _refField.SetValue(null, GetItem(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, ItemType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModNPC)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModNPC))
                        _refField.SetValue(null, GetNPC(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, NPCType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModProjectile)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModProjectile))
                        _refField.SetValue(null, GetProjectile(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, ProjectileType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModDust)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModDust))
                        _refField.SetValue(null, GetDust(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, DustType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModTile)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModTile))
                        _refField.SetValue(null, GetTile(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, TileType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModWall)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModWall))
                        _refField.SetValue(null, GetWall(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, WallType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModBuff)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModBuff))
                        _refField.SetValue(null, GetBuff(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, BuffType(type.Name));
                }
                else if (type.IsSubclassOf(typeof(ModMountData)))
                {
                    if (isDefR && _refField.FieldType == typeof(ModMountData))
                        _refField.SetValue(null, GetMount(type.Name));
                    if (isDefT)
                        _typeField.SetValue(null, MountType(type.Name));
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
                while (TextureExists(path + "_" + (texCount + 1)))
                {
                    texCount++;
                }
                Texture2D[] textures = new Texture2D[texCount + 1];
                if (TextureExists(path))
                    textures[0] = GetTexture(path);
                for (int i = 1; i <= texCount; i++)
                {
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

            TrailManager.DoTrailCreation(projectile);

            return index;
        }
        public override void Unload()
        {
            spiritRNG = null;
            auroraEffect = null;
            noise = null;
            instance = null;
            SpiritGlowmask.Unload();
            StructureLoader.Unload();
            TrailManager = null;
        }

        public override void MidUpdateProjectileItem()
        {
            TrailManager.UpdateTrails();
        }
        internal static int GetRainDustType(int rainType, out Color color)
        {
            color = Color.White;
            switch (rainType / 3)
            {
                default:
                case 0: //normal rain
                    return 154;
                case 1: //blood moon rain
                case 7: //blood rain
                    return 218;
                case 2: //healing rain
                    return Instance.DustType("ReachWaterSplash");
                case 3: //acid rain
                    return Instance.DustType("AcidRainDust");
                case 4: //crystal rain
                    return Instance.DustType("CrystalRainDust");
                case 5: //vile rain
                    return 98;
                case 6: //crimson rain
                    return 218;
            }
        }
        public override void AddRecipeGroups()
        {
            RecipeGroup group1 = new RecipeGroup(() => Lang.misc[37] + " Iron Bar" + Lang.GetItemNameValue(ItemType("Iron Bar")), new int[]
            {
                22,
                704
            });
            RecipeGroup.RegisterGroup("LeadBars", group1);

            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Gold Bar" + Lang.GetItemNameValue(ItemType("Gold Bar")), new int[]
            {
                19,
                706
            });
            RecipeGroup.RegisterGroup("GoldBars", group);
            group = new RecipeGroup(() => Lang.misc[37] + " Lunar Fragment" + Lang.GetItemNameValue(ItemType("Lunar Fragment")), new int[]
            {
                3456,
                3457,
                3458,
                3459
            });
            RecipeGroup.RegisterGroup("CelestialFragment", group);
            group = new RecipeGroup(() => Lang.misc[37] + " Cursed Flame" + Lang.GetItemNameValue(ItemType("Cursed Flame")), new int[]
            {
                ItemID.Ichor,
                ItemID.CursedFlame
            });
            RecipeGroup.RegisterGroup("EvilMaterial", group);
            group = new RecipeGroup(() => Lang.misc[37] + " Ichor Pendant" + Lang.GetItemNameValue(ItemType("Ichor Pendant")), new int[]
            {
                ItemType("CursedPendant"),
                ItemType("IchorPendant")
            });

            RecipeGroup.RegisterGroup("EvilNecklace", group);
            group = new RecipeGroup(() => Lang.misc[37] + " Shadow Scale" + Lang.GetItemNameValue(ItemType("Shadow Scale")), new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });

            RecipeGroup.RegisterGroup("EvilMaterial1", group);
            group = new RecipeGroup(() => Lang.misc[37] + " Nightmare Fuel" + Lang.GetItemNameValue(ItemType("Nightmare Fuel")), new int[]
            {
                ItemType("CursedFire"),
                ItemType("NightmareFuel")
            });

            RecipeGroup.RegisterGroup("ModEvil", group);
            group = new RecipeGroup(() => Lang.misc[37] + " Tungsten Bar" + Lang.GetItemNameValue(ItemType(" Tungsten Bar")), new int[]
            {
                705,
                21
            });

            RecipeGroup.RegisterGroup("SilverBars", group);
        }

        public override void PostSetupContent()
        {
            Items.Glyphs.GlyphBase.InitializeGlyphLookup();

            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                // 14 is moolord, 12 is duke fishron
                bossChecklist.Call("AddBossWithInfo", "Scarabeus", 0.8f, (Func<bool>)(() => MyWorld.downedScarabeus), "Use a [i:" + ItemType("ScarabIdol") + "] in the Desert biome at any time");
                bossChecklist.Call("AddBossWithInfo", "Vinewrath Bane", 3.5f, (Func<bool>)(() => MyWorld.downedReachBoss), "Use a [i:" + ItemType("ReachBossSummon") + "] in the Reach at daytime");
                bossChecklist.Call("AddBossWithInfo", "Ancient Avian", 4.2f, (Func<bool>)(() => MyWorld.downedAncientFlier), "Use a [i:" + ItemType("JewelCrown") + "] in the sky at any time");
                bossChecklist.Call("AddBossWithInfo", "Starplate Raider", 5.2f, (Func<bool>)(() => MyWorld.downedRaider), "Use a [i:" + ItemType("StarWormSummon") + "] at nighttime");
                bossChecklist.Call("AddBossWithInfo", "Infernon", 6.5f, (Func<bool>)(() => MyWorld.downedInfernon), "Use [i:" + ItemType("CursedCloth") + "] in the underworld at any time");

                bossChecklist.Call("AddBossWithInfo", "Dusking", 7.3f, (Func<bool>)(() => MyWorld.downedDusking), "Use a [i:" + ItemType("DuskCrown") + "] at nighttime");
                bossChecklist.Call("AddBossWithInfo", "Ethereal Umbra", 7.8f, (Func<bool>)(() => MyWorld.downedSpiritCore), "Use a [i:" + ItemType("UmbraSummon") + "] in the Spirit Biome at nighttime");
                bossChecklist.Call("AddBossWithInfo", "Atlas", 12.4f, (Func<bool>)(() => MyWorld.downedAtlas), "Use a [i:" + ItemType("StoneSkin") + "] at any time");
                bossChecklist.Call("AddBossWithInfo", "Overseer", 14.2f, (Func<bool>)(() => MyWorld.downedOverseer), "Use a [i:" + ItemType("SpiritIdol") + "] at the Spirit Biome during nighttime");
            }

        }


        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            TidePlayer modPlayer1 = Main.player[Main.myPlayer].GetModPlayer<TidePlayer>();
            if (TideWorld.TheTide)
            {
                int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                if (index >= 0)
                {
                    LegacyGameInterfaceLayer TideThing = new LegacyGameInterfaceLayer("SpiritMod: TideBenis",
                        delegate
                        {
                            DrawTide(Main.spriteBatch);
                            return true;
                        },
                        InterfaceScaleType.UI);
                    layers.Insert(index, TideThing);
                }
            }
        }

        public override void HotKeyPressed(string name)
        {
            if (name == "Concentration_Hotkey")
            {
                MyPlayer mp = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                if (mp.leatherSet && !mp.concentrated && mp.concentratedCooldown <= 0)
                {
                    mp.concentrated = true;
                }
            }
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            Mod mod = SpiritMod.instance;
            Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
            Microsoft.Xna.Framework.Color white2 = Microsoft.Xna.Framework.Color.White;
            Player player = Main.LocalPlayer;
            if (MyWorld.SpiritTiles > 0)
            {
                float num255 = (float)MyWorld.SpiritTiles / 160f;
                if (num255 > MyWorld.spiritLight)
                {
                    MyWorld.spiritLight += 0.01f;
                }
                if (num255 < MyWorld.spiritLight)
                {
                    MyWorld.spiritLight -= 0.01f;
                }
            }
            else
            {
                MyWorld.spiritLight -= 0.02f;
            }
            if (MyWorld.spiritLight < 0f)
            {
                MyWorld.spiritLight = 0f;
            }
            if (MyWorld.spiritLight > 1f)
            {
                MyWorld.spiritLight = 1f;
            }
            if (MyWorld.spiritLight > 0f)
            {
                float num161 = MyWorld.spiritLight;
                int num160 = Main.bgColor.R;
                int num159 = Main.bgColor.G;
                int num158 = Main.bgColor.B;
                num159 -= (int)(250f/ 1.14f * num161 * ((float)(int)Main.bgColor.G / 255f));
                num160 -= (int)(250f/ 1.14f * num161 * ((float)(int)Main.bgColor.R / 255f));
                num158 -= (int)(250f/ 1.14f * num161 * ((float)(int)Main.bgColor.B / 255f));
                if (num159 < 15)
                {
                    num159 = 15;
                }
                if (num160 < 15)
                {
                    num160 = 15;
                }
                if (num158 < 15)
                {
                    num158 = 15;
                }
                Main.bgColor.R = (byte)num160;
                Main.bgColor.G = (byte)num159;
                Main.bgColor.B = (byte)num158;
                num160 = white.R;
                num159 = white.G;
                num158 = white.B;
                num159 -= (int)(10f/ 1.14f * num161 * ((float)(int)white.G / 255f));
                num160 -= (int)(30f/ 1.14f * num161 * ((float)(int)white.R / 255f));
                num158 -= (int)(10f/ 1.14f * num161 * ((float)(int)white.B / 255f));
                if (num160 < 15)
                {
                    num160 = 15;
                }
                if (num159 < 15)
                {
                    num159 = 15;
                }
                if (num158 < 15)
                {
                    num158 = 15;
                }
                white.R = (byte)num160;
                white.G = (byte)num159;
                white.B = (byte)num158;
                num160 = white2.R;
                num159 = white2.G;
                num158 = white2.B;
                num159 -= (int)(140f/1.14f * num161 * ((float)(int)white2.R / 255f));
                num160 -= (int)(170f/1.14f * num161 * ((float)(int)white2.G / 255f));
                num158 -= (int)(190f/1.14f * num161 * ((float)(int)white2.B / 255f));
                if (num160 < 15)
                {
                    num160 = 15;
                }
                if (num159 < 15)
                {
                    num159 = 15;
                }
                if (num158 < 15)
                {
                    num158 = 15;
                }
                white2.R = (byte)num160;
                white2.G = (byte)num159;
                white2.B = (byte)num158;
            }
       
            if (MyWorld.AsteroidTiles > 0)
            {
                float num255 = (float)MyWorld.AsteroidTiles / 160f;
                if (num255 > MyWorld.asteroidLight)
                {
                    MyWorld.asteroidLight += 0.01f;
                }
                if (num255 < MyWorld.asteroidLight)
                {
                    MyWorld.asteroidLight -= 0.01f;
                }
            }
            else
            {
                MyWorld.asteroidLight -= 0.02f;
            }
            if (MyWorld.asteroidLight < 0f)
            {
                MyWorld.asteroidLight = 0f;
            }
            if (MyWorld.asteroidLight > 1f)
            {
                MyWorld.asteroidLight = 1f;
            }
            if (MyWorld.asteroidLight > 0f)
            {
                float num161 = MyWorld.asteroidLight;
                int num160 = Main.bgColor.R;
                int num159 = Main.bgColor.G;
                int num158 = Main.bgColor.B;
                num159 -= (int)(250f * num161 * ((float)(int)Main.bgColor.G / 255f));
                num160 -= (int)(250f * num161 * ((float)(int)Main.bgColor.R / 255f));
                num158 -= (int)(250f * num161 * ((float)(int)Main.bgColor.B / 255f));
                if (num159 < 15)
                {
                    num159 = 15;
                }
                if (num160 < 15)
                {
                    num160 = 15;
                }
                if (num158 < 15)
                {
                    num158 = 15;
                }
                Main.bgColor.R = (byte)num160;
                Main.bgColor.G = (byte)num159;
                Main.bgColor.B = (byte)num158;
                num160 = white.R;
                num159 = white.G;
                num158 = white.B;
                num159 -= (int)(10f * num161 * ((float)(int)white.G / 255f));
                num160 -= (int)(30f * num161 * ((float)(int)white.R / 255f));
                num158 -= (int)(10f * num161 * ((float)(int)white.B / 255f));
                if (num160 < 15)
                {
                    num160 = 15;
                }
                if (num159 < 15)
                {
                    num159 = 15;
                }
                if (num158 < 15)
                {
                    num158 = 15;
                }
                white.R = (byte)num160;
                white.G = (byte)num159;
                white.B = (byte)num158;
                num160 = white2.R;
                num159 = white2.G;
                num158 = white2.B;
                num159 -= (int)(140f * num161 * ((float)(int)white2.R / 255f));
                num160 -= (int)(170f * num161 * ((float)(int)white2.G / 255f));
                num158 -= (int)(190f * num161 * ((float)(int)white2.B / 255f));
                if (num160 < 15)
                {
                    num160 = 15;
                }
                if (num159 < 15)
                {
                    num159 = 15;
                }
                if (num158 < 15)
                {
                    num158 = 15;
                }
                white2.R = (byte)num160;
                white2.G = (byte)num159;
                white2.B = (byte)num158;
            }

            int num226 = 15;
            int num227 = 0;
            int num228 = 80;
            int num229 = 80;
            int num230 = 32;
        }
      

    const int ShakeLength = 5;
        int ShakeCount = 0;
        float previousRotation = 0;
        float targetRotation = 0;
        float previousOffsetX = 0;
        float previousOffsetY = 0;
        float targetOffsetX = 0;
        float targetOffsetY = 0;

        public static float shittyModTime;
        public static float tremorTime;
        public int screenshakeTimer = 0;
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (!Main.gameMenu)
            {
                screenshakeTimer++;
                if (tremorTime >= 0 && screenshakeTimer >= 20) // so it doesnt immediately decrease
                {
                    tremorTime -= 0.5f;
                }
                if (tremorTime < 0)
                {
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

        public void DrawTide(SpriteBatch spriteBatch)
        {
            TidePlayer modPlayer1 = Main.player[Main.myPlayer].GetModPlayer<TidePlayer>();
            if (TideWorld.TheTide && TideWorld.InBeach)
            {

                float alpha = 0.5f;
                Texture2D backGround1 = Main.colorBarTexture;
                Texture2D progressColor = Main.colorBarTexture;
                Texture2D TideIcon = SpiritMod.instance.GetTexture("Effects/InvasionIcons/Depths_Icon");
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
                float cleared = TideWorld.TidePoints2 / 80f;
                string waveText = "Cleared " + Math.Round(100 * cleared) + "%";
                Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.X + waveBackground.Width / 2, waveBackground.Y + 5), Color.White, scmp * 0.8f, 0.5f, -0.1f);
                Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.X + waveBackground.Width * 0.5f, waveBackground.Y + waveBackground.Height * 0.75f), new Vector2(progressColor.Width, progressColor.Height));
                Rectangle waveProgressAmount = new Rectangle(0, 0, (int)(progressColor.Width * MathHelper.Clamp(cleared, 0f, 1f)), progressColor.Height);
                Vector2 offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * scmp)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * scmp)) * 0.5f);
                spriteBatch.Draw(backGround1, waveProgressBar.Location.ToVector2() + offset, null, Color.White * alpha, 0f, new Vector2(0f), scmp, SpriteEffects.None, 0f);
                spriteBatch.Draw(backGround1, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), scmp, SpriteEffects.None, 0f);
                const int internalOffset = 6;
                Vector2 descSize = new Vector2(154, 40) * scmp;
                Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 19f), new Vector2(width, height));
                Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), descSize * .8f);
                Utils.DrawInvBG(spriteBatch, descBackground, descColor * alpha);
                int descOffset = (descBackground.Height - (int)(32f * scmp)) / 2;
                Rectangle icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * scmp), (int)(32 * scmp));
                spriteBatch.Draw(TideIcon, icon, Color.White);
                Utils.DrawBorderString(spriteBatch, customEventName, new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), Color.White, 0.8f, 0.3f, 0.4f);
            }
        }
        #region pin stuff
        public override void PostDrawFullscreenMap(ref string mouseText)
        {
            var myplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            DrawMirrorOnFullscreenMap(myplayer.RedPinX, myplayer.RedPinY, true, GetTexture("Textures/Pins/RedPin"));
            DrawMirrorOnFullscreenMap(myplayer.BluePinX, myplayer.BluePinY, true, GetTexture("Textures/Pins/BluePin"));
            DrawMirrorOnFullscreenMap(myplayer.GreenPinX, myplayer.GreenPinY, true, GetTexture("Textures/Pins/GreenPin"));
            DrawMirrorOnFullscreenMap(myplayer.YellowPinX, myplayer.YellowPinY, true, GetTexture("Textures/Pins/YellowPin"));
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

            if (overMapData.IsOnScreen && tileX > 0 && tileY > 0)
            {
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