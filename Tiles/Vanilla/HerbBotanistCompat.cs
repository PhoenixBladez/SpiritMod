using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace SpiritMod.Tiles.Vanilla
{
	internal class HerbBotanistCompat
	{
		internal static void BotanistDrops(int i, int j)
		{
			(int type, int seed) = GetDropsByFrame(i, j);

			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, type, 1);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, seed, 2);
		}

		internal static HerbType GetTypeByFrame(int i, int j) => (HerbType)(Main.tile[i, j].TileFrameX / 18);

		internal static (int, int) GetDropsByFrame(int i, int j)
		{
			HerbType herb = GetTypeByFrame(i, j);

			if (herb == HerbType.Daybloom)
				return (ItemID.Daybloom, ItemID.DaybloomSeeds);
			else if (herb == HerbType.Moonglow)
				return (ItemID.Moonglow, ItemID.MoonglowSeeds);
			else if (herb == HerbType.Blinkroot)
				return (ItemID.Blinkroot, ItemID.BlinkrootSeeds);
			else if (herb == HerbType.Deathweed)
				return (ItemID.Deathweed, ItemID.DeathweedSeeds);
			else if (herb == HerbType.Waterleaf)
				return (ItemID.Waterleaf, ItemID.WaterleafSeeds);
			else if (herb == HerbType.Fireblossom)
				return (ItemID.Fireblossom, ItemID.FireblossomSeeds);
			return (ItemID.Shiverthorn, ItemID.ShiverthornSeeds);
		}
	}
}
