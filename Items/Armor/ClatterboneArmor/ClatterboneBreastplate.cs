
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ClatterboneArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class ClatterboneBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatterbone Breastplate");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Item.buyPrice(silver: 60);
			item.rare = ItemRarityID.Green;

			item.defense = 4;
		}
	}
}
