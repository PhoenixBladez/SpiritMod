using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class IceKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sliding Ice");
			Tooltip.SetDefault("Slides along the ground");
		}


		public override void SetDefaults()
		{
			item.damage = 9;
			item.melee = true;
			item.width = 44;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 0;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 10);
			item.rare = ItemRarityID.Green;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("SlidingIce");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.consumable = true;
			item.maxStack = 999;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(speedX > 0) {
				speedX = 2;
			} else {
				speedX = -2;
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}

	}
}
