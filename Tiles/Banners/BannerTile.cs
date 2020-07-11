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
		public override void SetDefaults() {
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

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
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
                    item = "KakmoraShamanBanner";
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
                default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}

		public override void NearbyEffects(int i, int j, bool closer) {
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
                    default:
						return;
				}
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}
