using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class JungleStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rosevine Staff");
			Tooltip.SetDefault("Shoots a bouncing mass of vines and flowers");
		}


		public override void SetDefaults()
		{
			Item.damage = 31;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 33;
			Item.useAnimation = 33;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<ThornSpike>();
			Item.shootSpeed = 13f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddIngredient(ItemID.Stinger, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
