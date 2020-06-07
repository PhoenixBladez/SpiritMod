using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class TitaniumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Staff");
			Tooltip.SetDefault("Surrounds the player in blades\nHold left-click to release a barrage of blades");
		}


		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.mana = 11;
            item.channel = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 6;
            item.useTurn = true;
            item.value = Terraria.Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item88;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("TitaniumStaffProj");
			item.shootSpeed = 30f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }
        int counter;
        public override void HoldItem(Player player)
        {
            counter++;
            int spikes = player.GetSpiritPlayer().shadowCount;
            if (counter >= 85 && !player.channel && spikes <= 3)
            {
                counter = 0;
                int num = 4 - spikes;
                for (int I = 0; I < num; I++)
                {
                    int DegreeDifference = (int)(360 / num);
                    Projectile.NewProjectile((int)player.Center.X + (int)(Math.Sin(I * DegreeDifference) * 80), (int)player.Center.Y + (int)(Math.Sin(I * DegreeDifference) * 80), 0, 0, mod.ProjectileType("TitaniumStaffProj"), item.damage, item.knockBack, player.whoAmI, 0, I * DegreeDifference);
                    player.GetSpiritPlayer().shadowCount++;
                }
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
