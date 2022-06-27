using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.ButterflyStaff
{
	public class ButterflyStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Butterfly Staff");
			Tooltip.SetDefault("Summons a magical butterfly to fight for you\nLeft-click to cause butterflies to dissipate and leave behind exploding arcane stars");
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 10;
			Item.knockBack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<ButterflyMinion>();
			Item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.Butterflies, 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}