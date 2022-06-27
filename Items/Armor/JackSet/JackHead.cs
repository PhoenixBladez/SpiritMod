using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.JackSet
{
	[AutoloadEquip(EquipType.Head)]
	public class JackHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Handsome Jack's Beautiful Visage");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightPurple;

			Item.vanity = true;
		}
    }
}
