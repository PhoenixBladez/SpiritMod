using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class BottomlessHealingPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottomless Healing Potion");
			Tooltip.SetDefault("Non-consumable");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 34;
			item.rare = 4;
			item.maxStack = 1;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = false;
			item.autoReuse = false;
			ItemID.Sets.ItemNoGravity[item.type] = true;
			item.potion = true;
			item.healLife = 120;
			item.UseSound = SoundID.Item3;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool CanUseItem(Player player)
		{
			if (player.FindBuffIndex(BuffID.PotionSickness) >= 0) {
				return false;
			}
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) //pulsating glow effect in world
		{
			spriteBatch.Draw(Main.itemTexture[item.type], 
				item.Center - Main.screenPosition,
				null, 
				Color.Lerp(Color.White, Color.Transparent, 0.75f), 
				rotation, 
				item.Size / 2, 
				MathHelper.Lerp(1f, 1.3f, (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f), 
				SpriteEffects.None, 
				0);
		}
		public override bool UseItem(Player player)
		{
			item.healLife = 0; //set item's heal life to 0 when actually used, so it doesnt heal player
			if (!player.pStone)
				player.AddBuff(BuffID.PotionSickness, 3600);
			else
				player.AddBuff(BuffID.PotionSickness, 2700);
            if (player.statLife == player.statLifeMax2)
            {
                return false;
            }
			return true;
		}
		public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
		{
			healValue = 100;
		}
		public override bool ConsumeItem(Player player)
		{
			return false;
		}
		public override void UpdateInventory(Player player) => item.healLife = 100; //update the heal life back to 120 for tooltip and quick heal purposes

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach(TooltipLine line in tooltips.Where(x => x.mod == "Terraria" && x.Name == "HealLife")) {
				line.text = "Restores 100 health";
			}
		}
	}
}
