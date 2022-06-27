using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Face)]
	public class WinterHat : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winter Hat");
			Tooltip.SetDefault("Grants immunity to chilly water");

		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;

			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.wet) {
				player.buffImmune[BuffID.Chilled] = true;
			}
		}
	}
}
