using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class FolvBlade3 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Enchanted Blade");
			Tooltip.SetDefault("Returns a large amount of mana on swing \n Inflicts an Arcane Burn on foes \n  ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 68;
            item.melee = true;
            item.width = 60;
            item.height = 60;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 1;
            item.autoReuse = true;
            item.knockBack = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item1;
            item.crit = 12;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(1) == 0)
            {
                target.AddBuff(mod.BuffType("ArcaneSurge"), 120);
            }
            {
                player.statMana += 16;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Enchantment", 1);
            recipe.AddIngredient(null, "FolvBlade2", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}