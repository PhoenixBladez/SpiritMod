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
			Item.width = 30;
			Item.height = 20;
			Item.value = Terraria.Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
		}
	}
}
