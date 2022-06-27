
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
			Item.damage = 78;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.value = 200000;
			Item.rare = ItemRarityID.Cyan;
			Item.crit = 20;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.TerraProj>();
			Item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<TrueDarkStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<TrueHallowedStaff>(), 1);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();

			modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<TrueBloodStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<TrueHallowedStaff>(), 1);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}
