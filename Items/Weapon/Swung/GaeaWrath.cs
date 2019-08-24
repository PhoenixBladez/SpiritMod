using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class GaeaWrath : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gaea's Wrath");
            Tooltip.SetDefault("'The Earth is angry...'\nShoots a blast of Gaea Energy\nGaea energy leaves a lingering portal that shoots out multiple leaves\nInflicts Poison, Acid Burn, and Venom");

        }


        public override void SetDefaults()
        {
            item.damage = 95;
            item.useTime = 25;
            item.useAnimation = 25;
            item.melee = true;            
            item.width = 56;              
            item.height = 56;
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 8;
            item.crit = 6;
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item70;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("GaeaBlast");
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(4) == 0)

            {
                target.AddBuff(mod.BuffType("AcidBurn"), 240);
            }
            if (Main.rand.Next(4) == 0)

            {
                target.AddBuff(BuffID.Venom, 240);

            }
            if (Main.rand.Next(4) == 0)

            {
                target.AddBuff(BuffID.Poisoned, 240);
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
             
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Earthblade", 1);
            recipe.AddIngredient(null, "VenomBlade", 1);
            recipe.AddIngredient(null, "PalmSword", 1);
            recipe.AddIngredient(null, "Acid", 8);
            recipe.AddIngredient(null, "PrimevalEssence", 12);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddIngredient(ItemID.Ectoplasm, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}