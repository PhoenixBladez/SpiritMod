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
			item.width = 22;
			item.height = 22;

			item.value = 3000;
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.wet) {
				player.buffImmune[BuffID.Chilled] = true;
			}
		}
	}
}
