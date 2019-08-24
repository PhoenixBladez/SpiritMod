using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class HowlingScepter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Scepter");
			Tooltip.SetDefault("Shoots out a chilling bolt");
		}



        public override void SetDefaults()
        {
            item.damage = 11;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 25;
            item.mana = 4;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 0, 5, 0);
            item.rare = 1;
            item.autoReuse = false;
            item.shootSpeed = 9;
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("HowlingBolt");
        }

        public override void AddRecipes()
        {

            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FrigidFragment", 12);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
