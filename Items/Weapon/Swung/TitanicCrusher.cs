using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class TitanicCrusher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanic Crusher");
			Tooltip.SetDefault("Enemies around the head of the flail will be severely slowed");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 10;
			item.value = Item.sellPrice(0, 7, 43, 0);
			item.rare = ItemRarityID.LightPurple;
			item.crit = 8;
			item.damage = 74;
			item.knockBack = 8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = item.useAnimation = 32;
			item.scale = 1.1F;
			item.melee = true;
			item.noMelee = true;
			item.channel = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Flail.TitanicCrusher>();
			item.shootSpeed = 12.5F;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TidalEssence>(), 14);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}