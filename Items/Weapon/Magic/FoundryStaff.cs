using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class FoundryStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flamering Staff");
            Tooltip.SetDefault("Shoots out multiple intertwined rings of fire at the cursor position\nOnly one chain of rings can exist at once");
        }



        public override void SetDefaults()
        {
            item.damage = 60;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 32;
            item.mana = 9;
            item.useAnimation = 32;
            item.useStyle = 5;
            item.knockBack = 2.3f ;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shootSpeed = 14;
            item.shoot = mod.ProjectileType("FireChain1");
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //remove any other owned SpiritBow projectiles, just like any other sentry minion
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == item.shoot && p.owner == player.whoAmI)
                {
                    p.active = false;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ThermiteBar", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
