using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class MarbleBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Longbow");
			Tooltip.SetDefault("Converts wooden arrows into heavy arrows \nHeavy arrows shatter upon hitting enemies or tiles");
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.noMelee = true;
			item.ranged = true;
			item.width = 22;
			item.height = 46;
			item.useTime = 37;
			item.useAnimation = 37;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.useTurn = false;
			item.shootSpeed = 6.2f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<MarbleLongbowArrow>();
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}