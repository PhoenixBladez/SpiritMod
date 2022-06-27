using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ToolsMisc.OreWarhammersSet
{
	public class TitaniumWarhammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Warhammer");
		}


		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;

			Item.hammer = 89;

			Item.damage = 50;
			Item.knockBack = 8.5f;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;

			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 13);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}