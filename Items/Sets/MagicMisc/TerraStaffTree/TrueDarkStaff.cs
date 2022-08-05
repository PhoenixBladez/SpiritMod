using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class TrueDarkStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Horizon's Edge");
			Tooltip.SetDefault("Shoots out a splitting volley of homing pestilence and cursed fire.");
		}


		public override void SetDefaults()
		{
			Item.damage = 62;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 15;
			Item.width = 66;
			Item.height = 68;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item92;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.CursedFire>();
			Item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ModContent.ItemType<NightStaff>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}