using Microsoft.Xna.Framework;
using SpiritMod.Particles;
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
			Item.damage = 36;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 58;
			Item.height = 24;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useTurn = false;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item96;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<IronBomberProj>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-20, -6);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 velUnit = Vector2.Normalize(new Vector2(speedX, speedY));
			Vector2 muzzleOffset = Vector2.Normalize(velUnit) * 34f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) 
				position += muzzleOffset;

			type = ModContent.ProjectileType<IronBomberProj>();

			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddRecipeGroup("IronBar", 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddIngredient(ModContent.ItemType<Placeable.Tiles.SpaceJunkItem>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}