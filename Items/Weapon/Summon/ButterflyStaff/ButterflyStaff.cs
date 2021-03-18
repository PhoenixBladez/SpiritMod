using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon.ButterflyMinion;
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
			item.width = 40;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = 2;
			item.mana = 10;
			item.damage = 5;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<ButterflyMinion>();
			item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
        }
        public override bool CanUseItem(Player player)
        {
            item.shoot = ModContent.ProjectileType<ButterflyMinion>();
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            if (player.altFunctionUse != 2)
            {
                Vector2 mouse = Main.MouseWorld;
                float distance = Vector2.Distance(mouse, position);
                if (distance < 600f)
                {
                    Projectile.NewProjectile(mouse.X, mouse.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                }
                player.AddBuff(ModContent.BuffType<ButterflyMinionBuff>(), 3600);
            }
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

			ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddRecipeGroup(ModContent.ItemType<Items.Consumable.BriarmothItem>(), 1);
            recipe1.AddIngredient(ItemID.FallenStar, 2);
            recipe1.AddRecipeGroup(RecipeGroupID.Wood, 15);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}