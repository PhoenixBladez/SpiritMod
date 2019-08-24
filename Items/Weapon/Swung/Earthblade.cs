using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Earthblade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Earthblade");
            Tooltip.SetDefault("Melee hits occasionally ensnare them in vines, lowering their movement speed\nMelee critical hits grant the player 'Earthwrought,' inreasing life regeneration\nOccasionally shoots out a cluster of powerful leaves");

        }


        public override void SetDefaults()
        {
            item.damage = 29;            
            item.melee = true;            
            item.width = 34;              
            item.height = 22;             
            item.useTime = 29;
            item.autoReuse = true;
            item.useAnimation = 29;     
            item.useStyle = 1;        
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.crit = 6;
            item.shoot = mod.ProjectileType("OvergrowthLeaf");
            item.shootSpeed = 9f;
            item.UseSound = SoundID.Item1;                                     
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(2) == 0)
            {
                int proj = Projectile.NewProjectile(position.X, position.Y, (speedX / 3) * 2, speedY, type, 18, 2, player.whoAmI);
                int proj2 = Projectile.NewProjectile(position.X, position.Y, (speedX / 3) * 2, speedY, type, 18, 2, player.whoAmI);
            }
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 44);
                }
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(mod.BuffType("VineTrap"), 180);
            }
            if (crit)
            {
                player.AddBuff(mod.BuffType("Earthwrought"), 180);
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteSword", 1);
            recipe.AddIngredient(null, "SanctifiedStabber", 1);
            recipe.AddIngredient(null, "FloranBar", 12);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}