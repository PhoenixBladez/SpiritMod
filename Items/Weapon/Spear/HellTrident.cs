using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Spear
{
    public class HellTrident : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fury of the Underworld");
			Tooltip.SetDefault("May rapidly shoots out tridents that sticks to enemies before exploding, combusting hit foes");
		}


        int currentHit;
        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 64;
            item.value = Item.sellPrice(0, 4, 30, 0);
            item.rare = 8;
            item.damage = 69;
            item.knockBack = 9f;
            item.useStyle = 5;
            item.useTime = 17;
            item.useAnimation = 17;
            item.melee = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("HellTridentProj");
            item.shootSpeed = 15f;
            item.UseSound = SoundID.Item1;
            this.currentHit = 0;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[item.shoot] > 0)
                return false;
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//create velocity vectors for the two angled projectiles (outwards at PI/15 radians)
			Vector2 origVect = new Vector2(speedX, speedY);
            Vector2 newVect = Vector2.Zero;
            if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
			speedX = newVect.X;
			speedY = newVect.Y;
            this.currentHit++;
            return true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuskLance", 1);
            recipe.AddIngredient(null, "Talonginus", 1);
            recipe.AddIngredient(ItemID.Gungnir, 1);
            recipe.AddIngredient(null, "SunShard", 8);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}