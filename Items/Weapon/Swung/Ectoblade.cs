using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Ectoblade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodshadow");
            Tooltip.SetDefault("Left-Click to steal large amounts of life\nLeft-Clicking causes the sword to swing extremely slowly, but with great damage and knockback\nRight-Click to swing much faster, but with less damage and knockback\nRight-Clicking shoots out shadows, and hitting enemies with the blade grants you 'Shadow Tread,' increasing movement speed");

        }


        public override void SetDefaults()
        {
            item.damage = 200;            
            item.melee = true;            
            item.width = 64;              
            item.height = 64;
            item.useTime = 36;
            item.useAnimation = 36;     
            item.useStyle = 1;        
            item.knockBack = 15;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.shoot = mod.ProjectileType("ShadowBlast");
            item.rare = 8;
            item.shootSpeed = 15f;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (player.altFunctionUse == 2)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 62);

            }
            else
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 60);

            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (player.altFunctionUse == 2)
            {
                player.AddBuff(mod.BuffType("ShadowTread"), 180);
            }
            else
            {
                player.statLife += 7;
                player.HealEffect(7);
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.damage = 75;
                item.useTime = 20;
                item.useAnimation = 20;
                item.shoot = mod.ProjectileType("ShadowBlast");
                item.knockBack = 3;
            }
            else
            {
                item.damage = 150;
                item.useTime = 60;
                item.useAnimation = 60;
                item.knockBack = 15;
                item.shoot = 0;
            }
            return base.CanUseItem(player);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Slugger", 1);
            recipe.AddIngredient(null, "Tenderizer", 1);
            recipe.AddIngredient(null, "SpiritBar", 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}