/*using SpiritMod.Tiles.Furniture;
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

namespace SpiritMod.Items.Consumable.Quest
{
	public class WeaponMasterHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weapon Master");
			Tooltip.SetDefault("'Or what's left of him, anyway...'\n'These strikes are too precise for just any monster'\n'One of the villagers must have killed him!'");
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (!QuestManager.GetQuest<MurderMysteryQuest>().IsCompleted)
			{
				Texture2D tex = ModContent.Request<Texture2D>("UI/QuestUI/Textures/ExclamationMark");
				float excscale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(item.Center.X, item.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + excscale, SpriteEffects.None, 0f);			
			}
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 0, 0);
			item.rare = -1;
			item.maxStack = 99;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
		}
	}
}*/