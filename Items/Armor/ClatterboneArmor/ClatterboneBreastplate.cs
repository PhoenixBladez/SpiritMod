
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
			Item.width = 34;
			Item.height = 30;
			Item.value = Item.buyPrice(silver: 60);
			Item.rare = ItemRarityID.Green;

			Item.defense = 4;
		}
	}
}
