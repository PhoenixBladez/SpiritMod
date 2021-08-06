using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.LaunchersMisc.IronBomber
{
	public class IronBomber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iron Bomber");
			Tooltip.SetDefault("Converts rockets into pulse grenades");
		}

		public override void SetDefaults()
		{
			item.damage = 36;
			item.ranged = true;
			item.width = 58;
			item.height = 24;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useTurn = false;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item96;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<IronBomberProj>();
			item.shootSpeed = 18f;
			item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-20, -6);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 34f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) 
				position += muzzleOffset;

			type = ModContent.ProjectileType<IronBomberProj>();
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddIngredient(ModContent.ItemType<Placeable.Tiles.SpaceJunkItem>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}