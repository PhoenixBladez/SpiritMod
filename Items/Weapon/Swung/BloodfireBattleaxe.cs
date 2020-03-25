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
    public class BloodfireBattleaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarlet Battleaxe");
			Tooltip.SetDefault("Inflicts 'Blood Corruption'\nCritical hits steal life");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/BloodfireBattleaxe_Glow");
        }


        public override void SetDefaults()
        {
            item.damage = 30;            
            item.melee = true;            
            item.width = 60;              
            item.height = 70;             
            item.useTime = 45;
            item.axe = 10;
            item.useAnimation = 45;     
            item.useStyle = 1;        
            item.knockBack = 9;      
            item.value = Item.sellPrice(0, 0, 8, 0);        
            item.rare = 2;
            item.UseSound = SoundID.Item1;         
            item.autoReuse = true;
            item.useTurn = true;                                   
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("BCorrupt"), 120);
        }
    	public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
		    Lighting.AddLight(item.position, 0.46f, .07f, .12f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Swung/BloodfireBattleaxe_Glow"),
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
            recipe.AddIngredient(null, "BloodFire", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}