
using SpiritMod.Items.Sets.BismiteSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shield)]
	public class BismiteShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxious Targe");
			Tooltip.SetDefault("Being struck by an enemy poisons them\nIncreases defense by 1 for every poisoned enemy near the player\nThis effect stacks up to five times");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
			Item.DamageType = DamageClass.Melee;
            Item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
            Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().bismiteShield = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<LeatherShield>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
