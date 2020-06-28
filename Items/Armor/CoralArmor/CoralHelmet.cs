
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CoralArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class CoralHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Helmet");
			Tooltip.SetDefault("Increases breath time while underwater");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Terraria.Item.sellPrice(0, 0, 8, 0);
			item.rare = 1;
			item.defense = 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<CoralBody>() && legs.type == ModContent.ItemType<CoralLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Getting hurt by enemies also causes them to take some damage\nAllows for free movement while underwater";
			player.GetSpiritPlayer().coralSet = true;
			player.accFlipper = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.breathMax = 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Coral, 8);
			recipe.AddIngredient(ItemID.Seashell, 2);
			recipe.AddIngredient(ItemID.Starfish, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
