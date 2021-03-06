using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class BlightedBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blighted Bow");
			Tooltip.SetDefault("Wooden Arrows turn into pestilent arrows");
		}



		public override void SetDefaults()
		{
			item.damage = 37;
			item.noMelee = true;
			item.ranged = true;
			item.width = 24;
			item.height = 46;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 14f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<PestilentArrow>();
			}
				Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PutridPiece>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
