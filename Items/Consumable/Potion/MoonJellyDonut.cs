using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Potion;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class MoonJellyDonut : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Moon Jelly Donut");

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 26;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 30;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.potion = true;
			Item.healLife = 180;
			Item.UseSound = SoundID.Item2;
		}

		public override bool CanUseItem(Player player) => player.FindBuffIndex(BuffID.PotionSickness) == -1;
		public override void UpdateInventory(Player player) => Item.healLife = 180; //update the heal life back to 180 for tooltip and quick heal purposes

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) //pulsating glow effect in world
		{
            Lighting.AddLight(Item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                Mod.Assets.Request<Texture2D>("Items/Consumable/Potion/MoonJellyDonut_Glow").Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Consumable/Potion/MoonJellyDonut_Glow").Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
                ),
                null, 
				Color.Lerp(Color.White, Color.Transparent, 0.75f), 
				rotation, 
				Item.Size / 2, 
				MathHelper.Lerp(1f, 1.2f, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f), 
				SpriteEffects.None, 
				0);
		}

		public override bool? UseItem(Player player)
		{
			Item.healLife = 0; //set item's heal life to 0 when actually used, so it doesnt heal player

			if (!player.pStone)
				player.AddBuff(BuffID.PotionSickness, 3600);
			else
				player.AddBuff(BuffID.PotionSickness, 2700);

			player.AddBuff(ModContent.BuffType<MoonBlessingDonut>(), 900);
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips.Where(x => x.Mod == "Terraria" && x.Name == "HealLife"))
				line.Text = "Restores 225 life over 10 seconds";
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<MoonJelly>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
