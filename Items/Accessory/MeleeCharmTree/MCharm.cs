using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class MCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marrow Pendant");
			Tooltip.SetDefault("Reduces damage taken by 5%");
		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 24;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
			Item.UseSound = SoundID.Item11;
			Item.accessory = true;
			Item.value = Item.buyPrice(0, 0, 30, 0);
			Item.value = Item.sellPrice(0, 0, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.Bone, 25);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
