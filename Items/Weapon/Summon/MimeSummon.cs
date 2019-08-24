using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Summon
{
    public class MimeSummon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Two-Faced Mask");
			Tooltip.SetDefault("Summons either a Soul of Happiness or Sadness at the cursor position\nThe Soul of Happiness shoots out beams at foes\nThe Soul of Sadness shoots out homing tears at foes\nOnly one of each soul can exist at once");
		}


        public override void SetDefaults()
        {
            item.damage = 25;
            item.summon = true;
            item.mana = 9;
            item.width = 44;
            item.height = 48;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("HappySoul");
            item.shootSpeed = 0f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (Main.rand.Next(2) == 0)
			{
				type = mod.ProjectileType("SadSoul");
			}
            //remove any other owned SpiritBow projectiles, just like any other sentry minion
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == item.shoot && p.owner == player.whoAmI)
                {
                    p.active = false;
                }
            }
            //projectile spawns at mouse cursor
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            return true;
        }
		   public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "MimeMask", 1);
            modRecipe.AddIngredient(null, "BloodFire", 5);
            modRecipe.AddIngredient(null, "FossilFeather", 2);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}