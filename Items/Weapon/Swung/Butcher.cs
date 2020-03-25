using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Butcher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodfire Scythe");
			Tooltip.SetDefault("Inflicts 'Blood Corruption'\nCritical hits steal life");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/Butcher_Glow");
        }


        public override void SetDefaults()
        {
            item.damage = 21;            
            item.melee = true;            
            item.width = 60;              
            item.height = 70;             
            item.useTime = 29;           
            item.useAnimation = 29;     
            item.useStyle = 1;        
            item.knockBack = 5;      
            item.value = Item.sellPrice(0, 0, 5, 0);        
            item.rare = 2;
            item.UseSound = SoundID.Item1;         
            item.autoReuse = true;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.value = Item.sellPrice(0, 0, 32, 0);
            item.useTurn = true;                                   
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("BCorrupt"), 120);
            if (crit)
            {
                if (player.statLife <= player.statLifeMax - 6)
                player.HealEffect(6);
                player.statLife += 6;
            }
        }
    	public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
		    Lighting.AddLight(item.position, 0.46f, .07f, .12f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Swung/Butcher_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}