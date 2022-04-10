using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class PendantOfTheOcean : AccessoryItem, ITimerItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pendant of the Ocean");
			Tooltip.SetDefault("Double tap {0} to call an ancient storm to the cursor location");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string down = Main.ReversedUpDownArmorSetBonuses ? "UP" : "DOWN";

			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0")
					line.text = line.text.Replace("{0}", down);
			}
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 48;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 0, 80, 0);
			item.melee = true;
			item.accessory = true;
			item.knockBack = 5f;
		}

		public int TimerCount() => 1;

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 8);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 4);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 8);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 4);
			recipe.AddIngredient(ItemID.LeadBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
