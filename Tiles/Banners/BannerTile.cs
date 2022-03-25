using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Banners
{
	public class BannerTile : ModTile
	{
		public override void SetDefaults()
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

			dustType = -1;
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Banners/BannerTile_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White * .8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			string item;
			switch (style)
			{
				case 0:
					item = "OccultistBanner";
					break;
				case 1:
					item = "BeholderBanner";
					break;
				case 2:
					item = "BottomFeederBanner";
					break;
				case 3:
					item = "ValkyrieBanner";
					break;
				case 4:
					item = "YureiBanner";
					break;
				case 5:
					item = "SporeWheezerBanner";
					break;
				case 6:
					item = "WheezerBanner";
					break;
				case 7:
					item = "AstralAmalgamBanner";
					break;
				case 8:
					item = "ShockhopperBanner";
					break;
				case 9:
					item = "AncientApostleBanner";
					break;
				case 10:
					item = "LostMimeBanner";
					break;
				case 11:
					item = "StardancerBanner";
					break;
				case 12:
					item = "CavernCrawlerBanner";
					break;
				case 13:
					item = "OrbititeBanner";
					break;
				case 14:
					item = "GladiatorSpiritBanner";
					break;
				case 15:
					item = "AntlionAssassinBanner";
					break;
				case 16:
					item = "CrystalDrifterBanner";
					break;
				case 17:
					item = "GoldCrateMimicBanner";
					break;
				case 18:
					item = "IronCrateMimicBanner";
					break;
				case 19:
					item = "WoodCrateMimicBanner";
					break;
				case 20:
					item = "GraniteSlimeBanner";
					break;
				case 21:
					item = "BlazingRattlerBanner";
					break;
				case 22:
					item = "GhastBanner";
					break;
				case 23:
					item = "SpectralSkullBanner";
					break;
				case 24:
					item = "GreenDungeonCubeBanner";
					break;
				case 25:
					item = "PinkDungeonCubeBanner";
					break;
				case 26:
					item = "BlueDungeonCubeBanner";
					break;
				case 27:
					item = "WinterbornBanner";
					break;
				case 28:
					item = "WinterbornHeraldBanner";
					break;
				case 29:
					item = "DiseasedSlimeBanner";
					break;
				case 30:
					item = "DiseasedBatBanner";
					break;
				case 31:
					item = "CoconutSlimeBanner";
					break;
				case 32:
					item = "BloaterBanner";
					break;
				case 33:
					item = "ArterialGrasperBanner";
					break;
				case 34:
					item = "FesterflyBanner";
					break;
				case 35:
					item = "PutromaBanner";
					break;
				case 36:
					item = "MasticatorBanner";
					break;
				case 37:
					item = "BubbleBruteBanner";
					break;
				case 38:
					item = "GluttonousDevourerBanner";
					break;
				case 39:
					item = "ElectricEelBanner";
					break;
				case 40:
					item = "BlossomHoundBanner";
					break;
				case 41:
					item = "RlyehianBanner";
					break;
				case 42:
					item = "MangoWarBanner";
					break;
				case 43:
					item = "CrocosaurBanner";
					break;
				case 44:
					item = "KakamoraGliderBanner";
					break;
				case 45:
					item = "KakamoraThrowerBanner";
					break;
				case 46:
					item = "KakamoraBruteBanner";
					break;
				case 47:
					item = "KakamoraShielderBanner";
					break;
				case 48:
					item = "KakamoraShielderBanner1";
					break;
				case 49:
					item = "KakamoraShamanBanner";
					break;
				case 50:
					item = "BriarthornSlimeBanner";
					break;
				case 51:
					item = "DroseranTrapperBanner";
					break;
				case 52:
					item = "GladeWraithBanner";
					break;
				case 53:
					item = "CaptiveMaskBanner";
					break;
				case 54:
					item = "DarkAlchemistBanner";
					break;
				case 55:
					item = "BloatfishBanner";
					break;
				case 56:
					item = "MechromancerBanner";
					break;
				case 57:
					item = "KakamoraBanner";
					break;
				case 58:
					item = "GloopBanner";
					break;
				case 59:
					item = "ThornStalkerBanner";
					break;
				case 60:
					item = "ForgottenOneBanner";
					break;
				case 61:
					item = "DeadeyeMarksmanBanner";
					break;
				case 62:
					item = "PhantomSamuraiBanner";
					break;
				case 63:
					item = "FleshHoundBanner";
					break;
				case 64:
					item = "CracklingCoreBanner";
					break;
				case 65:
					item = "CavernBanditBanner";
					break;
				case 66:
					item = "ReachmanBanner";
					break;
				case 67:
					item = "HemaphoraBanner";
					break;
				case 68:
					item = "MyceliumBotanistBanner";
					break;
				case 69:
					item = "MoonlightPreserverBanner";
					break;
				case 70:
					item = "MoonlightRupturerBanner";
					break;
				case 71:
					item = "GiantJellyBanner";
					break;
				case 72:
					item = "BloomshroomBanner";
					break;
				case 73:
					item = "GlitterflyBanner";
					break;
				case 74:
					item = "GlowToadBanner";
					break;
				case 75:
					item = "LumantisBanner";
					break;
				case 76:
					item = "LunarSlimeBanner";
					break;
				case 77:
					item = "BlizzardBanditBanner";
					break;
				case 78:
					item = "CrystalDrifterBanner";
					break;
				case 79:
					item = "BloodGazerBanner";
					break;
				case 80:
					item = "CystalBanner";
					break;
				case 81:
					item = "WildwoodWatcherBanner";
					break;
				case 82:
					item = "MoltenCoreBanner";
					break;
				case 83:
					item = "PokeyBanner";
					break;
				case 84:
					item = "ScreechOwlBanner";
					break;
				case 85:
					item = "ArachmatonBanner";
					break;
				case 86:
					item = "AstralAdventurerBanner";
					break;
				case 87:
					item = "TrochmatonBanner";
					break;
				case 88:
					item = "ChestZombie";
					break;
				case 89:
					item = "BoulderBehemothBanner";
					break;
				case 90:
					item = "FallingAsteroidBanner";
					break;
				case 91:
					item = "GoblinGrenadierBanner";
					break;
				case 92:
					item = "BlazingSkullBanner";
					break;
				case 93:
					item = "StymphalianBatBanner";
					break;
				case 94:
					item = "SkeletonBruteBanner";
					break;
				case 95:
					item = "DraugrBanner";
					break;
				case 96:
					item = "PirateLobberBanner";
					break;
				default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;

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

				player.NPCBannerBuff[mod.NPCType(names[style])] = true;
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
		}
	}
}