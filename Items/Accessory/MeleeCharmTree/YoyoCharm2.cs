using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class YoyoCharm2 : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amazonian Charm");
			Tooltip.SetDefault("Increases melee speed by 7%\nAttacks may inflict poison");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 22;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item11;
			item.accessory = true;
			item.value = Item.sellPrice(0, 0, 30, 0);
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual) => player.meleeSpeed += 0.07f;

		public override void AddRecipes()
		{
			var modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.JungleSpores, 10);
			modRecipe.AddIngredient(ItemID.Stinger, 3);
			modRecipe.AddIngredient(ItemID.Vine, 2);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
