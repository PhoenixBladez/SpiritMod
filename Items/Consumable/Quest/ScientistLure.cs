using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ScientistLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grisly Science Experiment");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<ZombieOriginQuest>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
				line.overrideColor = new Color(100, 222, 122);
				tooltips.Add(line);
				TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'Hopefully, the scientist is intrigued by this abomination'");
				line1.overrideColor = new Color(255, 255, 255);
				tooltips.Add(line1);
			}
			else
			{
				TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "The Undead Scientist is no more!\nThere's no need for this disgusting thing anymore");
				line1.overrideColor = new Color(255, 255, 255);
				tooltips.Add(line1);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Material.OldLeather>(), 2);
			recipe.AddIngredient(ItemID.RottenChunk, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}  
	}
}
