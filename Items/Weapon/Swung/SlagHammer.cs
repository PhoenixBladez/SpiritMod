using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class SlagHammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Breaker");
			Tooltip.SetDefault("This hammer explodes after hitting 4 targets\nHold 'down' to keep swinging\nHitting enemies releases damaging sparks");
		}


        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.useStyle = 100;
            item.width = 40;
            item.height = 32;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
			item.channel = true;
            item.noMelee = true;
            item.useAnimation = 46;
            item.useTime = 46;
            item.shootSpeed = 6f;
            item.knockBack = 9f;
            item.damage = 39;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.rare = 3;
            item.shoot = mod.ProjectileType("SlagHammerProj");
        }
		public override bool UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            return true;
		
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 16);
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}