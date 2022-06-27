using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class HellEater : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Maw");
			Tooltip.SetDefault("Magic attacks may shoot out fiery spit that explode upon hitting enemies\n7% increased magic damage");

		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().fireMaw = true;
			player.GetDamage(DamageClass.Magic) += 0.07f;
		}
	}
}
