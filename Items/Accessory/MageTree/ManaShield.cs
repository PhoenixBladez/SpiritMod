
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using SpiritMod.Items.Accessory.Leather;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MageTree
{
    [AutoloadEquip(EquipType.Shield)]
    public class ManaShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Shield");
			Tooltip.SetDefault("Increases maximum mana by 20\nAbsorbs 10% of the damage dealt by enemies\nThis damage is converted into a loss of mana instead\nThe amount of mana lost is equal to 4x the damage absorbed");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().manaShield = true;
			player.statManaMax2 += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<LeatherShield>());
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddRecipeGroup("SpiritMod:PHMEvilMaterial", 2);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
