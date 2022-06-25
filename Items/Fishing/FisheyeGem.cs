using SpiritMod.GlobalClasses.Players;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SpiritMod.Items.Fishing
{
	public class FisheyeGem : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fisheye Gem");
			Tooltip.SetDefault("Increases the sell price of fish by 50%.\nWorks while in the inventory.");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateInventory(Player player) => player.GetModPlayer<MiscAccessoryPlayer>().accessory[AccName] = true;
	}

	public class FisheyeGlobalItem : GlobalItem
	{
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;

		private bool _increasedValue = false;

		public override void UpdateInventory(Item item, Player player)
		{
			if (!ItemSets.Fish.Contains(item.type))
				return;

			if (player.HasAccessory<FisheyeGem>() && !_increasedValue)
			{
				item.value = (int)(item.value * 1.5f);
				_increasedValue = true;
			}

			if (!player.HasAccessory<FisheyeGem>() && _increasedValue)
			{
				item.value = (int)(item.value / 1.5f);
				_increasedValue = false;
			}
		}
	}
}