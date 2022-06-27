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
			Item.width = 16;
			Item.height = 22;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item11;
			Item.accessory = true;
			Item.value = Item.sellPrice(0, 0, 30, 0);
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual) => player.GetAttackSpeed(DamageClass.Melee) += 0.07f;

		public override void AddRecipes()
		{
			var modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.JungleSpores, 10);
			modRecipe.AddIngredient(ItemID.Stinger, 3);
			modRecipe.AddIngredient(ItemID.Vine, 2);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
