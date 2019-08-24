using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Summon
{
    public class StarZenith : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion's Zenith");
            Tooltip.SetDefault("Left click to summon a Bright Star to shoot beams at foes above the player's head\nRight click to summon more unstable Bright Stars at the cursor position\nBeams shot out of Stars are capable of hitting multiple enemies\nEach star takes up one minion slot");

        }


        public override void SetDefaults()
        {
            item.damage = 62;
            item.summon = true;
            item.mana = 19;
            item.width = 66;
            item.height = 68;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("StarSun");
            item.shootSpeed = 1f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, mod.ProjectileType("StarSun1"), damage, knockBack, player.whoAmI);
                return false;

            }
            else
            {
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null,"Zenith", 1);
            modRecipe.AddIngredient(null,"SpiritBar", 7);
            modRecipe.AddIngredient(ItemID.FragmentStardust, 6);
            modRecipe.AddTile(TileID.LunarCraftingStation);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }

    }
}