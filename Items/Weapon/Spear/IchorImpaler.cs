using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
	public class IchorImpaler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Impaler");
			Tooltip.SetDefault("Hit foes course with ichor");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 24;
			item.height = 24;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.noMelee = true;
			item.useAnimation = 40;
			item.useTime = 40;
			item.shootSpeed = 6f;
			item.knockBack = 7f;
			item.damage = 50;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.LightRed;
			item.shoot = ModContent.ProjectileType<IchorImpalerProj>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FleshClump>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
