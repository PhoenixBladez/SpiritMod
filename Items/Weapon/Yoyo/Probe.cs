using SpiritMod.Items.Placeable.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Probe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Probe");
			Tooltip.SetDefault("Shoots out lasers in random arcs");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 52;
			Item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			base.Item.rare = ItemRarityID.LightPurple;
			base.Item.knockBack = 3f;
			base.Item.channel = true;
			base.Item.useStyle = ItemUseStyleID.Shoot;
			base.Item.useAnimation = 25;
			base.Item.useTime = 24;
			base.Item.shoot = ModContent.ProjectileType<ProbeP>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.HallowedBar, 11);
			recipe.AddIngredient(ItemID.SoulofMight, 13);
			recipe.AddIngredient(ModContent.ItemType<PrintProbe>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
