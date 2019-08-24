using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class MagalaBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elendskraft");
			Tooltip.SetDefault("Right click to convert Elendskraft into a damaging shield\n Enemies in an aura around the shield receive 'Frenzy Virus' and are knocked back\n ~Donator Item~ \n 'Shoutout to Powpowitsme!'");
		}


        public override void SetDefaults()
        {
            item.damage = 50;            
            item.melee = true;            
            item.width = 54;              
            item.height = 44;
            item.useTime = 23;
            item.noUseGraphic = false;
            item.useAnimation = 23;     
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.shoot = mod.ProjectileType("MagalaShield");
            item.rare = 5;
            item.shootSpeed = 5f;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.altFunctionUse == 2)
            {
                target.AddBuff(mod.BuffType("FrenzyVirus"), 580);
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.damage = 10;
                item.noUseGraphic = true;
                item.shoot = mod.ProjectileType("MagalaShield");
                item.useStyle = 3;
                item.height = 2;
                item.width = 2;
                item.knockBack = 9;
                item.autoReuse = false;
                item.shootSpeed = 2f;
            }
            else
            {
                item.damage = 50;
                item.noUseGraphic = false;
                item.useTime = 24;
                item.width = 54;
                item.height = 44;
                item.useAnimation = 24;
                item.shoot = 0;
                item.knockBack = 5;
                item.useStyle = 1;
                item.autoReuse = true;
                item.shootSpeed = 0f;
            }
            return base.CanUseItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (player.altFunctionUse == 2)
            {

            }
            else
            {
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
                }
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MagalaScale", 20);
            recipe.AddIngredient(ItemID.DD2SquireDemonSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}