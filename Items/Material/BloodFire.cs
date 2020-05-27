using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Material
{
    public class BloodFire : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
			Tooltip.SetDefault("'The stuff of literal nightmares'");
            ItemID.Sets.ItemIconPulse[item.type] = true;
		}


        public override void SetDefaults()
        {
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.width = 24;
            item.height = 28;
            item.value = 100;
            item.rare = 2;

            item.maxStack = 999;
        }
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
		     Lighting.AddLight(item.position, 0.92f, .14f, .24f);
        }
    	public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
    }
}