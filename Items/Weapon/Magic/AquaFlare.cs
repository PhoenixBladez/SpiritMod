using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class AquaFlare : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquafire");
			Tooltip.SetDefault("Shoots out waves of Aquafire that revolve round the player or around one another\nMay inflict On Fire! or Frostburn");
		}



        public override void SetDefaults()
        {
            item.damage = 52;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 32;
            item.mana = 10;
            item.useAnimation = 32;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 90000;
            item.rare = 6;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shootSpeed = 6;
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("AquaFlareProj");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, 0, 5, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {

            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Flamelash, 1);
            modRecipe.AddIngredient(ItemID.AquaScepter, 1);
            modRecipe.AddIngredient(null, "SpiritBar", 5);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
