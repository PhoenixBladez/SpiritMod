using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class FolvBlade4 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Ancient Blade");
			Tooltip.SetDefault("Returns a huge amount of mana on swing \n Inflicts an Arcane Burn on foes \n Shoots out a powerful Arcane sword \n 'The power of ancient mana runs through your sword.'\n  ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 200;
            item.melee = true;
            item.width = 80;
            item.height = 80;
            item.useTime = 17;
            item.useAnimation = 17;
            item.useStyle = 1;
            item.autoReuse = true;
            item.knockBack = 8.3f;
            item.value = Item.sellPrice(0, 40, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("ArcaneSword");
            item.shootSpeed = 7;
            item.crit = 15;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                int dust1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);

            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(1) == 0)
            {
                target.AddBuff(mod.BuffType("ArcaneSurge"), 120);
            }
            {
                player.statMana += 25;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Hilt", 1);
            recipe.AddIngredient(null, "FolvBlade3", 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}