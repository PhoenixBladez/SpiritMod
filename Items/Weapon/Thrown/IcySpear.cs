using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Thrown.Charge;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class IcySpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Javelin");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage\nOccaisionally frostburns foes");
		}


		public override void SetDefaults()
		{
			item.damage = 9;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = ItemRarityID.Blue;
			item.width = 18;
			item.height = 18;
			item.useTime = 15;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.melee = true;
			item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<FrigidJavelinProj>();
			item.shootSpeed = 0f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
