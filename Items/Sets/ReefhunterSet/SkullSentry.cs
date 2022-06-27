using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class SkullSentry : ModItem
	{
		const float MAX_DISTANCE = 600f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maneater Skull");
			Tooltip.SetDefault("Summons a skull infested by maneater worms that fire red mucus at nearby enemies");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StaffoftheFrostHydra);
			Item.damage = 14;
			Item.width = 28;
			Item.height = 14;
			Item.useTime = Item.useAnimation = 30;
			Item.knockBack = 2f;
			Item.shootSpeed = 0f;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.sentry = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item77;
			Item.shoot = ModContent.ProjectileType<SkullSentrySentry>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
			if (MouseTooFar(player))
				position = player.DirectionTo(position) * MAX_DISTANCE;

			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			if (MouseTooFar(player))
				return false;

			Projectile dummy = new Projectile();
			dummy.SetDefaults(Item.shoot);

			Point topLeft = (Main.MouseWorld - dummy.Size / 2).ToTileCoordinates();
			Point bottomRight = (Main.MouseWorld + dummy.Size / 2).ToTileCoordinates();

			return !Collision.SolidTilesVersatile(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 12);
			recipe.AddIngredient(ItemID.Lens, 3);
			recipe.AddIngredient(ItemID.Worm);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		private bool MouseTooFar(Player player) => player.Distance(Main.MouseWorld) >= MAX_DISTANCE;
	}
}