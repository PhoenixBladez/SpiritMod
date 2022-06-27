using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Tome");
			Tooltip.SetDefault("Rains down gilded stalactites from the sky\nThese stalactites stick to enemies and slow them down");
		}

		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MarbleStalactite>();
			Item.shootSpeed = 20f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 13);
			recipe.AddIngredient(ItemID.Book, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				Vector2 mouse = Main.MouseWorld;
				Projectile.NewProjectile(source, mouse.X, player.Center.Y - 700 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ModContent.ProjectileType<MarbleStalactite>(), damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
