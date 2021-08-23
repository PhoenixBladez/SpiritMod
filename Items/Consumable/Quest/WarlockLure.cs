using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Items.Consumable.Quest
{
	public class WarlockLureCorruption : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Magic");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 7));
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
				TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'Hopefully, the evil necromancer is intrigued by this strange magic'");
				line1.overrideColor = new Color(255, 255, 255);
				tooltips.Add(line1);
			}
			else
			{
				TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "The Undead Warlock is no more!\nThere's no need for this repulsive thing anymore");
				line1.overrideColor = new Color(255, 255, 255);
				tooltips.Add(line1);
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddIngredient(ItemID.RottenChunk, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}   
	}
    public class WarlockLureCrimson : WarlockLureCorruption
    {
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}        
    }
}
