using SpiritMod.Items.Sets.SlagSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class HCharm : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellstone Charm");
			Tooltip.SetDefault("5% increased attack damage\nAttacks may burn enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item11;
			Item.accessory = true;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.value = Item.sellPrice(0, 0, 50, 0);
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
		}

		public override void AddRecipes()
		{
			var modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.HellstoneBar, 10);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
