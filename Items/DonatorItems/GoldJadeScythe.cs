using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class GoldJadeScythe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crook of the Tormented");
			Tooltip.SetDefault("Occasionally spawns forth Ornate Scarabs to defend you upon hitting enemies \n ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 42;            
            item.melee = true;            
            item.width = 50;              
            item.height = 50;             
            item.useTime = 19;           
            item.useAnimation = 19;     
            item.useStyle = 1;        
            item.knockBack = 6;            
            item.rare = 3;
            item.UseSound = SoundID.Item1;         
            item.autoReuse = true;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.useTurn = true;
            item.crit = 9;                                    
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(8) == 1)
            {
                Vector2 velocity = new Vector2(player.direction, 0) * 4f;
                int proj = Terraria.Projectile.NewProjectile(player.Center.X, player.position.Y + player.height + -35, velocity.X, velocity.Y, mod.ProjectileType("JadeScarab"), 31, item.owner, 0, 0f);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Chitin", 12);
            recipe.AddIngredient(ItemID.Emerald, 4);
            recipe.AddRecipeGroup("GoldBars", 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}