using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class IceKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Knife");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 22;
			item.height = 22;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.channel = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.IceKnife>();
			item.useAnimation = 27;
			item.consumable = true;
			item.maxStack = 999;
			item.useTime = 27;
			item.shootSpeed = 9.0f;
			item.damage = 9;
			item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 3);
			item.rare = 1;
			item.autoReuse = false;
			item.maxStack = 999;
			item.consumable = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}

	}
}
