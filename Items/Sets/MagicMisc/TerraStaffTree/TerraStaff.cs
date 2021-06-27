
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class TerraStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Scepter");
			Tooltip.SetDefault("'May the Wrath of the Elements consume your foes.'");
		}


		public override void SetDefaults()
		{
			item.damage = 78;
			item.magic = true;
			item.mana = 20;
			item.width = 58;
			item.height = 58;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 6;
			item.value = 200000;
			item.rare = ItemRarityID.Cyan;
			item.crit = 20;
			item.UseSound = SoundID.Item60;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Magic.TerraProj>();
			item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<TrueDarkStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<TrueHallowedStaff>(), 1);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();

			modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<TrueBloodStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<TrueHallowedStaff>(), 1);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
