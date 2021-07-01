
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Potion;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class MoonJellyDonut : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly Donut");
		}


		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.rare = 5;
			item.maxStack = 30;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;
			item.potion = true;
			item.healLife = 180;
			item.UseSound = SoundID.Item2;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.FindBuffIndex(BuffID.PotionSickness) >= 0) {
				return false;
			}
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) //pulsating glow effect in world
		{
            Lighting.AddLight(item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Consumable/Potion/MoonJellyDonut_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(mod.GetTexture("Items/Consumable/Potion/MoonJellyDonut_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                null, 
				Color.Lerp(Color.White, Color.Transparent, 0.75f), 
				rotation, 
				item.Size / 2, 
				MathHelper.Lerp(1f, 1.2f, (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f), 
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

			player.AddBuff(ModContent.BuffType<MoonBlessingDonut>(), 900);
			return true;
		}

		public override void UpdateInventory(Player player) => item.healLife = 180; //update the heal life back to 180 for tooltip and quick heal purposes

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach(TooltipLine line in tooltips.Where(x => x.mod == "Terraria" && x.Name == "HealLife")) {
				line.text = "Restores 180 life over 10 seconds";
			}
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<MoonJelly>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
