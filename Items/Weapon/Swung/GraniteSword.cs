using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class GraniteSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Slicer");
			Tooltip.SetDefault("Critical hits inflict Energy Flux, causing enemies to move spasmodically");
		}


        public override void SetDefaults()
        {
            item.damage = 23;            
            item.melee = true;            
            item.width = 44;              
            item.height = 44;
            item.useTime = 26;
            item.useAnimation = 26;     
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.crit = 10;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (crit)
            {
                target.AddBuff(mod.BuffType("EnergyFlux"), 240);
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "GraniteChunk", 18);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}