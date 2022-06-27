using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class Dartboard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dartboard");
			Tooltip.SetDefault("13% reduced damage\n15% increased critical strike chance\n'Right on the mark'");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.buyPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) -= 0.13f;
			player.GetCritChance(DamageClass.Melee) += 15;
		}
	}
}
