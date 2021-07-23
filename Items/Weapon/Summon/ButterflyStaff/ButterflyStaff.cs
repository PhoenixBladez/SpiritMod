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
			Tooltip.SetDefault("Summons a magical butterfly to fight for you\nRight-click to cause butterflies to dissipate and leave behind exploding arcane stars");
		}

		public override void SetDefaults()
		{
			item.damage = 12;
			item.width = 40;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Blue;
			item.mana = 10;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<ButterflyMinion>();
			item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup(RecipeGroupID.Butterflies, 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}