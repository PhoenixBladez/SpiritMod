using SpiritMod.Tiles.Furniture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using System.Collections.Generic;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class OccultistMap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Occult Wall Scroll");
			Tooltip.SetDefault("'The ink is of questionable origin'");
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (!QuestWorld.zombieQuestStart)
			{
				Texture2D tex = mod.GetTexture("UI/QuestUI/Textures/ExclamationMark");
				float excscale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(item.Center.X, item.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + excscale, SpriteEffects.None, 0f);			
			}
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.White;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<OccultistMapTile>();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<ZombieOriginQuest>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
				line.overrideColor = new Color(100, 222, 122);
				tooltips.Add(line);
			}
		}
		public override bool OnPickup(Player player) 
		{
			if (!QuestWorld.zombieQuestStart)
			{
				QuestWorld.zombieQuestStart = true;
			}
             return true;
         }
	}
}