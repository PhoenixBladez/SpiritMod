using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class FolvBlade2 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Sharpened Blade");
			Tooltip.SetDefault("Occasionally returns a larger amount of mana on swing.\n  ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 54;
            item.melee = true;
            item.width = 50;
            item.height = 50;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 1;
            item.autoReuse = true;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.crit = 10;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.statMana += 9;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Whetstone", 1);
            recipe.AddIngredient(null, "FolvBlade1", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}