using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	[AutoloadEquip(EquipType.Back)]
	public class CloakOfVampire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloak Of The Vampire");
			Tooltip.SetDefault("Minions have a large chance to return life\nMinions do 18% less damage");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().VampireCloak = true;
			player.minionDamage -= 0.18f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CloakOfSpirit>());
			recipe.AddIngredient(ItemID.VampireKnives, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
