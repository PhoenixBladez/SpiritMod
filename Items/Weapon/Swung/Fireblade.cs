using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class Fireblade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viashino Fireblade");
            Tooltip.SetDefault("'Unearthed from the crag of Jund'\nEnemy hits may trigger fiery explosions\nAlways inflicts On Fire! on hit foes\nHit enemies can have their defense reduced to 5");

        }


        public override void SetDefaults()
        {
            item.damage = 70;
            item.useTime = 21;
            item.useAnimation = 21;
            item.melee = true;            
            item.width = 60;              
            item.height = 64;
            item.useStyle = 1;        
            item.knockBack = 7;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
            item.crit = 4;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180, true);
            if (Main.rand.Next(4) == 0)
            {
                {
                    Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, mod.ProjectileType("ViashinoExplosion"), damage, knockback, player.whoAmI, 0f, 0f);
                }
            }
            if (Main.rand.Next(10) == 0)
            {
                target.defense = 5;
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 244);
                    int dust1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
                }
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BoneFlail", 1);
            recipe.AddIngredient(ItemID.FieryGreatsword, 1);
            recipe.AddIngredient(null, "InfernalAppendage", 12);
            recipe.AddIngredient(null, "FieryEssence", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}