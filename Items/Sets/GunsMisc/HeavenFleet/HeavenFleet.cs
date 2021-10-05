using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenFleet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heaven Fleet");
			Tooltip.SetDefault("Converts regular bullets into bouncing stars \nHold down for a bigger blast");
		}

		public override void SetDefaults()
		{
			item.channel = true;
			item.damage = 45;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<HeavenFleetProj>();
			item.shootSpeed = 25f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<ConfluxPellet>();

			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<HeavenFleetProj>(), damage, knockBack, player.whoAmI, type);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Shotgun, 1);
			recipe.AddIngredient(ItemID.Star, 8);
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}