using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves
{
	public class AdamantiteStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Staff");
			Tooltip.SetDefault("Shoots out a cluster of energy that splits into different directions");
		}


		public override void SetDefaults()
		{
			Item.damage = 51;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 41;
			Item.height = 41;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = Terraria.Item.sellPrice(0, 1, 40, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<AdamantiteStaffProj>();
			Item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.AdamantiteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
