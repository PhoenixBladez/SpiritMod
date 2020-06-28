using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class YoyoCharm2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amazonian Charm");
			Tooltip.SetDefault("Increases melee speed by 7%\nAttacks may inflict poison");
		}


		public override void SetDefaults()
		{
			base.item.width = 16;
			base.item.height = 22;
			base.item.rare = ItemRarityID.Green;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.sellPrice(0, 0, 30, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeSpeed += 0.07f;
			player.GetSpiritPlayer().amazonCharm = true;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ItemID.JungleSpores, 10);
			modRecipe.AddIngredient(ItemID.Stinger, 3);
			modRecipe.AddIngredient(ItemID.Vine, 2);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
