using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class CCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grisly Totem");
			Tooltip.SetDefault("Increases critical strike chance by 6%");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item11;
			Item.accessory = true;
			Item.value = Item.sellPrice(0, 0, 30, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Generic) += 6;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.Vertebrae, 8);
			modRecipe.AddIngredient(ItemID.TissueSample, 5);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
