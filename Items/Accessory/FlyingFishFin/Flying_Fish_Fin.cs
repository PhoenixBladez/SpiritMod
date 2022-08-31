using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.FlyingFishFin
{
	public class Flying_Fish_Fin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flying Fish Fin");
			Tooltip.SetDefault("Increases jump height\n5% increased movement speed");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 10);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			Player.jumpHeight += 10;
			player.maxFallSpeed *= 1.35f;
		}	
	}
}