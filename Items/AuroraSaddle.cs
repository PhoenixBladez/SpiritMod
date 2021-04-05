using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	// TODO: make this into an item that summons a mount
	public class AuroraSaddle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Saddle");
			Tooltip.SetDefault("Summons a vibrant steed mount\nYou shouldn't see this");
		}

		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 34;
			item.maxStack = 1;
		}

		public override bool ItemSpace(Player player)
		{
			return true;
		}
	}
}
