using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
{
	public class CoreCrusher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Crusher");
			Tooltip.SetDefault("Launches fire waves");
		}


		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.rare = 3;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 34;
			item.useTime = 34;
			item.knockBack = 7;
			item.value = 2000;
			item.damage = 57;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Flail.CoreCrusher>();
			item.shootSpeed = 15f;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ThermiteBar>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}