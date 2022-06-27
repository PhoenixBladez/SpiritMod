using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ToolsMisc.OreWarhammersSet
{
	public class CobaltWarhammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Warhammer");
		}


		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 40;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;

			Item.hammer = 80;

			Item.damage = 38;
			Item.knockBack = 5.5f;

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
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}