using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
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
			Item.width = 26;
			Item.height = 34;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 1;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = false;
			Item.autoReuse = false;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			Item.potion = true;
			Item.healLife = 120;
			Item.UseSound = SoundID.Item3;
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
			spriteBatch.Draw(TextureAssets.Item[Item.type].Value, 
				Item.Center - Main.screenPosition,
				null, 
				Color.Lerp(Color.White, Color.Transparent, 0.75f), 
				rotation, 
				Item.Size / 2, 
				MathHelper.Lerp(1f, 1.3f, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f), 
				SpriteEffects.None, 
				0);
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.healLife = 0; //set item's heal life to 0 when actually used, so it doesnt heal player
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
		public override void UpdateInventory(Player player) => Item.healLife = 100; //update the heal life back to 120 for tooltip and quick heal purposes

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach(TooltipLine line in tooltips.Where(x => x.Mod == "Terraria" && x.Name == "HealLife")) {
				line.Text = "Restores 100 health";
			}
		}
	}
}
