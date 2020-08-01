
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class KoiTotem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Koi Totem");
			Tooltip.SetDefault("Increases fishing skill\nTotem occasionally spits out the bait that was used for reusability");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 36;
			item.value = Item.sellPrice(gold: 1);
			item.rare = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().KoiTotem = true;
			player.fishingSkill = player.fishingSkill + 5;
		}

	}
}
