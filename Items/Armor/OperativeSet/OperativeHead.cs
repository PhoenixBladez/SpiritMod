using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.OperativeSet
{
	[AutoloadEquip(EquipType.Head)]
	public class OperativeHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Operative's Digistruct Mask");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Terraria.Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}
	}
}
