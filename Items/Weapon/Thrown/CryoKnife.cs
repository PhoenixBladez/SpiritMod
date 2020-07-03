using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CryoKnife : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bomb");
			Tooltip.SetDefault("Occasionally inflicts 'Cryo Crush'\n'Cryo Crush' does more damage as enemy health wanes\nThis effect does not apply to bosses, and deals a flat amount of damage instead");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Shuriken);
			item.width = 30;
			item.height = 30;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.CryoKnife>();
			item.useAnimation = 55;
			item.useTime = 55;
			item.shootSpeed = 10f;
			item.ranged = true;
			item.damage = 16;
			item.autoReuse = true;
			item.knockBack = 1f;
			item.value = Item.buyPrice(0, 0, 0, 35);
			item.rare = ItemRarityID.LightRed;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
