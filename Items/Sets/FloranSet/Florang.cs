using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet
{
	public class Florang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Cutter");
			Tooltip.SetDefault("Rolls along the ground, cutting up enemies \nVines occasionally ensnare the foes, reducing their movement speed \n'Sharp as a razorleaf'");
		}

		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 40;
			Item.useTime = 61;
			Item.useAnimation = 61;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 5f;
			Item.shoot = ModContent.ProjectileType<FloraP>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (velocity.X > 0)
				velocity.X = 2;
			else
				velocity.X = -2;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}