using SpiritMod.Tiles.Banners;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Banners
{
	public abstract class BaseBannerItem : ModItem 
	{
		protected abstract int Style { get; }

		public sealed override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<BannerTile>();
			Item.placeStyle = Style;
		}
	}

	public class OccultistBanner : BaseBannerItem 
	{
		protected override int Style => 0;
	}

    public class BeholderBanner : BaseBannerItem
	{
		protected override int Style => 1;
	}

	public class BottomFeederBanner : BaseBannerItem
	{
		protected override int Style => 2;
	}

	public class ValkyrieBanner : BaseBannerItem
	{
		protected override int Style => 3;
	}

	public class YureiBanner : BaseBannerItem
    {
		protected override int Style => 4;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Yuurei Banner");
    }

    public class SporeWheezerBanner : BaseBannerItem
	{
		protected override int Style => 5;
	}

	public class WheezerBanner : BaseBannerItem
	{
		protected override int Style => 6;
	}

	public class AstralAmalgamBanner : BaseBannerItem
	{
		protected override int Style => 7;
	}

	public class ShockhopperBanner : BaseBannerItem
	{
		protected override int Style => 8;
	}

	public class AncientApostleBanner : BaseBannerItem
	{
		protected override int Style => 9;
	}

	public class LostMimeBanner : BaseBannerItem
	{
		protected override int Style => 10;
	}

	public class StardancerBanner : BaseBannerItem
	{
		protected override int Style => 11;
	}

	public class CavernCrawlerBanner : BaseBannerItem
	{
		protected override int Style => 12;
	}

	public class OrbititeBanner : BaseBannerItem
	{
		protected override int Style => 13;
	}

	public class GladiatorSpiritBanner : BaseBannerItem
	{
		protected override int Style => 14;
	}

	public class AntlionAssassinBanner : BaseBannerItem
	{
		protected override int Style => 15;
	}

	public class GoldCrateMimicBanner : BaseBannerItem
	{
		protected override int Style => 16;
	}

	public class IronCrateMimicBanner : BaseBannerItem
	{
		protected override int Style => 17;
	}

	public class WoodCrateMimicBanner : BaseBannerItem
	{
		protected override int Style => 18;
	}

	public class GraniteSlimeBanner : BaseBannerItem
	{
		protected override int Style => 19;
	}

	public class BlazingRattlerBanner : BaseBannerItem
	{
		protected override int Style => 20;
	}

	public class GhastBanner : BaseBannerItem
	{
		protected override int Style => 21;
	}

	public class SpectralSkullBanner : BaseBannerItem
	{
		protected override int Style => 22;
	}

	public class GreenDungeonCubeBanner : BaseBannerItem
	{
		protected override int Style => 23;
	}

	public class PinkDungeonCubeBanner : BaseBannerItem
	{
		protected override int Style => 24;
	}

	public class BlueDungeonCubeBanner : BaseBannerItem
	{
		protected override int Style => 25;
	}

	public class WinterbornBanner : BaseBannerItem
	{
		protected override int Style => 26;
	}

	public class WinterbornHeraldBanner : BaseBannerItem
	{
		protected override int Style => 27;
	}

	public class DiseasedSlimeBanner : BaseBannerItem
	{
		protected override int Style => 28;
	}

	public class DiseasedBatBanner : BaseBannerItem
	{
		protected override int Style => 29;
	}

	public class CoconutSlimeBanner : BaseBannerItem
	{
		protected override int Style => 30;
	}

	public class BloaterBanner : BaseBannerItem
	{
		protected override int Style => 31;
	}

	public class ArterialGrasperBanner : BaseBannerItem
	{
		protected override int Style => 32;
	}

	public class FesterflyBanner : BaseBannerItem
	{
		protected override int Style => 33;
	}

	public class PutromaBanner : BaseBannerItem
	{
		protected override int Style => 34;
	}

	public class MasticatorBanner : BaseBannerItem
	{
		protected override int Style => 35;
	}

	public class BubbleBruteBanner : BaseBannerItem
	{
		protected override int Style => 36;
	}

	public class GluttonousDevourerBanner : BaseBannerItem
	{
		protected override int Style => 37;
	}

	public class ElectricEelBanner : BaseBannerItem
	{
		protected override int Style => 38;
	}

	public class BlossomHoundBanner : BaseBannerItem
	{
		protected override int Style => 39;
	}

	public class RlyehianBanner : BaseBannerItem
    {
		protected override int Style => 40;
		public override void SetStaticDefaults() => DisplayName.SetDefault("R'lyehian Banner");
    }

    public class MangoWarBanner : BaseBannerItem
    {
		protected override int Style => 41;
        public override void SetStaticDefaults() => DisplayName.SetDefault("Mang O' War Banner");
	}

    public class CrocosaurBanner : BaseBannerItem
	{
		protected override int Style => 42;
	}

	public class KakamoraGliderBanner : BaseBannerItem
	{
		protected override int Style => 43;
	}

	public class KakamoraThrowerBanner : BaseBannerItem
    {
		protected override int Style => 44;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Kakamora Lobber Banner");
	}

    public class KakamoraBruteBanner : BaseBannerItem
	{
		protected override int Style => 45;
	}

	public class KakamoraShielderBanner : BaseBannerItem
	{
		protected override int Style => 46;
	}

	public class KakamoraShielderBanner1 : BaseBannerItem
    {
		protected override int Style => 47;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Kakamora Guard Banner");
    }

    public class KakamoraShamanBanner : BaseBannerItem
	{
		protected override int Style => 48;
	}

	public class BriarthornSlimeBanner : BaseBannerItem
	{
		protected override int Style => 49;
	}

	public class DroseranTrapperBanner : BaseBannerItem
	{
		protected override int Style => 50;
	}

	public class GladeWraithBanner : BaseBannerItem
	{
		protected override int Style => 51;
	}

	public class CaptiveMaskBanner : BaseBannerItem
	{
		protected override int Style => 52;
	}

	public class DarkAlchemistBanner : BaseBannerItem
	{
		protected override int Style => 53;
	}

	public class BloatfishBanner : BaseBannerItem
	{
		protected override int Style => 54;
	}

	public class MechromancerBanner : BaseBannerItem
	{
		protected override int Style => 56;
	}

	public class KakamoraBanner : BaseBannerItem
	{
		protected override int Style => 57;
	}

	public class GloopBanner : BaseBannerItem
	{
		protected override int Style => 58;
	}

	public class ThornStalkerBanner : BaseBannerItem
	{
		protected override int Style => 59;
	}

	public class ForgottenOneBanner : BaseBannerItem
	{
		protected override int Style => 60;
	}

	public class DeadeyeMarksmanBanner : BaseBannerItem
	{
		protected override int Style => 61;
	}

	public class PhantomSamuraiBanner : BaseBannerItem
	{
		protected override int Style => 62;
	}

	public class FleshHoundBanner : BaseBannerItem
	{
		protected override int Style => 63;
	}

	public class CracklingCoreBanner : BaseBannerItem
	{
		protected override int Style => 64;
	}

	public class CavernBanditBanner : BaseBannerItem
	{
		protected override int Style => 65;
	}

	public class ReachmanBanner : BaseBannerItem
	{
		protected override int Style => 66;
	}

	public class HemaphoraBanner : BaseBannerItem
	{
		protected override int Style => 67;
	}

	public class MyceliumBotanistBanner : BaseBannerItem
	{
		protected override int Style => 68;
	}

	public class MoonlightPreserverBanner : BaseBannerItem
	{
		protected override int Style => 69;
	}

	public class MoonlightRupturerBanner : BaseBannerItem
	{
		protected override int Style => 70;
	}

	public class GiantJellyBanner : BaseBannerItem
	{
		protected override int Style => 71;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Tethervolt Jelly Banner");
    }

    public class BloomshroomBanner : BaseBannerItem
	{
		protected override int Style => 72;
	}

	public class GlitterflyBanner : BaseBannerItem
	{
		protected override int Style => 73;
	}

	public class GlowToadBanner : BaseBannerItem
	{
		protected override int Style => 74;
	}

	public class LumantisBanner : BaseBannerItem
	{
		protected override int Style => 75;
	}

	public class LunarSlimeBanner : BaseBannerItem
	{
		protected override int Style => 76;
	}

	public class BlizzardBanditBanner : BaseBannerItem
	{
		protected override int Style => 77;
	}

	public class CrystalDrifterBanner : BaseBannerItem
	{
		protected override int Style => 78;
	}

	public class BloodGazerBanner : BaseBannerItem
	{
		protected override int Style => 79;
	}

	public class CystalBanner : BaseBannerItem
	{
		protected override int Style => 80;
	}

	public class WildwoodWatcherBanner : BaseBannerItem
	{
		protected override int Style => 81;
	}

	public class MoltenCoreBanner : BaseBannerItem
	{
		protected override int Style => 82;
	}

	public class PokeyBanner : BaseBannerItem
	{
		protected override int Style => 83;
	}

	public class ScreechOwlBanner : BaseBannerItem
	{
		protected override int Style => 84;
	}

	public class ArachmatonBanner : BaseBannerItem
	{
		protected override int Style => 85;
	}

	public class AstralAdventurerBanner : BaseBannerItem
	{
		protected override int Style => 86;
	}

	public class TrochmatonBanner : BaseBannerItem
	{
		protected override int Style => 87;
	}

	public class ChestZombieBanner : BaseBannerItem
	{
		protected override int Style => 88;
	}

	public class BoulderBehemothBanner : BaseBannerItem
	{
		protected override int Style => 89;
	}

	public class FallingAsteroidBanner : BaseBannerItem
	{
		protected override int Style => 90;
	}

	public class GoblinGrenadierBanner : BaseBannerItem
	{
		protected override int Style => 91;
	}

	public class BlazingSkullBanner : BaseBannerItem
	{
		protected override int Style => 92;
	}

	public class StymphalianBatBanner : BaseBannerItem
	{
		protected override int Style => 93;
	}

	public class SkeletonBruteBanner : BaseBannerItem
	{
		protected override int Style => 94;
	}

	public class DraugrBanner : BaseBannerItem
	{
		protected override int Style => 95;
	}

	public class PirateLobberBanner : BaseBannerItem
	{
		protected override int Style => 96;
	}
}
