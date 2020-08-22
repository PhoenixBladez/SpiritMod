using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Armor;
using SpiritMod.Buffs.Artifact;
using SpiritMod.Buffs.Glyph;
using SpiritMod.Buffs.Summon;
using SpiritMod.Dusts;
using SpiritMod.Items;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.DonatorItems;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Mounts;
using SpiritMod.NPCs;
using SpiritMod.NPCs.BloodMoon;
using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.NPCs.Boss.Overseer;
using SpiritMod.NPCs.Mimic;
using SpiritMod.Projectiles;
using SpiritMod.Tide;
using SpiritMod.Tide.NPCs;
using SpiritMod.NPCs.Tides;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Summon;
using SpiritMod.Projectiles.Sword;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod
{
	public class MyPlayer : ModPlayer
	{
		 public int Shake = 0;
		public List<SpiritPlayerEffect> effects = new List<SpiritPlayerEffect>();
		public List<SpiritPlayerEffect> removedEffects = new List<SpiritPlayerEffect>();
		public SpiritPlayerEffect setbonus = null;
		public const int CAMO_DELAY = 100;
		public int Soldiers = 0;
		internal static bool swingingCheck;
		internal static Item swingingItem;
		public bool TormentLantern = false;
		public bool clockActive = false;
		public bool QuacklingMinion = false;
		public bool bismiteShield = false;
		public bool zipline = false;
		public bool ziplineActive = false;
		public float ziplineX = 0;
		public float ziplineY = 0;
		public float ziplineCounter = 0f;
		public int shieldCounter = 0;
		public int bismiteShieldStacks;
		public bool MetalBand = false;
		public bool KoiTotem = false;
		public bool VampireCloak = false;
		public bool starplateGlitchEffect = false;
		public bool HealCloak = false;
		public bool SpiritCloak = false;
		public bool firewall = false;
		public int clockX = 0;
		public int clockY = 0;
		private int Counter;
		private int timerz;
		public bool caltfist = false;
		public bool briarSlimePet = false;
		public bool ZoneBlueMoon = false;
		private int timer1;
		public bool astralSet = false;
		public bool bloodcourtSet = false;
		public int astralSetStacks;
		public bool frigidGloves = false;
		public bool mushroomPotion = false;
		public bool magnifyingGlass = false;
		public bool ShieldCore = false;
		public int shieldsLeft = 2;
		public bool spawnedShield = false;
		public bool SoulStone = false;
		public bool AceOfSpades = false;
		public bool AceOfHearts = false;
		public bool AceOfClubs = false;
		public bool AceOfDiamonds = false;
		public bool geodeSet = false;
		public bool assassinMag = false;
		public bool shadowFang = false;
		public bool reachBrooch = false;
		public bool cleftHorn = false;
		public bool daybloomSet = false;
		public int dazzleStacks;
		public bool ToxicExtract = false;
		public bool vitaStone = false;
		public bool throwerGlove = false;
		public bool firedSharpshooter = false;
		public int throwerStacks;
		public bool scarabCharm = false;
		public bool floranCharm = false;
		public bool sunStone = false;
		public bool moonStone = false;
		public bool bloodCourtHead = false;
		public bool animusLens = false;
		public bool timScroll = false;
		public bool cultistScarf = false;
		public bool fateToken = false;
		public bool geodeRanged = false;
		public bool atmos = false;
		public bool fireMaw = false;
		public bool deathRose = false;
		public bool anglure = false;
		public bool Fierysoul = false;
		public bool manaWings = false;
		public bool infernalFlame = false;
		public bool floranSet = false;
		public bool silkenLegs = false;
		public bool silkenRobe = false;
		public bool rogueSet = false;
		public bool crystal = false;
		public bool eyezorEye = false;
		public bool shamanBand = false;
		public bool ChaosCrystal = false;
		public bool wheezeScale = false;
		public bool winterbornCharmMage = false;
		public bool sepulchreCharm = false;
		public bool HellGaze = false;
		public bool hungryMinion = false;
		public bool magazine = false;
		public bool EaterSummon = false;
		public bool CreeperSummon = false;
		public bool leatherGlove = false;
		public bool forbiddenTome = false;
		public bool moonHeart = false;
		public bool Phantom = false;
		public bool gremlinTooth = false;
		public bool illusionistEye = false;
		public bool sacredVine = false;
		public bool BlueDust = false;

		public bool reachFireflies = false;

		public bool onGround = false;
		public bool moving = false;
		public bool flying = false;
		public bool swimming = false;
		public bool copterBrake = false;
		public bool copterFiring = false;
		public int copterFireFrame = 1000;

		public int beetleStacks = 1;
		public int shootDelay = 0;
		public bool bloodfireShield;
		public int bloodfireShieldStacks;
		public int shootDelay1 = 0;
		public int shootDelay2 = 0;
		public int shootDelay3 = 0;
		public bool unboundSoulMinion = false;
		public bool cragboundMinion = false;
		public bool crawlerockMinion = false;
		public bool pigronMinion = false;
		public bool mangoMinion = false;
		public bool terror1Summon = false;
		public bool terror2Summon = false;
		public bool terror3Summon = false;
		public bool clatterboneShield = false;
		public bool terror4Summon = false;
		public bool minior = false;

		public bool cthulhuMinion = false;
		public double pressedSpecial;
		float distYT = 0f;
		float distXT = 0f;
		float distY = 0f;
		float distX = 0f;
		public Entity LastEnemyHit = null;
		public bool TiteRing = false;
		public bool NebulaPearl = false;
		public bool CursedPendant = false;
		public bool KingRock = false;
		public bool starMap = false;
		private const int saveVersion = 0;
		public bool minionName = false;
		public bool Ward = false;
		public bool Ward1 = false;
		public static bool hasProjectile;
		public bool DoomDestiny = false;
		public int HitNumber;
		public bool ZoneSpirit = false;
		public bool ZoneReach = false;
		public int PutridHits = 0;
		public int Rangedhits = 0;
		public bool flametrail = false;
		public bool daybloomGarb = false;
		public bool icytrail = false;
		public bool silkenSet = false;
		public bool EnchantedPaladinsHammerMinion = false;
		public bool ProbeMinion = false;
		public int frigidGloveStacks;
		public int weaponAnimationCounter;

		public bool gemPickaxe = false;
		public int hexBowAnimationFrame;
		public bool CrystalShield = false;
		public bool carnivorousPlantMinion = false;
		public bool skeletalonMinion = false;
		public bool beetleMinion = false;
		public bool steamMinion = false;
		public bool aeonMinion = false;
		public bool lihzahrdMinion = false;
		public bool SnakeMinion = false;
		public bool Ghast = false;
        public bool jellyfishMinion = false;
		public bool DungeonSummon = false;
		public bool ReachSummon = false;
		public bool gasopodMinion = false;
        public bool rogueCrest = false;
        public bool spellswordCrest = false;
		public bool tankMinion = false;
		public bool OG = false;
		public bool lavaRock = false;
		public bool Flayer = false;
		public int soulSiphon;
		public bool maskPet = false;
		public bool phantomPet = false;
		public bool lanternPet = false;
		public bool leatherHood = false;
		public bool chitinSet = false;
		public bool thrallPet = false;
		public bool jellyfishPet = false;
		public int clatterStacks;
		public bool starPet = false;
		public bool saucerPet = false;
		public bool bookPet = false;
		public bool SwordPet = false;
		public bool shadowPet = false;

        public bool arcaneNecklace = false;
        public bool manaShield = false;
        public bool seraphimBulwark = false;

		//Adventurer related
		public int explorerTimer;
		public bool emptyExplorerScroll = false;
		public bool emptyWinterbornScroll = false;
		public bool emptyBeholderScroll = false;
		public bool emptyValkyrieScroll = false;
		public bool emptyAntlionScroll = false;
		public bool emptyDrBonesScroll = false;
		public bool emptyWheezerScroll = false;
		public bool emptyStardancerScroll = false;

		public float SpeedMPH { get; private set; }
		public DashType ActiveDash { get; private set; }
		public GlyphType glyph;
		public int voidStacks = 1;
		public int camoCounter;
		public int veilCounter;
		public bool blazeBurn;
		public int phaseCounter;
		public int phaseStacks;
		public bool phaseShift;
		private float[] phaseSlice = new float[60];
		public int divineCounter;
		public int divineStacks = 1;
		public int stormStacks;
		public int frostCooldown;
		public float frostRotation;
		public bool frostUpdate;
		public int frostTally;
		public int frostCount;
		public bool stoneHead;

		public float shadowRotation;
		public bool shadowUpdate;
		public int shadowTally;
		public int shadowCount;

		// Armor set booleans.
		public bool duskSet;
		public bool runicSet;
		public bool icySet;
		public bool depthSet;
		public bool elderbarkWoodSet;
		public bool primalSet;
		public bool spiritSet;
		public bool putridSet;
		public bool duneSet;
		public bool lihzahrdSet;
		public bool acidSet;
		public bool reachSet;
		public bool coralSet;
		public bool leatherSet;
		public bool witherSet;
		public bool titanicSet;
		public bool reaperSet;
		public bool shadowSet;
		public bool oceanSet;
		public bool marbleSet;
		public bool windSet;
		public bool cometSet;
		public bool hellSet;
		public bool bloodfireSet;
		public bool quickSilverSet;
		public bool stellarSet;
		public bool magicshadowSet;
		public bool cryoChestplate;
		public bool cryoSet;
		public bool frigidSet;
		public bool rangedshadowSet;
		public bool graniteSet;
		public bool graniteslam = false;
		public bool meleeshadowSet;
		public bool infernalSet;
		public bool crystalSet;
		public bool bloomwindSet;
		public bool fierySet;
		public bool starSet;
		public bool magalaSet;
		public bool thermalSet;
		public bool veinstoneSet;
		public bool clatterboneSet;
		public bool ichorSet1;
		public bool ichorSet2;
		public bool talonSet;
		public bool OverseerCharm = false;

		public bool ZoneAsteroid = false;
		public bool ZoneMarble = false;
		public bool ZoneGranite = false;
		public bool ZoneHive = false;

		public bool Bauble = false;

		public bool marbleJustJumped;

		// Accessory booleans.
		public bool OriRing;
		public bool SRingOn;
		public bool goldenApple;
		public bool hpRegenRing;
		public bool bubbleShield;
		public bool icySoul;
		public bool mythrilCharm;
		public bool infernalShield;
		public bool shadowGauntlet;
		public bool amazonCharm;
		public bool KingSlayerFlask;
		public bool Resolve;
		public bool DarkBough;
		public bool hellCharm;
		public bool bloodyBauble;
		public bool twilightTalisman;
		public bool MoonSongBlossom;
		public bool HolyGrail;
		public bool surferSet;
		public float virulence = 600f;
		public int illusionistTimer;
		public float cryoTimer = 0f;
		public float marbleJump = 420f;
		public bool moonGauntlet;
		public bool starCharm;
		public int timeLeft = 0;
		public int infernalHit;
		public int infernalDash;

		public bool windEffect;
		public bool windEffect2;
		public int infernalSetCooldown;
		public int fierySetTimer = 480;
		public int surferTimer = 330;
		public int firewallHit;
		public int bubbleTimer;
		public float starplateGlitchIntensity;
		public int clatterboneTimer;
		public int roseTimer;
		public int baubleTimer;
		public int cometTimer;
		public bool concentrated; // For the leather armor set.
		public int concentratedCooldown = 360;
		public int stompCooldown = 600;
		public bool basiliskMount;
		public bool drakomireMount;
		public int drakomireFlameTimer;
		public bool drakinMount;
		public bool toxify;
		public bool acidImbue;
		public bool spiritBuff;
		public bool starBuff;
		public bool runeBuff;
		public bool poisonPotion;
		public bool soulPotion;
		public bool gremlinBuff;

		public int candyInBowl;
		private IList<string> candyFromTown = new List<string>();

		public override void UpdateBiomeVisuals()
		{
			bool showAurora = (player.ZoneSnow || ZoneSpirit || player.ZoneSkyHeight) && !Main.dayTime && !Main.raining && !player.ZoneCorrupt && !player.ZoneCrimson && MyWorld.aurora;
			bool reach = (!Main.dayTime && ZoneReach && !reachBrooch && player.ZoneOverworldHeight) || (ZoneReach && player.ZoneOverworldHeight && MyWorld.downedReachBoss && Main.dayTime);
			bool spirit = (player.ZoneOverworldHeight && ZoneSpirit);

			bool region1 = ZoneSpirit && player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f;
			bool region2 = ZoneSpirit && player.position.Y / 16 >= Main.maxTilesY - 300;

			bool greenOcean = player.ZoneBeach && MyWorld.luminousType == 1 && MyWorld.luminousOcean;
			bool blueOcean = player.ZoneBeach && MyWorld.luminousType == 2 && MyWorld.luminousOcean;
			bool purpleOcean = player.ZoneBeach && MyWorld.luminousType == 3 && MyWorld.luminousOcean;

			bool blueMoon = ZoneBlueMoon && (player.ZoneOverworldHeight || player.ZoneSkyHeight);

			if(ZoneSpirit && player.position.Y / 16 >= Main.maxTilesY - 330) {
				SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.2f); //0.4f is default
				SpiritMod.glitchScreenShader.UseIntensity(0.004f);
				player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
			} else if(ZoneSpirit && player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f) {
				SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.1f); //0.4f is default
				SpiritMod.glitchScreenShader.UseIntensity(0.0005f);
				player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
			} else if(starplateGlitchEffect) {
				SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.3f);
				SpiritMod.glitchScreenShader.UseIntensity(starplateGlitchIntensity);
				player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
			} else {
				player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", false);
			}

			player.ManageSpecialBiomeVisuals("SpiritMod:AuroraSky", showAurora);
			player.ManageSpecialBiomeVisuals("SpiritMod:SpiritBiomeSky", spirit);
			player.ManageSpecialBiomeVisuals("SpiritMod:AsteroidSky2", ZoneAsteroid);

			player.ManageSpecialBiomeVisuals("SpiritMod:GreenAlgaeSky", greenOcean);
			player.ManageSpecialBiomeVisuals("SpiritMod:BlueAlgaeSky", blueOcean);
			player.ManageSpecialBiomeVisuals("SpiritMod:PurpleAlgaeSky", purpleOcean);

			player.ManageSpecialBiomeVisuals("SpiritMod:SpiritUG1", region1);
			player.ManageSpecialBiomeVisuals("SpiritMod:SpiritUG2", region2);

			player.ManageSpecialBiomeVisuals("SpiritMod:ReachSky", reach, player.Center);
			player.ManageSpecialBiomeVisuals("SpiritMod:BlueMoonSky", blueMoon, player.Center);
			player.ManageSpecialBiomeVisuals("SpiritMod:MeteorSky", ZoneAsteroid);
			player.ManageSpecialBiomeVisuals("SpiritMod:WindEffect", windEffect, player.Center);
			player.ManageSpecialBiomeVisuals("SpiritMod:WindEffect2", windEffect2, player.Center);
			player.ManageSpecialBiomeVisuals("SpiritMod:Overseer", NPC.AnyNPCs(ModContent.NPCType<Overseer>()));
			player.ManageSpecialBiomeVisuals("SpiritMod:Atlas", NPC.AnyNPCs(ModContent.NPCType<Atlas>()));
		}

		public override void UpdateBiomes()
		{
			ZoneSpirit = MyWorld.SpiritTiles > 200;
			ZoneBlueMoon = MyWorld.BlueMoon;
			ZoneReach = MyWorld.ReachTiles > 50;
			ZoneMarble = MyWorld.MarbleTiles > 310;
			ZoneGranite = MyWorld.GraniteTiles > 400;
			ZoneAsteroid = MyWorld.AsteroidTiles > 130;
			ZoneHive = MyWorld.HiveTiles > 100;
		}

		public override bool CustomBiomesMatch(Player other)
		{
			MyPlayer modOther = other.GetSpiritPlayer();
			return ZoneSpirit == modOther.ZoneSpirit && ZoneReach == modOther.ZoneReach && ZoneAsteroid == modOther.ZoneAsteroid && ZoneGranite == modOther.ZoneGranite && ZoneMarble == modOther.ZoneMarble && ZoneHive == modOther.ZoneHive;
		}

		public override void CopyCustomBiomesTo(Player other)
		{
			MyPlayer modOther = other.GetSpiritPlayer();
			modOther.ZoneSpirit = ZoneSpirit;
			modOther.ZoneReach = ZoneReach;
			modOther.ZoneAsteroid = ZoneAsteroid;
			modOther.ZoneGranite = ZoneGranite;
			modOther.ZoneMarble = ZoneMarble;
			modOther.ZoneHive = ZoneHive;
		}

		public override void SendCustomBiomes(BinaryWriter writer)
		{
			byte flags = 0;
			if(ZoneSpirit) {
				flags |= 1;
			}

			if(ZoneReach) {
				flags |= 2;
			}
			if(ZoneAsteroid) {
				flags |= 3;
			}
			if(ZoneMarble) {
				flags |= 4;
			}
			if(ZoneGranite) {
				flags |= 5;
			}
			if(ZoneHive) {
				flags |= 6;
			}
			writer.Write(flags);
		}

		public override void ReceiveCustomBiomes(BinaryReader reader)
		{
			byte flags = reader.ReadByte();
			ZoneSpirit = ((flags & 1) == 1);
			ZoneReach = ((flags & 2) == 2);
			ZoneAsteroid = ((flags & 3) == 3);
			ZoneMarble = ((flags & 4) == 4);
			ZoneGranite = ((flags & 5) == 5);
			ZoneHive = ((flags & 6) == 6);
		}

		public override Texture2D GetMapBackgroundImage()
		{
			if(ZoneSpirit) {
				return mod.GetTexture("Backgrounds/SpiritMapBackground");
			}
			return null;
		}
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound
			{
				{ "candyInBowl", candyInBowl },
				{ "candyFromTown", candyFromTown }
			};
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			candyInBowl = tag.GetInt("candyInBowl");
			candyFromTown = tag.GetList<string>("candyFromTown");
		}


		public override void ResetEffects()
		{
			removedEffects = effects;
			effects = new List<SpiritPlayerEffect>();
			MetalBand = false;
			KoiTotem = false;
			setbonus = null;
            rogueCrest = false;
            spellswordCrest = false;
			stoneHead = false;
			silkenRobe = false;
			zipline = false;
			clockActive = false;
			bloodcourtSet = false;
			ShieldCore = false;
			caltfist = false;
			briarSlimePet = false;
			firewall = false;
			hellCharm = false;
			bloodyBauble = false;
			elderbarkWoodSet = false;
			amazonCharm = false;
			cleftHorn = false;
			TormentLantern = false;
			phantomPet = false;
			throwerGlove = false;
			QuacklingMinion = false;
			VampireCloak = false;
			SpiritCloak = false;
			HealCloak = false;
			vitaStone = false;
			silkenLegs = false;
			astralSet = false;
			mushroomPotion = false;
			floranCharm = false;
			ChaosCrystal = false;
			twilightTalisman = false;
			ToxicExtract = false;
			shadowFang = false;
			gemPickaxe = false;
			cultistScarf = false;
			surferSet = false;
			bloodCourtHead = false;
			scarabCharm = false;
			assassinMag = false;

            arcaneNecklace = false;
            manaShield = false;
            seraphimBulwark = false;

			moonHeart = false;
			chitinSet = false;
			starplateGlitchEffect = false;
			Fierysoul = false;
			infernalFlame = false;
			reachBrooch = false;
			windEffect = false;
			windEffect2 = false;
			gremlinTooth = false;
			leatherHood = false;
			floranSet = false;
			atmos = false;
			SoulStone = false;
			AceOfSpades = false;
			AceOfHearts = false;
			AceOfClubs = false;
			AceOfDiamonds = false;
			anglure = false;
			geodeSet = false;
			manaWings = false;
			sunStone = false;
			fireMaw = false;
			moonStone = false;
			rogueSet = false;
			timScroll = false;
			wheezeScale = false;
			crystal = false;
			HellGaze = false;
			Bauble = false;
			geodeRanged = false;
			OverseerCharm = false;
			ReachSummon = false;
			hungryMinion = false;
			silkenSet = false;
			EaterSummon = false;
			CreeperSummon = false;
			CrystalShield = false;
			bloodfireShield = false;
			tankMinion = false;
			Phantom = false;
			magnifyingGlass = false;
			magazine = false;
			daybloomSet = false;
			daybloomGarb = false;
			Ward = false;
			Ward1 = false;
			CursedPendant = false;
			BlueDust = false;
			SnakeMinion = false;
			Ghast = false;
			starCharm = false;
			eyezorEye = false;
			minionName = false;
			starMap = false;
			frigidGloves = false;
			NebulaPearl = false;
			TiteRing = false;
			bismiteShield = false;
			sacredVine = false;
			winterbornCharmMage = false;
			sepulchreCharm = false;
			clatterboneShield = false;
			KingRock = false;
			cthulhuMinion = false;
			leatherGlove = false;
			flametrail = false;
			icytrail = false;
			EnchantedPaladinsHammerMinion = false;
			ProbeMinion = false;
			crawlerockMinion = false;
			mangoMinion = false;
			pigronMinion = false;
			skeletalonMinion = false;
			cragboundMinion = false;
			beetleMinion = false;
			shamanBand = false;
			lihzahrdMinion = false;
			aeonMinion = false;
			gasopodMinion = false;
			Flayer = false;
			steamMinion = false;
			DungeonSummon = false;
			OG = false;
			lavaRock = false;
			maskPet = false;
			starPet = false;
			bookPet = false;
			SwordPet = false;
			lanternPet = false;
			jellyfishPet = false;
			thrallPet = false;
            jellyfishMinion = false;
			shadowPet = false;
			saucerPet = false;
			terror1Summon = false;
			terror2Summon = false;
			terror3Summon = false;
			terror4Summon = false;
			minior = false;
			drakomireMount = false;
			basiliskMount = false;
			toxify = false;
			gremlinBuff = false;
			spiritBuff = false;
			drakinMount = false;
			poisonPotion = false;
			starBuff = false;
			runeBuff = false;
			soulPotion = false;
			carnivorousPlantMinion = false;

			emptyExplorerScroll = false;
			emptyWinterbornScroll = false;
			emptyBeholderScroll = false;
			emptyValkyrieScroll = false;
			emptyDrBonesScroll = false;
			emptyWheezerScroll = false;
			emptyStardancerScroll = false;

			// Reset armor set booleans.
			duskSet = false;
			runicSet = false;
			primalSet = false;
			shadowSet = false;
			cometSet = false;
			meleeshadowSet = false;
			rangedshadowSet = false;
			magicshadowSet = false;
			witherSet = false;
			hellSet = false;
			quickSilverSet = false;
			reaperSet = false;
			spiritSet = false;
			coralSet = false;
			ichorSet1 = false;
			stellarSet = false;
			ichorSet2 = false;
			icySet = false;
			graniteSet = false;
			fierySet = false;
			putridSet = false;
			reachSet = false;
			duneSet = false;
			leatherSet = false;
			starSet = false;
			bloodfireSet = false;
			oceanSet = false;
			titanicSet = false;
			cryoChestplate = false;
			cryoSet = false;
			frigidSet = false;
			windSet = false;
			marbleSet = false;
			crystalSet = false;
			magalaSet = false;
			depthSet = false;
			thermalSet = false;
			infernalSet = false;
			bloomwindSet = false;
			veinstoneSet = false;
			clatterboneSet = false;
			lihzahrdSet = false;
			talonSet = false;

			marbleJustJumped = false;

			// Reset accessory booleans.
			OriRing = false;
			SRingOn = false;
			goldenApple = false;
			hpRegenRing = false;
			forbiddenTome = false;
			bubbleShield = false;
			animusLens = false;
			deathRose = false;
			mythrilCharm = false;
			KingSlayerFlask = false;
			DarkBough = false;
			Resolve = false;
			MoonSongBlossom = false;
			HolyGrail = false;
			infernalShield = false;
			illusionistEye = false;
			shadowGauntlet = false;
			moonGauntlet = false;
			unboundSoulMinion = false;

			if(player.FindBuffIndex(ModContent.BuffType<BeetleFortitude>()) < 0) {
				beetleStacks = 1;
			}

			if(player.FindBuffIndex(ModContent.BuffType<CollapsingVoid>()) < 0) {
				voidStacks = 1;
			}

			phaseShift = false;
			blazeBurn = false;

			if(glyph != GlyphType.Phase) {
				phaseStacks = 0;
				phaseCounter = 0;
			}

			if(glyph != GlyphType.Veil) {
				veilCounter = 0;
			}

			if(glyph != GlyphType.Radiant) {
				divineStacks = 1;
				divineCounter = 0;
			}

			if(glyph != GlyphType.Storm) {
				stormStacks = 0;
			}

			if(frostCooldown > 0) {
				frostCooldown--;
			}

			frostRotation += Items.Glyphs.FrostGlyph.TURNRATE;
			if(frostRotation > MathHelper.TwoPi) {
				frostRotation -= MathHelper.TwoPi;
			}

			if(frostUpdate) {
				frostUpdate = false;
				if(glyph == GlyphType.Frost) {
					Items.Glyphs.FrostGlyph.UpdateIceSpikes(player);
				}
			}

			frostCount = frostTally;
			frostTally = 0;

			copterFireFrame++;

			onGround = false;
			flying = false;
			swimming = false;
			if(player.velocity.Y != 0f) {
				if(player.mount.Active && player.mount.FlyTime > 0 && player.jump == 0 && player.controlJump && !player.mount.CanHover) {
					flying = true;
				} else if(player.wet) {
					swimming = true;
				}
			} else {
				onGround = true;
			}

			moving = false;
			if(player.velocity.X != 0f) {
				moving = true;
			}
		}

		public bool flag8 = false;
		public bool marbleJumpEffects = false;
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if(marbleSet && player.controlUp && player.releaseUp && marbleJump <= 0) {
				player.AddBuff(ModContent.BuffType<MarbleDivineWinds>(), 120);
				Main.PlaySound(SoundID.Item, player.position, 20);
				for(int i = 0; i < 8; i++) {
					int num = Dust.NewDust(player.position, player.width, player.height, 222, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if(Main.dust[num].position != player.Center)
						Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
				}
				marbleJump = 420;
			}
			if(marbleSet) {
				if(player.sliding || player.velocity.Y == 0f) {
					player.justJumped = true;
				}
			}
			if(player.controlJump) {
				if(marbleJustJumped) {
					marbleJustJumped = false;
					flag8 = true;
					if(flag8 && player.HasBuff(ModContent.BuffType<MarbleDivineWinds>())) {
						if(Main.rand.Next(20) == 0) {
							Main.PlaySound(SoundID.Item, player.position, 24);
						}
						marbleJumpEffects = true;
						int num23 = player.height;
						if(player.gravDir == -1f) {
							num23 = 0;
						}
						player.velocity.Y = (0f - Player.jumpSpeed) * player.gravDir;
						player.jump = (int)(Player.jumpHeight * 1.25);
						for(int m = 0; m < 4; m++) {
							int num22 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + num23), player.width, 6, 222, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, 100, Color.White, .8f);
							if(m % 2 == 0) {
								Dust expr_79B_cp_0 = Main.dust[num22];
								expr_79B_cp_0.velocity.X = expr_79B_cp_0.velocity.X + (float)Main.rand.Next(10, 31) * 0.1f;
							} else {
								Dust expr_7CB_cp_0 = Main.dust[num22];
								expr_7CB_cp_0.velocity.X = expr_7CB_cp_0.velocity.X - Main.rand.Next(31, 71) * 0.1f;
							}
							Dust expr_7F9_cp_0 = Main.dust[num22];
							expr_7F9_cp_0.velocity.Y = expr_7F9_cp_0.velocity.Y + Main.rand.Next(-10, 31) * 0.1f;
							Main.dust[num22].noGravity = true;
							Main.dust[num22].scale += Main.rand.Next(-10, 11) * .0025f;
							Dust obj = Main.dust[num22];
							obj.velocity *= Main.dust[num22].scale * 0.7f;
							Vector2 value3 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
							value3.Normalize();
							value3 *= Main.rand.Next(81) * 0.08f;
						}
					}
				}
			}
		}

		public override bool PreItemCheck()
		{
			PrepareShotDetection();
			return true;
		}

		public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
		{
			BeginShotDetection(item);

			if(silkenLegs) {
				if(item.summon) {
					flat += 1;
				}
			}
			if(silkenRobe) {
				if(item.summon) {
					flat += 1;
				}
			}
			if(stoneHead) {
				if(item.melee) {
					flat += 1;
				}
			}
			if(daybloomGarb) {
				if(item.magic) {
					flat += 1;
				}
			}
			if(leatherHood) {
				if(item.ranged) {
					flat += 1;
				}
			}
			if(elderbarkWoodSet) {
				flat += 1;
			}
		}

		public override void PostItemCheck()
		{
			EndShotDetection();
		}

		private void PrepareShotDetection()
		{
			if(player.whoAmI == Main.myPlayer && !player.HeldItem.IsAir && !Main.gamePaused) {
				swingingItem = player.HeldItem;
			}
		}

		private void BeginShotDetection(Item item)
		{
			if(swingingItem == item) {
				swingingCheck = true;
			}
		}

		private void EndShotDetection()
		{
			swingingItem = null;
			swingingCheck = false;
		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			/* Item item = new Item();
             item.SetDefaults(ModContent.ItemType<OddKeystone>());
             items.Add(item);*/
		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			if(junk) {
				return;
			}
			if (KoiTotem && Main.rand.Next(10) == 0)
			{
				for (int j = 0; j < player.inventory.Length; ++j)
                {
                    if (player.inventory[j].stack > 0 && player.inventory[j].bait > 0)
                    {
						Item.NewItem((int) player.position.X, (int) player.position.Y, player.width, player.height, player.inventory[j].type, 1, false, 0, false, false);
						break;
					}			
                }
			}
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			int bobberIndex = -1;
			if(Main.bloodMoon && Main.rand.Next(20) == 0) {
				for(int i = 0; i < 1000; i++) {
					if(Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].bobber) {
						bobberIndex = i;
					}
				}
				if(bobberIndex != -1) {
					Vector2 bobberPos = Main.projectile[bobberIndex].Center;
					caughtType = NPC.NewNPC((int)bobberPos.X, (int)bobberPos.Y, ModContent.NPCType<BottomFeeder>(), 0, 2, 1, 0, 0, Main.myPlayer);
				}
			}
			if(MyWorld.spawnHornetFish && Main.rand.Next(15) == 0 && player.ZoneJungle) {
				for(int i = 0; i < 1000; i++) {
					if(Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].bobber) {
						bobberIndex = i;
					}
				}
				if(bobberIndex != -1) {
					Vector2 bobberPos = Main.projectile[bobberIndex].Center;
					caughtType = NPC.NewNPC((int)bobberPos.X, (int)bobberPos.Y, ModContent.NPCType<Hornetfish>(), 0, 2, 1, 0, 0, Main.myPlayer);
				}
			}
			if(player.ZoneDungeon && power >= 30 && Main.rand.NextBool(25)) {
				caughtType = ModContent.ItemType<MysticalCage>();
			}

			if(modPlayer.ZoneSpirit && NPC.downedMechBossAny && Main.rand.NextBool(player.cratePotion ? 35 : 65)) {
				caughtType = ModContent.ItemType<SpiritCrate>();
			}
			if(Main.rand.NextBool(player.cratePotion ? 80 : 95)) {
				for(int i = 0; i < 1000; i++) {
					if(Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].bobber) {
						bobberIndex = i;
					}
				}
				if(bobberIndex != -1) {
					Vector2 bobberPos = Main.projectile[bobberIndex].Center;
					caughtType = NPC.NewNPC((int)bobberPos.X, (int)bobberPos.Y, ModContent.NPCType<WoodCrateMimic>(), 0, 2, 1, 0, 0, Main.myPlayer);
				}
			}
			if(Main.rand.NextBool(player.cratePotion ? 100 : 125)) {
				for(int i = 0; i < 1000; i++) {
					if(Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].bobber) {
						bobberIndex = i;
					}
				}
				if(bobberIndex != -1) {
					Vector2 bobberPos = Main.projectile[bobberIndex].Center;
					caughtType = NPC.NewNPC((int)bobberPos.X, (int)bobberPos.Y, ModContent.NPCType<IronCrateMimic>(), 0, 2, 1, 0, 0, Main.myPlayer);
				}
			}
			if(Main.rand.NextBool(player.cratePotion ? 100 : 125) && Main.raining) {
				for(int i = 0; i < 1000; i++) {
					if(Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].bobber) {
						bobberIndex = i;
					}
				}
				if(bobberIndex != -1) {
					Vector2 bobberPos = Main.projectile[bobberIndex].Center;
					caughtType = NPC.NewNPC((int)bobberPos.X, (int)bobberPos.Y, ModContent.NPCType<GoldCrateMimic>(), 0, 2, 1, 0, 0, Main.myPlayer);
				}
			}
			if(modPlayer.ZoneSpirit && NPC.downedMechBossAny && Main.rand.NextBool(5)) {
				caughtType = ModContent.ItemType<SpiritKoi>();
			}
			if(modPlayer.ZoneReach && Main.rand.NextBool(5)) {
				caughtType = ModContent.ItemType<ReachFishingCatch>();
			}
			if(modPlayer.ZoneReach && Main.rand.NextBool(player.cratePotion ? 25 : 45)) {
				caughtType = ModContent.ItemType<ReachCrate>();
			}
			if(modPlayer.ZoneReach && Main.rand.NextBool(25)) {
				caughtType = ModContent.ItemType<ThornDevilfish>();
			}
			if(player.ZoneGlowshroom && Main.rand.NextBool(27)) {
				caughtType = ModContent.ItemType<ShroomFishSummon>();
			}
            if (player.ZoneBeach && Main.rand.NextBool(125))
            {
                caughtType = ModContent.ItemType<Items.Weapon.Club.BassSlapper>();
            }
        }

		public override void OnHitAnything(float x, float y, Entity victim)
		{
			if(TiteRing && LastEnemyHit == victim && Main.rand.NextBool(10)) {
				player.AddBuff(BuffID.ShadowDodge, 145);
			}

			if(hpRegenRing && LastEnemyHit == victim && Main.rand.NextBool(3)) {
				player.AddBuff(BuffID.RapidHealing, 120);
			}

			if(OriRing && LastEnemyHit == victim && Main.rand.NextBool(10)) {
				if(player.position.Y <= victim.position.Y) {
					float distanceX = player.position.X - victim.position.X;  // change myplayer to nearest player in full version
					float distanceY = player.position.Y - victim.position.Y; // change myplayer to nearest player in full version
					float angle = (float)Math.Atan(distanceX / distanceY);

					distXT = (float)(Math.Sin(angle) * 300);
					distYT = (float)(Math.Cos(angle) * 300);

					distX = player.position.X - distXT;
					distY = player.position.Y - distYT;
				}

				if(player.position.Y > victim.position.Y) {
					float distanceX = player.position.X - victim.position.X;  // change myplayer to nearest player in full version
					float distanceY = player.position.Y - victim.position.Y; // change myplayer to nearest player in full version
					float angle = (float)Math.Atan(distanceX / distanceY);

					distXT = (float)(Math.Sin(angle) * 300);
					distYT = (float)(Math.Cos(angle) * 300);

					distX = (player.position.X + distXT);
					distY = (player.position.Y + distYT);
				}

				Vector2 direction = victim.Center - player.Center;
				direction.Normalize();
				direction.X *= 20f;
				direction.Y *= 20f;

				float A = Main.rand.Next(-100, 100) * 0.01f;
				float B = Main.rand.Next(-100, 100) * 0.01f;

				Projectile.NewProjectile(distX, distY, direction.X + A, direction.Y + B, ModContent.ProjectileType<OriPetal>(), 30, 1, player.whoAmI, 0f, 0f);
			}

			LastEnemyHit = victim;
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			foreach(var effect in effects)
				effect.PlayerOnHitNPC(player, item, target, damage, knockback, crit);

			if(AceOfHearts && target.life <= 0 && crit && !target.friendly && target.lifeMax > 15 && !target.SpawnedFromStatue) {
				ItemUtils.NewItemWithSync(player.whoAmI, (int)target.position.X, (int)target.position.Y, target.width, target.height, Main.halloween ? ItemID.CandyApple : ItemID.Heart);
				for(int i = 0; i < 3; i++) {
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<HeartDust>(), 0, -0.8f);
				}
			}
			if(AceOfDiamonds && target.life <= 0 && crit && !target.friendly && target.lifeMax > 15 && !target.SpawnedFromStatue) {
				ItemUtils.NewItemWithSync(player.whoAmI, (int)target.position.X, (int)target.position.Y, target.width, target.height, ModContent.ItemType<DiamondAce>());
				for(int i = 0; i < 3; i++) {
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<DiamondDust>(), 0, -0.8f);
				}
			}
			if(AceOfClubs && crit && target.lifeMax > 15 && !target.friendly && !target.SpawnedFromStatue && target.type != 488) {
				int money = (int)(300 * MathHelper.Clamp((float)damage / target.lifeMax, 1/300f, 1f));
				for(int i = 0; i < 3; i++) {
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<ClubDust>(), 0, -0.8f);
				}
				if(money / 1000000 > 0) ItemUtils.NewItemWithSync(player.whoAmI, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.PlatinumCoin, money / 1000000);
				money %= 1000000;
				if(money / 10000 > 0) ItemUtils.NewItemWithSync(player.whoAmI, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.GoldCoin, money / 10000);
				money %= 10000;
				if(money / 100 > 0) ItemUtils.NewItemWithSync(player.whoAmI, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.SilverCoin, money / 100);
				money %= 100;
				if(money > 0) ItemUtils.NewItemWithSync(player.whoAmI, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.CopperCoin, money);
			}
			if(astralSet) {
				if(crit) {
					damage = (int)(damage + (.1f * astralSetStacks));
				}
			}
			if(shadowFang) {
				if(target.life <= target.lifeMax / 2 && Main.rand.Next(7) == 0) {
					Projectile.NewProjectile(target.position, Vector2.Zero, ModContent.ProjectileType<ShadowSingeProj>(), item.damage / 3 * 2, 4, Main.myPlayer);
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 6));
					player.statLife -= 3;
				}
			}
			if(bloodyBauble) {
				if(Main.rand.Next(20) <= 1 && player.statLife != player.statLifeMax2) {
					int lifeToHeal = 0;

					if(player.statLife + damage / 4 <= player.statLifeMax2)
						lifeToHeal = damage / 4;
					else
						lifeToHeal = player.statLifeMax2 - player.statLife;

					player.statLife += lifeToHeal;
					player.HealEffect(lifeToHeal);
				}
			}
			if(frigidGloves && crit && item.melee) {
				if(Main.rand.NextBool(2)) {
					target.AddBuff(BuffID.Frostburn, 180);
				}
			}

			if(forbiddenTome) {
				if(target.life <= 0 && !target.SpawnedFromStatue) {
					for(int i = 0; i < 40; i++) {
						int num = Dust.NewDust(target.position, target.width, target.height, 156, 0f, -2f, 117, new Color(0, 255, 142), .6f);
						Main.dust[num].noGravity = true;
						Dust expr_62_cp_0 = Main.dust[num];
						expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((Main.rand.Next(-50, 51) / 20) - 1.5f);
						if(Main.dust[num].position != target.Center) {
							Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
					int upperClamp = (int)MathHelper.Clamp(target.lifeMax, 0, 75);
					int p = Projectile.NewProjectile(target.position, new Vector2(Main.rand.Next(-6, 6), Main.rand.Next(-5, -1)), ModContent.ProjectileType<GhastSkullFriendly>(), (int)MathHelper.Clamp((damage / 5 * 2), 0, upperClamp), knockback, Main.myPlayer);
					if(item.ranged) {
						Main.projectile[p].ranged = true;
					}
					if(item.melee) {
						Main.projectile[p].melee = true;
					}
					if(item.magic) {
						Main.projectile[p].magic = true;
					}
					if(item.summon) {
						Main.projectile[p].minion = true;
					}
				}
			}
			if(icySet && item.magic && Main.rand.NextBool(14)) {
				player.AddBuff(ModContent.BuffType<BlizzardWrath>(), 240);
			}

			if(meleeshadowSet && Main.rand.NextBool(14) && item.melee) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(magicshadowSet && Main.rand.NextBool(14) && item.magic) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(magicshadowSet && Main.rand.NextBool(14) && item.summon) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(rangedshadowSet && Main.rand.NextBool(14) && item.ranged) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(reaperSet && Main.rand.NextBool(15)) {
				target.AddBuff(ModContent.BuffType<FelBrand>(), 160);
			}

			if(magalaSet && Main.rand.NextBool(6)) {
				target.AddBuff(ModContent.BuffType<FrenzyVirus>(), 240);
			}

			if(wheezeScale && Main.rand.NextBool(9) && item.melee) {
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 8f;
				Projectile.NewProjectile(target.Center, vel, ModContent.ProjectileType<Wheeze>(), item.damage / 2, 0, Main.myPlayer);
			}

			if(ToxicExtract && Main.rand.NextBool(5) && item.magic) {
				target.AddBuff(BuffID.Venom, 240);
			}

			if(magalaSet && (item.magic || item.ranged || item.melee) && Main.rand.NextBool(14)) {
				player.AddBuff(ModContent.BuffType<FrenzyVirus1>(), 240);
			}

			if(sunStone && item.melee && Main.rand.NextBool(18)) {
				target.AddBuff(ModContent.BuffType<SunBurn>(), 240);
			}

			if(geodeSet && crit && Main.rand.NextBool(5)) {
				target.AddBuff(ModContent.BuffType<Buffs.Crystal>(), 180);
			}

			if(gremlinBuff && item.melee) {
				target.AddBuff(BuffID.Poisoned, 120);
			}
			if(amazonCharm && item.melee && Main.rand.Next(10) == 0) {
				target.AddBuff(BuffID.Poisoned, 120);
			}
			if(hellCharm && item.melee && Main.rand.Next(10) == 0) {
				target.AddBuff(BuffID.OnFire, 120);
			}
			if(infernalFlame && item.melee) {
				if(crit) {
					if(Main.rand.NextBool(12)) {
						Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<PhoenixProjectile>(), 50, 4, Main.myPlayer);
					}
				}
			}
			if(cryoChestplate && item.melee && Main.rand.NextBool(8)) {
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180);
			}
		}

		int Charger;
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			foreach(var effect in effects)
				effect.PlayerOnHitNPCWithProj(player, proj, target, damage, knockback, crit);

			if(stellarSet) {
				if(proj.minion) {
					if(target.life <= 0) {
						player.AddBuff(ModContent.BuffType<StellarSpeed>(), 300);
					}
				}
				if(!target.SpawnedFromStatue) {
					if(proj.ranged && crit) {
						player.AddBuff(ModContent.BuffType<StellarMinionBonus>(), 360);
					}
				}
			}
			if(shadowFang) {
				if(target.life <= target.lifeMax / 2 && Main.rand.Next(7) == 0) {
					Projectile.NewProjectile(target.position, Vector2.Zero, ModContent.ProjectileType<ShadowSingeProj>(), proj.damage / 3 * 2, 4, Main.myPlayer);
					player.statLife -= 3;
				}
			}
			if(throwerGlove && proj.ranged) {
				throwerStacks++;
			}

			if(bloodyBauble) {
				if(Main.rand.Next(25) <= 1 && player.statLife != player.statLifeMax2) {
					int lifeToHeal = 0;

					if(player.statLife + damage / 4 <= player.statLifeMax2)
						lifeToHeal = damage / 4;
					else
						lifeToHeal = player.statLifeMax2 - player.statLife;

					player.statLife += lifeToHeal;
					player.HealEffect(lifeToHeal);
				}
			}
			if(sacredVine) {
				if(Main.rand.NextBool(20)) {
					player.AddBuff(BuffID.Regeneration, 180);
				}
			}
			if(forbiddenTome) {
				if(target.life <= 0 && !target.SpawnedFromStatue) {
					for(int i = 0; i < 40; i++) {
						int num = Dust.NewDust(target.position, target.width, target.height, 156, 0f, -2f, 117, new Color(0, 255, 142), .6f);
						Main.dust[num].noGravity = true;
						Dust expr_62_cp_0 = Main.dust[num];
						expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						if(Main.dust[num].position != target.Center) {
							Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
					int upperClamp = (int)MathHelper.Clamp(target.lifeMax, 0, 75);
					int p = Projectile.NewProjectile(target.position, new Vector2(Main.rand.Next(-6, 6), Main.rand.Next(-5, -1)), ModContent.ProjectileType<GhastSkullFriendly>(), (int)MathHelper.Clamp((damage / 5 * 2), 0, upperClamp), knockback, Main.myPlayer);
					if(proj.ranged) {
						Main.projectile[p].ranged = true;
					}
					if(proj.melee) {
						Main.projectile[p].melee = true;
					}
					if(proj.magic) {
						Main.projectile[p].magic = true;
					}
					if(proj.minion) {
						Main.projectile[p].minion = true;
					}
				}
			}
			if(reaperSet && Main.rand.NextBool(15)) {
				target.AddBuff(ModContent.BuffType<FelBrand>(), 160);
			}

			if(KingRock && Main.rand.NextBool(5) && proj.magic) {
				Projectile.NewProjectile(player.position.X + Main.rand.Next(-350, 350), player.position.Y - 350, 0, 12, ModContent.ProjectileType<PrismaticBolt>(), 55, 0, Main.myPlayer);
				Projectile.NewProjectile(player.position.X + Main.rand.Next(-350, 350), player.position.Y - 350, 0, 12, ModContent.ProjectileType<PrismaticBolt>(), 55, 0, Main.myPlayer);
			}

			if(geodeSet && crit && Main.rand.NextBool(5)) {
				target.AddBuff(ModContent.BuffType<Buffs.Crystal>(), 180);
			}
			if(amazonCharm && Main.rand.Next(12) == 0) {
				target.AddBuff(BuffID.Poisoned, 120);
			}
			if(hellCharm && Main.rand.Next(12) == 0) {
				target.AddBuff(BuffID.OnFire, 120);
			}
			if(geodeRanged && proj.ranged && Main.rand.NextBool(24)) {
				target.AddBuff(BuffID.Frostburn, 180);
				target.AddBuff(BuffID.OnFire, 180);
				target.AddBuff(BuffID.CursedInferno, 180);
			}

			if(shamanBand && proj.magic && Main.rand.NextBool(9)) {
				target.AddBuff(BuffID.OnFire, 180);
			}

			if(bloodfireSet && proj.magic) {
				if(Main.rand.NextBool(15)) {
					target.AddBuff(ModContent.BuffType<BCorrupt>(), 180);
				}

				if(Main.rand.NextBool(30)) {
					player.statLife += 2;
					player.HealEffect(2);
				}
			}

			if(eyezorEye && proj.magic && crit && Main.rand.NextBool(3)) {
				target.StrikeNPC(40, 0f, 0, crit);
			}

			if(sunStone && proj.melee && Main.rand.NextBool(18)) {
				target.AddBuff(ModContent.BuffType<SunBurn>(), 240);
			}

			if(moonStone && proj.ranged && Main.rand.NextBool(18)) {
				target.AddBuff(ModContent.BuffType<MoonBurn>(), 240);
			}

			if(wheezeScale && Main.rand.NextBool(9) && proj.melee) {
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 8f;
				Projectile.NewProjectile(target.Center, vel, ModContent.ProjectileType<Wheeze>(), Main.hardMode ? 40 : 20, 0, player.whoAmI);
			}

			if(DarkBough && proj.minion) {
				if(Main.rand.NextBool(15)) {
					for(int h = 0; h < 6; h++) {
						Vector2 vel = new Vector2(0, -1);
						float rand = Main.rand.NextFloat() * 6.283f;
						vel = vel.RotatedBy(rand);
						vel *= 8f;
						Projectile.NewProjectile(target.Center, vel, ModContent.ProjectileType<NightmareBarb>(), 29, 1, player.whoAmI);
					}
				}

				if(Main.rand.NextBool(30)) {
					player.statLife += 2;
					player.HealEffect(2);
				}
			}

			if(magazine && proj.ranged && ++Charger > 10) {
				crit = true;
				Charger = 0;
			}

			if(windSet && proj.minion && Main.rand.NextBool(6)) {
				for(int i = 0; i <= 3; i++) {
					Projectile.NewProjectile(target.Center.X, target.Center.Y, Main.rand.Next(-2, 4), -5, ModContent.ProjectileType<DeitySoul2>(), 39, 1, player.whoAmI);
				}
			}

			if(magalaSet && (proj.melee || proj.minion || proj.magic || proj.ranged)) {
				target.AddBuff(ModContent.BuffType<FrenzyVirus>(), 180);
			}

			if(timScroll && proj.magic) {
				switch(Main.rand.Next(12)) {
					case 0:
						target.AddBuff(BuffID.OnFire, 120);
						break;

					case 1:
						target.AddBuff(BuffID.Venom, 120);
						break;

					case 2:
						target.AddBuff(BuffID.CursedInferno, 120);
						break;

					case 3:
						target.AddBuff(BuffID.Frostburn, 120);
						break;

					case 4:
						target.AddBuff(BuffID.Confused, 120);
						break;

					case 5:
						target.AddBuff(BuffID.ShadowFlame, 120);
						break;

					default:
						break;
				}
			}

			if(gremlinBuff) {
				target.AddBuff(BuffID.Poisoned, 120);
			}

			if(Fierysoul && proj.minion && Main.rand.NextBool(14)) {
				target.AddBuff(BuffID.OnFire, 240);
			}

			if(crystalSet && proj.minion && Main.rand.NextBool(15)) {
				target.AddBuff(ModContent.BuffType<SoulBurn>(), 240);
			}

			if(winterbornCharmMage && proj.magic && Main.rand.NextBool(7)) {
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180);
			}
			if(cryoChestplate && proj.melee && Main.rand.NextBool(8)) {
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180);
			}
			if(putridSet && proj.ranged && ++Rangedhits >= 4) {
				Projectile.NewProjectile(proj.position, Vector2.Zero, ModContent.ProjectileType<CursedFlame>(), proj.damage, 0f, proj.owner);
				Rangedhits = 0;
			}

			if(magicshadowSet && Main.rand.NextBool(10) && proj.magic) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(meleeshadowSet && Main.rand.NextBool(10) && proj.melee) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(rangedshadowSet && Main.rand.Next(10) == 2 && (proj.ranged)) {
				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, ModContent.ProjectileType<SpiritShardFriendly>(), 60, 0, Main.myPlayer);
			}

			if(ToxicExtract && Main.rand.NextBool(5) && proj.magic) {
				target.AddBuff(BuffID.Venom, 240);
			}

			if(infernalFlame && proj.melee && crit && Main.rand.NextBool(8)) {
				Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<PhoenixProjectile>(), 50, 4, player.whoAmI);
			}

			if(NebulaPearl && Main.rand.NextBool(8) && proj.magic) {
				Item.NewItem(target.Hitbox, 3454);
			}

			if(hellSet && Main.rand.NextBool(8) && proj.ranged) {
				for(int h = 0; h < 4; h++) {
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 4f;
					Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, ProjectileID.BallofFire, 40, 0, Main.myPlayer);
				}

				Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 0, ModContent.ProjectileType<FireExplosion>(), 39, 0, Main.myPlayer);
			}

			/*if (duneSet && proj.thrown)
            {
                GNPC info = target.GetGlobalNPC<GNPC>();
                if (info.duneSetStacks++ >= 4)
                {
                    player.AddBuff(ModContent.BuffType<DesertWinds>(), 180);
                    Projectile.NewProjectile(player.position.X + 20, player.position.Y, 0, -2, ModContent.ProjectileType<DuneKnife>(), 40, 0, Main.myPlayer);
                    info.duneSetStacks = 0;
                }
            }*/

			if(icySet && proj.magic && Main.rand.NextBool(14)) {
				player.AddBuff(ModContent.BuffType<BlizzardWrath>(), 240);
			}

			if(magalaSet && Main.rand.NextBool(6)) {
				player.AddBuff(ModContent.BuffType<FrenzyVirus1>(), 240);
			}

			if(titanicSet && proj.melee) {
				GNPC info = target.GetGlobalNPC<GNPC>();
				if(info.titanicSetStacks++ >= 4) {
					Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, ModContent.ProjectileType<WaterMass>(), 60, 0, Main.myPlayer);
					info.titanicSetStacks = 0;
				}
			}
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if(Main.rand.NextBool(5) && sepulchreCharm) {
				for(int k = 0; k < 5; k++) {
					Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, 75, 0f, 0f, 100, default, .84f);
				}
				for(int i = 0; i < 200; i++) {
					if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy) {
						int distance = (int)Main.npc[i].Distance(player.Center);
						if(distance < 320) {
							Main.npc[i].AddBuff(BuffID.CursedInferno, 120);
						}
					}
				}
			}

			if(ActiveDash == DashType.Shinigami) {
				return false;
			}

			int index = player.FindBuffIndex(ModContent.BuffType<PhantomVeil>());
			if(index >= 0) {
				player.DelBuff(index);
				Items.Glyphs.VeilGlyph.Block(player);
				veilCounter = 0;

				return false;
			}

			if(bubbleTimer > 0) {
				return false;
			}

			return true;
		}
		public override void ModifyScreenPosition()
        {
            Main.screenPosition.Y += Main.rand.Next(-Shake, Shake);
            Main.screenPosition.X += Main.rand.Next(-Shake, Shake);
            if (Shake > 0) { Shake--; }
        }
		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			foreach(var effect in effects)
				effect.PlayerHurt(player, pvp, quiet, damage, hitDirection, crit);

			veilCounter = 0;

			if(glyph == GlyphType.Daze && Main.rand.NextBool(2)) {
				player.AddBuff(BuffID.Confused, 180);
			}
			if (manaShield)
            {
                damage = damage - (int)damage / 10;
                if (player.statMana > (int)damage / 10 * 4)
                {
                    if ((player.statMana - (int)damage / 10 * 4) > 0)
                    {
                        player.statMana -= (int)damage / 10 * 4;
                    }
					else
                    {
                        player.statMana = 0;
                    }
                }
            }
            if (seraphimBulwark)
            {
                damage = damage - (int)damage/10;
                if (player.statMana > (int)damage / 10)
                {
                    if ((player.statMana - (int)damage / 10 * 4) > 0)
                    {
                        player.statMana -= (int)damage / 10;
                    }
                    else
                    {
                        player.statMana = 0;
                    }
                }

            }
            if (rogueSet && !player.HasBuff(ModContent.BuffType<RogueCooldown>())) {
				player.AddBuff(BuffID.Invisibility, 260);
				player.AddBuff(ModContent.BuffType<RogueCooldown>(), 1520);
			}

			if(leatherSet) {
				concentratedCooldown = 360;
				concentrated = false;
			}

			if(cryoSet) {
				cryoTimer = 0;
			}

			if(SRingOn) {
				for(int h = 0; h < 3; h++) {
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
					vel = vel.RotatedBy(rand);
					vel *= 2f;
					Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, ProjectileID.LostSoulFriendly, 45, 0, Main.myPlayer);
				}
			}

			if(moonHeart) {
				int n = 5;
				for(int i = 0; i < n; i++) {
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
					vel = vel.RotatedBy(rand);
					vel *= 5f;
					Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, ModContent.ProjectileType<AlienSpit>(), 65, 0, Main.myPlayer);
				}
			}

			if(cryoSet) {
				Main.PlaySound(SoundID.Item, player.position, 50);
			}

			if(ChaosCrystal && Main.rand.Next(4) == 1) {
				bool canSpawn = false;
				int teleportStartX = (int)(Main.player[Main.myPlayer].position.X / 16) - 35;
				int teleportRangeX = 70;
				int teleportStartY = (int)(Main.player[Main.myPlayer].position.Y / 16) - 35;
				int teleportRangeY = 70;
				Vector2 vector2 = TestTeleport(ref canSpawn, teleportStartX, teleportRangeX, teleportStartY, teleportRangeY);

				if(canSpawn) {
					Vector2 newPos = vector2;
					Main.player[Main.myPlayer].Teleport(newPos, 2, 0);
					Main.player[Main.myPlayer].velocity = Vector2.Zero;
					Main.PlaySound((int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, 27);
					Main.PlaySound(SoundID.Item, (int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, 8);
					if(Main.netMode == NetmodeID.Server) {
						RemoteClient.CheckSection(Main.myPlayer, Main.player[Main.myPlayer].position, 1);
						NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, Main.myPlayer, newPos.X, newPos.Y, 3, 0, 0);
					}
				}

			}

			// IRIAZUL
			if(veinstoneSet && Main.rand.NextBool(8)) {
				int amount = Main.rand.Next(2, 5);
				for(int i = 0; i < amount; ++i) {
					Vector2 position = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201), player.Center.Y - 600f);
					position.X = (position.X * 10f + player.position.X) / 11f + Main.rand.Next(-100, 101);
					position.Y -= 150;

					float speedX = player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
					float speedY = player.Center.Y - position.Y;
					if(speedY < 0f) {
						speedY *= -1f;
					}

					if(speedY < 20f) {
						speedY = 20f;
					}

					float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
					length = 12 / length;

					speedX *= length;
					speedY *= length;
					speedX += Main.rand.Next(-40, 41) * 0.03f;
					speedY += Main.rand.Next(-40, 41) * 0.03f;
					speedX *= Main.rand.Next(75, 150) * 0.01f;

					position.X += Main.rand.Next(-50, 51);
					Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<VeinstoneBlood>(), 40, 1, player.whoAmI);
				}
			}

			if(infernalSet && Main.rand.NextBool(10)) {
				Projectile.NewProjectile(player.position, new Vector2(0, -2), ModContent.ProjectileType<InfernalBlast>(), 50, 7, Main.myPlayer);
			}

			if(starCharm) {
				int amount = Main.rand.Next(4, 6);
				for(int i = 0; i < amount; ++i) {
					Vector2 position = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201), player.Center.Y - 600f);
					position.X = (position.X * 10f + player.position.X) / 11f + Main.rand.Next(-100, 101);
					position.Y -= 150;

					float speedX = player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
					float speedY = player.Center.Y - position.Y;
					if(speedY < 0f) {
						speedY *= -1f;
					}

					if(speedY < 20f) {
						speedY = 20f;
					}

					float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
					length = 12 / length;

					speedX *= length;
					speedY *= length;
					speedX = speedX + Main.rand.Next(-40, 41) * 0.03f;
					speedY = speedY + Main.rand.Next(-40, 41) * 0.03f;
					speedX *= Main.rand.Next(75, 150) * 0.01f;

					position.X += Main.rand.Next(-50, 51);
					Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<Starshock1>(), 35, 1, player.whoAmI);
				}
			}

			if(gremlinTooth && Main.rand.NextBool(3)) {
				player.AddBuff(ModContent.BuffType<ToothBuff>(), 300);
			}

			if(starMap && Main.rand.NextBool(2)) {
				int amount = Main.rand.Next(2, 3);
				for(int i = 0; i < amount; ++i) {
					Vector2 position = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(-300, 301), player.Center.Y - 800f);
					position.X = (position.X * 10f + player.position.X) / 11f + Main.rand.Next(-100, 101);
					position.Y -= 150;

					float speedX = player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
					float speedY = player.Center.Y - position.Y;
					if(speedY < 0f) {
						speedY *= -1f;
					}

					if(speedY < 30f) {
						speedY = 30f;
					}

					float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
					length = 12 / length;

					speedX *= length;
					speedY *= length;
					speedX = speedX + Main.rand.Next(-40, 41) * 0.03f;
					speedY = speedY + Main.rand.Next(-40, 41) * 0.03f;
					speedX *= Main.rand.Next(75, 150) * 0.01f;

					position.X += Main.rand.Next(-10, 11);
					int p = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<Starshock1>(), 24, 1, player.whoAmI);
					Main.projectile[p].timeLeft = 600;
				}
			}

			if(Bauble && player.statLife < (player.statLifeMax2 >> 1) && baubleTimer <= 0) {
				Projectile.NewProjectile(Main.player[Main.myPlayer].Center, Vector2.Zero, ModContent.ProjectileType<IceReflector>(), 0, 0, Main.myPlayer);
				player.endurance += .30f;
				baubleTimer = 7200;
			}

			if(OverseerCharm) {
				for(int h = 0; h < 8; h++) {
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 4f;
					int proj = Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, ModContent.ProjectileType<SpiritShardFriendly>(), 250, 0, Main.myPlayer);
				}
			}

			if(mythrilCharm && Main.rand.NextBool(2)) {
				int mythrilCharmDamage = (int)(damage / 4);
				if(mythrilCharmDamage < 1) {
					mythrilCharmDamage = 5;
				}

				Rectangle mythrilCharmCollision = new Rectangle((int)player.Center.X - 120, (int)player.Center.Y - 120, 240, 240);
				for(int i = 0; i < 200; ++i) {
					if(Main.npc[i].active && Main.npc[i].Hitbox.Intersects(mythrilCharmCollision)) {
						Main.npc[i].StrikeNPCNoInteraction(mythrilCharmDamage, 0, 0);
					}
				}

				for(int i = 0; i < 15; ++i) {
					Dust.NewDust(new Vector2(mythrilCharmCollision.X, mythrilCharmCollision.Y), mythrilCharmCollision.Width, mythrilCharmCollision.Height, DustID.LunarOre);
				}
			}
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			if(SRingOn) {
				int newProj = Projectile.NewProjectile(player.Center, new Vector2(6, 6), ProjectileID.SpectreWrath, 40, 0f, Main.myPlayer);

				int dist = 800;
				int target = -1;
				for(int i = 0; i < 200; ++i) {
					if(Main.npc[i].active && Main.npc[i].CanBeChasedBy(Main.projectile[newProj], false)) {
						if((Main.npc[i].Center - Main.projectile[newProj].Center).Length() < dist) {
							target = i;
							break;
						}
					}
				}

				Main.projectile[newProj].ai[0] = target;
			}

			if(cryoSet) {
				quiet = true;
			}

			if(Fierysoul) {
				Projectile.NewProjectile(player.Center, new Vector2(6, 6), ProjectileID.MolotovFire2, 30, 0f, Main.myPlayer);
			}

			if(soulPotion && Main.rand.NextBool(5)) {
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<SoulPotionWard>(), 0, 0f, Main.myPlayer);
			}

			if(spiritBuff && Main.rand.NextBool(3)) {
				int newProj = Projectile.NewProjectile(player.Center, new Vector2(6, 6), ModContent.ProjectileType<StarSoul>(), 40, 0f, Main.myPlayer);

				int dist = 800;
				int target = -1;
				for(int i = 0; i < 200; ++i) {
					if(Main.npc[i].active && Main.npc[i].CanBeChasedBy(Main.projectile[newProj], false)) {
						if((Main.npc[i].Center - Main.projectile[newProj].Center).Length() < dist) {
							target = i;
							break;
						}
					}
				}
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if(fateToken) {
				player.statLife = 500;
				timeLeft = 0;
				MyPlayer myPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
				Rectangle textPos = new Rectangle((int)myPlayer.player.position.X, (int)myPlayer.player.position.Y - 60, myPlayer.player.width, myPlayer.player.height);
				CombatText.NewText(textPos, new Color(255, 240, 0, 100), "Fate has protected you!");
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<Shockwave>(), 0, 0, player.whoAmI);
				fateToken = false;
				return false;
			}
			if(illusionistEye) {
				if(illusionistTimer <= 0) {
					for(int index3 = 0; index3 < 100; ++index3) {
						NPC npc = Main.npc[index3];
						if(!npc.boss) {
							Main.PlaySound(new Terraria.Audio.LegacySoundStyle(29, 53));
							illusionistTimer = 36000;
							player.statLife += 20;
							for(int i = 0; i < 12; i++) {
								int num = Dust.NewDust(player.position, player.width, player.width, 156, 0f, -2f, 0, new Color(0, 255, 142), 2f);
								Main.dust[num].noGravity = true;
								Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
								Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
								Main.dust[num].scale *= .25f;
								if(Main.dust[num].position != player.Center)
									Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
							}
							player.grappling[0] = -1;
							player.grapCount = 0;
							for(int num373 = 0; num373 < 1000; num373++) {
								if(Main.projectile[num373].active && Main.projectile[num373].owner == Main.myPlayer && Main.projectile[num373].aiStyle == 7) {
									Main.projectile[num373].Kill();
								}
							}
							bool flag33 = player.immune;
							int num372 = player.immuneTime;
							player.Spawn();
							player.immune = flag33;
							player.immuneTime = num372;
							for(int i = 0; i < 6; i++) {
								int num = Dust.NewDust(player.position, player.width, player.width, 156, 0f, -2f, 0, new Color(0, 255, 142), 2f);
								Main.dust[num].noGravity = true;
								Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
								Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
								Main.dust[num].scale *= .25f;
								if(Main.dust[num].position != player.Center)
									Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
							}
						}
					}
				}
			}
			if(bubbleShield) {
				for(int i = 3; i < 8 + player.extraAccessorySlots; i++) {
					int type = player.armor[i].type;
					if(type == ModContent.ItemType<BubbleShield>()) {
						player.armor[i].SetDefaults(0);

						break;
					}
				}
				player.statLife = 150;
				bubbleTimer = 360;
				return false;
			}

			if(clatterboneSet && clatterboneTimer <= 0) {
				player.AddBuff(ModContent.BuffType<Sturdy>(), 21600);
				MyPlayer myPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
				Rectangle textPos = new Rectangle((int)myPlayer.player.position.X, (int)myPlayer.player.position.Y - 60, myPlayer.player.width, myPlayer.player.height);
				CombatText.NewText(textPos, new Color(100, 240, 0, 100), "Sturdy Activated!");

				player.statLife += (int)damage;
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<Shockwave>(), 0, 0, player.whoAmI);
				clatterboneTimer = 21600; // 6 minute timer.

				return false;
			}

			if(damageSource.SourceOtherIndex == 8) {
				CustomDeath(ref damageSource);
			}

			return true;
		}

		private void CustomDeath(ref PlayerDeathReason reason)
		{
			if(player.FindBuffIndex(ModContent.BuffType<BurningRage>()) >= 0) {
				reason = PlayerDeathReason.ByCustomReason(player.name + " was consumed by Rage.");
			}
		}

		public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			/*if(throwerGlove && throwerStacks >= 7) {
				damage += (int)(damage / 4f + 0.5f);
				knockBack += 2;
				firedSharpshooter = true;
				Main.NewText("glove damage");
			}*/
			return true;
		}

		int shroomtimer;
		int bloodTimer;
		int spawnTimer;
		public override void PreUpdate()
		{
			if(TideWorld.TheTide && !NPC.AnyNPCs(ModContent.NPCType<Rylheian>()) && player.ZoneBeach && TideWorld.TidePoints >= 99) {
				spawnTimer++;
				if(spawnTimer >= 30) {
					spawnTimer = 0;
					int n = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 400, ModContent.NPCType<Rylheian>(), 0, 2, 1, 0, 0, Main.myPlayer);
					Main.PlaySound(SoundID.Zombie, Main.npc[n].Center, 89);
					DustHelper.DrawDiamond(new Vector2(Main.npc[n].Center.X, Main.npc[n].Center.Y), 173, 8);
					DustHelper.DrawTriangle(new Vector2(Main.npc[n].Center.X, Main.npc[n].Center.Y), 173, 8);
				}
			}
			if(emptyWinterbornScroll && MyWorld.numWinterbornKilled >= 10) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<WinterbornSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numWinterbornKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<WinterbornSlayerScrollFull>());
						emptyWinterbornScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyAntlionScroll && MyWorld.numAntlionsKilled >= 5) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<AntlionSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numAntlionsKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<AntlionSlayerScrollFull>());
						emptyAntlionScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyStardancerScroll && MyWorld.numStardancersKilled >= 3) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<StardancerSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numStardancersKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<StardancerSlayerScrollFull>());
						emptyStardancerScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyBeholderScroll && MyWorld.numBeholdersKilled > 0) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<BeholderSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numBeholdersKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<BeholderSlayerScrollFull>());
						emptyBeholderScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyDrBonesScroll && MyWorld.numDrBonesKilled >= 1) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<DrBonesSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numDrBonesKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<DrBonesSlayerQuestFull>());
						emptyDrBonesScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyValkyrieScroll && MyWorld.numValkyriesKilled > 0) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<ValkyrieSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numValkyriesKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<ValkyrieSlayerScrollFull>());
						emptyValkyrieScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyWheezerScroll && MyWorld.numWheezersKilled >= 12) {
				for(int index = 0; index < 58; ++index) {
					if(player.inventory[index].type == ModContent.ItemType<WheezerSlayerScrollEmpty>()) {
						player.inventory[index].stack -= 1;
						MyWorld.numWheezersKilled = 0;
						player.QuickSpawnItem(ModContent.ItemType<WheezerSlayerScrollFull>());
						emptyWheezerScroll = false;
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Contract Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerComplete"));
						break;
					}
				}
			}
			if(emptyExplorerScroll) {
				if(player.ZoneGlowshroom) {
					explorerTimer++;
					if(explorerTimer >= 900) {
						for(int index = 0; index < 58; ++index) {
							if(player.inventory[index].type == ModContent.ItemType<ExplorerScrollMushroomEmpty>()) {
								emptyExplorerScroll = false;
								explorerTimer = 0;
								player.inventory[index].stack -= 1;
								player.QuickSpawnItem(ModContent.ItemType<ExplorerScrollMushroomFull>());
								CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
								"Map Filled!");
								Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MapComplete"));
								break;
							}
						}
					}
				}
				if(ZoneAsteroid) {
					explorerTimer++;
					if(explorerTimer >= 900) {
						for(int index = 0; index < 58; ++index) {
							if(player.inventory[index].type == ModContent.ItemType<ExplorerScrollAsteroidEmpty>()) {
								emptyExplorerScroll = false;
								explorerTimer = 0;
								player.inventory[index].stack -= 1;
								player.QuickSpawnItem(ModContent.ItemType<ExplorerScrollAsteroidFull>());
								CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
								"Map Filled!");
								Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MapComplete"));
								break;
							}
						}
					}
				}
				if(ZoneHive) {
					explorerTimer++;
					if(explorerTimer >= 900) {
						for(int index = 0; index < 58; ++index) {
							if(player.inventory[index].type == ModContent.ItemType<ExplorerScrollHiveEmpty>()) {
								emptyExplorerScroll = false;
								explorerTimer = 0;
								player.inventory[index].stack -= 1;
								player.QuickSpawnItem(ModContent.ItemType<ExplorerScrollHiveFull>());
								CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
								"Map Filled!");
								Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MapComplete"));
								break;
							}
						}
					}
				}
				if(ZoneGranite) {
					explorerTimer++;
					if(explorerTimer >= 900) {
						for(int index = 0; index < 58; ++index) {
							if(player.inventory[index].type == ModContent.ItemType<ExplorerScrollGraniteEmpty>()) {
								emptyExplorerScroll = false;
								explorerTimer = 0;
								player.inventory[index].stack -= 1;
								player.QuickSpawnItem(ModContent.ItemType<ExplorerScrollGraniteFull>());
								CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
								"Map Filled!");
								Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MapComplete"));
								break;
							}
						}
					}
				}
				if(ZoneMarble) {
					explorerTimer++;
					if(explorerTimer >= 900) {
						for(int index = 0; index < 58; ++index) {
							if(player.inventory[index].type == ModContent.ItemType<ExplorerScrollMarbleEmpty>()) {
								emptyExplorerScroll = false;
								explorerTimer = 0;
								player.inventory[index].stack -= 1;
								player.QuickSpawnItem(ModContent.ItemType<ExplorerScrollMarbleFull>());
								CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
								"Map Filled!");
								Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MapComplete"));
								break;
							}
						}
					}
				}
			} else {
				explorerTimer = 0;
			}
			if(zipline) {
				if(!ziplineActive) {
					ziplineCounter = 45;
					ziplineActive = true;
				}
				player.justJumped = true;
				float g = 0.18f;
				if(ziplineCounter * g * ziplineY < 20 && ziplineCounter < 400)
					ziplineCounter += 2;
				Dust.NewDustPerfect(new Vector2(player.position.X + Main.rand.Next(player.width), player.position.Y + player.height - Main.rand.Next(7)), 6, new Vector2(-ziplineX * Main.rand.Next(6), -ziplineY * Main.rand.Next(10)));
				player.velocity = MathHelper.Max(ziplineCounter * g * ziplineY, 5) * new Vector2(ziplineX, ziplineY);
			} else if(ziplineCounter > 45) {
				ziplineCounter -= 0.75f;
			}
			if(mushroomPotion) {
				shroomtimer++;
				if(shroomtimer >= 20 && player.velocity != Vector2.Zero) {
					shroomtimer = 0;
					int proj = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ProjectileID.Mushroom, 10, 1, Main.myPlayer, 0.0f, 1);
					Main.projectile[proj].timeLeft = 120;
				}
			}
			if(bloodCourtHead) {
				bloodTimer++;
				if(bloodTimer >= 40) {
					bloodTimer = 0;
					int proj = Projectile.NewProjectile(player.Center.X + Main.rand.Next(-30, 30), player.Center.Y - Main.rand.Next(40, 50), Main.rand.Next(-1, 1), -1, ModContent.ProjectileType<BloodRuneEffect>(), 0, 0, Main.myPlayer, 0.0f, 1);
				}
			}
			if(MyWorld.meteorShowerWeather && Main.rand.Next(270) == 0 && ZoneAsteroid) {
				Vector2 vector2_1 = new Vector2((float)(player.position.X + player.width * 0.5 + (Main.rand.Next(201) * -player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X)), (float)(player.position.Y + player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
				vector2_1.X = (float)((vector2_1.X + player.Center.X) / 2.0) + Main.rand.Next(-200, 201);
				vector2_1.Y -= 100;
				float num12 = Main.rand.Next(-30, 30);
				float num13 = 100;
				if(num13 < 0) num13 *= -1f;
				if(num13 < 20) num13 = 20f;
				float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
				float num15 = 10 / num14;
				float num16 = num12 * num15;
				float num17 = num13 * num15;
				float SpeedX = num16 + Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile X position speed and randomnes
				float SpeedY = num17 + Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
				int proj = Projectile.NewProjectile(player.Center.X + Main.rand.Next(-1000, 1000), player.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, ModContent.ProjectileType<Projectiles.Hostile.Meteor>(), 30, 3, Main.myPlayer, 0.0f, 1);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = true;
			}

			if(!throwerGlove || (throwerStacks >= 7 && firedSharpshooter)) {
				throwerStacks = 0;
				firedSharpshooter = false;
			}

			if(ShieldCore) {
				if(!spawnedShield) {
					//shootingCrystal = false;
					Player player = Main.player[Main.myPlayer];
					int num = shieldsLeft;
					for(int i = 0; i < num; i++) {
						int DegreeDifference = 360 / num;
						Projectile.NewProjectile((int)player.Center.X + (int)(Math.Sin(i * DegreeDifference) * 80), (int)player.Center.Y + (int)(Math.Sin(i * DegreeDifference) * 80), 0, 0, ModContent.ProjectileType<InterstellarShield>(), 1, 0, player.whoAmI, 0, i * DegreeDifference);
					}
					spawnedShield = true;

				}
				if(shieldsLeft == 0) {
					shieldCounter++;
					if(shieldCounter > 2000) {
						shieldsLeft = 2;
						spawnedShield = false;
						shieldCounter = 0;
					}
				}
			} else {
				spawnedShield = false;
			}

			if(ZoneAsteroid && MyWorld.stardustWeather) {
				int d = Main.rand.Next(new int[] { 180, 226, 206 });
				{
					//int maxValue = 800;
					int num1 = (int)((int)(500.0 * (Main.screenWidth / (float)Main.maxScreenW)) * (1.0 + 2.0 * Main.cloudAlpha));
					float num2 = (float)(1.0 + 50.0 * Main.cloudAlpha);
					if(Main.rand.Next(10) == 0) {
						int num3 = Main.rand.Next(Main.screenWidth + 1000) - 500;
						int num4 = (int)Main.screenPosition.Y - Main.rand.Next(50);
						if(Main.player[Main.myPlayer].velocity.Y > 0.0)
							num4 -= (int)Main.player[Main.myPlayer].velocity.Y;
						if(Main.rand.Next(5) == 0)
							num3 = Main.rand.Next(500) - 500;
						else if(Main.rand.Next(5) == 0)
							num3 = Main.rand.Next(500) + Main.screenWidth;
						if(num3 < 0 || num3 > Main.screenWidth)
							num4 += Main.rand.Next((int)(Main.screenHeight * 0.8)) + (int)(Main.screenHeight * 0.1);
						int num5 = num3 + (int)Main.screenPosition.X;
						int index2 = num5 / 16;
						int index3 = num4 / 16;
						if(Main.tile[index2, index3] != null) {
							if(Main.tile[index2, index3].wall == 0) {
								int index4 = Dust.NewDust(new Vector2(num5, num4), 10, 10, d, 0.0f, 0.0f, 0, new Color(), 1f);
								Main.dust[index4].scale += Main.cloudAlpha * 0.2f;
								Main.dust[index4].velocity.Y = (float)(3.0 + Main.rand.Next(30) * 0.100000001490116);
								Main.dust[index4].velocity.Y *= Main.dust[index4].scale;
								if(!Main.raining) {
									Main.dust[index4].velocity.X = Main.windSpeed + Main.rand.Next(-10, 10) * 0.1f;
									Main.dust[index4].velocity.X += (float)(Main.windSpeed * Main.cloudAlpha * 10.0);
								} else {
									Main.dust[index4].velocity.X = (float)(Math.Sqrt(Math.Abs(Main.windSpeed)) * Math.Sign(Main.windSpeed) * (Main.cloudAlpha + 0.5) * 25.0 + (double)Main.rand.NextFloat() * 0.200000002980232 - 0.100000001490116);
									Main.dust[index4].velocity.Y *= 0.5f;
								}
								Main.dust[index4].velocity.Y *= (float)(1.0 + 0.300000011920929 * Main.cloudAlpha);
								Main.dust[index4].scale += Main.cloudAlpha * 0.2f;
								Main.dust[index4].velocity *= (float)(1.0 + Main.cloudAlpha * 0.5);
							}
						}
					}
				}
			}
			if(ZoneAsteroid && MyWorld.spaceJunkWeather && Main.rand.Next(59) == 0) {
				Vector2 vector2_1 = new Vector2((float)(player.position.X + player.width * 0.5 + (Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position

				string[] smallDebris = { "SpaceDebris1", "SpaceDebris2", "MeteorShard3", "MeteorShard4" };
				int small = Main.rand.Next(smallDebris.Length);
				string[] bigDebris = { "SpaceDebris3", "SpaceDebris4", "MeteorShard5", "MeteorShard6" };
				int big = Main.rand.Next(bigDebris.Length);
				vector2_1.X = (float)(((double)vector2_1.X + player.Center.X) / 2.0) + Main.rand.Next(-200, 201);
				vector2_1.Y -= 100;
				float num12 = Main.rand.Next(-30, 30);
				float num13 = 100;
				if(num13 < 0.0) num13 *= -1f;
				if(num13 < 20.0) num13 = 20f;
				float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
				float num15 = 10 / num14;
				float num16 = num12 * num15;
				float num17 = num13 * num15;
				float SpeedX = num16 + Main.rand.Next(-40, 41) * Main.windSpeed + (.01f * Main.windSpeed);  //this defines the projectile X position speed and randomnes
				float SpeedY = num17 + Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
				if(Main.rand.Next(7) == 0) {
					int proj = Projectile.NewProjectile(player.Center.X + Main.rand.Next(-1000, 1000), player.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, mod.ProjectileType(bigDebris[big]), 16, 3, Main.myPlayer, 0.0f, 1);
				} else {
					int proj1 = Projectile.NewProjectile(player.Center.X + Main.rand.Next(-1000, 1000), player.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, mod.ProjectileType(smallDebris[small]), 7, 3, Main.myPlayer, 0.0f, 1);
				}
			}
			if(ZoneReach && !Main.raining && !MyWorld.downedReachBoss) {
				Main.cloudAlpha += .007f;
				if(Main.cloudAlpha >= .65f) {
					Main.cloudAlpha = .65f;
				}
			}
			if(illusionistEye) {
				illusionistTimer--;
				if(illusionistTimer == 0) {
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
					for(int i = 0; i < 6; i++) {
						int num = Dust.NewDust(player.position, player.width, player.height, 156, 0f, -2f, 0, new Color(0, 255, 142), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != player.Center)
							Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
			if(fierySet) {
				fierySetTimer--;
			} else {
				fierySetTimer = 480;
			}
			if(surferSet) {
				surferTimer--;
			} else {
				surferTimer = 330;
			}
			if(fierySetTimer == 0) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
				for(int i = 0; i < 2; i++) {
					int num = Dust.NewDust(player.position, player.width, player.height, 6, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if(Main.dust[num].position != player.Center)
						Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			if(marbleSet) {
				marbleJump--;
				marbleJustJumped = true;
			}
			if(marbleJump == 0) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
				for(int i = 0; i < 2; i++) {
					int num = Dust.NewDust(player.position, player.width, player.height, 222, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if(Main.dust[num].position != player.Center)
						Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			if(!marbleSet) {
				marbleJustJumped = false;
			}
			if((player.velocity.Y == 0f || player.sliding || (player.autoJump && player.justJumped)) && marbleJustJumped) {
				marbleJustJumped = true;
			}
			if(graniteSet) {
				int num323;
				int num326;
				if(player.velocity.Y == 0f && player.HasBuff(ModContent.BuffType<GraniteBonus>()) && !player.mount.Active) {
					num326 = 1;
					num326 += player.extraFall;
					num323 = (int)((player.position.Y / 16f) - player.fallStart) / 2;
					if(num323 >= 8) {
						num323 = 8;
					}
					{
						if(player.gravDir == 1f && num323 > num326) {
							player.ClearBuff(ModContent.BuffType<GraniteBonus>());
							Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
							{
								for(int i = 0; i < 8 * num323; i++) {
									int num = Dust.NewDust(player.position, player.width, player.height, 226, 0f, -2f, 0, default, 2f);
									Main.dust[num].noGravity = true;
									Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
									Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
									Main.dust[num].scale *= .25f;
									if(Main.dust[num].position != player.Center)
										Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
								}
							}
							int proj = Projectile.NewProjectile(player.position.X, player.position.Y,
								0, 0, ModContent.ProjectileType<GraniteSpike1>(), num323 * 10, 0, player.whoAmI);
							Projectile.NewProjectile(player.position.X, player.position.Y,
							   0, 0, ModContent.ProjectileType<StompExplosion>(), num323 * 10, 6, player.whoAmI);
							Main.projectile[proj].timeLeft = 0;
							Main.projectile[proj].ranged = true;

						}
					}
					stompCooldown = 540;
				}
				stompCooldown--;
				if(stompCooldown == 0) {
					Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
					CombatText.NewText(textPos, new Color(82, 226, 255, 100), "Energy Stomp Ready!");
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
					for(int i = 0; i < 2; i++) {
						int num = Dust.NewDust(player.position, player.width, player.height, 226, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != player.Center)
							Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
					}

				}
			}
			if(!Main.dayTime && MyWorld.dayTimeSwitched) {
				candyInBowl = 2;
				candyFromTown.Clear();
			}
			if(ZoneAsteroid) {
				Main.numCloudsTemp = 0;
			}
			if(Main.rand.NextBool(6) && ZoneReach && player.ZoneOverworldHeight && !player.ZoneBeach && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneJungle && !player.ZoneHoly) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(new Vector2(player.Center.X + Main.rand.Next(-1000, 1000), player.Center.Y + (Main.rand.Next(-1000, -100))), new Vector2(Main.windSpeed * 3f, 0f), 911, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 0f;
				Main.gore[a].velocity = new Vector2(Main.windSpeed * 40f, Main.rand.NextFloat(0.2f, 2f));
			}
			if(Main.rand.Next(9) == 0 && ZoneReach && player.ZoneOverworldHeight && !player.ZoneBeach && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneJungle && !player.ZoneHoly) {
				float goreScale = Main.rand.NextFloat(0.5f, 0.9f);
				int x = (int)(Main.windSpeed > 0 ? Main.screenPosition.X - 100 : Main.screenPosition.X + Main.screenWidth + 100);
				int y = (int)Main.screenPosition.Y + Main.rand.Next(-100, Main.screenHeight);
				int a = Gore.NewGore(new Vector2(x, y), Vector2.Zero, mod.GetGoreSlot("Gores/GreenLeaf"), goreScale);
				Main.gore[a].rotation = 0f;
				Main.gore[a].velocity.Y = Main.rand.NextFloat(1f, 3f);
			}

			if(windEffect) {
				if(Main.windSpeed <= -.01f) {
					Main.windSpeed = -.8f; ;
				}

				if(Main.windSpeed >= .01f) {
					Main.windSpeed = .8f;
				}
			}

			CalculateSpeed();

			if(player.whoAmI == Main.myPlayer) {
				if(!player.HeldItem.IsAir) {
					glyph = player.HeldItem.GetGlobalItem<Items.GItem>().Glyph;
					if(glyph == GlyphType.None && player.nonTorch >= 0 && player.nonTorch != player.selectedItem) {
						if(!player.inventory[player.nonTorch].IsAir) {
							glyph = player.inventory[player.nonTorch].GetGlobalItem<Items.GItem>().Glyph;
						}
					}
				} else {
					glyph = GlyphType.None;
				}

				if(Main.netMode == NetmodeID.MultiplayerClient) {
					ModPacket packet = SpiritMod.instance.GetPacket(MessageType.PlayerGlyph, 2);
					packet.Write((byte)Main.myPlayer);
					packet.Write((byte)glyph);
					packet.Send();
				}
			}

			if(glyph == GlyphType.Bee) {
				player.AddBuff(BuffID.Honey, 2);
			} else if(glyph == GlyphType.Phase) {
				if(phaseStacks < 3) {
					phaseCounter++;
					if(phaseCounter >= 12 * 60) {
						phaseCounter = 0;
						phaseStacks++;
						player.AddBuff(ModContent.BuffType<TemporalShift>(), 2);
					}
				}
			} else if(glyph == GlyphType.Veil) {
				veilCounter++;
				if(veilCounter >= 8 * 60) {
					veilCounter = 0;
					player.AddBuff(ModContent.BuffType<PhantomVeil>(), 2);
				}
			} else if(glyph == GlyphType.Void) {
				Items.Glyphs.VoidGlyph.DevouringVoid(player);
			} else if(glyph == GlyphType.Radiant) {
				divineCounter++;
				if(divineCounter >= 60) {
					divineCounter = 0;
					player.AddBuff(ModContent.BuffType<DivineStrike>(), 2);
				}
			}
			if(phaseStacks > 3) {
				phaseStacks = 3;
			}
			if(icytrail && player.velocity.X != 0) {
				Projectile.NewProjectile(player.position.X, player.position.Y + 40, 0f, 0f, ModContent.ProjectileType<FrostTrail>(), 35, 0f, player.whoAmI);
			}

			if(flametrail && player.velocity.X != 0) {
				Projectile.NewProjectile(player.position.X, player.position.Y + 40, 0f, 0f, ModContent.ProjectileType<CursedFlameTrail>(), 35, 0f, player.whoAmI);
			}

			if(CrystalShield && player.velocity.X != 0 && Main.rand.NextBool(3)) {
				if(player.velocity.X < 0) {
					Projectile.NewProjectile(player.position.X, player.Center.Y, Main.rand.Next(6, 10), Main.rand.Next(-3, 3), ProjectileID.CrystalShard, 36, 0f, player.whoAmI);
				}

				if(player.velocity.X > 0) {
					Projectile.NewProjectile(player.position.X, player.Center.Y, Main.rand.Next(-10, -6), Main.rand.Next(-3, 3), ProjectileID.CrystalShard, 36, 0f, player.whoAmI);
				}
			}
		}

		private void CalculateSpeed()
		{
			//Mimics the Stopwatch accessory
			float slice = player.velocity.Length();
			int count = (int)(1f + slice * 6f);
			if(count > phaseSlice.Length) {
				count = phaseSlice.Length;
			}

			for(int i = count - 1; i > 0; i--) {
				phaseSlice[i] = phaseSlice[i - 1];
			}

			phaseSlice[0] = slice;
			float inverse = 1f / count;
			float sum = 0f;
			for(int n = 0; n < phaseSlice.Length; n++) {
				if(n < count) {
					sum += phaseSlice[n];
				} else {
					phaseSlice[n] = sum * inverse;
				}
			}

			sum *= inverse;
			float boost = sum * (216000 / 42240f);
			if(!player.merman && !player.ignoreWater) {
				if(player.honeyWet) {
					boost *= .25f;
				} else if(player.wet) {
					boost *= .5f;
				}
			}

			SpeedMPH = (float)Math.Round(boost);
		}

		public override void UpdateBadLifeRegen()
		{
			int before = player.lifeRegen;
			bool drain = false;

			if(DoomDestiny) {
				drain = true;
				player.lifeRegen -= 16;
			}

			if(blazeBurn) {
				drain = true;
				player.lifeRegen -= 10;
			}

			if(drain && before > 0) {
				player.lifeRegenTime = 0;
				player.lifeRegen -= before;
			}
		}

		public override void UpdateLifeRegen()
		{
			if(glyph == GlyphType.Sanguine) {
				player.lifeRegen += 4;
			}
		}

		public override void NaturalLifeRegen(ref float regen)
		{
			// Last hook before player.DashMovement
			DashType dash = FindDashes();
			if(dash != DashType.None) {
				// Prevent vanilla dashes
				player.dash = 0;

				if(player.pulley) {
					DashMovement(dash);
				}
			}
		}

		private void DashEnd()
		{
			if(ActiveDash == DashType.Shinigami) {
				player.itemAnimation = 0;
			}
		}

		private void DashMovement(DashType dash)
		{
			if(player.dashDelay > 0) {
				if(ActiveDash != DashType.None) {
					DashEnd();
					ActiveDash = DashType.None;
				}
			} else if(player.dashDelay < 0) {
				// Powered phase
				// Manage dash abilities here
				float speedCap = 20f;
				float decayCapped = 0.992f;
				float speedMax = Math.Max(player.accRunSpeed, player.maxRunSpeed);
				float decayMax = 0.96f;
				int delay = 20;
				if(ActiveDash == DashType.Phase) {
					for(int k = 0; k < 2; k++) {
						int dust;
						if(player.velocity.Y == 0f) {
							dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 1.4f);
						} else {
							dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height >> 1) - 8f), player.width, 16, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 1.4f);
						}

						Main.dust[dust].velocity *= 0.1f;
						Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
					}

					speedCap = speedMax;
					decayCapped = 0.985f;
					decayMax = decayCapped;
					delay = 30;
				} else if(ActiveDash == DashType.Firewall) {
					if(firewallHit < 0) {
						Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<BinaryDust>());
						Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<BinaryDust>());
						Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<BinaryDust>());
						Rectangle hitbox = new Rectangle((int)(player.position.X + player.velocity.X * 0.5 - 4), (int)(player.position.Y + player.velocity.Y * 0.5 - 4), player.width + 8, player.height + 8);
						for(int i = 0; i < Main.maxNPCs; i++) {
							var npc = Main.npc[i];
							if(npc.active && !npc.dontTakeDamage && !npc.friendly) {
								if(hitbox.Intersects(npc.Hitbox) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height))) {
									float damage = 40f * player.meleeDamage;
									float knockback = 12f;
									bool crit = false;

									if(player.kbGlove) {
										knockback *= 2f;
									}

									if(player.kbBuff) {
										knockback *= 1.5f;
									}

									if(Main.rand.Next(100) < player.meleeCrit) {
										crit = true;
									}

									int hitDirection = player.velocity.X < 0f ? -1 : 1;

									if(player.whoAmI == Main.myPlayer) {
										npc.AddBuff(ModContent.BuffType<StackingFireBuff>(), 600);
										npc.StrikeNPC((int)damage, knockback, hitDirection, crit);
										if(Main.netMode != NetmodeID.SinglePlayer) {
											NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, i, damage, knockback, hitDirection, 0, 0, 0);
										}
									}

									player.dashDelay = 30;
									player.velocity.X = -hitDirection * 1f;
									player.velocity.Y = -4f;
									player.immune = true;
									player.immuneTime = 2;
									firewallHit = i;
								}
							}
						}
					}
				} else if(ActiveDash == DashType.Shinigami) {
					speedCap = speedMax;
					decayCapped = 0.88f;
					delay = 30;

					int animationLimit = (int)(player.itemAnimationMax * 0.6f);
					if(player.itemAnimation > 0 && player.itemAnimation < animationLimit) {
						player.itemAnimation = animationLimit;
					}
				}

				if(ActiveDash != DashType.None) {
					if(speedCap < speedMax) {
						speedCap = speedMax;
					}

					player.vortexStealthActive = false;
					if(player.velocity.X > speedCap || player.velocity.X < -speedCap) {
						player.velocity.X = player.velocity.X * decayCapped;
					} else if(player.velocity.X > speedMax || player.velocity.X < -speedMax) {
						player.velocity.X = player.velocity.X * decayMax;
					} else {
						player.dashDelay = delay;

						if(player.velocity.X < 0f) {
							player.velocity.X = -speedMax;
						} else if(player.velocity.X > 0f) {
							player.velocity.X = speedMax;
						}
					}
				}
			} else if(dash != DashType.None && player.whoAmI == Main.myPlayer) {
				sbyte dir = 0;
				bool dashInput = false;
				if(player.dashTime > 0) {
					player.dashTime--;
				} else if(player.dashTime < 0) {
					player.dashTime++;
				}

				if(player.controlRight && player.releaseRight) {
					if(player.dashTime > 0) {
						dir = 1;
						dashInput = true;
						player.dashTime = 0;
					} else {
						player.dashTime = 15;
					}
				} else if(player.controlLeft && player.releaseLeft) {
					if(player.dashTime < 0) {
						dir = -1;
						dashInput = true;
						player.dashTime = 0;
					} else {
						player.dashTime = -15;
					}
				}

				if(dashInput) {
					PerformDash(dash, dir);
				}
			}
		}

		internal void PerformDash(DashType dash, sbyte dir, bool local = true)
		{
			float velocity = dir;
			if(dash == DashType.Phase) {
				velocity *= 30f;
				phaseStacks--;

				if(local) {
					player.AddBuff(ModContent.BuffType<TemporalShift>(), 3 * 60);
				}

				// vfx
				for(int num17 = 0; num17 < 20; num17++) {
					int dust = Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 2f);
					Main.dust[dust].position.X += Main.rand.Next(-5, 6);
					Main.dust[dust].position.Y += Main.rand.Next(-5, 6);
					Main.dust[dust].velocity *= 0.2f;
					Main.dust[dust].scale *= 1.4f + Main.rand.Next(20) * 0.01f;
				}
			} else if(dash == DashType.Firewall) {
				firewallHit = -1;

				Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<BinaryDust>(), 0f, 0f, 0, default, 1f);
				Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<BinaryDust>(), 0f, 0f, 0, default, 1f);

				velocity *= 18.5f;

				for(int num22 = 0; num22 < 0; num22++) {
					int num23f = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 2f);
					Main.dust[num23f].position.X = Main.dust[num23f].position.X + Main.rand.Next(-5, 6);
					Main.dust[num23f].position.Y = Main.dust[num23f].position.Y + Main.rand.Next(-5, 6);
					Main.dust[num23f].velocity *= 0.2f;
					Main.dust[num23f].shader = GameShaders.Armor.GetSecondaryShader(player.shield, player);
				}
			} else if(dash == DashType.Shinigami) {
				velocity *= 40;
			}

			player.velocity.X = velocity;

			Point feet = (player.Center + new Vector2(dir * (player.width >> 1) + 2, player.gravDir * -player.height * .5f + player.gravDir * 2f)).ToTileCoordinates();
			Point legs = (player.Center + new Vector2(dir * (player.width >> 1) + 2, 0f)).ToTileCoordinates();
			if(WorldGen.SolidOrSlopedTile(feet.X, feet.Y) || WorldGen.SolidOrSlopedTile(legs.X, legs.Y)) {
				player.velocity.X = player.velocity.X / 2f;
			}
			player.dashDelay = -1;
			ActiveDash = dash;

			if(!local || Main.netMode == NetmodeID.SinglePlayer) {
				return;
			}

			ModPacket packet = SpiritMod.instance.GetPacket(MessageType.Dash, 3);
			packet.Write((byte)player.whoAmI);
			packet.Write((byte)dash);
			packet.Write(dir);
			packet.Send();
		}

		public DashType FindDashes()
		{
			if(phaseStacks > 0) {
				return DashType.Phase;
			} else if(firewall) {
				return DashType.Firewall;
			}

			return DashType.None;
		}

		public override void PostUpdateEquips()
		{
			int num323;
			if(graniteSet) {

				if(player.velocity.Y > 0 && player.HasBuff(ModContent.BuffType<GraniteBonus>())) {
					player.noFallDmg = true;
					player.velocity.Y = 15.53f;
					player.maxFallSpeed = 30f;
					for(int j = 0; j < 12; j++) {
						num323 = (int)(player.position.Y / 16f) - player.fallStart;
						if(num323 >= 16) {
							num323 = 16;
						}
						Vector2 vector2 = Vector2.UnitX;
						vector2 += -Vector2.UnitY.RotatedBy(j * 3.141591734f / 6f, default) * new Vector2(1f * num323, 16f);
						int num8 = Dust.NewDust(player.Center, 0, 0, 226, 0f, 0f, 160, new Color(), 1f);
						Main.dust[num8].scale = .68f;
						Main.dust[num8].noGravity = true;
						Main.dust[num8].position = player.Center + vector2;
						Main.dust[num8].velocity = player.velocity * 0.1f;
						Main.dust[num8].velocity = Vector2.Normalize(player.Center - player.velocity * 3f - Main.dust[num8].position) * 1.25f;
					}

					player.armorEffectDrawShadow = true;
				}
			}
			if(Main.mouseRight && magnifyingGlass && Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].damage <= 0) {
				player.scope = true;
			}

			if(leatherSet) {
				if(concentratedCooldown == 0) {
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
				}
				if(player.velocity.X != 0f) {
					concentratedCooldown--;
				} else {
					concentratedCooldown -= 2;
				}
			} else {
				concentrated = false;
				concentratedCooldown = 420;
			}

			if(concentratedCooldown <= 0) {
				concentrated = true;
			}

			if(concentrated && leatherSet) {
				Yoraiz0rEye();
			}
			if(bloodCourtHead) {
				BloodCourtEye();
			}
			if(clatterboneShield) {
				clatterStacks = 0;
				for(int i = 0; i < 200; i++) {
					if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy) {
						int distance = (int)Main.npc[i].Distance(player.Center);
						if(distance < 480) {
							clatterStacks++;
						}

						for(int k = 0; k < clatterStacks; k++) {
							if(Main.rand.NextBool(4)) {
								int d = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 2, 0, 0f, 0f, 100, default, .64f);
								Main.dust[d].noGravity = true;
							}
						}
					}
				}

				if(clatterStacks >= 5) {
					clatterStacks = 5;
				}
			} else {
				clatterStacks = 0;
			}
			if(astralSet) {
				player.meleeSpeed += .06f * astralSetStacks;
				player.manaCost -= .06f * astralSetStacks;
				player.lifeRegen += 1 * astralSetStacks;
				astralSetStacks = 0;
				for(int i = 0; i < 200; i++) {
					if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy) {
						int distance = (int)Main.npc[i].Distance(player.Center);
						if(distance < 240) {
							astralSetStacks++;
						}
						Vector2 center = player.Center;
						float num8 = player.miscCounter / 40f;
						float num7 = 1.0471975512f * 2;
						for(int k = 0; k < astralSetStacks; k++) {
							{
								int num6 = Dust.NewDust(center, 0, 0, 206, 0f, 0f, 100, default, 1.3f);
								Main.dust[num6].noGravity = true;
								Main.dust[num6].velocity = Vector2.Zero;
								Main.dust[num6].noLight = true;
								Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * i).ToRotationVector2() * 12f;
							}
						}
					}
				}

				if(astralSetStacks >= 3) {
					astralSetStacks = 3;
				}
			}
			if(bismiteShield) {
				bismiteShieldStacks = 0;
				for(int i = 0; i < 200; i++) {
					if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].HasBuff(BuffID.Poisoned) && Main.npc[i].type != NPCID.TargetDummy) {
						int distance = (int)Main.npc[i].Distance(player.Center);
						if(distance < 320) {
							bismiteShieldStacks++;

						}

						for(int k = 0; k < bismiteShieldStacks; k++) {
							if(Main.rand.NextBool(6)) {
								int d = Dust.NewDust(player.position, player.width, player.height, 167, 0f, 0f, 100, default, .45f * bismiteShieldStacks);
								Main.dust[d].noGravity = true;
							}
						}
					}
				}
				player.statDefense += player.GetSpiritPlayer().bismiteShieldStacks;
				if(bismiteShieldStacks >= 5) {
					bismiteShieldStacks = 5;
				}
			}

			if(frigidGloves) {
				frigidGloveStacks = 0;
				for(int i = 0; i < 200; i++) {
					if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy) {
						int distance = (int)Main.npc[i].Distance(player.Center);
						if(distance < 320) {
							frigidGloveStacks++;
						}

						for(int k = 0; k < frigidGloveStacks; k++) {
							if(Main.rand.NextBool(6)) {
								int d = Dust.NewDust(player.position, player.width, player.height, 68, 0f, 0f, 100, default, .45f * bismiteShieldStacks);
								Main.dust[d].noGravity = true;
							}
						}
					}
				}

				if(frigidGloveStacks >= 6) {
					frigidGloveStacks = 6;
				}
			}

			if(bloodfireShield) {
				if(player.lifeRegen >= 0) {
					player.lifeRegen = 0;
				}

				player.lifeRegen--;
				if(player.lifeRegen < 0) {
					player.lifeRegen = 0;
				}

				player.lifeRegenTime = 0;
				player.lifeRegenCount = 0;

				bloodfireShieldStacks = 0;
				if(bloodfireShieldStacks >= 5) {
					bloodfireShieldStacks = 5;
				}
				for(int i = 0; i < 200; i++) {
					if(Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy) {
						int distance = (int)Main.npc[i].Distance(player.Center);
						if(distance < 320) {
							bloodfireShieldStacks++;
						}
						if(bloodfireShieldStacks >= 5) {
							bloodfireShieldStacks = 5;
						}
						for(int k = 0; k < bloodfireShieldStacks; k++) {
							if(Main.rand.NextBool(6)) {
								Dust.NewDust(player.position, player.width, player.height, 5, 0f, 0f, 0, default, .14f * bloodfireShieldStacks);
							}
						}
					}
				}
			} else {
				bismiteShieldStacks = 0;
			}

			if(player.controlUp && scarabCharm) {
								player.noFallDmg = true;
				if(player.gravDir == -1.0f) {
					player.itemRotation = -player.itemRotation;
					player.itemLocation.Y = (float)(player.position.Y + player.height + (player.position.Y - player.itemLocation.Y));
					if(player.velocity.Y < -2.0f)
						player.velocity.Y = -2f;
				} else if(player.velocity.Y > 2.0f)
					player.velocity.Y = 2f;
			}

			//if (glyph == GlyphType.Veil && Math.Abs(player.velocity.X) < 0.05 && Math.Abs(player.velocity.Y) < 0.05)
			//	camoCounter++;
			//else if (camoCounter > 5)
			//	camoCounter -= 5;
			//else
			//	camoCounter = 0;

			if(glyph == GlyphType.Void) {
				player.endurance += .08f;
			} else if(glyph == GlyphType.Veil) {
				//	float camo = (1f / CAMO_DELAY) * camoCounter;
				//	if (camoCounter > CAMO_DELAY)
				//	{
				//		camo = 1f;
				//		camoCounter = CAMO_DELAY;
				//		player.lifeRegen += 3;
				//	}
				//	if (camoCounter > 0 && Main.rand.NextDouble() < camo * .6)
				//	{
				//		player.AddBuff(Buffs.Glyph.PhantomVeil._type, 2);
				//		int dust = Dust.NewDust(player.position, player.width, player.height, 110);
				//		Main.dust[dust].scale = 2.5f * (.75f + .25f * camo);
				//		Main.dust[dust].noGravity = true;
				//	}
				//	player.rangedDamage *= 1 + .15f * camo;
				//	player.magicDamage *= 1 + .15f * camo;
				//	player.thrownDamage *= 1 + .15f * camo;
				//	player.minionDamage *= 1 + .15f * camo;
				//	player.meleeDamage *= 1 + .15f * camo;
			}

			if(phaseShift) {
				player.noKnockback = true;
				player.buffImmune[BuffID.Slow] = true;
				player.buffImmune[BuffID.Chilled] = true;
				player.buffImmune[BuffID.Frozen] = true;
				player.buffImmune[BuffID.Webbed] = true;
				player.buffImmune[BuffID.Stoned] = true;
				player.buffImmune[BuffID.OgreSpit] = true;
				player.buffImmune[BuffID.Confused] = true;

				int dust;
				if(player.velocity.Y == 0f) {
					dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 1.4f);
				} else {
					dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height >> 1) - 8f), player.width, 16, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 1.4f);
				}

				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
			}

			if(clatterboneSet) {
				clatterboneTimer--;
			}
            bool spawnedProj = true;
            if (rogueCrest)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<KnifeMinionProjectile>()] < 1)
                { 
                    int newProj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<KnifeMinionProjectile>(), (int)(5 * player.minionDamage), .5f, player.whoAmI, 90, 1);
                }
            }
            if (spellswordCrest)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<HolyKnifeMinion>()] < 1)
                {
                    int newProj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<HolyKnifeMinion>(), (int)(27 * player.minionDamage), 1.25f, player.whoAmI, 90, 1);
                }
            }
            // Update armor sets.
            if (infernalSet) {
				float percentageLifeLeft = (float)player.statLife / player.statLifeMax2;
				if(percentageLifeLeft <= 0.25f) {
					player.statDefense -= 4;
					player.manaCost += 0.25F;
					player.magicDamage += 0.5F;

					bool spawnProj = true;
					for(int i = 0; i < 1000; ++i) {
						if(Main.projectile[i].type == ModContent.ProjectileType<InfernalGuard>() && Main.projectile[i].owner == player.whoAmI) {
							spawnProj = false;
							break;
						}
					}

					if(spawnProj) {
						for(int i = 0; i < 3; ++i) {
							int newProj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<InfernalGuard>(), 0, 0, player.whoAmI, 90, 1);
							Main.projectile[newProj].localAI[1] = 2f * MathHelper.Pi / 3f * i;
						}
					}

					player.AddBuff(ModContent.BuffType<InfernalRage>(), 2);
					infernalSetCooldown = 60;
				}
			}

			if(infernalSetCooldown > 0) {
				infernalSetCooldown--;
			}

			if(runicSet) {
				SpawnRunicRunes();
			}

			if(spiritSet) {
				if(Main.rand.NextBool(5)) {
					int num = Dust.NewDust(player.position, player.width, player.height, 261, 0f, 0f, 0, default, 1f);
					Main.dust[num].noGravity = true;
				}

				if(player.statLife >= 400) {
					player.meleeDamage += 0.08f;
					player.magicDamage += 0.08f;
					player.minionDamage += 0.08f;
					player.rangedDamage += 0.08f;
				} else if(player.statLife >= 200) {
					player.statDefense += 6;
				} else if(player.statLife >= 50) {
					player.lifeRegenTime += 5;
				} else if(player.statLife > 0) {
					player.noKnockback = true;
				}
			}

			if(bloomwindSet) {
				player.AddBuff(ModContent.BuffType<BloomwindMinionBuff>(), 3600);
				timer1++;
				if(player.ownedProjectileCounts[ModContent.ProjectileType<BloomwindMinion>()] <= 0 && timer1 > 30) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<BloomwindMinion>(), 35, 0, player.whoAmI);
					timer1 = 0;
				}
			}

			if(Ward || Ward1) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<WardProj>()] <= 1) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<WardProj>(), 0, 0, player.whoAmI);
				}
			}
			if(cryoSet) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<CryoProj>()] <= 1) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<CryoProj>(), 0, 0, player.whoAmI);
				}
			}
			if(atmos) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<AtmosProj>()] <= 1) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<AtmosProj>(), 0, 0, player.whoAmI);
				}
			}

			if(animusLens) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<ShadowGuard>()] <= 0) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<ShadowGuard>(), 20, 0, player.whoAmI);
				}

				if(player.ownedProjectileCounts[ModContent.ProjectileType<SpiritGuard>()] <= 0) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<SpiritGuard>(), 20, 0, player.whoAmI);
				}
			}

			if(witherSet) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<WitherOrb>()] <= 0) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<WitherOrb>(), 45, 0, player.whoAmI);
				}
			}

			Counter++;
			if(MoonSongBlossom) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<MoonShard>()] <= 2 && Counter > 120) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<MoonShard>(), 25, 0, player.whoAmI);
					Counter = 0;
				}
			}

			if(crystalSet) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<HedronCrystal>()] <= 1) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<HedronCrystal>(), 25, 0, player.whoAmI);
				}
			}

			if(shadowSet && (Main.rand.NextBool(2))) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<Spirit>()] <= 2) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<Spirit>(), 56, 0, player.whoAmI);
				}
			}

			if(SoulStone && (Main.rand.NextBool(2))) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<StoneSpirit>()] < 1) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<StoneSpirit>(), 35, 0, player.whoAmI);
				}
			}

			if(duskSet) {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<ShadowCircleRune1>()] <= 0) {
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<ShadowCircleRune1>(), 18, 0, player.whoAmI);
				}
			}


			if(shadowSet) {
				if(infernalDash > 0) {
					infernalDash--;
				} else {
					infernalHit = -1;
				}

				if(infernalDash > 0 && infernalHit < 0) {
					Rectangle rectangle = new Rectangle((int)(player.position.X + player.velocity.X * 0.5 - 4.0), (int)(player.position.Y + player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
					for(int i = 0; i < 200; i++) {
						if(Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly) {
							NPC npc = Main.npc[i];
							Rectangle rect = npc.getRect();
							if(rectangle.Intersects(rect) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height))) {
								float damage = 100f * player.meleeDamage;

								float knockback = 10f;
								if(player.kbGlove) {
									knockback *= 0f;
								}

								if(player.kbBuff) {
									knockback *= 1f;
								}

								bool crit = false;
								if(Main.rand.Next(100) < player.meleeCrit) {
									crit = true;
								}

								int hitDirection = player.direction;
								if(player.velocity.X < 0f) {
									hitDirection = -1;
								}

								if(player.velocity.X > 0f) {
									hitDirection = 1;
								}

								if(player.whoAmI == Main.myPlayer) {
									npc.AddBuff(ModContent.BuffType<SoulFlare>(), 600);
									npc.StrikeNPC((int)damage, knockback, hitDirection, crit, false, false);

									if(Main.netMode != NetmodeID.SinglePlayer) {
										NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, i, damage, knockback, hitDirection);
									}
								}

								infernalDash = 10;
								player.dashDelay = 0;
								player.velocity.X = -hitDirection * 2f;
								player.velocity.Y = -2f;
								player.immune = true;
								player.immuneTime = 7;
								infernalHit = i;
							}
						}
					}
				}

				if(player.dash <= 0 && player.dashDelay == 0 && !player.mount.Active) {
					int num21 = 0;
					bool flag2 = false;

					if(player.dashTime > 0) {
						player.dashTime--;
					}

					if(player.dashTime < 0) {
						player.dashTime++;
					}

					if(player.controlRight && player.releaseRight) {
						if(player.dashTime > 0) {
							num21 = 1;
							flag2 = true;
							player.dashTime = 0;
						} else {
							player.dashTime = 15;
						}
					} else if(player.controlLeft && player.releaseLeft) {
						if(player.dashTime < 0) {
							num21 = -1;
							flag2 = true;
							player.dashTime = 0;
						} else {
							player.dashTime = -15;
						}
					}

					if(flag2) {
						player.velocity.X = 15.5f * num21;
						Point point3 = (player.Center + new Vector2(num21 * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
						Point point4 = (player.Center + new Vector2(num21 * player.width / 2 + 2, 0f)).ToTileCoordinates();
						if(WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y)) {
							player.velocity.X = player.velocity.X / 2f;
						}

						player.dashDelay = -1;
						infernalDash = 15;

						for(int num22 = 0; num22 < 0; num22++) {
							int num23 = Dust.NewDust(player.position, player.width, player.height, 31, 0f, 0f, 100, default, 2f);
							Main.dust[num23].position.X = Main.dust[num23].position.X + Main.rand.Next(-5, 6);
							Main.dust[num23].position.Y = Main.dust[num23].position.Y + Main.rand.Next(-5, 6);
							Main.dust[num23].velocity *= 0.2f;
							Main.dust[num23].scale *= 1f + Main.rand.Next(20) * 0.01f;
							Main.dust[num23].shader = GameShaders.Armor.GetSecondaryShader(player.shield, player);
						}
					}
				}
			}

			if(infernalDash > 0) {
				infernalDash--;
			}

			if(player.dashDelay < 0) {
				for(int l = 0; l < 0; l++) {
					int num14;
					if(player.velocity.Y == 0f) {
						num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, 31, 0f, 0f, 100, default, 1.4f);
					} else {
						num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height / 2) - 8f), player.width, 16, 31, 0f, 0f, 100, default, 1.4f);
					}
					Main.dust[num14].velocity *= 0.1f;
					Main.dust[num14].scale *= 1f + Main.rand.Next(20) * 0.01f;
					Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(player.shoe, player);

					int dust = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
					Main.dust[dust].scale *= 10f;

					int dust2 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
					Main.dust[dust2].scale *= 10f;

					int dust3 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
					Main.dust[dust3].scale *= 10f;
				}

				player.vortexStealthActive = false;

				float maxSpeed = Math.Max(player.accRunSpeed, player.maxRunSpeed);
				if(player.velocity.X > 12f || player.velocity.X < -12f) {
					player.velocity.X = player.velocity.X * 0.985f;
					return;
				}

				if(player.velocity.X > maxSpeed || player.velocity.X < -maxSpeed) {
					player.velocity.X = player.velocity.X * 0.94f;
					return;
				}

				player.dashDelay = 30;

				if(player.velocity.X < 0f) {
					player.velocity.X = -maxSpeed;
					return;
				}

				if(player.velocity.X > 0f) {
					player.velocity.X = maxSpeed;
					return;
				}
			}

			// Update accessories.
			if(infernalShield) {
				if(infernalDash > 0) {
					infernalDash--;
				} else {
					infernalHit = -1;
				}

				if(infernalDash > 0 && infernalHit < 0) {
					int dust = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
					Main.dust[dust].scale *= 2f;

					int dust2 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
					Main.dust[dust2].scale *= 2f;

					int dust3 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
					Main.dust[dust3].scale *= 2f;

					Rectangle rectangle = new Rectangle((int)(player.position.X + player.velocity.X * 0.5 - 4.0), (int)(player.position.Y + player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
					for(int i = 0; i < 200; i++) {
						if(Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly) {
							NPC npc = Main.npc[i];
							Rectangle rect = npc.getRect();
							if(rectangle.Intersects(rect) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height))) {
								float damage = 30f * player.meleeDamage;

								float knockback = 12f;
								if(player.kbGlove) {
									knockback *= 2f;
								}

								if(player.kbBuff) {
									knockback *= 1.5f;
								}

								bool crit = false;
								if(Main.rand.Next(100) < player.meleeCrit) {
									crit = true;
								}

								int hitDirection = player.direction;
								if(player.velocity.X < 0f) {
									hitDirection = -1;
								}

								if(player.velocity.X > 0f) {
									hitDirection = 1;
								}

								if(player.whoAmI == Main.myPlayer) {
									npc.AddBuff(ModContent.BuffType<StackingFireBuff>(), 600);
									npc.StrikeNPC((int)damage, knockback, hitDirection, crit, false, false);

									if(Main.netMode != NetmodeID.SinglePlayer) {
										NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, i, damage, knockback, hitDirection, 0, 0, 0);
									}
								}

								infernalDash = 10;
								player.dashDelay = 30;
								player.velocity.X = -hitDirection * 1f;
								player.velocity.Y = -4f;
								player.immune = true;
								player.immuneTime = 2;
								infernalHit = i;
							}
						}
					}
				}

				if(player.dash <= 0 && player.dashDelay == 0 && !player.mount.Active) {
					int num21 = 0;
					bool flag2 = false;

					if(player.dashTime > 0) {
						player.dashTime--;
					}

					if(player.dashTime < 0) {
						player.dashTime++;
					}

					if(player.controlRight && player.releaseRight) {
						if(player.dashTime > 0) {
							num21 = 1;
							flag2 = true;
							player.dashTime = 0;
						} else {
							player.dashTime = 15;
						}
					} else if(player.controlLeft && player.releaseLeft) {
						if(player.dashTime < 0) {
							num21 = -1;
							flag2 = true;
							player.dashTime = 0;
						} else {
							player.dashTime = -15;
						}
					}

					if(flag2) {
						int dust = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
						Main.dust[dust].scale *= 2f;

						int dust2 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
						Main.dust[dust2].scale *= 2f;

						player.velocity.X = 15.5f * num21;

						Point point3 = (player.Center + new Vector2(num21 * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
						Point point4 = (player.Center + new Vector2(num21 * player.width / 2 + 2, 0f)).ToTileCoordinates();
						if(WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y)) {
							player.velocity.X = player.velocity.X / 2f;
						}

						player.dashDelay = -1;
						infernalDash = 15;

						for(int num22 = 0; num22 < 0; num22++) {
							int num23 = Dust.NewDust(player.position, player.width, player.height, 31, 0f, 0f, 100, default, 2f);
							Main.dust[num23].position.X = Main.dust[num23].position.X + Main.rand.Next(-5, 6);
							Main.dust[num23].position.Y = Main.dust[num23].position.Y + Main.rand.Next(-5, 6);
							Main.dust[num23].velocity *= 0.2f;
							Main.dust[num23].scale *= 1f + Main.rand.Next(20) * 0.01f;
							Main.dust[num23].shader = GameShaders.Armor.GetSecondaryShader(player.shield, player);
						}
					}
				}
			}

			if(infernalDash > 0) {
				infernalDash--;
			}

			if(player.dashDelay < 0) {
				for(int l = 0; l < 0; l++) {
					int num14;
					if(player.velocity.Y == 0f) {
						num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, 31, 0f, 0f, 100, default, 1.4f);
					} else {
						num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height / 2) - 8f), player.width, 16, 31, 0f, 0f, 100, default, 1.4f);
					}
					Main.dust[num14].velocity *= 0.1f;
					Main.dust[num14].scale *= 1f + Main.rand.Next(20) * 0.01f;
					Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(player.shoe, player);
				}

				player.vortexStealthActive = false;

				float maxSpeed = Math.Max(player.accRunSpeed, player.maxRunSpeed);
				if(player.velocity.X > 12f || player.velocity.X < -12f) {
					player.velocity.X = player.velocity.X * 0.985f;
					return;
				}

				if(player.velocity.X > maxSpeed || player.velocity.X < -maxSpeed) {
					player.velocity.X = player.velocity.X * 0.94f;
					return;
				}

				player.dashDelay = 30;

				if(player.velocity.X < 0f) {
					player.velocity.X = -maxSpeed;
					return;
				}

				if(player.velocity.X > 0f) {
					player.velocity.X = maxSpeed;
					return;
				}
			}

			if(shadowGauntlet) {
				player.kbGlove = true;
				player.meleeDamage += 0.07F;
				player.meleeSpeed += 0.07F;
			}

			if(goldenApple) {
				int num2 = 20;
				float num3 = (player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * num2;
				player.statDefense += (int)num3;
			}

			if(bubbleTimer > 0) {
				bubbleTimer--;
			}

			if(soulSiphon > 0) {
				player.lifeRegenTime += 2;

				int num = (5 + soulSiphon) / 2;
				player.lifeRegenTime += num;
				player.lifeRegen += num;

				soulSiphon = 0;
			}

			if(drakomireMount) {
				player.statDefense += 40;
				player.noKnockback = true;

				if(player.dashDelay > 0) {
					player.dashDelay--;
				} else {
					int num4 = 0;
					bool flag = false;

					if(player.dashTime > 0) {
						player.dashTime--;
					} else if(player.dashTime < 0) {
						player.dashTime++;
					}

					if(player.controlRight && player.releaseRight) {
						if(player.dashTime > 0) {
							num4 = 1;
							flag = true;
							player.dashTime = 0;
						} else {
							player.dashTime = 15;
						}
					} else if(player.controlLeft && player.releaseLeft) {
						if(player.dashTime < 0) {
							num4 = -1;
							flag = true;
							player.dashTime = 0;
						} else {
							player.dashTime = -15;
						}
					}

					if(flag) {
						player.velocity.X = 16.9f * num4;
						Point point = Utils.ToTileCoordinates(player.Center + new Vector2(num4 * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f));
						Point point2 = Utils.ToTileCoordinates(player.Center + new Vector2(num4 * player.width / 2 + 2, 0f));
						if(WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y)) {
							player.velocity.X = player.velocity.X / 2f;
						}

						player.dashDelay = 600;
					}
				}

				if(player.velocity.X != 0f && player.velocity.Y == 0f) {
					drakomireFlameTimer += (int)Math.Abs(player.velocity.X);
					if(drakomireFlameTimer >= 15) {
						Vector2 vector = player.Center + new Vector2(26 * -(float)player.direction, 26f * player.gravDir);
						Projectile.NewProjectile(vector.X, vector.Y, 0f, 0f, ModContent.ProjectileType<DrakomireFlame>(), player.statDefense / 2, 0f, player.whoAmI, 0f, 0f);
						drakomireFlameTimer = 0;
					}
				}

				if(Main.rand.Next(10) == 0) {
					Vector2 vector2 = player.Center + new Vector2(-48 * player.direction, -6f * player.gravDir);
					if(player.direction == -1) {
						vector2.X -= 20f;
					}

					Dust.NewDust(vector2, 16, 16, 6, 0f, 0f, 0, default, 1f);
				}
			}
		}

		public override void PostUpdateRunSpeeds()
		{
			if(copterBrake && player.mount.Active && player.mount.Type == ModContent.MountType<CandyCopter>()) {
				// Prevent horizontal movement
				player.maxRunSpeed = 0f;
				player.runAcceleration = 0f;

				// Deplete horizontal velocity
				if(player.velocity.X > CandyCopter.groundSlowdown) {
					player.velocity.X -= CandyCopter.groundSlowdown;
				} else if(player.velocity.X < -CandyCopter.groundSlowdown) {
					player.velocity.X += CandyCopter.groundSlowdown;
				} else {
					player.velocity.X = 0f;
				}

				// Prevent further depletion by game engine
				player.runSlowdown = 0f;
			}

			// Adjust speed here to also affect mounted speed.
			float speed = 1f;
			float sprint = 1f;
			float accel = 1f;
			float slowdown = 1f;

			if(glyph == GlyphType.Frost) {
				sprint += .05f;
			}

			if(phaseShift) {
				speed += 0.55f;
				sprint += 0.55f;
				accel += 3f;
				slowdown += 3f;
			}

			player.maxRunSpeed *= speed;
			player.accRunSpeed *= sprint;
			player.runAcceleration *= accel;
			player.runSlowdown *= slowdown;

			DashMovement(FindDashes());
		}

		public override void PostUpdate()
		{
			foreach(var effect in removedEffects)
				if(!effects.Contains(effect)) effect.EffectRemoved(player);

			foreach(var effect in effects)
				effect.PlayerPostUpdate(player);

			if(ZoneReach && Main.expertMode) {
				int off = 5; //Change this value depending on the strength of your light. Too big and it might cause lag, though. Never go above ~20 or so.
				int x = (int)(Main.screenPosition.X / 16f) - off;
				int y = (int)(Main.screenPosition.Y / 16f) - off;
				int x2 = x + (int)(Main.screenWidth / 16f) + off * 2;
				int y2 = y + (int)(Main.screenHeight / 16f) + off * 2;

				for(int i = x; i <= x2; i++) {
					for(int j = y; j <= y2; j++) {
						Tile t = Main.tile[i, j];
						if(t == null) return;
						if(!t.active() && t.liquid > 0 && t.liquidType() == 0 && Framing.GetTileSafely(i, j - 1).liquid == 0 && !Framing.GetTileSafely(i, j - 1).active()) {
							if(Main.rand.Next(20) == 0) {
								int d = Dust.NewDust(new Vector2(i * 16, j * 16), Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), 256, 0.0f, -1, 0, new Color(), 0.95f);//Leave this line how it is, it uses int division
								Main.dust[d].velocity *= .8f;
								Main.dust[d].noGravity = true;
							}
						}
					}
				}
			}
			if(ZoneReach && player.wet && Main.expertMode) {
				player.AddBuff(BuffID.Poisoned, 120);
			}
			if(cryoSet) {
				cryoTimer += .5f;
				if(cryoTimer >= 450) {
					cryoTimer = 450;
				}
			} else {
				cryoTimer = 0;
			}
			if(surferSet && surferTimer == 0) {
				Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
				CombatText.NewText(textPos, new Color(121, 195, 237, 100), "Water Spout Charged!");
			}
			if(daybloomSet) {
				if(dazzleStacks == 1800) {
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
					Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
					CombatText.NewText(textPos, new Color(245, 212, 69, 100), "Energy Charged!");
				}

				if(dazzleStacks >= 1800) {
					if(Main.rand.Next(6) == 0) {
						int d = Dust.NewDust(player.position, player.width, player.height, 228, 0f, 0f, 0, default, .14f * bloodfireShieldStacks);
					}
				}
			}

			if(shootDelay > 0) {
				shootDelay--;

			}
			if(shootDelay == 1) {
				Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
				CombatText.NewText(textPos, new Color(29, 240, 255, 100), "Cooldown over!");
			}

			if(shootDelay1 > 0) {
				shootDelay1--;
			}

			if(shootDelay2 > 0) {
				shootDelay2--;
			}

			if(shootDelay3 > 0) {
				shootDelay3--;
			}
		}


		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			foreach(var effect in effects)
				effect.PlayerModifyHitNPC(player, item, target, ref damage, ref knockback, ref crit);

			if(CursedPendant && Main.rand.NextBool(5)) {
				target.AddBuff(BuffID.CursedInferno, 180);
			}

			if(shadowGauntlet && Main.rand.NextBool(2)) {
				target.AddBuff(BuffID.ShadowFlame, 180);
			}
			if(twilightTalisman && Main.rand.NextBool(13)) {
				target.AddBuff(BuffID.ShadowFlame, 180);
			}
			if(duskSet && item.magic && Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.ShadowFlame, 300);
			}

			if(primalSet && item.melee && Main.rand.NextBool(2)) {
				target.AddBuff(ModContent.BuffType<Afflicted>(), 120);
			}

			if(moonGauntlet && item.melee) {
				if(Main.rand.NextBool(4)) {
					target.AddBuff(BuffID.CursedInferno, 180);
				}

				if(Main.rand.NextBool(4)) {
					target.AddBuff(BuffID.Ichor, 180);
				}

				if(Main.rand.NextBool(4)) {
					target.AddBuff(BuffID.Daybreak, 180);
				}

				if(Main.rand.NextBool(8)) {
					player.AddBuff(ModContent.BuffType<OnyxWind>(), 120);
				}
			}

			if(Ward1 && crit) {
				if(Main.rand.NextBool(10)) {
					player.statLife += 2;
				}

				player.HealEffect(2);
			}

			if(starBuff && crit) {
				if(Main.rand.NextBool(10)) {
					for(int i = 0; i < 3; ++i) {
						if(Main.myPlayer == player.whoAmI) {
							Projectile.NewProjectile(target.Center.X + Main.rand.Next(-140, 140), target.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ProjectileID.HallowStar, 40, 3, player.whoAmI);
						}
					}
				}
			}

			if(poisonPotion && crit) {
				if(Main.rand.NextBool(10)) {
					target.AddBuff(BuffID.Poisoned, 180);
				}
			}

			if(runeBuff && item.magic) {
				if(Main.rand.NextBool(10)) {
					for(int h = 0; h < 3; h++) {
						Vector2 vel = new Vector2(0, -1);
						float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
						vel = vel.RotatedBy(rand);
						vel *= 8f;
						Projectile.NewProjectile(target.Center - new Vector2(10f, 10f), vel, ModContent.ProjectileType<Projectiles.Magic.Rune>(), 27, 1, player.whoAmI);
					}
				}
			}

			if(moonHeart && Main.rand.NextBool(12)) {
				if(item.melee || item.ranged || item.magic) {
					player.AddBuff(ModContent.BuffType<CelestialWill>(), 300);
				}
			}

			if(concentrated) {
				for(int i = 0; i < 40; i++) {
					int dust = Dust.NewDust(target.Center, target.width, target.height, DustID.GoldCoin);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;

					Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
					vector2_1.Normalize();

					Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();

					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = target.Center - vector2_3;
				}

				damage = (int)(damage * 1.2f);
				crit = true;
				concentrated = false;
				concentratedCooldown = 300;
			}

			if(AceOfSpades && crit) {
				damage = (int)(damage * 1.1f + 0.5f);
				for(int i = 0; i < 3; i++) {
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<SpadeDust>(), 0, -0.8f);
				}
			}
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(icySoul && Main.rand.NextBool(6)) {
				if(proj.magic) {
					target.AddBuff(BuffID.Frostburn, 280);
				}
			}
			if(twilightTalisman && Main.rand.NextBool(15)) {
				target.AddBuff(BuffID.ShadowFlame, 180);
			}
			if(shadowGauntlet && proj.melee && Main.rand.NextBool(2)) {
				target.AddBuff(BuffID.ShadowFlame, 180);
			}

			if(poisonPotion && crit && Main.rand.NextBool(10)) {
				target.AddBuff(BuffID.Poisoned, 180);
			}

			if(moonGauntlet && proj.melee) {
				if(Main.rand.NextBool(4)) {
					target.AddBuff(BuffID.CursedInferno, 180);
				}

				if(Main.rand.NextBool(4)) {
					target.AddBuff(BuffID.Ichor, 180);
				}

				if(Main.rand.NextBool(4)) {
					target.AddBuff(BuffID.Daybreak, 180);
				}

				if(Main.rand.NextBool(8)) {
					player.AddBuff(ModContent.BuffType<OnyxWind>(), 120);
				}
			}

			if(runeBuff && proj.magic && Main.rand.NextBool(10)) {
				for(int h = 0; h < 3; h++) {
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 8f;
					Projectile.NewProjectile(target.Center.X - 10, target.Center.Y - 10, vel.X, vel.Y, ModContent.ProjectileType<Projectiles.Magic.Rune>(), 27, 1, player.whoAmI);
				}
			}

			if(starBuff && crit && Main.rand.NextBool(10)) {
				for(int i = 0; i < 3; ++i) {
					if(Main.myPlayer == player.whoAmI) {
						Projectile.NewProjectile(target.Center.X + Main.rand.Next(-140, 140), target.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ProjectileID.HallowStar, 40, 3, player.whoAmI);
					}
				}
			}
			if(moonHeart && Main.rand.NextBool(15)) {
				player.AddBuff(ModContent.BuffType<CelestialWill>(), 300);
			}

			if(primalSet && Main.rand.NextBool(2)) {
				if(proj.magic || proj.melee) {
					target.AddBuff(ModContent.BuffType<Afflicted>(), 120);
				}
			}

			if(duskSet && proj.magic && Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.ShadowFlame, 300);
			}

			if(Ward1 && crit && Main.rand.NextBool(10)) {
				player.statLife += 2;
				player.HealEffect(2);
			}

			if(concentrated) {
				for(int i = 0; i < 40; i++) {
					int dust = Dust.NewDust(target.Center, target.width, target.height, DustID.GoldCoin);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;

					Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
					vector2_1.Normalize();

					Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();

					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = target.Center - vector2_3;
				}

				damage = (int)(damage * 1.2F);
				crit = true;
				concentrated = false;
				concentratedCooldown = 300;
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			if(npc.whoAmI == infernalHit) {
				damage = 0;
			}
			if(coralSet) {
				for(int k = 0; k < 10; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 225, 2.5f, -2.5f, 0, Color.White, 0.7f);
					Dust.NewDust(npc.position, npc.width, npc.height, 225, 2.5f, -2.5f, 0, default(Color), .34f);
				}
				npc.StrikeNPC(damage / 3, 1f, 0, crit);
			}
		}

		public void Yoraiz0rEye()
		{
			int index = 0 + player.bodyFrame.Y / 56;
			if(index >= Main.OffsetsPlayerHeadgear.Length) {
				index = 0;
			}

			Vector2 vector2_1 = Vector2.Zero;
			if(player.mount.Active && player.mount.Cart) {
				int num = Math.Sign(player.velocity.X);
				if(num == 0) {
					num = player.direction;
				}

				vector2_1 = new Vector2(MathHelper.Lerp(0.0f, -8f, player.fullRotation / 0.7853982f), MathHelper.Lerp(0.0f, 2f, Math.Abs(player.fullRotation / 0.7853982f))).RotatedBy(player.fullRotation, new Vector2());
				if(num == Math.Sign(player.fullRotation)) {
					vector2_1 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / 0.7853982f));
				}
			}

			Vector2 spinningpoint1 = new Vector2(3 * player.direction - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir) + Vector2.UnitY * player.gfxOffY + player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			Vector2 spinningpoint2 = new Vector2(3 * player.shadowDirection[1] - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir) + player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			if(player.fullRotation != 0.0) {
				spinningpoint1 = spinningpoint1.RotatedBy(player.fullRotation, player.fullRotationOrigin);
				spinningpoint2 = spinningpoint2.RotatedBy(player.fullRotation, player.fullRotationOrigin);
			}

			float num1 = 0.0f;
			if(player.mount.Active) {
				num1 = player.mount.PlayerOffset;
			}

			Vector2 vector2_2 = player.position + spinningpoint1 + vector2_1;
			vector2_2.Y -= num1 / 2f;

			Vector2 vector2_3 = player.oldPosition + spinningpoint2 + vector2_1;
			vector2_3.Y -= num1 / 2f;

			float num2 = 1f;

			int num3 = (int)Vector2.Distance(vector2_2, vector2_3) / 3 + 1;
			if(Vector2.Distance(vector2_2, vector2_3) % 3.0 != 0.0) {
				++num3;
			}

			for(float num4 = 1f; num4 <= (double)num3; ++num4) {
				Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f)];
				dust.position = Vector2.Lerp(vector2_3, vector2_2, num4 / num3);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
				dust.customData = player;
				dust.scale = num2;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cYorai, player);
			}
		}

		public void BloodCourtEye()
		{
			int index = 0 + player.bodyFrame.Y / 56;
			if(index >= Main.OffsetsPlayerHeadgear.Length) {
				index = 0;
			}

			Vector2 vector2_1 = Vector2.Zero;
			if(player.mount.Active && player.mount.Cart) {
				int num = Math.Sign(player.velocity.X);
				if(num == 0) {
					num = player.direction;
				}

				vector2_1 = new Vector2(MathHelper.Lerp(0.0f, -8f, player.fullRotation / 0.7853982f), MathHelper.Lerp(0.0f, 2f, Math.Abs(player.fullRotation / 0.7853982f))).RotatedBy(player.fullRotation, new Vector2());
				if(num == Math.Sign(player.fullRotation)) {
					vector2_1 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / 0.7853982f));
				}
			}

			Vector2 spinningpoint1 = new Vector2(3 * player.direction - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir) + Vector2.UnitY * player.gfxOffY + player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			Vector2 spinningpoint2 = new Vector2(3 * player.shadowDirection[1] - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir) + player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			if(player.fullRotation != 0.0) {
				spinningpoint1 = spinningpoint1.RotatedBy(player.fullRotation, player.fullRotationOrigin);
				spinningpoint2 = spinningpoint2.RotatedBy(player.fullRotation, player.fullRotationOrigin);
			}

			float num1 = 0.0f;
			if(player.mount.Active) {
				num1 = player.mount.PlayerOffset;
			}

			Vector2 vector2_2 = player.position + spinningpoint1 + vector2_1;
			vector2_2.Y -= num1 / 2f;

			Vector2 vector2_3 = player.oldPosition + spinningpoint2 + vector2_1;
			vector2_3.Y -= num1 / 2f;

			float num2 = 1f;
			int num3 = (int)Vector2.Distance(vector2_2, vector2_3) / 4 + 1;
			if(Vector2.Distance(vector2_2, vector2_3) % 3.0 != 0.0) {
				++num3;
			}

			for(float num4 = 1f; num4 <= (double)num3; ++num4) {
				Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, ModContent.DustType<NightmareDust>(), 0.0f, 0.0f, 0, new Color(), .8f)];
				dust.position = Vector2.Lerp(vector2_3, vector2_2, num4 / num3);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
				dust.customData = player;
				dust.scale = num2;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cYorai, player);
			}
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if(chitinSet) {
				if(player.velocity.X != 0) {
					int knockBack = 9;
					int dam = 18;

					int hitDirection = player.direction;
					if(player.velocity.X < 0f) {
						hitDirection = -1;
					}

					if(player.velocity.X > 0f) {
						hitDirection = 1;
					}

					npc.StrikeNPCNoInteraction(dam, knockBack, -hitDirection, false, false, false);

					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<ScarabProjectile>(), dam / 2, knockBack, 0, Main.myPlayer);
					Main.projectile[p].melee = false;
				}
			}

			if(bismiteShield) {
				npc.AddBuff(BuffID.Poisoned, 300);
			}

			if(basiliskMount) {
				int num = player.statDefense / 2;
				npc.StrikeNPCNoInteraction(num, 0f, 0, false, false, false);
			}
		}

		internal bool CanTrickOrTreat(NPC npc)
		{
			if(!npc.townNPC) {
				return false;
			}

			string fullName;
			if(npc.modNPC == null) {
				fullName = "Terraria:" + npc.TypeName;
			} else {
				fullName = npc.modNPC.mod.Name + ":" + npc.TypeName;
			}

			if(candyFromTown.Contains(fullName)) {
				return false;
			}

			candyFromTown.Add(fullName);
			return true;
		}

		private void SpawnRunicRunes()
		{
			if(Main.rand.Next(15) == 0) {
				int runeAmount = 0;
				for(int i = 0; i < 1000; i++) {
					if(Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<RunicRune>()) {
						runeAmount++;
					}
				}

				if(Main.rand.Next(15) >= runeAmount && runeAmount < 10) {
					int dimension = 24;
					int dimension2 = 90;
					for(int j = 0; j < 50; j++) {
						int randValue = Main.rand.Next(200 - j * 2, 400 + j * 2);
						Vector2 center = player.Center;
						center.X += Main.rand.Next(-randValue, randValue + 1);
						center.Y += Main.rand.Next(-randValue, randValue + 1);

						if(!Collision.SolidCollision(center, dimension, dimension) && !Collision.WetCollision(center, dimension, dimension)) {
							center.X += dimension / 2;
							center.Y += dimension / 2;

							if(Collision.CanHit(new Vector2(player.Center.X, player.position.Y), 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(player.Center.X, player.position.Y - 50f), 1, 1, center, 1, 1)) {
								int x = (int)center.X / 16;
								int y = (int)center.Y / 16;

								bool flag = false;
								if(Main.rand.Next(4) == 0 && Main.tile[x, y] != null && Main.tile[x, y].wall > 0) {
									flag = true;
								} else {
									center.X -= dimension2 / 2;
									center.Y -= dimension2 / 2;

									if(Collision.SolidCollision(center, dimension2, dimension2)) {
										center.X += dimension2 / 2;
										center.Y += dimension2 / 2;
										flag = true;
									}
								}

								if(flag) {
									for(int k = 0; k < 1000; k++) {
										if(Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].type == ModContent.ProjectileType<RunicRune>() && (center - Main.projectile[k].Center).Length() < 48f) {
											flag = false;
											break;
										}
									}

									if(flag && Main.myPlayer == player.whoAmI) {
										Projectile.NewProjectile(center.X, center.Y, 0f, 0f, ModContent.ProjectileType<RunicRune>(), 40, 1.5f, player.whoAmI, 0f, 0f);
										return;
									}
								}
							}
						}
					}
				}
			}
		}

		private Vector2 TestTeleport(ref bool canSpawn, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY)
		{
			Player player = Main.player[Main.myPlayer];

			int num1 = 0;
			int num2 = 0;
			int num3 = 0;

			Vector2 Position = new Vector2(num2, num3) * 16f + new Vector2(-player.width / 2 + 8, -player.height);
			while(!canSpawn && num1 < 1000) {
				++num1;

				int index1 = teleportStartX + Main.rand.Next(teleportRangeX);
				int index2 = teleportStartY + Main.rand.Next(teleportRangeY);
				Position = new Vector2(index1, index2) * 16f + new Vector2(-player.width / 2 + 8, -player.height);

				if(!Collision.SolidCollision(Position, player.width, player.height)) {
					if(Main.tile[index1, index2] == null) {
						Main.tile[index1, index2] = new Tile();
					}

					if((Main.tile[index1, index2].wall != 87 || index2 <= Main.worldSurface || NPC.downedPlantBoss) && (!Main.wallDungeon[Main.tile[index1, index2].wall] || index2 <= Main.worldSurface || NPC.downedBoss3)) {
						int num4 = 0;
						while(num4 < 100) {
							if(Main.tile[index1, index2 + num4] == null) {
								Main.tile[index1, index2 + num4] = new Tile();
							}

							Tile tile = Main.tile[index1, index2 + num4];
							Position = new Vector2(index1, index2 + num4) * 16f + new Vector2(-player.width / 2 + 8, -player.height);
							Vector4 vector4 = Collision.SlopeCollision(Position, player.velocity, player.width, player.height, player.gravDir, false);

							bool flag = !Collision.SolidCollision(Position, player.width, player.height);
							if(vector4.Z == (double)player.velocity.X) {
								double y = player.velocity.Y;
							}

							if(flag) {
								++num4;
							} else if(!tile.active() || tile.inActive() || !Main.tileSolid[tile.type]) {
								++num4;
							} else {
								break;
							}
						}
						if(!Collision.LavaCollision(Position, player.width, player.height) && Collision.HurtTiles(Position, player.velocity, player.width, player.height, false).Y <= 0.0) {
							Collision.SlopeCollision(Position, player.velocity, player.width, player.height, player.gravDir, false);
							if(Collision.SolidCollision(Position, player.width, player.height) && num4 < 99) {
								Vector2 Velocity1 = Vector2.UnitX * 16f;
								if(!(Collision.TileCollision(Position - Velocity1, Velocity1, player.width, player.height, false, false, (int)player.gravDir) != Velocity1)) {
									Vector2 Velocity2 = -Vector2.UnitX * 16f;
									if(!(Collision.TileCollision(Position - Velocity2, Velocity2, player.width, player.height, false, false, (int)player.gravDir) != Velocity2)) {
										Vector2 Velocity3 = Vector2.UnitY * 16f;
										if(!(Collision.TileCollision(Position - Velocity3, Velocity3, player.width, player.height, false, false, (int)player.gravDir) != Velocity3)) {
											Vector2 Velocity4 = -Vector2.UnitY * 16f;
											if(!(Collision.TileCollision(Position - Velocity4, Velocity4, player.width, player.height, false, false, (int)player.gravDir) != Velocity4)) {
												canSpawn = true;
												int num5 = index2 + num4;
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}

			return Position;
		}

		public override void MeleeEffects(Item item, Rectangle hitbox)
		{
			if(shadowGauntlet && item.melee) {
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame);
			}
		}

		public override void FrameEffects()
		{
			// Prevent potential bug with shot projectile detection.
			EndShotDetection();

			// Hide players wings, etc. when mounted
			if(player.mount.Active) {
				int mount = player.mount.Type;
				if(mount == ModContent.MountType<CandyCopter>()) {
					// Supposed to make players legs disappear, but only makes them skin-colored.
					player.legs = -1;
					player.wings = -1;
					player.back = -1;
					player.shield = -1;
					// player.handoff = -1;
					// player.handon = -1;
				} else if(mount == ModContent.MountType<Drakomire>()) {
					player.wings = -1;
				}
			}
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			foreach(var effect in effects)
				effect.PlayerDrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

			if(daybloomSet && dazzleStacks != 0 && !player.mount.Active) {
				a = 255 - .0001180555f * dazzleStacks;
				if(dazzleStacks >= 1800) {
					a = 255 - .0001180555f * 1800;
				}
			}

			if(toxify) {
				if(Main.rand.Next(2) == 0) {
					int dust = Dust.NewDust(player.position, player.width + 4, 30, 110, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}

				r *= 0f;
				g *= 1f;
				b *= 0f;
				fullBright = true;
			}

			if(BlueDust) {
				if(Main.rand.NextBool(4)) {
					int dust = Dust.NewDust(player.position, player.width + 4, 30, 206, player.velocity.X, player.velocity.Y, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}

				r *= 0f;
				g *= 1f;
				b *= 0f;
				fullBright = true;
			}

			if(HellGaze) {
				if(Main.rand.Next(4) == 0) {
					int dust = Dust.NewDust(player.position, player.width + 26, 30, 6, player.velocity.X, player.velocity.Y, 100, default, 1f);
					Main.playerDrawDust.Add(dust);
				}

				r *= 0f;
				g *= 1f;
				b *= 0f;
				fullBright = true;
			}
		}

		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			if(camoCounter > 0) {
				float camo = 1 - .75f / CAMO_DELAY * camoCounter;
				drawInfo.upperArmorColor = Color.Multiply(drawInfo.upperArmorColor, camo);
				drawInfo.middleArmorColor = Color.Multiply(drawInfo.middleArmorColor, camo);
				drawInfo.lowerArmorColor = Color.Multiply(drawInfo.lowerArmorColor, camo);
				camo *= camo;
				drawInfo.hairColor = Color.Multiply(drawInfo.hairColor, camo);
				drawInfo.eyeWhiteColor = Color.Multiply(drawInfo.eyeWhiteColor, camo);
				drawInfo.eyeColor = Color.Multiply(drawInfo.eyeColor, camo);
				drawInfo.faceColor = Color.Multiply(drawInfo.faceColor, camo);
				drawInfo.bodyColor = Color.Multiply(drawInfo.bodyColor, camo);
				drawInfo.legColor = Color.Multiply(drawInfo.legColor, camo);
				drawInfo.shirtColor = Color.Multiply(drawInfo.shirtColor, camo);
				drawInfo.underShirtColor = Color.Multiply(drawInfo.underShirtColor, camo);
				drawInfo.pantsColor = Color.Multiply(drawInfo.pantsColor, camo);
				drawInfo.shoeColor = Color.Multiply(drawInfo.shoeColor, camo);
				drawInfo.headGlowMaskColor = Color.Multiply(drawInfo.headGlowMaskColor, camo);
				drawInfo.bodyGlowMaskColor = Color.Multiply(drawInfo.bodyGlowMaskColor, camo);
				drawInfo.armGlowMaskColor = Color.Multiply(drawInfo.armGlowMaskColor, camo);
				drawInfo.legGlowMaskColor = Color.Multiply(drawInfo.legGlowMaskColor, camo);
			}
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			for(int i = 0; i < layers.Count; i++) {
				if((drakomireMount || basiliskMount) && layers[i].Name == "Wings") {
					layers[i].visible = false;
				}

				if(layers[i].Name == "HeldItem") {
					// Can someone please tell me what HexBow was? It. Doesn't. Exist.
					/*if (player.inventory[player.selectedItem].type == ModContent.ItemType<HexBow>() && player.itemAnimation > 0)
                    {
                        weaponAnimationCounter++;
                        if (weaponAnimationCounter >= 10)
                        {
                            hexBowAnimationFrame = (hexBowAnimationFrame + 1) % 4;
                            weaponAnimationCounter = 0;
                        }
                        layers[i] = WeaponLayer;
                    }*/
				}
			}

			if(bubbleTimer > 0) {
				BubbleLayer.visible = true;
				layers.Add(BubbleLayer);
			}
		}

		public static readonly PlayerLayer WeaponLayer = new PlayerLayer("SpiritMod", "WeaponLayer", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo) {
			if(drawInfo.shadow != 0f) {
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;
			MyPlayer myPlayer = drawPlayer.GetModPlayer<MyPlayer>();
			if(drawPlayer.active && !drawPlayer.outOfRange) {
				Texture2D weaponTexture = Main.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type];
				Vector2 vector8 = new Vector2(weaponTexture.Width / 2, (weaponTexture.Height / 4) / 2);
				Vector2 vector9 = new Vector2(8, 0);

				vector8.Y = vector9.Y;

				Vector2 vector = drawPlayer.itemLocation;
				vector.Y += weaponTexture.Height * 0.5F;

				int num84 = (int)vector9.X;
				Vector2 origin2 = new Vector2(-num84, (weaponTexture.Height / 4) / 2);
				if(drawPlayer.direction == -1) {
					origin2 = new Vector2(weaponTexture.Width + num84, (weaponTexture.Height / 4) / 2);
				}

				SpriteEffects effect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

				DrawData drawData = new DrawData(weaponTexture, new Vector2((int)(vector.X - Main.screenPosition.X + vector8.X), (int)(vector.Y - Main.screenPosition.Y + vector8.Y)), new Rectangle?(new Rectangle(0, (weaponTexture.Height / 4) * myPlayer.hexBowAnimationFrame, weaponTexture.Width, weaponTexture.Height / 4)), drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(Color.White), drawPlayer.itemRotation, origin2, drawPlayer.inventory[drawPlayer.selectedItem].scale, effect, 0);
				Main.playerDrawData.Add(drawData);

				if(drawPlayer.inventory[drawPlayer.selectedItem].color != default) {
					drawData = new DrawData(weaponTexture, new Vector2((int)(vector.X - Main.screenPosition.X + vector8.X), (int)(vector.Y - Main.screenPosition.Y + vector8.Y)), new Rectangle?(new Rectangle(0, (weaponTexture.Height / 4) * myPlayer.hexBowAnimationFrame, weaponTexture.Width, weaponTexture.Height / 4)), drawPlayer.inventory[drawPlayer.selectedItem].GetColor(Color.White), drawPlayer.itemRotation, origin2, drawPlayer.inventory[drawPlayer.selectedItem].scale, effect, 0);
					Main.playerDrawData.Add(drawData);
				}
			}
		});

		public static readonly PlayerLayer BubbleLayer = new PlayerLayer("SpiritMod", "BubbleLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo) {
			if(drawInfo.shadow != 0f) {
				return;
			}

			Mod mod = ModLoader.GetMod("SpiritMod");
			Player drawPlayer = drawInfo.drawPlayer;
			if(drawPlayer.active && !drawPlayer.outOfRange) {
				Texture2D texture = mod.GetTexture("Effects/PlayerVisuals/BubbleShield_Visual");
				Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

				Vector2 drawPos = drawPlayer.position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f);
				drawPos.X = (int)drawPos.X;
				drawPos.Y = (int)drawPos.Y;

				DrawData drawData = new DrawData(texture, drawPos - Main.screenPosition, new Rectangle?(), Color.White * 0.75f, 0, origin, 1, SpriteEffects.None, 0);
				Main.playerDrawData.Add(drawData);
			}
		});

		public void DoubleTapEffects(int keyDir)
		{
			if(keyDir == (Main.ReversedUpDownArmorSetBonuses ? 0 : 1)) {
				//Double tap up
				if(deathRose && !player.HasBuff(ModContent.BuffType<DeathRoseCooldown>())) {
					player.AddBuff(ModContent.BuffType<DeathRoseCooldown>(), 240);
					Vector2 mouse = Main.MouseScreen + Main.screenPosition;
					Projectile.NewProjectile(mouse, Vector2.Zero, ModContent.ProjectileType<BrambleTrap>(), 30, 0, Main.myPlayer, mouse.X, mouse.Y);
				}

				if(assassinMag && player.HeldItem.useAmmo > AmmoID.None) {
					var ammoItems = new List<Item>();
					var ammoPos = new List<int>();
					// 54-57 are the ammo slots
					for(int i = 54; i < 58; i++) {
						if(!player.inventory[i].IsAir && player.inventory[i].ammo == player.HeldItem.useAmmo) {
							ammoItems.Add(player.inventory[i]);
							ammoPos.Add(i);
						}
					}
					if(ammoItems.Count > 0) {
						// Shift the top item to the bottom
						var temp = ammoItems[0];
						ammoItems.RemoveAt(0);
						ammoItems.Add(temp);
						// Move the items around accordingly and trigger sync messages
						for(int i = 0; i < ammoItems.Count; i++) {
							player.inventory[ammoPos[i]] = ammoItems[i];
							if(Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendData(MessageID.SyncEquipment, -1, -1, null, player.whoAmI, ammoPos[i]);
							}
						}
						//Display a text for the item you just swapped to
						Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
						CombatText.NewText(textPos, ammoItems[0].RarityColor(), ammoItems[0].Name);
					}
				}
			} else if(keyDir == (Main.ReversedUpDownArmorSetBonuses ? 1 : 0)) {
				// Double tap down
				if(starSet && !player.HasBuff(ModContent.BuffType<StarCooldown>())) {
                    player.AddBuff(ModContent.BuffType<StarCooldown>(), 1020);
                    Main.PlaySound(SoundID.Item, player.position, 92);
					Vector2 mouse = Main.MouseScreen + Main.screenPosition;
					Projectile.NewProjectile(mouse, Vector2.Zero, ModContent.ProjectileType<EnergyFieldStarplate>(), 0, 0, player.whoAmI);
					for(int i = 0; i < 8; i++) {
						int num = Dust.NewDust(player.position, player.width, player.height, 226, 0f, -2f, 0, default(Color), .7f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != player.Center)
							Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
				if(surferSet && surferTimer <= 0) {
					surferTimer = 420;
					Main.PlaySound(SoundID.Splash, player.position, 0);
					Main.PlaySound(SoundID.Item, player.position, 20);
					Projectile.NewProjectile(player.Center - new Vector2(0, 30), Vector2.Zero, ModContent.ProjectileType<WaterSpout>(), 30, 8, player.whoAmI);
				}
				if(graniteSet && !player.mount.Active) {
					if(player.velocity.Y != 0 && stompCooldown <= 0) {
						player.AddBuff(ModContent.BuffType<GraniteBonus>(), 300);
					}
				}
				if(fierySet && fierySetTimer <= 0) {
					Main.PlaySound(SoundID.Item, player.position, 74);
					for(int i = 0; i < 8; i++) {
						int num = Dust.NewDust(player.position, player.width, player.height, 6, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != player.Center)
							Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
					}
					for(int projFinder = 0; projFinder < 300; ++projFinder) {
						if(Main.projectile[projFinder].sentry == true && Main.projectile[projFinder].active) {
							int p = Projectile.NewProjectile(Main.projectile[projFinder].Center.X, Main.projectile[projFinder].Center.Y - 20, 0f, 0f, ModContent.ProjectileType<FierySetExplosion>(), Main.projectile[projFinder].damage, Main.projectile[projFinder].knockBack, player.whoAmI);
						}
						fierySetTimer = 480;

					}
				}

				if(daybloomSet && dazzleStacks >= 1800) {
					Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Dazzle>(), 0, 0, player.whoAmI);
					Main.PlaySound(SoundID.Item, player.position, 9);
					dazzleStacks = 0;
				}

				if(quickSilverSet && !player.HasBuff(ModContent.BuffType<SilverCooldown>())) {
					player.AddBuff(ModContent.BuffType<SilverCooldown>(), 1800);

					for(int h = 0; h < 12; h++) {
						Vector2 vel = new Vector2(0, -1);
						float rand = Main.rand.NextFloat() * (float)(Math.PI * 2);

						vel = vel.RotatedBy(rand);
						vel *= 7f;

						Vector2 mouse = Main.MouseScreen + Main.screenPosition;
						Projectile.NewProjectile(mouse, vel, ModContent.ProjectileType<QuicksilverDroplet>(), 62, 2, player.whoAmI);
					}
				}

				if(reaperSet && !player.HasBuff(ModContent.BuffType<FelCooldown>())) {
					player.AddBuff(ModContent.BuffType<FelCooldown>(), 2700);
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<FelProj>(), 0, 0, player.whoAmI);
				}

				if(bloodcourtSet && !player.HasBuff(ModContent.BuffType<CourtCooldown>())) {
					player.AddBuff(ModContent.BuffType<CourtCooldown>(), 500);
					Vector2 mouse = Main.MouseScreen + Main.screenPosition;
					Vector2 dir = mouse - player.Center;
					dir.Normalize();
					dir *= 12;
					player.statLife -= (int)(player.statLifeMax * .08f);
					for(int i = 0; i < 18; i++) {
						int num = Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<NightmareDust>(), 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .85f;
						if(Main.dust[num].position != player.Center)
							Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
					}
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
					Projectile.NewProjectile(player.Center, dir, ModContent.ProjectileType<DarkAnima>(), 55, 0, player.whoAmI);
				}

				if(depthSet && !player.HasBuff(ModContent.BuffType<SharkAttackBuff>())) {
					MyPlayer myPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
					Rectangle textPos = new Rectangle((int)myPlayer.player.position.X, (int)myPlayer.player.position.Y - 60, myPlayer.player.width, myPlayer.player.height);

					CombatText.NewText(textPos, new Color(29, 240, 255, 100), "Shark Attack!");
					player.AddBuff(ModContent.BuffType<SharkAttackBuff>(), 1800);
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<SharkBlast>(), 35, 0, player.whoAmI);
				}

				if(ichorSet1 && !player.HasBuff(ModContent.BuffType<GoreCooldown1>())) {
					player.AddBuff(ModContent.BuffType<GoreCooldown1>(), 3600);
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Gores>(), 4, 0, player.whoAmI);
				}

				if(ichorSet2 && !player.HasBuff(ModContent.BuffType<GoreCooldown2>())) {
					player.AddBuff(ModContent.BuffType<GoreCooldown2>(), 3600);
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<Gore1>(), 21, 0, player.whoAmI);
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<Gore1>(), 21, 0, player.whoAmI);
					Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<Gore1>(), 21, 0, player.whoAmI);
				}

				if(frigidSet && !player.HasBuff(ModContent.BuffType<FrigidCooldown>())) {
					Vector2 mouse = Main.MouseScreen + Main.screenPosition;
					Projectile.NewProjectile(mouse, Vector2.Zero, ModContent.ProjectileType<FrigidWall>(), 14, 8, player.whoAmI);
					player.AddBuff(ModContent.BuffType<FrigidCooldown>(), 500);
				}
			}
		}
	}
}