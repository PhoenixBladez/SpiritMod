using SpiritMod.Tiles.Furniture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
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

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.White;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<OccultistMapTile>();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<ZombieOriginQuest>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(Mod, "ItemName", "Quest Item");
				line.OverrideColor = new Color(100, 222, 122);
				tooltips.Add(line);
			}
		}

		public override bool OnPickup(Player player)
		{
			QuestManager.UnlockQuest<ZombieOriginQuest>();
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (!QuestManager.GetQuest<ZombieOriginQuest>().IsUnlocked)
			{
				Texture2D tex = Mod.Assets.Request<Texture2D>("UI/QuestUI/Textures/ExclamationMark").Value;
				float excscale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(Item.Center.X, Item.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + excscale, SpriteEffects.None, 0f);
			}
		}
	}
}