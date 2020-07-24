using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class SeedBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floral Seed Bag");
			Tooltip.SetDefault("Contains exotic grass seeds");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = 2;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.maxStack = 99;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.useTime = item.useAnimation = 20;
			item.useAnimation = 15;
			item.useTime = 10;
			item.noMelee = true;
			item.autoReuse = false;
		}


		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.MushroomGrassSeeds, Main.rand.Next(1, 5));
			player.QuickSpawnItem(ItemID.JungleGrassSeeds, Main.rand.Next(1, 5));
		}
	}
}
