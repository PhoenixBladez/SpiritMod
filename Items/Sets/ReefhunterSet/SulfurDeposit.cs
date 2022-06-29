using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class SulfurDeposit : ModItem
	{
		private int subID = 0; //Controls the in-world sprite for this item

		public override void SetStaticDefaults() => Tooltip.SetDefault("Highly explosive");

		public override void SetDefaults()
		{
			subID = Main.rand.Next(4);

			Item.value = 100;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
			Item.width = 28;
			Item.height = 28;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_World", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw(tex, Item.position - Main.screenPosition, new Rectangle(0, 28 * subID, 28, 26), GetAlpha(lightColor) ?? lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = Mod.CreateRecipe(ItemID.Grenade, 5);
			recipe.AddIngredient(this, 5);
			recipe.AddRecipeGroup("SpiritMod:CopperBars", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Mod.CreateRecipe(ItemID.Bomb, 3);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Mod.CreateRecipe(ItemID.Dynamite, 3);
			recipe.AddIngredient(this, 5);
			recipe.AddIngredient(ItemID.Rope, 1);
			recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
