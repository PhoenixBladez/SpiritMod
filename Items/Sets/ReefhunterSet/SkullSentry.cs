using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
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
			item.CloneDefaults(ItemID.StaffoftheFrostHydra);
			item.damage = 14;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.knockBack = 2f;
			item.shootSpeed = 0f;
			item.noMelee = true;
			item.autoReuse = true;
			item.sentry = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(gold: 2);
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item77;
			item.shoot = ModContent.ProjectileType<SkullSentrySentry>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			if (MouseTooFar(player))
				position = player.DirectionTo(position) * MAX_DISTANCE;

			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			if (MouseTooFar(player))
				return false;

			Projectile dummy = new Projectile();
			dummy.SetDefaults(item.shoot);

			Point topLeft = (Main.MouseWorld - dummy.Size / 2).ToTileCoordinates();
			Point bottomRight = (Main.MouseWorld + dummy.Size / 2).ToTileCoordinates();

			return !Collision.SolidTilesVersatile(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 12);
			recipe.AddIngredient(ItemID.Lens, 3);
			recipe.AddIngredient(ItemID.Worm);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		private bool MouseTooFar(Player player) => player.Distance(Main.MouseWorld) >= MAX_DISTANCE;
	}
}