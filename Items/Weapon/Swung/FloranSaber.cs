using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class FloranSaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Saber");
			Tooltip.SetDefault("Vines occasionally ensnare foes, reducing movement speed");
		}


		public override void SetDefaults()
		{
            item.damage = 17;            
            item.melee = true;
            item.width = 30;
            item.height = 36;
			item.useTime = 19;
			item.useAnimation = 19;
            item.useStyle = 1;
			item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FloranBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }


        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            {
                if (Main.rand.Next(5) == 0) target.AddBuff(mod.BuffType("VineTrap"), 180);
            }
        }

    }
}