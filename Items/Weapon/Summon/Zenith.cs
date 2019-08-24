using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Summon
{
    public class Zenith : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Sun's Zenith");
            Tooltip.SetDefault("Summons a Dark Sun above the player's head to shoot beams at foes\nBeams are capable of hitting individual enemies multiple times\nShadow Suns dissipate quickly and take up one minion slot");

        }


        public override void SetDefaults()
        {
            item.damage = 46;
            item.summon = true;
            item.mana = 9;
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
            item.shoot = mod.ProjectileType("ShadowSun");
            item.shootSpeed = 1f;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.ShadowFlameHexDoll, 1);
            modRecipe.AddIngredient(ItemID.HeatRay, 1);
            modRecipe.AddIngredient(ItemID.DarkShard, 1);
            modRecipe.AddIngredient(ItemID.LightShard, 1);
            modRecipe.AddIngredient(ItemID.LunarTabletFragment, 10);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }

    }
}