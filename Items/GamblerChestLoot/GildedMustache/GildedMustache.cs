using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.GamblerChestLoot.GildedMustache
{
	[AutoloadEquip(EquipType.Head)]
	public class GildedMustache : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Gilded Mustache");
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Blue;

			item.vanity = true;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
			=> drawHair = true;

	}
}
