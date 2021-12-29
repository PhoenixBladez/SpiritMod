using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

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
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Banners/BannerTile_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White * .8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			string item;
			switch (style) {
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
			if (closer) {
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				string type;
				switch (style) {
					case 0:
						type = "Occultist";
						break;
					case 1:
						type = "Beholder";
						break;
					case 2:
						type = "BottomFeeder";
						break;
					case 3:
						type = "Valkyrie";
						break;
					case 4:
						type = "PagodaGhostHostile";
						break;
					case 5:
						type = "SporeWheezer";
						break;
					case 6:
						type = "Wheezer";
						break;
					case 7:
						type = "AstralAmalgram";
						break;
					case 8:
						type = "DeepspaceHopper";
						break;
					case 9:
						type = "BoneHarpy";
						break;
					case 10:
						type = "LostMime";
						break;
					case 11:
						type = "CogTrapperHead";
						break;
					case 12:
						type = "CavernCrawler";
						break;
					case 13:
						type = "Mineroid";
						break;
					case 14:
						type = "GladiatorSpirit";
						break;
					case 15:
						type = "AntlionAssassin";
						break;
					case 16:
						type = "CrystalDrifter";
						break;
					case 17:
						type = "GoldCrateMimic";
						break;
					case 18:
						type = "IronCrateMimic";
						break;
					case 19:
						type = "WoodCrateMimic";
						break;
					case 20:
						type = "GraniteSlime";
						break;
					case 21:
						type = "BlazingRattler";
						break;
					case 22:
						type = "Illusionist";
						break;
					case 23:
						type = "SpectralSkull";
						break;
					case 24:
						type = "DungeonCubeGreen";
						break;
					case 25:
						type = "DungeonCubePink";
						break;
					case 26:
						type = "DungeonCubeBlue";
						break;
					case 27:
						type = "WinterbornMelee";
						break;
					case 28:
						type = "WinterbornMagic";
						break;
					case 29:
						type = "DiseasedSlime";
						break;
					case 30:
						type = "DiseasedBat";
						break;
					case 31:
						type = "OceanSlime";
						break;
					case 32:
						type = "Spewer";
						break;
					case 33:
						type = "CrimsonTrapper";
						break;
					case 34:
						type = "Vilemoth";
						break;
					case 35:
						type = "Teratoma";
						break;
					case 36:
						type = "Masticator";
						break;
					case 37:
						type = "LargeCrustecean";
						break;
					case 38:
						type = "HellEater";
						break;
					case 39:
						type = "ElectricEel";
						break;
					case 40:
						type = "BlossomHound";
						break;
					case 41:
						type = "Rylheian";
						break;
					case 42:
						type = "MangoJelly";
						break;
					case 43:
						type = "Crocomount";
						break;
					case 44:
						type = "KakamoraParachuter";
						break;
					case 45:
						type = "SpearKakamora";
						break;
					case 46:
						type = "SwordKakamora";
						break;
					case 47:
						type = "KakamoraShielder";
						break;
					case 48:
						type = "KakamoraShielderRare";
						break;
					case 49:
						type = "KakmoraShaman";
						break;
					case 50:
						type = "ReachSlime";
						break;
					case 51:
						type = "GrassVine";
						break;
					case 52:
						type = "ForestWraith";
						break;
					case 53:
						type = "CaptiveMask";
						break;
					case 54:
						type = "PlagueDoctor";
						break;
					case 55:
						type = "SwollenFish";
						break;
					case 56:
						type = "Mecromancer";
						break;
					case 57:
						type = "KakamoraRunner";
						break;
                    case 58:
                        type = "GloopGloop";
                        break;
                    case 59:
                        type = "ThornStalker";
                        break;
                    case 60:
                        type = "ForgottenOne";
                        break;
                    case 61:
                        type = "DeadArcher";
                        break;
                    case 62:
                        type = "SamuraiHostile";
                        break;
                    case 63:
                        type = "FleshHound";
                        break;
                    case 64:
                        type = "GraniteCore";
                        break;
                    case 65:
                        type = "CavernBandit";
                        break;
                    case 66:
                        type = "Reachman";
                        break;
                    case 67:
                        type = "Hemophora";
                        break;
                    case 68:
                        type = "MycelialBotanist";
                        break;
                    case 69:
                        type = "MoonlightPreserver";
                        break;
                    case 70:
                        type = "ExplodingMoonjelly";
                        break;
                    case 71:
                        type = "MoonjellyGiant";
                        break;
                    case 72:
                        type = "Bloomshroom";
                        break;
                    case 73:
                        type = "Glitterfly";
                        break;
                    case 74:
                        type = "GlowToad";
                        break;
                    case 75:
                        type = "Lumantis";
                        break;
                    case 76:
                        type = "LunarSlime";
                        break;
                    case 77:
                        type = "BlizzardBandit";
                        break;
                    case 78:
                        type = "CrystalDrifter";
                        break;
                    case 79:
                        type = "BloodGazer";
                        break;
					case 80:
						type = "Cystal";
						break;
					case 81:
						type = "ReachObserver";
						break;
					case 82:
						type = "Molten_Core";
						break;
					case 83:
						type = "Pokey_Body";
						break;
					case 84:
						type = "ScreechOwl";
						break;
					case 85:
						type = "AutomataCreeper";
						break;
					case 86:
						type = "AstralAdventurer";
						break;
					case 87:
						type = "AutomataSpinner";
						break;
					case 88:
						type = "Chest_Zombie";
						break;
					case 89:
						type = "Boulder_Termagant";
						break;
					case 90:
						type = "Falling_Asteroid";
						break;
					case 91:
						type = "Goblin_Grenadier";
						break;
					case 92:
						type = "BlazingSkull";
						break;
					case 93:
						type = "StymphalianBat";
						break;
					case 94:
						type = "Skeleton_Brute";
						break;
					case 95:
						type = "Enchanted_Armor";
						break;
					case 96:
						type = "PirateLobber";
						break;
					default:
						return;
				}
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}
