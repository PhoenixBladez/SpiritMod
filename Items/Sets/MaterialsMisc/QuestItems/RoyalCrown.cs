using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Items.Sets.MaterialsMisc.QuestItems
{
	public class RoyalCrown : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Royal Crown");

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = -11;
			Item.maxStack = 99;
		}

		public override bool OnPickup(Player player) => !player.HasItem(ModContent.ItemType<RoyalCrown>());

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<StylistQuestSeafoam>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(Mod, "FavoriteDesc", "Quest Item") {
					OverrideColor = new Color(100, 222, 122)
				};
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(Mod, "FavoriteDesc", "'Stunningly ornate despite centuries of weathering'") {
				OverrideColor = new Color(255, 255, 255)
			};
			tooltips.Add(line1);
		}
		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.Name == "ItemName")
			{
				Vector2 lineposition = new Vector2(line.OriginalX, line.OriginalY);
				Utils.DrawBorderString(Main.spriteBatch, line.Text, lineposition, Color.LightGoldenrodYellow);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix); //starting a new spritebatch here, since additive blend mode seems to be the only way to make the line transparent?
				for (int i = 0; i < 4; i++)
				{
					Vector2 drawpos = lineposition + new Vector2(0, 2 * (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 4) / 2) + 0.5f)).RotatedBy(i * MathHelper.PiOver2);
					Utils.DrawBorderString(Main.spriteBatch, line.Text, drawpos, Color.Goldenrod);
				}
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				return false;
			}
			return base.PreDrawTooltipLine(line, ref yOffset);
		}
	}
}
