using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BloodcourtSet
{
	public class FangTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Thousand Fangs");
			Tooltip.SetDefault("Summon a set of gnashing teeth\nInflicts 'Surging Anguish'");
		}

		static int offsetLength = 23;
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 12;
			Item.width = 28;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<TeethTop>();
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.shootSpeed = 8f;
			Item.crit = 6;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(source, position.X, (position.Y - offsetLength) - 12, 0, 0, ModContent.ProjectileType<TeethTop>(), damage, knockback, Main.myPlayer, offsetLength);
			Projectile.NewProjectile(source, position.X + 4, (position.Y + offsetLength) + 12, 0, 0, ModContent.ProjectileType<TeethBottom>(), damage, knockback, Main.myPlayer, offsetLength);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}