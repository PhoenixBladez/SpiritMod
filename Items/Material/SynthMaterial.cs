using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Material
{
	public class SynthMaterial : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Discharge Tubule");
			Tooltip.SetDefault("'The colorful tubes are filled with energized gas'");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 42;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 999;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Material/SynthMaterial_Glow"),
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
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ModContent.ItemType<Items.Sets.CoilSet.TechDrive>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
