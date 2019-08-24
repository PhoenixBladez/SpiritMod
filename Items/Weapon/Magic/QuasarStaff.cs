using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class QuasarStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectral Supernova");
			Tooltip.SetDefault("Shoots out a powerful beam that releases multiple homing bolts");
		}


        int charger;
        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 64;
            item.magic = true;
            item.mana = 9;
            item.width = 66;
            item.height = 66;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3.5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("NovaBeam4");
            item.shootSpeed = 8f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "NightSkyStaff", 1);
            recipe.AddIngredient(null, "StellarBar", 5);
            recipe.AddIngredient(ItemID.Ectoplasm, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
