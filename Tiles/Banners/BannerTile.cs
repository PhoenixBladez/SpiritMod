using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Banners
{

	public class BannerTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);

			DustType = -1;
			TileID.Sets.DisableSmartCursor[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			int height = tile.TileFrameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Banners/BannerTile_Glow").Value, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White * .8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, Mod.Find<ModItem>(GetBannerItem(frameX)).Type);

		private static string GetBannerItem(int frameX)
		{
			int style = frameX / 18;
			return style switch
			{
				0 => "OccultistBanner",
				1 => "BeholderBanner",
				2 => "BottomFeederBanner",
				3 => "ValkyrieBanner",
				4 => "YureiBanner",
				5 => "SporeWheezerBanner",
				6 => "WheezerBanner",
				7 => "AstralAmalgamBanner",
				8 => "ShockhopperBanner",
				9 => "AncientApostleBanner",
				10 => "LostMimeBanner",
				11 => "StardancerBanner",
				12 => "CavernCrawlerBanner",
				13 => "OrbititeBanner",
				14 => "GladiatorSpiritBanner",
				15 => "AntlionAssassinBanner",
				16 => "CrystalDrifterBanner",
				17 => "GoldCrateMimicBanner",
				18 => "IronCrateMimicBanner",
				19 => "WoodCrateMimicBanner",
				20 => "GraniteSlimeBanner",
				21 => "BlazingRattlerBanner",
				22 => "GhastBanner",
				23 => "SpectralSkullBanner",
				24 => "GreenDungeonCubeBanner",
				25 => "PinkDungeonCubeBanner",
				26 => "BlueDungeonCubeBanner",
				27 => "WinterbornBanner",
				28 => "WinterbornHeraldBanner",
				29 => "DiseasedSlimeBanner",
				30 => "DiseasedBatBanner",
				31 => "CoconutSlimeBanner",
				32 => "BloaterBanner",
				33 => "ArterialGrasperBanner",
				34 => "FesterflyBanner",
				35 => "PutromaBanner",
				36 => "MasticatorBanner",
				37 => "BubbleBruteBanner",
				38 => "GluttonousDevourerBanner",
				39 => "ElectricEelBanner",
				40 => "BlossomHoundBanner",
				41 => "RlyehianBanner",
				42 => "MangoWarBanner",
				43 => "CrocosaurBanner",
				44 => "KakamoraGliderBanner",
				45 => "KakamoraThrowerBanner",
				46 => "KakamoraBruteBanner",
				47 => "KakamoraShielderBanner",
				48 => "KakamoraShielderBanner1",
				49 => "KakamoraShamanBanner",
				50 => "BriarthornSlimeBanner",
				51 => "DroseranTrapperBanner",
				52 => "GladeWraithBanner",
				53 => "CaptiveMaskBanner",
				54 => "DarkAlchemistBanner",
				55 => "BloatfishBanner",
				56 => "MechromancerBanner",
				57 => "KakamoraBanner",
				58 => "GloopBanner",
				59 => "ThornStalkerBanner",
				60 => "ForgottenOneBanner",
				61 => "DeadeyeMarksmanBanner",
				62 => "PhantomSamuraiBanner",
				63 => "FleshHoundBanner",
				64 => "CracklingCoreBanner",
				65 => "CavernBanditBanner",
				66 => "ReachmanBanner",
				67 => "HemaphoraBanner",
				68 => "MyceliumBotanistBanner",
				69 => "MoonlightPreserverBanner",
				70 => "MoonlightRupturerBanner",
				71 => "GiantJellyBanner",
				72 => "BloomshroomBanner",
				73 => "GlitterflyBanner",
				74 => "GlowToadBanner",
				75 => "LumantisBanner",
				76 => "LunarSlimeBanner",
				77 => "BlizzardBanditBanner",
				78 => "CrystalDrifterBanner",
				79 => "BloodGazerBanner",
				80 => "CystalBanner",
				81 => "WildwoodWatcherBanner",
				82 => "MoltenCoreBanner",
				83 => "PokeyBanner",
				84 => "ScreechOwlBanner",
				85 => "ArachmatonBanner",
				86 => "AstralAdventurerBanner",
				87 => "TrochmatonBanner",
				88 => "ChestZombie",
				89 => "BoulderBehemothBanner",
				90 => "FallingAsteroidBanner",
				91 => "GoblinGrenadierBanner",
				92 => "BlazingSkullBanner",
				93 => "StymphalianBatBanner",
				94 => "SkeletonBruteBanner",
				95 => "DraugrBanner",
				96 => "PirateLobberBanner",
				_ => "",
			};
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			int style = Main.tile[i, j].TileFrameX / 18;

			// NPC internal name array for banner buff usage.
			// If you need to add another npc, make sure there is a space before/after every NPC name apart from the very last, i.e.
			// "NPCOne NPCTwo NPCThree"
			// And *always* add it at the *end* of the string. Messing up the order will offset everything.

			string[] names = ("Occultist Beholder BottomFeeder Valkyrie PagodaGhostHostile SporeWheezer Wheezer AstralAmalgram DeepspaceHopper BoneHarpy LostMime" +
				" CogTrapperHead CavernCrawler Mineroid GladiatorSpirit AntlionAssassin CrystalDrifter GoldCrateMimic IronCrateMimic WoodCrateMimic GraniteSlime BlazingRattler" +
				" Illusionist SpectralSkull DungeonCubeGreen DungeonCubePink DungeonCubeBlue WinterbornMagic DiseasedSlime DiseasedBat OceanSlime Spewer CrimsonTrapper Vilemoth" +
				" Teratoma Masticator LargeCrustecean HellEater ElectricEel BlossomHound Rylheian MangoJelly Crocomount KakamoraParachuter SpearKakamora SwordKakamora" +
				" KakamoraShielder KakamoraShielderRare KakmoraShaman ReachSlime GrassVine ForestWraith CaptiveMask PlagueDoctor SwollenFish Mecromancer KakamoraRunner" +
				" GloopGloop ThornStalker ForgottenOne DeadArcher DeadArcher SamuraiHostile FleshHound GraniteCore CavernBandit Reachman Hemophora MycelialBotanist MoonlightPreserver" +
				" ExplodingMoonjelly MoonjellyGiant Bloomshroom Glitterfly GlowToad Lumantis LunarSlime BlizzardBandit CrystalDrifter BloodGazer Cystal ReachObserver" +
				" Molten_Core Pokey_Body ScreechOwl AutomataCreeper AstralAdventurer AutomataSpinner Chest_Zombie Boulder_Termagant Falling_Asteroid Goblin_Grenadier" +
				" BlazingSkull StymphalianBat Skeleton_Brute Enchanted_Armor PirateLobber").Split(' ');

			Main.SceneMetrics.NPCBannerBuff[Mod.Find<ModNPC>(names[style]).Type] = true;
			Main.SceneMetrics.hasBanner = true;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
		}
	}
}